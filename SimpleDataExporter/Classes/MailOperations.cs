using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Redemption;
using Interop.Redemption;

namespace SimpleDataExporter.Classes
{
    public class MailOperations
    {
        private StringBuilder _status;
        private Summation _swObj;
        public delegate void UpdateStatusEventHandler(StringBuilder status);
        public event UpdateStatusEventHandler StatusUpdated;


        public StringBuilder Status
        {
            get { return _status; }
            set
            {
                _status = value;
                if (_status.Length != 0)
                {
                    OnStatusUpdated(value);
                }
            }
        }

        protected void OnStatusUpdated(StringBuilder status)
        {
            if (status.Length != 0)
            {
                StatusUpdated(status);
            }
        }

        public Summation swObj
        {
            get { return _swObj; }
            set { _swObj = value; }
        }


        public MailOperations()
        {
            if (_status == null)
            {
                _status = new StringBuilder();
            }
        }

        public void ExtractMSGFiles(string connectionString, string outputDirectory, string caseFriendlyName)
        {
            DataTable dtMailXMLInfo = new DataTable();
            DataTable dtDocIdCollection = new DataTable();
            List<RDOMail> mailItems = null;
            dtMailXMLInfo = GetPSTXMLInfo(connectionString);

            if (dtMailXMLInfo.Rows.Count != 0)
            {
                _status.AppendFormat("Beginning mail message extraction");
                _status.AppendLine();
                StatusUpdated(_status);

                //Declare a string for the outfile path.  We need to take into consideration the difference between 
                string strOutFilePath = string.Empty;

                //We need to use the case-friendly name as the name of the subdirectory
                //If the directory doesn't exist, create it, otherwise, leave it alone
                string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, "EmailExport");

                try
                {

                    foreach (DataRow drXml in dtMailXMLInfo.Rows)
                    {
                        //Query the coreDB and get the collection of DocIds and entryIds
                        string query = String.Format("Select docId, msgId from e-table where storeId='{0}'", drXml["pstId"].ToString());
                        
                        dtDocIdCollection = DatabaseOperations.GetSpecificTable(query, connectionString);
                        mailItems = readPst(drXml["file"].ToString(), dtDocIdCollection);

                        foreach (DataRow drDocId in dtDocIdCollection.Rows)
                        {

                            var msgQuery = (mailItems.Where(w => w.EntryID == drDocId["msgId"].ToString()));

                            foreach (var mi in msgQuery)
                            {
                                mi.SaveAs(directoryName + @"\" + drDocId["docId"].ToString() + ".msg", rdoSaveAsType.olMSG);
                                _status.AppendFormat("Creating: " + directoryName + @"\" + drDocId["docId"].ToString() + ".msg");
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    _status.AppendFormat("There was an error processing that occurred during mail extraction: {0}", ex.ToString());
                    _status.AppendLine();
                    StatusUpdated(_status);
                }
                finally
                {
                    mailItems = null;
                    GC.Collect();
                }

            }
            else
            {
                _status.AppendFormat("No mail messages to process for export.");
                _status.AppendLine();
                StatusUpdated(_status);

            }

        }

        public DataTable GetPSTXMLInfo(string coreDBPath)
        {
            //1. Determine if there's mail first - query the core db and look in the media column for the type "eMail"
            //2. If there is a result set, we need to capture the DocId, EntryId/MsgId, and PSTID in the Core DB into a datatable
            //3. Go fetch and read the repositories.xml file to get where the PST's live
            //4  Go and read all the xml files in the PST repositories directory and capture the: PSTID and Message Store ID
            //5. That's enough to start with on 2/10/2013

            DataTable dtpstXMLDetails = new DataTable();
            dtpstXMLDetails.Columns.Add("file", typeof(string));
            dtpstXMLDetails.Columns.Add("pstid", typeof(string));
            dtpstXMLDetails.Columns.Add("storeid", typeof(string));
            DataRow drTemp = null;


            DataTable dtMailResults = new DataTable();
            XDocument xmlPstDoc = new XDocument();
            string strQuery = "Select media from e-table where media='eMail'";
            dtMailResults = DatabaseOperations.GetSpecificTable(strQuery, coreDBPath);
            if (dtMailResults.Rows.Count != 0)
            {
                //If the result isn't equal to zero, let's go get the path and read the repositories.xml file

                string strPSTDir = string.Empty;
                string strAllProfilesDir = swObj.GetAllProfilesDirectoryPath();
                string strXMLFileName = strAllProfilesDir + "repositories.xml";
                if (File.Exists(strXMLFileName))
                {
                    XDocument xmlProfileDoc = XDocument.Load(strAllProfilesDir + "repositories.xml");

                    var pstDir = xmlProfileDoc.Descendants("pstdirs").Select(s => s.Descendants());

                    foreach (var path in pstDir)
                    {
                        foreach (var item in path)
                        {
                            string filePath = item.Value.ToLower();

                            if (!filePath.Contains(".map"))
                            {

                                //Next get a list of all the XML files in the directory and read in their store names and store ID's
                                string[] strFileList = Directory.GetFiles(filePath + @"\", "*.xml", SearchOption.AllDirectories);

                                if (strFileList.Length != 0)
                                {
                                    foreach (string file in strFileList)
                                    {
                                        xmlPstDoc = XDocument.Load(file);


                                        var pstFileInfo = (from child in xmlPstDoc.Descendants("pstinfo").Elements()
                                                           select child).ToArray();

                                        drTemp = dtpstXMLDetails.NewRow();

                                        foreach (var info in pstFileInfo)
                                        {

                                            switch (info.Name.LocalName)
                                            {
                                                case "file":
                                                    drTemp["file"] = info.Value;
                                                    break;
                                                case "pstid":
                                                    drTemp["pstid"] = info.Value;
                                                    break;
                                                case "storeid":
                                                    drTemp["storeid"] = info.Value;
                                                    break;
                                            }
                                        }

                                        //Add the results to the table
                                        dtpstXMLDetails.Rows.Add(drTemp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dtpstXMLDetails;
        }

        private List<RDOMail> readPst(string pstFilePath, DataTable dtDocIdCollection)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Redemption.RedemptionLoader.DllLocation32Bit = currentDirectory + @"\" + "Redemption.dll";
            RDOSession session = Redemption.RedemptionLoader.new_RDOSession();

            //RDOSession session = new RDOSession();
            List<RDOMail> mailItems = new List<RDOMail>();

            RDOPstStore pstStore = session.LogonPstStore(pstFilePath, null, null, null, null);

            foreach (DataRow entryId in dtDocIdCollection.Rows)
            {
                try
                {
                    mailItems.Add(session.GetMessageFromID(entryId["msgId"].ToString()));

                }
                catch (Exception ex)
                {
                    _status.AppendFormat("There was an error processing PST {0} with an error of: {1}", pstFilePath, ex.ToString());
                    _status.AppendLine();
                    StatusUpdated(_status);
                }
            }

            pstStore.Remove();
            session = null;
            pstStore = null;

            return mailItems;
        }

        public void CreateMSGReferenceFile(string strCoreDBPath, string outputDirectory, string caseFriendlyName)
        {
            DataTable dtSumMSGInfo = new DataTable();
            DataTable dtOutput = new DataTable();
            dtOutput.Columns.Add("docId", typeof(string)); //Comes from Summation
            dtOutput.Columns.Add("entryId", typeof(string)); //Comes from Summation
            dtOutput.Columns.Add("pstid", typeof(string)); //Comes from Summation and is also present in XML File used by Summation - KEY
            dtOutput.Columns.Add("filePath", typeof(string)); //Comes from XML file
            dtOutput.Columns.Add("storeid", typeof(string));  //Comes from XML file - this is the actual PST ID Outlook uses
            DataRow drOutput = null;
            DataTable dtXMLPSTInfo = GetPSTXMLInfo(strCoreDBPath);

            //1.  Create a method to pull all docIds, entryIds (msgIds), and pstId info from Summation
            //2.  Take the dtMailXMLInfo datatable and loop through it
            //3.  Create a new datatable with DocId, PST Path (from dtMailXMLInfo), entryID, and StoreId
            //4.  Use the pstid in the dtMailXMLInfo as the key to make the match between what comes from Summation to 
            //    dtXMLPSTInfo

            //Declare a string for the outfile path.  
            string strOutFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, "EmailExport");

            try
            {
                strOutFilePath = directoryName + @"\" + "PSTReference" + ".csv";
                _status.AppendFormat("Creating MSG Reference File");
                _status.AppendLine();
                StatusUpdated(_status);

                //Pull the information from Summation
                //Query the coreDB and get the collection of DocIds and entryIds
                if (dtXMLPSTInfo.Rows.Count != 0)
                {
                    foreach (DataRow drXml in dtXMLPSTInfo.Rows)
                    {
                        string strQuery = String.Format("Select docId, msgId, storeId from e-table where storeId='{0}'", drXml["pstId"].ToString());
                        dtSumMSGInfo = DatabaseOperations.GetSpecificTable(strCoreDBPath, strQuery);

                        foreach (DataRow dr in dtSumMSGInfo.Rows)
                        {

                            drOutput = dtOutput.NewRow();
                            drOutput["DocId"] = dr["docId"].ToString();
                            drOutput["entryId"] = dr["msgId"].ToString();
                            drOutput["pstId"] = drXml["pstId"].ToString();
                            drOutput["filePath"] = drXml["file"].ToString();
                            drOutput["storeid"] = drXml["storeid"].ToString();
                            dtOutput.Rows.Add(drOutput);
                        }
                    }
                    using (FileStream fs = new FileStream(strOutFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(dtOutput.ToCSV());
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    _status.AppendFormat("MSG Reference File created successfully");
                    _status.AppendLine();
                    StatusUpdated(_status);
                }
                else
                {
                    _status.AppendFormat("No rows to process.  MSG Reference File not created.");
                    _status.AppendLine();
                    StatusUpdated(_status);
                }
            }
            catch (Exception ex)
            {
                _status.AppendFormat("An error occurred while creating the MSG Reference File: {0}", ex.ToString());
                _status.AppendLine();
                StatusUpdated(_status);
            }
        }


    }
}
