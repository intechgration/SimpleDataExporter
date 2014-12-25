using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using System.IO;
using SimpleDataExporter.Classes;

namespace SimpleDataExporter.Classes
{
    class MailOperationsClass
    {
        public DataTable GetPSTXMLInfo(string strCoreDBPath)
        {
            //1. Determine if there's mail first - query the core db and look in the media column for the type "eMail"
            //2. If there is a result set, we need to capture the DocId, EntryId/MsgId, and PSTID in the Core DB into a datatable
            //3. Go fetch and read the repositories.xml file to get where the PST's live
            //4  Go and read all the xml files in the PST repositories directory and capture the: PSTID and Message Store ID
            //5. That's enough to start with on 2/10/2013

            //Update: November 2, 2013
            //Enhancement will be added to produce basic and enhanced PST reference files for a separate utility used 
            //for mail extraction.

            DataTable dtpstXMLDetails = new DataTable();
            dtpstXMLDetails.Columns.Add("file", typeof(string));
            dtpstXMLDetails.Columns.Add("pstid", typeof(string));
            dtpstXMLDetails.Columns.Add("storeid", typeof(string));
            DataRow drTemp = null;

            DataTable dtMailResults = new DataTable();
            XDocument xmlPstDoc = new XDocument();
            string strQuery = "Select media from e-table where media='eMail'";
            TableOperationClass _toc = new TableOperationClass();
            dtMailResults = _toc.GetSpecificTable(strCoreDBPath, strQuery);
            if (dtMailResults.Rows.Count != 0)
            {

                //If the result isn't equal to zero, let's go get the path and read the repositories.xml file
                SumInfoClass si = new SumInfoClass();
                string strPSTDir = string.Empty;
                string strAllProfilesDir = si.GetAllProfilesDirectoryPath();
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
    }
}
