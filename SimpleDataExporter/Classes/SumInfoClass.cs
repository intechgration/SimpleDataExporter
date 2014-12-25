using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace SimpleDataExporter.Classes
{
    class SumInfoClass : IDisposable
    {
        private dynamic _swObj;

        public string GetProductName()
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            string strsumProductName = _swObj.GetProductName();
            if (_swObj != null)
            {
                Dispose(true);
            }

            return strsumProductName;

        }

        public SortedDictionary<string, string> ReturnCasePathInfo(string strCaseName)
        {

            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            SortedDictionary<string, string> sdSumInfo = new SortedDictionary<string, string>();
            if (_swObj.CurrentCase.Reopen() == true)
            {
                _swObj.CaseOpen(strCaseName);
                sdSumInfo.Add("CoreDB", _swObj.CurrentCase.CoreDBPath());
                sdSumInfo.Add("NoteDB", _swObj.CurrentCase.NotePath());
                sdSumInfo.Add("ImageDir", _swObj.CurrentCase.ImageDir());
                sdSumInfo.Add("TransDir", _swObj.CurrentCase.TranscriptDir());
            }
            else
            {
                _swObj.CaseOpen(strCaseName);
                sdSumInfo.Add("CoreDB", _swObj.CurrentCase.CoreDBPath());
                sdSumInfo.Add("NoteDB", _swObj.CurrentCase.NotePath());
                sdSumInfo.Add("ImageDir", _swObj.CurrentCase.ImageDir());
                sdSumInfo.Add("TransDir", _swObj.CurrentCase.TranscriptDir());
            }

            if (_swObj != null)
            {
                Dispose(true);
            }
            return sdSumInfo;

        }

        public int GetTranscriptFileCount(string strTransPath)
        {

            int intFileCount = Directory.GetFiles(strTransPath, "*.txt", SearchOption.TopDirectoryOnly).Length;
            return intFileCount;

        }

        public string[] GetTranscriptFileList(string strTransPath)
        {

            var strFileArray = (Directory.GetFiles(strTransPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".trn", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".dep", StringComparison.OrdinalIgnoreCase))).ToArray();

            return strFileArray;
        }

        public List<string> GetOCRDocIdList()
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");

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

            if (_swObj != null)
            {
                Dispose(true);
            }

            return lstOCRCaseDocs;
        }

        public string GetTranscriptDescription(string strTransFileName)
        {
            string strTranscriptDescription = String.Empty;

            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");

            //Open the transcript to get the description
            _swObj.ShowTestimony(strTransFileName, 1);

            //Get the active view
            dynamic oTranscript = _swObj.Views.ActiveView();

            //Get the transcript description
            strTranscriptDescription = oTranscript.Description();

            if (_swObj != null)
            {
                Dispose(true);
            }

            return strTranscriptDescription;
        }

        public object GetCoreOCRDoc(string strOCRDoc)
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            object oOCRDocContents = _swObj.GetCoreOcrDoc(strOCRDoc.Trim());
            if (_swObj != null)
            {
                Dispose(true);
            }
            return oOCRDocContents;
        }

        public void ResetWorkspaceLayout()
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            _swObj.Views.Layout.Reset();
            if (_swObj != null)
            {
                Dispose(true);
            }
        }

        public string[] GetEMBDirContents()
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            string strEMBRootDir = _swObj.EMBRootDir();
            if (_swObj != null)
            {
                Dispose(true);
            }
            string[] strFileList = Directory.GetFiles(strEMBRootDir, "*.*", SearchOption.AllDirectories);
            return strFileList;

        }

        public string GetAllProfilesDirectoryPath()
        {
            dynamic _swObj = Marshal.GetActiveObject("Summation.Application");
            string strAllProfilesDir = _swObj.CurrentCase.SharedProfileDir();
            if (_swObj != null)
            {
                Dispose(true);
            }

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
