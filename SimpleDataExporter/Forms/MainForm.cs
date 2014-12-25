using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interop.Redemption;
using SimpleDataExporter.Classes;
using Redemption;

namespace SimpleDataExporter
{
    public partial class MainForm : Form
    {

        #region Global Variables

        //Make a list for the cases that will be processed
        private List<string> _casesToProcessList = new List<string>();

        //Declare global StringBuilder for showing status in the main report window
        private StringBuilder _status;

        //DataTable for obtaining schema information for the tables to be queried
        private DataTable _dbSchemaTable = new DataTable();

        //Global variable to track the number of tables processed
        private int _numberTablesProcessed = 0;

        //Global definition for Summation related objects
        private Summation _swObj;
        private SumImage _sumImage;
        private FileOperationsClass _fileOperations;
        private MailOperations _mailOperations;
        private Transcripts _transcripts;
        private string _connectionString;

        #endregion

        #region Main Form: Initialization
        public MainForm()
        {
            InitializeComponent();

            if (_status == null)
            {
                _status = new StringBuilder();
            }

            //Initialize the subscribers to the custom events
            _sumImage = new SumImage();
            _sumImage.StatusUpdated += new SumImage.UpdateStatusEventHandler(StatusUpdated);
            _fileOperations = new FileOperationsClass();
            _fileOperations.StatusUpdated += new FileOperationsClass.UpdateStatusEventHandler(StatusUpdated);
            _mailOperations = new MailOperations();
            _mailOperations.StatusUpdated += new MailOperations.UpdateStatusEventHandler(StatusUpdated);
            _transcripts = new Transcripts();
            _transcripts.StatusUpdated += new Transcripts.UpdateStatusEventHandler(StatusUpdated);

            //Set all text boxes to readonly upon initial loading of the application
            OutputPathTextBox.ReadOnly = true;
            OutputSummaryTextBox.ReadOnly = true;

            //Set the listbox buttons to disabled on load
            AddCaseButton.Enabled = false;
            RemoveCaseButton.Enabled = false;
            ClearAllCasesButton.Enabled = false;

            //Set the output, process, and cancel buttons to disabled on load
            OutputDirectoryButton.Enabled = false;
            OpenOutputDirectoryButton.Enabled = false;
            ProcessFilesButton.Enabled = false;
            CancelOperationButton.Enabled = false;

            //Set the processing status label on start-up to "Not started"
            SumProcessingBlnLabel.Text = "Not started";

            //Clear the global variables used to tracking the tables processed
            _numberTablesProcessed = 0;

            //Detect on start-up whether or not CT Summation is installed.  That way it's less likely this will blow up prior to running.
            Microsoft.Win32.RegistryKey subKey64Bit = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Summation Legal Technologies, Inc.\Blaze");
            Microsoft.Win32.RegistryKey subKey32Bit = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Summation Legal Technologies, Inc.\Blaze\Version");

            if (subKey32Bit != null | subKey64Bit != null)
            {
                SumInstalledBlnLabel.Text = "Yes";
                AddCaseButton.Enabled = true;
                RemoveCaseButton.Enabled = true;
                ClearAllCasesButton.Enabled = true;
            }
            else
            {
                SumInstalledBlnLabel.Text = "No";
                AddCaseButton.Enabled = false;
                RemoveCaseButton.Enabled = false;
                ClearAllCasesButton.Enabled = false;
            }

            DATRadioButton.Checked = true;
            LFPOutputRadioButton.Checked = true;
            DefaultVolNameRadioButton.Checked = true;
            LoadFileTypePanel.Enabled = false;
            VolumeNamePanel.Enabled = false;
        }
        #endregion

        #region Menu Strip
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Add cases for processing to listbox
        private void AddFilesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Case Info files (*.ci)|*.ci";
            DialogResult result = fileDialog.ShowDialog();
            List<String> caseList = CasesListBox.Items.OfType<string>().ToList();
            string strDirTemp = String.Empty;

