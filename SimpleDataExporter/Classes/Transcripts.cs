using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using SimpleDataExporter.Classes;

namespace SimpleDataExporter.Classes
{
    public class Transcripts
    {

        //Declare instance of StringBuilder to send the case processing back to the UI
        StringBuilder _status = new StringBuilder();
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

        public Transcripts()
        {
            if (_status == null)
            {
                _status = new StringBuilder();
            }
        }


        public void ExportTranscripts(string outputDirectory, string transcriptPath, string transcriptFileName, string transcriptDescription, string caseFriendlyName, string noteDbPath, string dbType)
        {

            //Query the note DB's note table for the specific data that is related to the testimony
            StringBuilder sbNoteQuery = new StringBuilder();
            sbNoteQuery.AppendFormat("Select * from note where Dep='{0}'", transcriptFileName);

            DataTable dtNoteData = new DataTable();

            StringBuilder transcript = new StringBuilder();
            transcript.Append(@"<?xml version=""1.0""?>");
            transcript.AppendLine();
            transcript.Append("<TRN>");
            transcript.AppendLine();
            transcript.Append("<DocType>Summation Transcript Export File</DocType>");
            transcript.AppendLine();
            transcript.Append("<Version>1.0</Version>");
            transcript.AppendLine();
            transcript.AppendFormat("<Source_Loc>{0}</Source_Loc>", transcriptPath);
            transcript.AppendLine();
            transcript.AppendFormat("<Transcript_Name>{0}</Transcript_Name>", transcriptFileName);
            transcript.AppendLine();
            transcript.AppendFormat("<Transcript_Description>{0}</Transcript_Description>", transcriptDescription);
            transcript.AppendLine();
            transcript.Append("<Notes>");
            transcript.AppendLine();

            //Get a connection to the Note DB
            string _connectionString = DatabaseOperations.GetDBConnectionString(noteDbPath);
            dtNoteData = DatabaseOperations.GetSpecificTable(sbNoteQuery.ToString(), _connectionString);

            if (dtNoteData.Rows.Count != 0)
            {
                var transScriptQuery = (dtNoteData.AsEnumerable()
                   .Select(s => new
                   {
                       dep = s.Field<string>("Dep"),
                       uuid = s.Field<string>("#UUID"),
                       toe = s.Field<double>("#TOE"),
                       tline = s.Field<Int32?>("TLine"),
                       page = s.Field<string>("Page"),
                       pline = s.Field<Int32?>("PLine"),
                       type = s.Field<Int32?>("Type"),
                       author = s.Field<string>("Author"),
                       date = s.Field<string>("Date"),
                       range = s.Field<string>("Range"),
                       issue = s.Field<string>("Issue"),
                       note = s.Field<string>("Note"),
                       hotfact = s.Field<string>("HotFact"),
                       image = s.Field<string>("Image"),
                       audio = s.Field<string>("Audio"),
                       folders = s.Field<string>("Folders")
                   }).OrderBy(t => t.tline)).ToList();


                foreach (var noteElement in transScriptQuery)
                {
                    transcript.AppendFormat(@"<Note Dep=""{0}"" ", noteElement.dep);
                    transcript.AppendFormat(@"UUID=""{0}"" ", noteElement.uuid);
                    transcript.AppendFormat(@"TOE=""{0}"" ", noteElement.toe);
                    transcript.AppendFormat(@"TLine=""{0}"" ", noteElement.tline);
                    transcript.AppendFormat(@"Page=""{0}"" ", noteElement.page.ToString().Trim());
                    transcript.AppendFormat(@"PLine=""{0}"" ", noteElement.pline);
                    transcript.AppendFormat(@"Type=""{0}"" ", noteElement.type);
                    transcript.AppendFormat(@"Author=""{0}"" ", noteElement.author);

                    //Tags that may not always be available with each note

                    //If the date element is empty, don't include in the tag
                    if (noteElement.date != null)
                    {
                        transcript.AppendFormat(@"Date=""{0}"" ", noteElement.date);
                    }

                    //If the range element is empty, don't include in the tag
                    if (noteElement.range != null)
                    {
                        transcript.AppendFormat(@"Range=""{0}"" ", noteElement.range);
                    }

                    //If the image element is empty, don't include in the tag
                    if (noteElement.image != null)
                    {
                        transcript.AppendFormat(@"Image=""{0}"" ", noteElement.image);
                    }

                    //If the audio element is empty, don't include in the tag
                    if (noteElement.audio != null)
                    {
                        transcript.AppendFormat(@"Audio=""{0}"" ", noteElement.audio);
                    }


                    // End of custom tag exclusions

                    transcript.Append(">");
                    transcript.AppendLine();

                    if (noteElement.issue != null)
                    {
                        transcript.Append("<Issues>");
                        transcript.AppendLine();
                        transcript.AppendFormat("<Issue>{0}</Issue>", noteElement.issue);
                        transcript.AppendLine();
                        transcript.Append("</Issues>");
                        transcript.AppendLine();
                    }

                    if (noteElement.hotfact != null)
                    {
                        transcript.Append("<HotFacts>");
                        transcript.AppendLine();
                        transcript.AppendFormat("<HotFact>{0}</HotFact>", noteElement.hotfact);
                        transcript.AppendLine();
                        transcript.Append("</HotFacts>");
                        transcript.AppendLine();
                    }

                    if (noteElement.note != null)
                    {
                        transcript.Append("<Text>");
                        transcript.AppendLine();
                        transcript.AppendFormat("{0}</Text>", noteElement.note);
                        transcript.AppendLine();
                        transcript.Append("</Note>");
                        transcript.AppendLine();
                    }
                }
            }

            transcript.Append("</Notes>");
            transcript.AppendLine();
            transcript.Append("<Transcript Info>");
            transcript.AppendLine();
            transcript.Append("-1,1,6,0,0");
            transcript.AppendLine();
            transcript.Append("</Transcript Info>");
            transcript.AppendLine();

            transcript.Append("<Transcript>");
            transcript.AppendLine();
            transcript.AppendFormat("{0}", File.ReadAllText(transcriptPath + transcriptFileName));

            transcript.AppendLine();
            transcript.Append("</Transcript>");
            transcript.AppendLine();
            transcript.Append("</TRN>");

            //Declare a string for the outfile path.  
            string outfilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, dbType);
            outfilePath = directoryName + @"\" + Path.GetFileNameWithoutExtension(transcriptFileName) + ".trn";
            FileOperationsClass.WriteAllLines(outfilePath, transcript.ToString());
            StatusUpdated(_status.AppendFormat("Exporting transcript: {0}", outfilePath));

        }

