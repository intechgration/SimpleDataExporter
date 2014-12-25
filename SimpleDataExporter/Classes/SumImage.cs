using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using BitMiracle.LibTiff.Classic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleDataExporter.Classes
{
    public class SumImage
    {
        private StringBuilder _status;
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

        public SumImage()
        {
            if (_status == null)
            {
                _status = new StringBuilder();
            }
        }

        public DataTable CreateOPTFile(DataTable dt, string strDBPath)
        {

            //Create new DataTable to add the items to output the OPT 
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("ImageTag", typeof(string)); //ImgTag
            dtTemp.Columns.Add("Volume", typeof(string));  //Set this to NULL - not needed
            dtTemp.Columns.Add("ImagePath", typeof(string));
            dtTemp.Columns.Add("NewPage", typeof(string));
            dtTemp.Columns.Add("Hist1", typeof(string));
            dtTemp.Columns.Add("Hist2", typeof(string));
            dtTemp.Columns.Add("NumberOfPages", typeof(string));

            //Make an anonymous datatype to reduce the column count and make this more workable
            var imgInfoQuery = (dt.AsEnumerable()
                .Select(s => new
                {
                    ImgTag = s.Field<string>("ImgTag"),
                    DefDir = s.Field<string>("DefDir"),
                    Pages = s.Field<Int32>("Pages"),
                    ImgFiles = s.Field<string>("ImgFiles")

                })).ToArray();

            foreach (var item in imgInfoQuery)
            {
                //Use these for all our local evaluations and assignments
                string strImgTag = string.Empty;
                string strDefDir = string.Empty;
                int intPages = 0;
                //This is needed for splitting the entries correctly within the ImgFiles column
                string[] strImgFiles = { };
                //These are used to assembly the necessary file structure we need to construct the OPT file records
                string strImgTempFileName = string.Empty;
                string strImgTempFileExt = string.Empty;
                int intIndexLeftBracket = 0;
                int intIndexRightBracket = 0;
                string strBracketContents = string.Empty;
                int intImgFileUBound = 0;
                int intImgFileLBound = 0;
                int intSeparatorIndex = 0;


                //Create a new row in the data table
                DataRow dr;

                //Preserve the Image Tag
                strImgTag = item.ImgTag;

                //Replace the @I in the DefDir column if it exists
                //The ImagePath is the third column in the data table and can have one of three possibilities
                if (item.DefDir.Contains("@I") && item.DefDir.Length < 3)
                {
                    strDefDir = strDBPath;
                }
                else if (item.DefDir.Contains("@I") && item.DefDir.Length > 3)
                {
                    //Remove the @I and replace the char string with the full path and concatenate with rest of file path
                    strDefDir = item.DefDir.Replace(@"@I\", string.Empty);
                    strDefDir = strDBPath + strDefDir;
                }
                else
                {
                    //Otherwise, leave the DefDir as it is because it will contain a full, valid path
                    strDefDir = item.DefDir;
                }

                //Grab the page count
                intPages = item.Pages;

                //Take care of the ImgFiles column.  This is used to split the entries into individual elements
                strImgFiles = item.ImgFiles.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                int intTestCount = 0;
                //This is going to be used for incrementing the image tag's count and will appended to it by an underscore + number (example: "_2")
                int intImgTagIncrement = 1;
                //We'll need another counter in case the Summation record has two entries with bracketed number sets so the increment happens properly
                //as opposed to duplicating the page count and having the "Y" flag set twice
                int intBracketEntry = 0;

                foreach (string file in strImgFiles)
                {

                    //This is if the element contains the following format:  ABC000{1-9}.TIF
                    if (file.Contains("{") | file.Contains("}"))
                    {
                        intIndexLeftBracket = file.IndexOf("{");
                        intIndexRightBracket = file.IndexOf("}");
                        if (intIndexLeftBracket == -1 || intIndexRightBracket == -1)
                        {
                            _status.AppendFormat("The following file is improperly formatted: {0}", file);
                            _status.AppendLine();
                            OnStatusUpdated(_status);



                        }
                        else
                        {
                            strBracketContents = file.Substring(intIndexLeftBracket + 1, intIndexRightBracket - intIndexLeftBracket - 1);
                            intSeparatorIndex = strBracketContents.IndexOf("-");
                            if (intSeparatorIndex == -1)
                            {
                                _status.AppendFormat("The following file is improperly formatted: {0}", file);
                                _status.AppendLine();
                                OnStatusUpdated(_status);

                            }
                            else
                            {
                                //This is the correct condition in which we'll format the string properly to form the file path
                                intImgFileLBound = Convert.ToInt32(strBracketContents.Substring(0, strBracketContents.Length - intSeparatorIndex - 1));
                                intImgFileUBound = Convert.ToInt32(strBracketContents.Substring(intSeparatorIndex + 1, strBracketContents.Length - intSeparatorIndex - 1));
                                strImgTempFileName = file.Substring(0, file.Length - (file.Length - intIndexLeftBracket));
                                strImgTempFileExt = file.Substring(intIndexRightBracket + 1, file.Length - (intIndexRightBracket + 1));
                                intBracketEntry++;
                                //After we get the lower and upper boundaries of what's in between brackets, we need to assemble the file path
                                for (int i = intImgFileLBound; i <= intImgFileUBound; i++)
                                {
                                    //This is for the first pass.  We need to lay out whether or not this is a new document set and the number of pages
                                    if (i == intImgFileLBound && intBracketEntry == 1)
                                    {
                                        //This will give us the first record in a set of files
                                        dr = dtTemp.NewRow();
                                        dr["ImageTag"] = strImgTag; //Set the first column to the image tag
                                        dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
                                        dr["ImagePath"] = strDefDir + strImgTempFileName + i.ToString() + strImgTempFileExt; //Set the image path
                                        dr["NewPage"] = "Y"; //Record whether or not this is a new page
                                        dr["Hist1"] = string.Empty; //Leave empty
                                        dr["Hist2"] = string.Empty; //:eave empty
                                        dr["NumberOfPages"] = intPages.ToString(); //Give the number of pages in the set
                                        dtTemp.Rows.Add(dr); //Add the row
                                    }
                                    else
                                    {
                                        //Increment the image tag extension and then add it to the image tag portion of the file
                                        intImgTagIncrement++;
                                        //This will give us subsequent records in the file that pertain to the same document set
                                        dr = dtTemp.NewRow();
                                        dr["ImageTag"] = strImgTag + "_" + intImgTagIncrement.ToString(); //Set the first column to the image tag
                                        dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
                                        dr["ImagePath"] = strDefDir + strImgTempFileName + i.ToString() + strImgTempFileExt; //Set the image path
                                        dr["NewPage"] = string.Empty; //New page is not needed in this iteration
                                        dr["Hist1"] = string.Empty; //Leave empty
                                        dr["Hist2"] = string.Empty; //Leave empty
                                        dr["NumberOfPages"] = string.Empty; //Pages can be left empty that pertain to the subsequent entries in the file
                                        dtTemp.Rows.Add(dr); //Add the row
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //This area will handle all the other file formats in the image files column
                        //MessageBox.Show("The file that is currently being processed is: " + file);

                        //This is for the first pass.  We need to lay out whether or not this is a new document set and the number of pages
                        if (intTestCount == 0)
                        {
                            //This will give us the first record in a set of files
                            dr = dtTemp.NewRow();
                            dr["ImageTag"] = strImgTag; //Set the first column to the image tag
                            dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
                            dr["ImagePath"] = strDefDir + file; //Set the image path
                            dr["NewPage"] = "Y"; //Record whether or not this is a new page
                            dr["Hist1"] = string.Empty; //Leave empty
                            dr["Hist2"] = string.Empty; //:eave empty
                            dr["NumberOfPages"] = intPages.ToString(); //Give the number of pages in the set
                            dtTemp.Rows.Add(dr); //Add the row
                        }
                        else
                        {
                            //Increment the image tag extension and then add it to the image tag portion of the file
                            intImgTagIncrement++;
                            //This will give us subsequent records in the file that pertain to the same document set
                            dr = dtTemp.NewRow();
                            dr["ImageTag"] = strImgTag + "_" + intImgTagIncrement.ToString(); //Set the first column to the image tag
                            dr["Volume"] = null; //Second column isn't needed so we'll just set it to null because it's the volume column
                            dr["ImagePath"] = strDefDir + file; //Set the image path
                            dr["NewPage"] = string.Empty; //New page is not needed in this iteration
                            dr["Hist1"] = string.Empty; //Leave empty
                            dr["Hist2"] = string.Empty; //Leave empty
                            dr["NumberOfPages"] = string.Empty; //Pages can be left empty that pertain to the subsequent entries in the file
                            dtTemp.Rows.Add(dr); //Add the row
                        }

                        if (strImgFiles.Count() > 1)
                        {
                            intTestCount++;
                        }

                    }
                }
            }
            return dtTemp;
        }

        public DataTable CreateLFPFile(DataTable dt, string strDBPath, string VolumeName)
        {

            //Create new DataTable to add the items to output the LFP 
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("CommandFunction", typeof(string)); //Command Function
            dtTemp.Columns.Add("ImageTag", typeof(string)); //ImgTag
            dtTemp.Columns.Add("NewDocument", typeof(string));
            dtTemp.Columns.Add("PageNumber", typeof(string));
            dtTemp.Columns.Add("Volume", typeof(string));  //Will be needed and can be customized
            dtTemp.Columns.Add("ImagePath", typeof(string));
            dtTemp.Columns.Add("FileName", typeof(string));
            dtTemp.Columns.Add("FileFormatType", typeof(string));
            dtTemp.Columns.Add("Rotation", typeof(string));

            //Make an anonymous datatype to reduce the column count and make this more workable
            var imgInfoQuery = (dt.AsEnumerable()
                .Select(s => new
                {
                    ImgTag = s.Field<string>("ImgTag"),
                    DefDir = s.Field<string>("DefDir"),
                    Pages = s.Field<Int32>("Pages"),
                    ImgFiles = s.Field<string>("ImgFiles")

                })).ToArray();


            foreach (var item in imgInfoQuery)
            {

                //Use these for all our local evaluations and assignments
                string strImgTag = string.Empty;
                string strDefDir = string.Empty;
                int intPages = 0;

                //This is needed for splitting the entries correctly within the ImgFiles column
                string[] ImgFiles = { };
                //These are used to assembly the necessary file structure we need to construct the LFP file records
                string ImgTempFileName = string.Empty;
                string ImgTempFileExt = string.Empty;
                int intIndexLeftBracket = 0;
                int intIndexRightBracket = 0;
                string strBracketContents = string.Empty;
                int intImgFileUBound = 0;
                int intImgFileLBound = 0;
                int intSeparatorIndex = 0;
                int pageCount = 0;


                //Create a new row in the data table
                DataRow dr;

                //Preserve the Image Tag
                strImgTag = item.ImgTag;

                //Replace the @I in the DefDir column if it exists
                //The ImagePath is the third column in the data table and can have one of three possibilities
                if (item.DefDir.Contains("@I") && item.DefDir.Length < 3)
                {
                    strDefDir = strDBPath;
                }
                else if (item.DefDir.Contains("@I") && item.DefDir.Length > 3)
                {
                    //Remove the @I and replace the char string with the full path and concatenate with rest of file path
                    strDefDir = item.DefDir.Replace(@"@I\", string.Empty);
                    strDefDir = strDBPath + strDefDir;
                }
                else
                {
                    //Otherwise, leave the DefDir as it is because it will contain a full, valid path
                    strDefDir = item.DefDir;
                }

                //Grab the page count
                intPages = item.Pages;

                //Take care of the ImgFiles column.  This is used to split the entries into individual elements
                ImgFiles = item.ImgFiles.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //This is going to be used for incrementing the image tag's count and will appended to it by an underscore + number (example: "_2")
                int ImgTagIncrement = 1;

                //We'll need another counter in case the Summation record has two entries with bracketed number sets so the increment happens properly
                //as opposed to duplicating the page count and having the "Y" flag set twice
                int IndexBracketEntry = 0;

                foreach (string file in ImgFiles)
                {

                    if (!file.Contains("{") | !file.Contains("}"))
                    {
                        //This section is used for single entry files that have no brackets. 
                        //Example: 015-0002.tif
                        //This will give us the first record in a set of files

                        //Test for page count first.  If the page count is greater than one, we'll need to iterate through them
                        if (Path.GetExtension(file).ToLower() == ".pdf" || Path.GetExtension(file).ToLower() == ".tif")
                        {
                            pageCount = GetPageCount(strDefDir + file);
                        }
                        else
                        {
                            pageCount = 0;
                        }

                        //This code is for single page tif's.  We're expecting the page count to be 1 only.
                        if (pageCount == 1)
                        {
                            dr = dtTemp.NewRow();
                            dr["CommandFunction"] = "IM";
                            dr["ImageTag"] = strImgTag; //Set the first column to the image tag
                            dr["NewDocument"] = "D"; //Record whether or not this is a new document
                            dr["PageNumber"] = "0"; //If this is a single page TIF, this needs to be zero.
                            dr["Volume"] = VolumeName; //Second column isn't needed so we'll just set it to null because it's the volume column
                            dr["ImagePath"] = strDefDir; //Set the image path
                            dr["FileName"] = file;
                            dr["FileFormatType"] = GetImageFileFormatType(Path.GetExtension(file)); //Return the file format type
                            dr["Rotation"] = "0";
                            dtTemp.Rows.Add(dr); //Add the row
                        }
                        else
                        {
                            //This is where we handle all other multipage tif's
                            for (int i = 1; i < pageCount + 1; i++)
                            {
                                dr = dtTemp.NewRow();
                                dr["CommandFunction"] = "IM";
                                dr["ImageTag"] = strImgTag; //Set the first column to the image tag

                                //Record whether or not this is a new document.  
                                //If it's a new document, the first entry will be a "D" and all subsequent page entries will be blank for this swObj
                                dr["NewDocument"] = (i == 1) ? "D" : string.Empty;

                                dr["PageNumber"] = i; //If this is a single page TIF, this needs to be zero.
                                dr["Volume"] = VolumeName; //Second column isn't needed so we'll just set it to null because it's the volume column
                                dr["ImagePath"] = strDefDir; //Set the image path
                                dr["FileName"] = file;
                                dr["FileFormatType"] = GetImageFileFormatType(Path.GetExtension(file)); //Return the file format type
                                dr["Rotation"] = "0";
                                dtTemp.Rows.Add(dr); //Add the row
                            }
                        }
                    }
                    else
                    {
                        //This is if the element contains the following format:  ABC000{1-9}.TIF
                        intIndexLeftBracket = file.IndexOf("{");
                        intIndexRightBracket = file.IndexOf("}");
                        if (intIndexLeftBracket == -1 || intIndexRightBracket == -1)
                        {
                            _status.AppendFormat("The following file is improperly formatted: {0}", file);
                            _status.AppendLine();
                            OnStatusUpdated(_status);
                        }
                        else
                        {
                            strBracketContents = file.Substring(intIndexLeftBracket + 1, intIndexRightBracket - intIndexLeftBracket - 1);
                            intSeparatorIndex = strBracketContents.IndexOf("-");
                            if (intSeparatorIndex == -1)
                            {
                                _status.AppendFormat("The following file is improperly formatted: {0}", file);
                                _status.AppendLine();
                                OnStatusUpdated(_status);
                            }
                            else
                            {
                                //This is the correct condition in which we'll format the string properly to form the file path
                                intImgFileLBound = Convert.ToInt32(strBracketContents.Substring(0, strBracketContents.Length - intSeparatorIndex - 1));
                                intImgFileUBound = Convert.ToInt32(strBracketContents.Substring(intSeparatorIndex + 1, strBracketContents.Length - intSeparatorIndex - 1));
                                ImgTempFileName = file.Substring(0, file.Length - (file.Length - intIndexLeftBracket));
                                ImgTempFileExt = Path.GetExtension(file);
                                IndexBracketEntry++;
                                //After we get the lower and upper boundaries of what's in between brackets, we need to assemble the file path
                                for (int i = intImgFileLBound; i <= intImgFileUBound; i++)
                                {
                                    //This is for the first pass.  We need to lay out whether or not this is a new document set and the number of pages
                                    if (i == intImgFileLBound && IndexBracketEntry == 1)
                                    {
                                        //This will give us the first record in a set of files
                                        dr = dtTemp.NewRow();
                                        dr["CommandFunction"] = "IM";
                                        dr["ImageTag"] = strImgTag; //Set the first column to the image tag
                                        dr["NewDocument"] = "D"; //Record whether or not this is a new document
                                        dr["PageNumber"] = "0"; //If this is a single page TIF, this needs to be zero.
                                        dr["Volume"] = VolumeName; //Second column isn't needed so we'll just set it to null because it's the volume column
                                        dr["ImagePath"] = strDefDir; //Set the image path
                                        dr["FileName"] = ImgTempFileName + i.ToString() + ImgTempFileExt;
                                        dr["FileFormatType"] = GetImageFileFormatType(Path.GetExtension(file)); //Return the file format type
                                        dr["Rotation"] = "0";
                                        dtTemp.Rows.Add(dr); //Add the row
                                    }
                                    else
                                    {
                                        //Increment the image tag extension and then add it to the image tag portion of the file
                                        ImgTagIncrement++;
                                        //This will give us subsequent records in the file that pertain to the same document set
                                        dr = dtTemp.NewRow();
                                        dr["CommandFunction"] = "IM";
                                        dr["ImageTag"] = strImgTag + "_" + ImgTagIncrement.ToString(); //Set the first column to the image tag
                                        dr["NewDocument"] = string.Empty; //Record whether or not this is a new document
                                        dr["PageNumber"] = "0"; //If this is a single page TIF, this needs to be zero.
                                        dr["Volume"] = VolumeName; //Second column isn't needed so we'll just set it to null because it's the volume column
                                        dr["ImagePath"] = strDefDir; //Set the image path
                                        dr["FileName"] = ImgTempFileName + i.ToString() + ImgTempFileExt;
                                        dr["FileFormatType"] = GetImageFileFormatType(Path.GetExtension(file)); //Return the file format type
                                        dr["Rotation"] = "0";
                                        dtTemp.Rows.Add(dr); //Add the row
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dtTemp;
        }

        public string GetImageFileFormatType(string FileExtensionType)
        {

            switch (FileExtensionType.ToLower())
            {
                case ".img":
                    FileExtensionType = "1";
                    break;
                case ".tif":
                case ".tiff":
                    FileExtensionType = "2";
                    break;
                case ".stf":
                    FileExtensionType = "3";
                    break;
                case ".bmp":
                case ".pcx":
                case ".jpeg":
                case ".png":
                    FileExtensionType = "4";
                    break;
                case ".pdf":
                    FileExtensionType = "5";
                    //We'll need to evaluate whether or not the PDF is color or b/w
                    break;
            }

            return FileExtensionType;
        }


        public int GetPageCount(string File)
        {
            int pageCount = 0;
            switch (Path.GetExtension(File).ToLower())
            {
                case ".pdf":
                    PdfReader pdf = new PdfReader(File);
                    pageCount = pdf.NumberOfPages;
                    pdf.Close();
                    break;
                case ".tif":
                case ".tiff":
                    using (Tiff image = Tiff.Open(File, "r"))
                    {
                        pageCount = image.NumberOfDirectories();
                    }
                    break;
            }

            return pageCount;
        }

        public string CreateLPFVolumeIdentifierLine(string volumeName)
        {
            //Construct the volume name and determine whether or not the custom volume name is being used.
            if (volumeName == string.Empty)
            {
                volumeName = "VOL001";
            }
            StringBuilder directions = new StringBuilder();
            directions.AppendLine("Below is the output from the ImgInfo table in Summation.");
            directions.AppendLine("Please replace the text <Insert your volume path here> with your own volume path ");
            directions.AppendLine("and remember to remove the volume path from the full path in the lines below the VN line. ");
            directions.AppendLine();
            string vnLine = directions.ToString() + "VN" + "," + volumeName + "," + "<Insert your volume path here>" + "," + "99" + "\r\n";
            return vnLine;
        }
    }
}