            if (result == DialogResult.OK)
            {
                foreach (String file in fileDialog.FileNames)
                {
                    //Get the case friendly name which is the filename minus the .CI at the end and also removing the leading slashes from the file path
                    try
                    {
                        //Add the cases to a temporary list first
                        caseList.Add(Path.GetFileNameWithoutExtension(file));

                    }
                    catch (Exception ex)
                    {

                        _status.AppendFormat("There was a problem processing file: {0}.  Error message: {1}", file, ex.Message);
                        _status.AppendLine();
                        StatusUpdated(_status);
                    }
                    CheckCaseCount();
                }

                CasesListBox.Items.Clear();
                foreach (string distinctCase in caseList.Distinct())
                {
                    CasesListBox.Items.Add(distinctCase);
                }
                CheckCaseCount();
            }
        }
        #endregion

        #region Remove case from listbox
        private void RemoveCaseButton_Click(object sender, EventArgs e)
        {

            int SelItemCount = CasesListBox.SelectedItems.Count;

            for (int i = 0; i < SelItemCount; i++)
            {
                CasesListBox.Items.Remove(CasesListBox.SelectedItem);
            }
            CheckCaseCount();
        }
        #endregion

        #region Remove all cases from listbox
        private void ClearAllCasesButton_Click(object sender, EventArgs e)
        {
            CasesListBox.Items.Clear();
            CheckCaseCount();
        }
        #endregion

        #region Select output directory
        private void OutputDirectoryButton_Click(object sender, EventArgs e)
        {
            OutputDirectoryButton.Enabled = true;
            OpenOutputDirectoryButton.Enabled = true;
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Select output directory where cases will be exported to";
            folderBrowser.ShowNewFolderButton = true;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                OutputPathTextBox.Text = folderBrowser.SelectedPath;
            }