        public List<KeyValuePair<string, string>> GetTranscriptFileList(string sharedProfileDir)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SWX));
            List<KeyValuePair<string, string>> transcriptFileKVP = new List<KeyValuePair<string, string>>();
            SWX swx;
            string filePath = Path.Combine(sharedProfileDir, "swcase.xml");
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                swx = (SWX)ser.Deserialize(reader);
            }

            foreach (var item in swx.FOLDER.Items)
            {
                Type type = item.GetType();
                if (type.Name == "SWXFOLDERFOLDER")
                {
                    SWXFOLDERFOLDER swxFolderFolder = (SWXFOLDERFOLDER)item;
                    if (swxFolderFolder.Name == "Transcripts")
                    {
                        foreach (var transcript in swxFolderFolder.ITEM)
                        {
                            //Get the filename and the description on the same read
                            transcriptFileKVP.Add(new KeyValuePair<string, string>(transcript.Extra, transcript.Name));
                        }

                    }

                }
            }
            return transcriptFileKVP;
        }

        public void GetTranscriptHighlights(string outputDirectory, string transcriptDescription, string caseFriendlyName, string noteDbPath, string dbType)
        {
            //Query the note DB's note table for the specific data that is related to the testimony
            StringBuilder _noteQuery = new StringBuilder();
            _noteQuery.AppendFormat("Select Tranname, Prange, Creator, Dt_crtd, Modifier, Dt_mod, #TOE from hilite where Tranname='{0}'", transcriptDescription);
            StringBuilder _highlights = new StringBuilder();

            //Get a connection to the Note DB
            string _connectionString = DatabaseOperations.GetDBConnectionString(noteDbPath);
            DataTable _highlightDataTable = DatabaseOperations.GetSpecificTable(_noteQuery.ToString(), _connectionString);

            if (_highlightDataTable.Rows.Count != 0)
            {

                string transcriptName = _highlightDataTable.Rows[0]["Tranname"].ToString();
                _highlights.AppendFormat("Case name: {0}", transcriptName);
                _highlights.AppendLine();
                _highlights.AppendLine();


                foreach (DataRow dr in _highlightDataTable.Rows)
                {
                    string[] pageRange = SplitPageRange(dr["Prange"].ToString());

                    _highlights.AppendFormat("Page range, Line Start: {0}", pageRange[0]);
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Page range, Character Start: {0}", pageRange[1]);
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Page range, Line End: {0}", pageRange[2]);
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Page range, Character End: {0}", pageRange[3]);
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Created By: {0}", dr["Creator"].ToString());
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Date Created: {0}",dr["Dt_crtd"].ToString());
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Modified By: {0}", dr["Modifier"].ToString());
                    _highlights.AppendLine();

                    _highlights.AppendFormat("Date Modified: {0}", dr["Dt_mod"].ToString());
                    _highlights.AppendLine();
                    _highlights.AppendLine();
                }
               
                //Declare a string for the outfile path.  
                string outfilePath = string.Empty;

                //We need to use the case-friendly name as the name of the subdirectory
                //If the directory doesn't exist, create it, otherwise, leave it alone
                string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, dbType);
                outfilePath = directoryName + @"\" + transcriptName + ".txt";
                FileOperationsClass.WriteAllLines(outfilePath, _highlights.ToString());
                StatusUpdated(_status.AppendLine());
                StatusUpdated(_status.AppendFormat("Exporting transcript highlights: {0}", outfilePath));
            }
        }


        private string[] SplitPageRange(string pageRange)
        {
            string[] pageRangeArray = pageRange.Split(',');
            return pageRangeArray;
        }
    }

}
