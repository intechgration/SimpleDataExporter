using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SimpleDataExporter.Classes;
using System.Runtime.InteropServices;

namespace SimpleDataExporter.Classes
{
    class SumOutputFileClass
    {
        public StringBuilder ExportTranscripts(string strTransPath, string strTransFileName, TextBox txtOutputPath, string strCaseFriendlyName, string strNoteDBPath, string strDBType)
        {
            //Declare instance of StringBuilder to send the case processing back to the UI
            StringBuilder sbStatus = new StringBuilder();

            //Declare new instance of Summation Info class
            SumInfoClass sTranscriptInfo = new SumInfoClass();

            string strTranscriptDescription = sTranscriptInfo.GetTranscriptDescription(strTransFileName);

            //Query the note DB's note table for the specific data that is related to the testimony
            StringBuilder sbNoteQuery = new StringBuilder();
            sbNoteQuery.AppendFormat("Select * from note where Dep='{0}'", strTransFileName);

            DataTable dtNoteData = new DataTable();

            StringBuilder sbTRN = new StringBuilder();
            sbTRN.Append(@"<?xml version=""1.0""?>");
            sbTRN.AppendLine();
            sbTRN.Append("<TRN>");
            sbTRN.AppendLine();
            sbTRN.Append("<DocType>Summation Transcript Export File</DocType>");
            sbTRN.AppendLine();
            sbTRN.Append("<Version>1.0</Version>");
            sbTRN.AppendLine();
            sbTRN.AppendFormat("<Source_Loc>{0}</Source_Loc>", strTransPath);
            sbTRN.AppendLine();
            sbTRN.AppendFormat("<Transcript_Name>{0}</Transcript_Name>", strTransFileName);
            sbTRN.AppendLine();
            sbTRN.AppendFormat("<Transcript_Description>{0}</Transcript_Description>", strTranscriptDescription);
            sbTRN.AppendLine();
            sbTRN.Append("<Notes>");
            sbTRN.AppendLine();

            //Get a connection to the Note DB
            TableOperationClass toc = new TableOperationClass();
            dtNoteData = toc.GetSpecificTable(strNoteDBPath, sbNoteQuery.ToString());

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
                    sbTRN.AppendFormat(@"<Note Dep=""{0}"" ", noteElement.dep);
                    sbTRN.AppendFormat(@"UUID=""{0}"" ", noteElement.uuid);
                    sbTRN.AppendFormat(@"TOE=""{0}"" ", noteElement.toe);
                    sbTRN.AppendFormat(@"TLine=""{0}"" ", noteElement.tline);
                    sbTRN.AppendFormat(@"Page=""{0}"" ", noteElement.page.ToString().Trim());
                    sbTRN.AppendFormat(@"PLine=""{0}"" ", noteElement.pline);
                    sbTRN.AppendFormat(@"Type=""{0}"" ", noteElement.type);
                    sbTRN.AppendFormat(@"Author=""{0}"" ", noteElement.author);

                    //Tags that may not always be available with each note

                    //If the date element is empty, don't include in the tag
                    if (noteElement.date != null)
                    {
                        sbTRN.AppendFormat(@"Date=""{0}"" ", noteElement.date);
                    }

                    //If the range element is empty, don't include in the tag
                    if (noteElement.range != null)
                    {
                        sbTRN.AppendFormat(@"Range=""{0}"" ", noteElement.range);
                    }

                    //If the image element is empty, don't include in the tag
                    if (noteElement.image != null)
                    {
                        sbTRN.AppendFormat(@"Image=""{0}"" ", noteElement.image);
                    }

                    //If the audio element is empty, don't include in the tag
                    if (noteElement.audio != null)
                    {
                        sbTRN.AppendFormat(@"Audio=""{0}"" ", noteElement.audio);
                    }


                    // End of custom tag exclusions

                    sbTRN.Append(">");
                    sbTRN.AppendLine();

                    if (noteElement.issue != null)
                    {
                        sbTRN.Append("<Issues>");
                        sbTRN.AppendLine();
                        sbTRN.AppendFormat("<Issue>{0}</Issue>", noteElement.issue);
                        sbTRN.AppendLine();
                        sbTRN.Append("</Issues>");
                        sbTRN.AppendLine();
                    }

                    if (noteElement.hotfact != null)
                    {
                        sbTRN.Append("<HotFacts>");
                        sbTRN.AppendLine();
                        sbTRN.AppendFormat("<HotFact>{0}</HotFact>", noteElement.hotfact);
                        sbTRN.AppendLine();
                        sbTRN.Append("</HotFacts>");
                        sbTRN.AppendLine();
                    }

                    if (noteElement.note != null)
                    {
                        sbTRN.Append("<Text>");
                        sbTRN.AppendLine();
                        sbTRN.AppendFormat("{0}</Text>", noteElement.note);
                        sbTRN.AppendLine();
                        sbTRN.Append("</Note>");
                        sbTRN.AppendLine();

                    }
                }
            }
            sbTRN.Append("</Notes>");
            sbTRN.AppendLine();
            sbTRN.Append("<Transcript Info>");
            sbTRN.AppendLine();
            sbTRN.Append("-1,1,6,0,0");
            sbTRN.AppendLine();
            sbTRN.Append("</Transcript Info>");
            sbTRN.AppendLine();

