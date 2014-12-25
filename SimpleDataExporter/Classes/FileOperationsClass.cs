using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Data;
using SimpleDataExporter.Classes;

namespace SimpleDataExporter.Classes
{
    public class FileOperationsClass
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

        public FileOperationsClass()
        {
            if (_status == null)
            {
                _status = new StringBuilder();
            }
        }



        internal static void WriteToLogFile(TextBox txtOutputPath, TextBox txtOutputSummary)
        {
            string strFileName = DateTime.Now.ToString("o") + ".txt";
            string strFilePath = txtOutputPath.Text + @"\" + strFileName.Replace(":", "_");
            File.WriteAllText(strFilePath, txtOutputSummary.Text);
        }

        internal static bool FindDirectory(string outputPath, string strCaseFriendlyName, bool overwriteOptionChecked)
        {
            bool blnDirectoryFound = false;
            //CheckBox tempCheckBox = (CheckBox)checkBox;
            //bool overwriteOptionChecked = tempCheckBox.Checked;

            string strdirectoryName = outputPath + @"\" + strCaseFriendlyName;

            if ((strdirectoryName.Length > 0) && (!Directory.Exists(strdirectoryName)) && overwriteOptionChecked == false)
            {
                blnDirectoryFound = false;
            }
            else if ((strdirectoryName.Length > 0) && (!Directory.Exists(strdirectoryName)) && overwriteOptionChecked == true)
            {
                blnDirectoryFound = false;
            }
            else if ((strdirectoryName.Length > 0) && (Directory.Exists(strdirectoryName)) && overwriteOptionChecked == true)
            {

                //perform overwrite of directory and report to output summary box
                Thread thread = new Thread(delegate() { DeleteDirectory(strdirectoryName); });
                thread.Start();
                thread.Join();

                while (thread.IsAlive == true)
                {
                    Thread.Sleep(1000);
                }

                blnDirectoryFound = false;
            }
            else
            {
                //directory does exist, therefore, skip the directory.
                blnDirectoryFound = true;
            }

            return blnDirectoryFound;
        }

        internal static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        internal static string CreateOutputDirectory(string outputPath, string caseFriendlyName, string dbType)
        {
            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = outputPath + @"\" + caseFriendlyName + @"\" + dbType;

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

            return directoryName;
        }

        internal static void OutputTableToFile(DataTable table, string outputDirectory, string caseFriendlyName, string DBType, string tableName, bool DelimiterTypeYesNo)
        {
            //Declare a string for the outfile path.  We need to take into consideration the difference between 
            string outFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, DBType);

            if (DelimiterTypeYesNo == true)
            {
                outFilePath = directoryName + @"\" + tableName + ".TXT";
                using (FileStream fs = new FileStream(outFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(table.ToPipeCarot());
                    fs.Write(bytes, 0, bytes.Length);
                }

            }
            else
            {
                outFilePath = directoryName + @"\" + tableName + ".DAT";
                using (FileStream fs = new FileStream(outFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(table.ToDatDelimiter());
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void OutputIMGTableToFile(DataTable table, string outputDirectory, string caseFriendlyName, string DBType, string tableName, string imageFileType,
            bool customVolumeNameYesNo, string customVolumeName)
        {
            //Declare a string for the outfile path.  We need to take into consideration the difference between 
            string outFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(outputDirectory, caseFriendlyName, DBType);

            switch (imageFileType)
            {
                case "OPT":
                    outFilePath = directoryName + @"\" + tableName + "." + imageFileType;
                    _status.AppendFormat("Creating image file type: {0}", imageFileType);
                    _status.AppendLine();
                    OnStatusUpdated(_status);

                    using (FileStream fs = new FileStream(outFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                    {

                        var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(table.ToOPT());
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    break;
                case "LFP":
                    outFilePath = directoryName + @"\" + tableName + "." + imageFileType;
                    _status.AppendFormat("Creating image file type: {0}", imageFileType);
                    _status.AppendLine();
                    using (FileStream fs = new FileStream(outFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        //New additions in this version of the SLFC to add VN line to the output.
                        //This will be changed in the future for greater flexibility

                        string volumeName = string.Empty;
                        if (customVolumeNameYesNo == true)
                        {
                            volumeName = customVolumeName;
                        }
                        string volumeNameInformation = CreateLPFVolumeIdentifierLine(volumeName);
                        string outputFile = volumeNameInformation + table.ToLFP();
                        var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(outputFile);
                        fs.Write(bytes, 0, bytes.Length);
                        break;
                    }
            }

        }

        private static string CreateLPFVolumeIdentifierLine(string volumeName)
        {
            //Construct the volume name and determine whether or not the custom volume name is being used.
            //If there's no custom volume name, then we'll just create a default
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

        public static void WriteAllLines(string outputFilePath, string stringToBeWritten)
        {
            File.WriteAllText(outputFilePath, stringToBeWritten);
        }


        public static void WriteToFile(string outputFile, string fileName)
        {
            File.WriteAllText(outputFile, fileName);
        }


    }


}