            //Make sure an output path exists so we can enable the buttons.  Otherwise, leave them disabled.
            if (OutputPathTextBox.Text != null)
            {
                ProcessFilesButton.Enabled = true;
                CancelOperationButton.Enabled = false;
            }
            else
            {
                ProcessFilesButton.Enabled = false;
                CancelOperationButton.Enabled = false;
            }
        }
        #endregion

        #region Start Processing
        private void btnProcessFiles_Click(object sender, EventArgs e)
        {
            //Ensure that there are cases and an output path to use
            if (CasesListBox.Items.Count == 0)  //Make sure there are cases to process before proceeding
            {
                MessageBox.Show("There are no cases to process.  Please provide cases to process", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (OutputPathTextBox.Text == String.Empty) //Make sure there are cases to process before proceeding
            {
                MessageBox.Show("Please provide an path to store the output files in", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {

                //Gather up the data in the rows, assemble it and pass it to the thread worker for processing
                foreach (string casename in CasesListBox.Items)
                {
                    _casesToProcessList.Add(casename);
                }

                //Make sure we clear the text box summary
                OutputSummaryTextBox.Text = string.Empty;

                //Set the processing status label to "Started"
                SumProcessingBlnLabel.Text = "Started";

                //Disable the process button since processing is now underway
                ProcessFilesButton.Enabled = false;

                //Enable the cancel button since we want to be able to abort the process
                CancelOperationButton.Enabled = true;

                //Pass the list of cases to the background worker for processing
                MainBackGroundWorker.RunWorkerAsync(_casesToProcessList);
            }
        }
        #endregion

        #region Cancel Operations

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (MainBackGroundWorker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                MainBackGroundWorker.CancelAsync();
            }
        }
        #endregion

        #region Background Worker: Do Work - Main Processing Area
        private void bgWorkerMain_DoWork(object sender, DoWorkEventArgs e)
        {
            //Receive the background worker from the Process Button and start processing the database tables
            BackgroundWorker worker = (BackgroundWorker)sender;

            //Send the list of cases/tables to be processed
            ProcessSummationTable(_casesToProcessList, worker, e);

        }
        #endregion

        #region "Background Worker: Progress Changed"
        private void bgWorkerMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StringBuilder reportProgress = ((StringBuilder)e.UserState);
            OutputSummaryTextBox.AppendText(reportProgress.ToString());
            reportProgress.Clear();
        }
        #endregion

        #region "Background Worker: Worker Completed"
        private void bgWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {

                AddCaseButton.Enabled = true;
                RemoveCaseButton.Enabled = true;
                OutputDirectoryButton.Enabled = true;
                OpenOutputDirectoryButton.Enabled = true;
                ProcessFilesButton.Enabled = true;
                CancelOperationButton.Enabled = false;

                _status.AppendLine("Operation Cancelled");
                OutputSummaryTextBox.AppendText(_status.ToString());

                //If the cancel button is pushed, set the status to aborted
                SumProcessingBlnLabel.Text = "Aborted";

                //Make sure we wipe out all previous totals
                ClearGlobals();

            }
            else if (e.Error != null)
            {
                //Not really sure if we need these here or not
                //Haven't seen this condition yet, so we may not need to do this
                //Take care of the buttons at the end of the process
                AddCaseButton.Enabled = true;
                RemoveCaseButton.Enabled = true;
                OutputDirectoryButton.Enabled = true;
                OpenOutputDirectoryButton.Enabled = true;
                ProcessFilesButton.Enabled = true;
                CancelOperationButton.Enabled = false;

                _status.AppendFormat("Error: {0}", e.Error.Message);
                _status.AppendLine();
                OutputSummaryTextBox.AppendText(_status.ToString());

                //Make sure we wipe out all previous totals
                ClearGlobals();


            }
            else
            {
                _status.AppendLine();
                _status.AppendFormat("Number of tables processed: {0}", _numberTablesProcessed);
                _status.AppendLine();
                _status.AppendFormat("Operation completed at: {0}", DateTime.Now);
                OutputSummaryTextBox.AppendText(_status.ToString());

                //Since we finished normally, set the status to Completed
                SumProcessingBlnLabel.Text = "Completed";

                //Make sure we wipe out all previous totals
                ClearGlobals();

                //Take care of the buttons at the end of the process
                AddCaseButton.Enabled = true;
                RemoveCaseButton.Enabled = true;
                OutputDirectoryButton.Enabled = true;
                OpenOutputDirectoryButton.Enabled = true;
                ProcessFilesButton.Enabled = true;
                CancelOperationButton.Enabled = false;

                //Test writing the log file out
                WriteToLogFile();
            }
        }
        #endregion

        #region "Process Summation Tables"

        //This is where the bulk of the work is done, including the OCR export
        public void ProcessSummationTable(List<string> casesToProcess, BackgroundWorker worker, DoWorkEventArgs e)
        {
            //This can be used to display the case name for the catch portion of the try/catch below when reporting errors
            string caseNameError = string.Empty;

            try
            {
                //Declare a temporary Datatable to work with tables locally for manipulating the Summation data
                DataTable tempTable = new DataTable();

                //Declare a new instance of a Datatable just for images
                DataTable imagesTable = new DataTable();

                //Declare a new instance of the class used to get the directory paths
                _swObj = new Summation(_swObj);

                //Declare a Hashtable to use to get the path information back from Summation
                SortedDictionary<string, string> casePathInfo = new SortedDictionary<string, string>();

                //Send the initial start of the process to the background worker update
                _status.AppendFormat("Operation started on / at: {0}", DateTime.Now);
                _status.AppendLine();              
                StatusUpdated(_status);

                foreach (string casename in casesToProcess)
                {
                    //Let's assume the directory doesn't exist first, so we'll set it to false.  If it does exist
                    //This variable will come back as true preventing the deletion of the directory

                    bool directoryFound = FileOperationsClass.FindDirectory(OutputPathTextBox.Text, casename, OverwriteDirOptionCheckBox.Checked);
                    if (directoryFound == true)
                    {
                        _status.AppendFormat("Deleting target directory for case: {0}", casename);
                        _status.AppendLine();
                        StatusUpdated(_status);
                    }


                    if (directoryFound == false)
                    {
                        casePathInfo = _swObj.ReturnCasePathInfo(casename);

                        foreach (var dirPath in casePathInfo)
                        {

                            switch (dirPath.Key.ToString())
                            {
                                case "CoreDB":
                                case "NoteDB":

                                    //Set up the provider string
                                    _connectionString = DatabaseOperations.GetDBConnectionString(dirPath.Value);

                                    _status.AppendLine();
                                    _status.AppendFormat("Processing case: {0}", casename);
                                    caseNameError = casename; //Set this for error reporting
                                    _status.AppendLine();
                                    _status.AppendFormat("Processing tables in: {0}", dirPath.Key.ToString());
                                    _status.AppendLine();
                                    StatusUpdated(_status);

                                    //Datatable to fill that will provide the data to display
                                    DataTable summationTable = new DataTable();

                                    DataTable schemaTable = DatabaseOperations.GetTableSchema(_connectionString);

                                    //Report when the table schema is being retrieved
                                    _status.AppendLine("Retrieving table schema");
                                    _status.AppendLine();
                                    StatusUpdated(_status);

                                    foreach (DataRow dr in schemaTable.Rows)
                                    {
                                        //The third element in the DataRow's itemArray represents the table name used in the query
                                        string query = "Select * from " + dr["table_name"].ToString();
                                        summationTable = DatabaseOperations.GetSpecificTable(query, _connectionString);

                                        if (summationTable.Rows.Count > 0)
                                        {
                                            //Produce a text file with contents only if there's a row count.  If it's nothing
                                            //but columns, skip it
                                            switch (dr["table_name"].ToString().ToUpper())
                                            {
                                                case "E-TABLE":
                                                case "STDTABLE":
                                                    _status.AppendFormat("Processing table: {0}, Rows Processed: {1}", dr["table_name"].ToString(), summationTable.Rows.Count);
                                                    _status.AppendLine();
                                                    StatusUpdated(_status);
                                                    tempTable = ExtensionHelper.RemoveUnusedColumns(summationTable);
                                                    FileOperationsClass.OutputTableToFile(summationTable, OutputPathTextBox.Text, casename, dirPath.Key.ToString(), dr["table_name"].ToString().ToUpper(), PipeCaretRadioButton.Checked);
                                                    tempTable.Reset();
                                                    break;
                                                case "IMGINFO":
                                                    if (CreateImageFileCheckBox.Checked == true)
                                                    {
                                                        _status.AppendFormat("Processing table: {0}, Rows Processed: {1}", dr["table_name"].ToString(), summationTable.Rows.Count);
                                                        _status.AppendLine();
                                                        StatusUpdated(_status);
                                                        string VolumeName = "@" + CustomVolNameTextBox.Text;
                                                        string imageFilePath = string.Empty;
                                                        if (casePathInfo.TryGetValue("ImageDir", out imageFilePath) == true)
                                                        {
                                                            if (OPTOutputRadioButton.Checked == true)
                                                            {
                                                                imagesTable = _sumImage.CreateOPTFile(summationTable, imageFilePath);
                                                                _fileOperations.OutputIMGTableToFile(imagesTable, OutputPathTextBox.Text, casename, "ImageDir", dr["table_name"].ToString(),
                                                                    "OPT", false, string.Empty);
                                                                imagesTable.Reset();
                                                            }
                                                            else
                                                            {
                                                                imagesTable = _sumImage.CreateLFPFile(summationTable, imageFilePath, VolumeName.ToUpper());
                                                                _fileOperations.OutputIMGTableToFile(imagesTable, OutputPathTextBox.Text, casename, "ImageDir", dr["table_name"].ToString(),
                                                                  "LFP", CustomVolNameRadioButton.Checked, CustomVolNameTextBox.Text);
                                                                imagesTable.Reset();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            _status.Append("Warning: No file path detected");
                                                            _status.AppendLine();
                                                            StatusUpdated(_status);
                                                            imagesTable.Reset();
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    _status.AppendFormat("Processing table: {0}, Rows Processed: {1}", dr["table_name"].ToString(), summationTable.Rows.Count);
                                                    _status.AppendLine();
                                                    StatusUpdated(_status);
                                                    FileOperationsClass.OutputTableToFile(summationTable, OutputPathTextBox.Text, casename, dirPath.Key.ToString(), dr["table_name"].ToString().ToUpper(), PipeCaretRadioButton.Checked);
                                                    break;
                                            }
                                            //Increment the counter for the tables we process to file
                                            _numberTablesProcessed += 1;
                                        }
                                        //Clear the table after 
                                        summationTable.Reset();
                                    }
                                    break;
                                case "TransDir":
                                    //Report the list of transcript files found associated with the case
                                    string noteDBPath = string.Empty;
                                    string sharedProfilePath = string.Empty;
                                    string transFilePath = string.Empty;
                                    List<KeyValuePair<string, string>> transFileList = new List<KeyValuePair<string, string>>();
                                    if (ExportTranscriptsOptionCheckBox.Checked == true)
                                    {
                                        if (casePathInfo.TryGetValue("TransDir", out transFilePath) == true)
                                        {

                                            if (casePathInfo.TryGetValue("AllProfilesDir", out sharedProfilePath) == true)
                                            {

                                                transFileList = _transcripts.GetTranscriptFileList(sharedProfilePath);
                                            }
                                            else
                                            {
                                                _status.Append("Transcript information unavailable.");
                                                _status.AppendLine();
                                                StatusUpdated(_status);
                                            }

                                            if (casePathInfo.TryGetValue("NoteDB", out noteDBPath) == true)
                                            {
                                                //Trying adding some space here
                                                _status.AppendLine();

                                                //Export the transcript files
                                                foreach (KeyValuePair<string, string> entry in transFileList)
                                                {
                                                    _transcripts.ExportTranscripts(OutputPathTextBox.Text, transFilePath, entry.Key, entry.Value, casename, noteDBPath, "TRANS");
                                                    _transcripts.GetTranscriptHighlights(OutputPathTextBox.Text, entry.Value, casename, noteDBPath, "TRANS");
                                                    _status.AppendLine();
                                                    StatusUpdated(_status);
                                                }
                                            }
                                            else
                                            {
                                                _status.Append("Unable to obtain path to Note DB.");
                                                _status.AppendLine();
                                                StatusUpdated(_status);
                                            }
                                        }
                                    }

                                    break;
                            }
                        }

                        //If the checkbox for OCR export is checked, we need to perform the export
                        if (ExportOCROptionCheckBox.Checked == true)
                        {
                            List<string> lstOCRCaseDocs = _swObj.GetOCRDocIdList();
                            if (lstOCRCaseDocs.Count != 0)
                            {
                                _status.AppendLine();
                                _status.AppendFormat("Exporting OCRBase for case: {0}", casename);
                                ExportOCRBase(casename, lstOCRCaseDocs);
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }
                            else
                            {
                                _status.Append("No OCR in case database available");
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }
                        }

                        //If the checkbox for the EMB copy is checked, we need to perform the copy
                        if (CopyEMBOptionCheckBox.Checked == true)
                        {
                            string[] strEMBDirPath = _swObj.GetEMBDirContents();
                            if (strEMBDirPath.Length != 0)
                            {
                                _status.AppendLine();
                                _status.AppendFormat("Copying email body files for case: {0}", casename);
                                CopyEMBDir(casename, strEMBDirPath);
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }
                            else
                            {
                                _status.Append("No email body files available");
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }

                        }


                        if (CreateMSGOptionCheckBox.Checked == true)
                        {
                            string coreDBPath = string.Empty;
                            if (casePathInfo.TryGetValue("CoreDB", out coreDBPath) == true)
                            {
                                _mailOperations.swObj = this._swObj;
                                _connectionString = DatabaseOperations.GetDBConnectionString(coreDBPath);
                                _mailOperations.ExtractMSGFiles(_connectionString, OutputPathTextBox.Text, casename);
                            }
                            else
                            {
                                _status.Append("Unable to obtain path to the Core DB");
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }

                        }

                        if (CreateMSGRefOptionCheckBox.Checked == true)
                        {
                            string coreDBPath = string.Empty;
                            if (casePathInfo.TryGetValue("CoreDB", out coreDBPath) == true)
                            {
                                _mailOperations.swObj = this._swObj;
                                _connectionString = DatabaseOperations.GetDBConnectionString(coreDBPath);
                                _mailOperations.CreateMSGReferenceFile(_connectionString, OutputPathTextBox.Text, casename);
                            }
                            else
                            {
                                _status.Append("Unable to obtain path to the Core DB");
                                _status.AppendLine();
                                StatusUpdated(_status);
                            }
                        }
                    }
                    else
                    {
                        _status.AppendFormat("Directory directory for {0} already exists.  Skipping case.", casename);
                        _status.AppendLine();
                        StatusUpdated(_status);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _status.AppendFormat("There was a problem processing case: {0}.  Error message: {1}", caseNameError, ex.Message);
                _status.AppendLine();
                StatusUpdated(_status);
            }
        }

        #endregion

        #region "Shared Functions and Utilities"

        private void StatusUpdated(StringBuilder status)
        {
            MainBackGroundWorker.ReportProgress(0, status);
        }

        private void CopyEMBDir(string caseFriendlyName, string[] strEMBDirPath)
        {
            //Declare a string for the outfile path.  We need to take into consideration the difference between 
            string strOutFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(OutputPathTextBox.Text, caseFriendlyName, "EMBDir");

            try
            {
                //Now that we have the collection of documents, we can loop through them and create new files
                _status.AppendLine();
                foreach (string file in strEMBDirPath)
                {
                    strOutFilePath = directoryName + @"\" + Path.GetFileName(file).Replace("^embody", string.Empty);
                    File.Copy(file, strOutFilePath);
                    _status.AppendFormat("Copying email body output file: {0}", strOutFilePath);
                    _status.AppendLine();
                    StatusUpdated(_status);
                }

            }
            catch (System.Exception ex)
            {
                _status.AppendFormat("There was an error copying the email body file to disk: {0}", ex.ToString());
                _status.AppendLine();
                StatusUpdated(_status);
            }

            _status.Append("EMail body copy process completed");
            _status.AppendLine();
            StatusUpdated(_status);
        }

        public void ClearGlobals()
        {
            string strCaseFriendlyName = string.Empty;

            //Clear the list for the cases that will be processed
            _casesToProcessList.Clear();

            //Declare global StringBuilder for showing status in the main report window
            _status.Clear();

            //Reset the DataTable for obtaining schema information for the tables to be queried
            _dbSchemaTable.Reset();

            //Reset the global variable to track the number of tables processed
            _numberTablesProcessed = 0;

            //Dispose of the background worker since we should be done with it
            MainBackGroundWorker.Dispose();

            //Reset the layout since we're done processing
            _swObj.ResetWorkspaceLayout();

            //Dispose of the Summation object if it's not null
            if (_swObj != null)
            {
                _swObj.Dispose();
            }

        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            Process[] pName = Process.GetProcessesByName("sw32");

            if (pName.Length != 0)
            {

                SummationRunningBlnLabel.Text = "Yes";
                AddCaseButton.Enabled = true;
                RemoveCaseButton.Enabled = true;
                ClearAllCasesButton.Enabled = true;
            }
            else
            {
                SummationRunningBlnLabel.Text = "No";
                AddCaseButton.Enabled = false;
                RemoveCaseButton.Enabled = false;
                ClearAllCasesButton.Enabled = false;
            }
        }

        private void WriteToLogFile()
        {
            string fileName = DateTime.Now.ToString("o") + ".txt";
            string filePath = OutputPathTextBox.Text + @"\" + fileName.Replace(":", "_");
            File.WriteAllText(filePath, OutputSummaryTextBox.Text);
        }

        private void ExportOCRBase(string caseFriendlyName, List<string> ocrCaseDocList)
        {
            //Declare a string for the outfile path.  We need to take into consideration the difference between 
            string outFilePath = string.Empty;

            //We need to use the case-friendly name as the name of the subdirectory
            //If the directory doesn't exist, create it, otherwise, leave it alone
            string directoryName = FileOperationsClass.CreateOutputDirectory(OutputPathTextBox.Text, caseFriendlyName, "OCRBase");

            try
            {
                //Now that we have the collection of documents, we can loop through them and create new files
                _status.AppendLine();
                foreach (string ocrDoc in ocrCaseDocList)
                {
                    object strOCRDocContents = _swObj.GetCoreOCRDoc(ocrDoc.Trim());
                    outFilePath = directoryName + @"\" + ocrDoc.Trim() + ".TXT";
                    File.WriteAllText(outFilePath, strOCRDocContents.ToString());
                    _status.AppendFormat("Creating OCR output file: {0}", outFilePath);
                    _status.AppendLine();
                    StatusUpdated(_status);
                }

            }
            catch (System.Exception ex)
            {
                _status.AppendFormat("There was an error writing the OCR export file to disk: {0}", ex.ToString());
                _status.AppendLine();
                StatusUpdated(_status);
            }

            _status.Append("OCR file export process completed");
            _status.AppendLine();
            StatusUpdated(_status);
        }

        private void CheckCaseCount()
        {
            //Make sure there are cases in the listbox before allowing a user to select the output directory
            if (CasesListBox.Items.Count != 0)
            {
                OutputDirectoryButton.Enabled = true;
                OpenOutputDirectoryButton.Enabled = true;
            }
            else
            {
                OutputDirectoryButton.Enabled = false;
                OpenOutputDirectoryButton.Enabled = false;
            }
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm frmAbout = new AboutForm();
            frmAbout.Show();
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strInstructionFile = AppDomain.CurrentDomain.BaseDirectory + @"Files\" + "instructions.pdf";
            if (File.Exists(strInstructionFile) == true)
            {
                Process.Start(strInstructionFile);
            }
            else
            {
                MessageBox.Show("Instructions are missing.  Please put instructions in the installation directory.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void rdoDefaultVolName_CheckedChanged(object sender, EventArgs e)
        {
            CustomVolNameTextBox.Clear();
            CustomVolNameTextBox.Text = "VOL001";
            CustomVolNameTextBox.Enabled = false;
        }

        private void rdoCustomVolName_CheckedChanged(object sender, EventArgs e)
        {
            CustomVolNameTextBox.Clear();
            CustomVolNameTextBox.Enabled = true;
        }

        private void rdoOPTOutput_CheckedChanged(object sender, EventArgs e)
        {
            VolumeNamePanel.Enabled = false;
        }

        private void rdoLFPOutput_CheckedChanged(object sender, EventArgs e)
        {
            VolumeNamePanel.Enabled = true;
        }

        private void OpenOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            if (OutputPathTextBox.Text != string.Empty)
            {
                Process.Start(@OutputPathTextBox.Text);
            }
            else
            {
                MessageBox.Show("Please select an output directory first", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }

        }

        private void CreateImageFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateImageFileCheckBox.Checked == true)
            {
                LoadFileTypePanel.Enabled = true;
                if (LFPOutputRadioButton.Checked == true)
                {
                    VolumeNamePanel.Enabled = true;
                }
                else
                {
                    VolumeNamePanel.Enabled = false;
                }
            }
            else
            {
                LoadFileTypePanel.Enabled = false;
                VolumeNamePanel.Enabled = false;
            }
        }
    }
}







