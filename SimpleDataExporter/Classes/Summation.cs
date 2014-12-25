using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SimpleDataExporter
{
    public class Summation : IDisposable
    {
        private dynamic _swObj;

        public string GetSystemInformation()
        {
            StringBuilder systemInformation = new StringBuilder();
            systemInformation.AppendLine();
            systemInformation.Append("System Information");
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Blaze Date: {0}", _swObj.SystemInfo.BlazeDate().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Blaze Name: {0}", _swObj.SystemInfo.BlazeName().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Blaze Type: {0}", _swObj.SystemInfo.BlazeType().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Blaze Version: {0}", _swObj.SystemInfo.BlazeVersion().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Blaze Version Month: {0}", _swObj.SystemInfo.BlazeVersionMonth().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Disk Free: {0}", _swObj.SystemInfo.DiskFreeString().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Engine Name: {0}", _swObj.SystemInfo.EngineName().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Engine Type: {0}", _swObj.SystemInfo.EngineType().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Memory Available: {0}", _swObj.SystemInfo.MemoryAvailable().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Net Name: {0}", _swObj.SystemInfo.NetName().ToString());
            systemInformation.AppendLine();
            systemInformation.AppendFormat("Serial Number: {0}", _swObj.SystemInfo.SerialNumber().ToString());
            systemInformation.AppendLine();

            return systemInformation.ToString(); 
        }

        public Summation(dynamic swObj)
        {
            if (_swObj == null)
            {
                InitializeSummationSession(_swObj);
            }
        }

        internal dynamic InitializeSummationSession(dynamic swObj)
        {
            //This is what we need to use for VS2013 to work properly
            Type type = Type.GetTypeFromProgID("Summation.Application");
            _swObj = Activator.CreateInstance(type);
            return _swObj;

        }

        public SortedDictionary<string, string> ReturnCasePathInfo(string caseName)
        {           
            SortedDictionary<string, string> sumInfoSorted = new SortedDictionary<string, string>();
            _swObj.CaseOpen(caseName);
            sumInfoSorted.Add("CoreDB", _swObj.CurrentCase.CoreDBPath());
            sumInfoSorted.Add("NoteDB", _swObj.CurrentCase.NotePath());
            sumInfoSorted.Add("ImageDir", _swObj.CurrentCase.ImageDir());
            sumInfoSorted.Add("TransDir", _swObj.CurrentCase.TranscriptDir());
            sumInfoSorted.Add("AllProfilesDir", _swObj.CurrentCase.SharedProfileDir());

            return sumInfoSorted;

        }

        public List<string> GetOCRDocIdList()
        {
            //Declare a list to store the DocId's of that pertain to the OCR documents
            List<string> lstOCRCaseDocs = new List<string>();
            //Declare a string for reading the contents of the file in line by line
            string strReadLine = string.Empty;
            //Get the core DB path since that's where the OCR base will reside also
            string strCoreDBPath = _swObj.CurrentCase.CoreDBPath();
            //Determine whether or not the OCR base exists next
            string strOCRBaseFile = strCoreDBPath + ".ftx";
            if (File.Exists(strOCRBaseFile))
            {

                using (StreamReader sr = new StreamReader(strOCRBaseFile))
                {
                    while (!sr.EndOfStream)
                    {
                        strReadLine = sr.ReadLine();
                        if (strReadLine.Contains("<<X>> Descrip="))
                        {
                            lstOCRCaseDocs.Add(strReadLine.Substring(14, strReadLine.Length - 14));
                        }
                    }
                }
            }

            return lstOCRCaseDocs;
        }

       

        public object GetCoreOCRDoc(string strOCRDoc)
        {
           
            object oOCRDocContents = _swObj.GetCoreOcrDoc(strOCRDoc.Trim());
          
            return oOCRDocContents;
        }

        public void ResetWorkspaceLayout()
        {           
            _swObj.Views.Layout.Reset();           
        }

        public string[] GetEMBDirContents()
        {
          
            string strEMBRootDir = _swObj.EMBRootDir();            
            string[] strFileList = Directory.GetFiles(strEMBRootDir, "*.*", SearchOption.AllDirectories);
            return strFileList;
        }

        public string GetAllProfilesDirectoryPath()
        {
            
            string strAllProfilesDir = _swObj.CurrentCase.SharedProfileDir();
            return strAllProfilesDir;
        }

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);

        }


        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {

            // free native resources if there are any.
            if (_swObj != null)
            {
                _swObj = null;

            }
        }
    }
}