            sbTRN.Append("<Transcript>");
            sbTRN.AppendLine();
            sbTRN.AppendFormat("{0}", File.ReadAllText(strTransPath + strTransFileName));

            sbTRN.AppendLine();
            sbTRN.Append("</Transcript>");
            sbTRN.AppendLine();
            sbTRN.Append("</TRN>");

            //Declare a string for the outfile path. 
            FileOperationsClass foc = new FileOperationsClass();
            foc.CreateOutputDirectory(txtOutputPath, strCaseFriendlyName, strDBType);

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string strdirectoryName = foc.CreateOutputDirectory(txtOutputPath, strCaseFriendlyName, strDBType);
            string strOutFilePath = strdirectoryName + @"\" + Path.GetFileNameWithoutExtension(strTransFileName) + ".trn";
            File.WriteAllText(strOutFilePath, sbTRN.ToString());
            sbStatus.AppendFormat("Exporting transcript: {0}", strOutFilePath);
            return sbStatus;
        }

        public void OutputTableToFile(TextBox txtOutputPath, DataTable dt, string strCaseFriendlyName,
            string strDBType, string strFileName, [Optional] RadioButton rdo)
        {
            FileOperationsClass _foc = new FileOperationsClass();

            //Declare a string for the outfile path.  
            string strOutFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string strdirectoryName = _foc.CreateOutputDirectory(txtOutputPath, strCaseFriendlyName, strDBType);

            if (strFileName.Contains("IMGINFO"))
            {
                strOutFilePath = strdirectoryName + @"\" + strFileName + ".OPT";
                using (FileStream fs = new FileStream(strOutFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(dt.ToCSV());
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                if (rdo != null)
                {

                    if (rdo.Checked == true)
                    {
                        strOutFilePath = strdirectoryName + @"\" + strFileName + ".TXT";
                        using (FileStream fs = new FileStream(strOutFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(dt.ToPipeCarot());
                            fs.Write(bytes, 0, bytes.Length);
                        }

                    }
                    else
                    {
                        strOutFilePath = strdirectoryName + @"\" + strFileName + ".DAT";
                        using (FileStream fs = new FileStream(strOutFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(dt.ToDatDelimiter());
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
        }


        //public StringBuilder CreateOPTFile(DataTable dt, string strDBPath)
        //{
        //    //Trying to change the way the output is reported - 11/09/2013
        //    StringBuilder sbStatus = new StringBuilder();

        //    //Create new DataTable to add the items to output the OPT 
        //    DataTable dtTemp = new DataTable();
        //    dtTemp.Columns.Add("ImageTag", typeof(string)); //ImgTag
        //    dtTemp.Columns.Add("Volume", typeof(string));  //Set this to NULL - not needed
        //    dtTemp.Columns.Add("ImagePath", typeof(string));
        //    dtTemp.Columns.Add("NewPage", typeof(string));
        //    dtTemp.Columns.Add("Hist1", typeof(string));
        //    dtTemp.Columns.Add("Hist2", typeof(string));
        //    dtTemp.Columns.Add("NumberOfPages", typeof(string));

        //    //Make an anonymous datatype to reduce the column count and make this more workable
        //    var imgInfoQuery = (dt.AsEnumerable()
        //        .Select(s => new
        //        {
        //            ImgTag = s.Field<string>("ImgTag"),
        //            DefDir = s.Field<string>("DefDir"),
        //            Pages = s.Field<Int32>("Pages"),
        //            ImgFiles = s.Field<string>("ImgFiles")

        //        })).ToArray();

        //    foreach (var item in imgInfoQuery)
        //    {
        //        //Use these for all our local evaluations and assignments
        //        string strImgTag = string.Empty;
        //        string strDefDir = string.Empty;
        //        int intPages = 0;
        //        //This is needed for splitting the entries correctly within the ImgFiles column
        //        string[] strImgFiles = { };
        //        //These are used to assembly the necessary file structure we need to construct the OPT file records
        //        string strImgTempFileName = string.Empty;
        //        string strImgTempFileExt = string.Empty;
        //        int intIndexLeftBracket = 0;
        //        int intIndexRightBracket = 0;
        //        string strBracketContents = string.Empty;
        //        int intImgFileUBound = 0;
        //        int intImgFileLBound = 0;
        //        int intSeparatorIndex = 0;


        //        //Create a new row in the data table
        //        DataRow dr;

        //        //Preserve the Image Tag
        //        strImgTag = item.ImgTag;

        //        //Replace the @I in the DefDir column if it exists
        //        //The ImagePath is the third column in the data table and can have one of three possibilities
        //        if (item.DefDir.Contains("@I") && item.DefDir.Length < 3)
        //        {
        //            strDefDir = strDBPath;
        //        }
        //        else if (item.DefDir.Contains("@I") && item.DefDir.Length > 3)
        //        {
        //            //Remove the @I and replace the char string with the full path and concatenate with rest of file path
        //            strDefDir = item.DefDir.Replace(@"@I\", string.Empty);
        //            strDefDir = strDBPath + strDefDir;
        //        }
        //        else
        //        {
        //            //Otherwise, leave the DefDir as it is because it will contain a full, valid path
        //            strDefDir = item.DefDir;
        //        }

        //        //Grab the page count
        //        intPages = item.Pages;

        //        //Take care of the ImgFiles column.  This is used to split the entries into individual elements
        //        strImgFiles = item.ImgFiles.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //        int intTestCount = 0;
        //        //This is going to be used for incrementing the image tag's count and will appended to it by an underscore + number (example: "_2")
        //        int intImgTagIncrement = 1;
        //        //We'll need another counter in case the Summation record has two entries with bracketed number sets so the increment happens properly
        //        //as opposed to duplicating the page count and having the "Y" flag set twice
        //        int intBracketEntry = 0;

        //        foreach (string file in strImgFiles)
        //        {

        //            //This is if the element contains the following format:  ABC000{1-9}.TIF
        //            if (file.Contains("{") | file.Contains("}"))
        //            {
        //                intIndexLeftBracket = file.IndexOf("{");
        //                intIndexRightBracket = file.IndexOf("}");
        //                if (intIndexLeftBracket == -1 || intIndexRightBracket == -1)
        //                {

        //                    return sbStatus.AppendFormat("The following file is improperly formatted: {0}", file);
        //                    //sbStatus.AppendLine();
        //                    //bgWorkerMain.ReportProgress(0, sbStatus);

        //                }
        //                else
        //                {
        //                    strBracketContents = file.Substring(intIndexLeftBracket + 1, intIndexRightBracket - intIndexLeftBracket - 1);
        //                    intSeparatorIndex = strBracketContents.IndexOf("-");
        //                    if (intSeparatorIndex == -1)
        //                    {
        //                        return sbStatus.AppendFormat("The following file is improperly formatted: {0}", file);
        //                        //sbStatus.AppendLine();
        //                        //bgWorkerMain.ReportProgress(0, sbStatus);

        //                    }
        //                    else
        //                    {
        //                        //This is the correct condition in which we'll format the string properly to form the file path
        //                        intImgFileLBound = Convert.ToInt32(strBracketContents.Substring(0, strBracketContents.Length - intSeparatorIndex - 1));
        //                        intImgFileUBound = Convert.ToInt32(strBracketContents.Substring(intSeparatorIndex + 1, strBracketContents.Length - intSeparatorIndex - 1));
        //                        strImgTempFileName = file.Substring(0, file.Length - (file.Length - intIndexLeftBracket));
        //                        strImgTempFileExt = file.Substring(intIndexRightBracket + 1, file.Length - (intIndexRightBracket + 1));
        //                        intBracketEntry++;
        //                        //After we get the lower and upper boundaries of what's in between brackets, we need to assemble the file path
        //                        for (int i = intImgFileLBound; i <= intImgFileUBound; i++)
        //                        {
        //                            //This is for the first pass.  We need to lay out whether or not this is a new document set and the number of pages
        //                            if (i == intImgFileLBound && intBracketEntry == 1)
        //                            {
        //                                //This will give us the first record in a set of files
        //                                dr = dtTemp.NewRow();
        //                                dr["ImageTag"] = strImgTag; //Set the first column to the image tag
        //                                dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
        //                                dr["ImagePath"] = strDefDir + strImgTempFileName + i.ToString() + strImgTempFileExt; //Set the image path
        //                                dr["NewPage"] = "Y"; //Record whether or not this is a new page
        //                                dr["Hist1"] = string.Empty; //Leave empty
        //                                dr["Hist2"] = string.Empty; //:eave empty
        //                                dr["NumberOfPages"] = intPages.ToString(); //Give the number of pages in the set
        //                                dtTemp.Rows.Add(dr); //Add the row
        //                            }
        //                            else
        //                            {
        //                                //Increment the image tag extension and then add it to the image tag portion of the file
        //                                intImgTagIncrement++;
        //                                //This will give us subsequent records in the file that pertain to the same document set
        //                                dr = dtTemp.NewRow();
        //                                dr["ImageTag"] = strImgTag + "_" + intImgTagIncrement.ToString(); //Set the first column to the image tag
        //                                dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
        //                                dr["ImagePath"] = strDefDir + strImgTempFileName + i.ToString() + strImgTempFileExt; //Set the image path
        //                                dr["NewPage"] = string.Empty; //New page is not needed in this iteration
        //                                dr["Hist1"] = string.Empty; //Leave empty
        //                                dr["Hist2"] = string.Empty; //Leave empty
        //                                dr["NumberOfPages"] = string.Empty; //Pages can be left empty that pertain to the subsequent entries in the file
        //                                dtTemp.Rows.Add(dr); //Add the row
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //This area will handle all the other file formats in the image files column

        //                //This is for the first pass.  We need to lay out whether or not this is a new document set and the number of pages
        //                if (intTestCount == 0)
        //                {
        //                    //This will give us the first record in a set of files
        //                    dr = dtTemp.NewRow();
        //                    dr["ImageTag"] = strImgTag; //Set the first column to the image tag
        //                    dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
        //                    dr["ImagePath"] = strDefDir + file; //Set the image path
        //                    dr["NewPage"] = "Y"; //Record whether or not this is a new page
        //                    dr["Hist1"] = string.Empty; //Leave empty
        //                    dr["Hist2"] = string.Empty; //:eave empty
        //                    dr["NumberOfPages"] = intPages.ToString(); //Give the number of pages in the set
        //                    dtTemp.Rows.Add(dr); //Add the row
        //                }
        //                else
        //                {
        //                    //Increment the image tag extension and then add it to the image tag portion of the file
        //                    intImgTagIncrement++;
        //                    //This will give us subsequent records in the file that pertain to the same document set
        //                    dr = dtTemp.NewRow();
        //                    dr["ImageTag"] = strImgTag + "_" + intImgTagIncrement.ToString(); //Set the first column to the image tag
        //                    dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
        //                    dr["ImagePath"] = strDefDir + file; //Set the image path
        //                    dr["NewPage"] = string.Empty; //New page is not needed in this iteration
        //                    dr["Hist1"] = string.Empty; //Leave empty
        //                    dr["Hist2"] = string.Empty; //Leave empty
        //                    dr["NumberOfPages"] = string.Empty; //Pages can be left empty that pertain to the subsequent entries in the file
        //                    dtTemp.Rows.Add(dr); //Add the row
        //                }

        //                if (strImgFiles.Count() > 1)
        //                {
        //                    intTestCount++;
        //                }

        //            }
        //        }
        //    }
        //    return sbStatus.AppendLine("OPT Process Completed");
        //}
    }
}
