namespace SimpleDataExporter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSourceGroupBox = new System.Windows.Forms.GroupBox();
            this.CasesListBox = new System.Windows.Forms.ListBox();
            this.AddCaseButton = new System.Windows.Forms.Button();
            this.ProcessFilesButton = new System.Windows.Forms.Button();
            this.OutputDirectoryButton = new System.Windows.Forms.Button();
            this.OutputPathTextBox = new System.Windows.Forms.TextBox();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.RemoveCaseButton = new System.Windows.Forms.Button();
            this.OutputSummaryTextBox = new System.Windows.Forms.TextBox();
            this.grpOutputSummary = new System.Windows.Forms.GroupBox();
            this.MainBackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ClearAllCasesButton = new System.Windows.Forms.Button();
            this.CancelOperationButton = new System.Windows.Forms.Button();
            this.grpSumInfo = new System.Windows.Forms.GroupBox();
            this.SummationRunningBlnLabel = new System.Windows.Forms.Label();
            this.SummationStatusLabel = new System.Windows.Forms.Label();
            this.SumProcessingBlnLabel = new System.Windows.Forms.Label();
            this.ProcessingStatusLabel = new System.Windows.Forms.Label();
            this.SumInstalledBlnLabel = new System.Windows.Forms.Label();
            this.SumInstalledLabel = new System.Windows.Forms.Label();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.OutputOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.ExportTranscriptsOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportTranscriptOptionLabel = new System.Windows.Forms.Label();
            this.OverwriteDirOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.OverwriteDirOptionLabel = new System.Windows.Forms.Label();
            this.ExportOCROptionCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportOCRBaseLabel = new System.Windows.Forms.Label();
            this.LoadFileOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.VolumeNamePanel = new System.Windows.Forms.Panel();
            this.VolNameLabel = new System.Windows.Forms.Label();
            this.DefaultVolNameRadioButton = new System.Windows.Forms.RadioButton();
            this.CustomVolNameTextBox = new System.Windows.Forms.TextBox();
            this.CustomVolNameRadioButton = new System.Windows.Forms.RadioButton();
            this.DelimiterTypePanel = new System.Windows.Forms.Panel();
            this.DelimiterOptionLabel = new System.Windows.Forms.Label();
            this.DATRadioButton = new System.Windows.Forms.RadioButton();
            this.PipeCaretRadioButton = new System.Windows.Forms.RadioButton();
            this.LoadFileTypePanel = new System.Windows.Forms.Panel();
            this.LoadFileTypeOptionLabel = new System.Windows.Forms.Label();
            this.OPTOutputRadioButton = new System.Windows.Forms.RadioButton();
            this.LFPOutputRadioButton = new System.Windows.Forms.RadioButton();
            this.EmailOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.CreateMSGRefOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateMSGRefOptionLabel = new System.Windows.Forms.Label();
            this.CreateMSGOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateMSGOptionLabel = new System.Windows.Forms.Label();
            this.CopyEMBOptionCheckBox = new System.Windows.Forms.CheckBox();
            this.CopyEMBOptionLabel = new System.Windows.Forms.Label();
            this.OpenOutputDirectoryButton = new System.Windows.Forms.Button();
            this.CreateImageFileCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateImageFileLabel = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.FileSourceGroupBox.SuspendLayout();
            this.grpOutputSummary.SuspendLayout();
            this.grpSumInfo.SuspendLayout();
            this.OutputOptionsGroupBox.SuspendLayout();
            this.LoadFileOptionsGroupBox.SuspendLayout();
            this.VolumeNamePanel.SuspendLayout();
            this.DelimiterTypePanel.SuspendLayout();
            this.LoadFileTypePanel.SuspendLayout();
            this.EmailOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(932, 24);
            this.MainMenu.TabIndex = 2;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instructionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.instructionsToolStripMenuItem.Text = "&Instructions";
            this.instructionsToolStripMenuItem.Click += new System.EventHandler(this.instructionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // FileSourceGroupBox
            // 
            this.FileSourceGroupBox.Controls.Add(this.CasesListBox);
            this.FileSourceGroupBox.Location = new System.Drawing.Point(12, 27);
            this.FileSourceGroupBox.Name = "FileSourceGroupBox";
            this.FileSourceGroupBox.Size = new System.Drawing.Size(266, 180);
            this.FileSourceGroupBox.TabIndex = 13;
            this.FileSourceGroupBox.TabStop = false;
            this.FileSourceGroupBox.Text = "Cases To Process";
            // 
            // CasesListBox
            // 
            this.CasesListBox.FormattingEnabled = true;
            this.CasesListBox.Location = new System.Drawing.Point(13, 20);
            this.CasesListBox.Name = "CasesListBox";
            this.CasesListBox.Size = new System.Drawing.Size(235, 147);
            this.CasesListBox.TabIndex = 0;
            // 
            // AddCaseButton
            // 
            this.AddCaseButton.Location = new System.Drawing.Point(284, 36);
            this.AddCaseButton.Name = "AddCaseButton";
            this.AddCaseButton.Size = new System.Drawing.Size(75, 23);
            this.AddCaseButton.TabIndex = 15;
            this.AddCaseButton.Text = "Add ";
            this.MainToolTip.SetToolTip(this.AddCaseButton, "Select cases to process");
            this.AddCaseButton.UseVisualStyleBackColor = true;
            this.AddCaseButton.Click += new System.EventHandler(this.AddFilesButton_Click);
            // 
            // ProcessFilesButton
            // 
            this.ProcessFilesButton.Location = new System.Drawing.Point(808, 410);
            this.ProcessFilesButton.Name = "ProcessFilesButton";
            this.ProcessFilesButton.Size = new System.Drawing.Size(75, 23);
            this.ProcessFilesButton.TabIndex = 17;
            this.ProcessFilesButton.Text = "Process";
            this.MainToolTip.SetToolTip(this.ProcessFilesButton, "Begin processing cases");
            this.ProcessFilesButton.UseVisualStyleBackColor = true;
            this.ProcessFilesButton.Click += new System.EventHandler(this.btnProcessFiles_Click);
            // 
            // OutputDirectoryButton
            // 
            this.OutputDirectoryButton.Location = new System.Drawing.Point(808, 323);
            this.OutputDirectoryButton.Name = "OutputDirectoryButton";
            this.OutputDirectoryButton.Size = new System.Drawing.Size(107, 23);
            this.OutputDirectoryButton.TabIndex = 20;
            this.OutputDirectoryButton.Text = "Output Directory";
            this.MainToolTip.SetToolTip(this.OutputDirectoryButton, "Select output directory where cases will be exported to");
            this.OutputDirectoryButton.UseVisualStyleBackColor = true;
            this.OutputDirectoryButton.Click += new System.EventHandler(this.OutputDirectoryButton_Click);
            // 
            // OutputPathTextBox
            // 
            this.OutputPathTextBox.Location = new System.Drawing.Point(86, 324);
            this.OutputPathTextBox.Name = "OutputPathTextBox";
            this.OutputPathTextBox.Size = new System.Drawing.Size(716, 20);
            this.OutputPathTextBox.TabIndex = 19;
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Location = new System.Drawing.Point(13, 328);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(67, 13);
            this.FilePathLabel.TabIndex = 18;
            this.FilePathLabel.Text = "Output Path:";
            // 
            // RemoveCaseButton
            // 
            this.RemoveCaseButton.Location = new System.Drawing.Point(284, 65);
            this.RemoveCaseButton.Name = "RemoveCaseButton";
            this.RemoveCaseButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveCaseButton.TabIndex = 21;
            this.RemoveCaseButton.Text = "Remove Case";
            this.MainToolTip.SetToolTip(this.RemoveCaseButton, "Remove cases from processing");
            this.RemoveCaseButton.UseVisualStyleBackColor = true;
            this.RemoveCaseButton.Click += new System.EventHandler(this.RemoveCaseButton_Click);
            // 
            // OutputSummaryTextBox
            // 
            this.OutputSummaryTextBox.Location = new System.Drawing.Point(7, 20);
            this.OutputSummaryTextBox.MaxLength = 0;
            this.OutputSummaryTextBox.Multiline = true;
            this.OutputSummaryTextBox.Name = "OutputSummaryTextBox";
            this.OutputSummaryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputSummaryTextBox.Size = new System.Drawing.Size(772, 84);
            this.OutputSummaryTextBox.TabIndex = 0;
            // 
            // grpOutputSummary
            // 
            this.grpOutputSummary.Controls.Add(this.OutputSummaryTextBox);
            this.grpOutputSummary.Location = new System.Drawing.Point(12, 351);
            this.grpOutputSummary.Name = "grpOutputSummary";
            this.grpOutputSummary.Size = new System.Drawing.Size(790, 111);
            this.grpOutputSummary.TabIndex = 15;
            this.grpOutputSummary.TabStop = false;
            this.grpOutputSummary.Text = "Output Summary";
            // 
            // MainBackGroundWorker
            // 
            this.MainBackGroundWorker.WorkerReportsProgress = true;
            this.MainBackGroundWorker.WorkerSupportsCancellation = true;
            this.MainBackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerMain_DoWork);
            this.MainBackGroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerMain_ProgressChanged);
            this.MainBackGroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerMain_RunWorkerCompleted);
            // 
            // ClearAllCasesButton
            // 
            this.ClearAllCasesButton.Location = new System.Drawing.Point(284, 94);
            this.ClearAllCasesButton.Name = "ClearAllCasesButton";
            this.ClearAllCasesButton.Size = new System.Drawing.Size(75, 37);
            this.ClearAllCasesButton.TabIndex = 28;
            this.ClearAllCasesButton.Text = "Clear All Cases";
            this.MainToolTip.SetToolTip(this.ClearAllCasesButton, "Remove cases from processing");
            this.ClearAllCasesButton.UseVisualStyleBackColor = true;
            this.ClearAllCasesButton.Click += new System.EventHandler(this.ClearAllCasesButton_Click);
            // 
            // CancelOperationButton
            // 
            this.CancelOperationButton.Location = new System.Drawing.Point(808, 439);
            this.CancelOperationButton.Name = "CancelOperationButton";
            this.CancelOperationButton.Size = new System.Drawing.Size(75, 23);
            this.CancelOperationButton.TabIndex = 25;
            this.CancelOperationButton.Text = "Cancel";
            this.MainToolTip.SetToolTip(this.CancelOperationButton, "Cancel processing");
            this.CancelOperationButton.UseVisualStyleBackColor = true;
            this.CancelOperationButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // grpSumInfo
            // 
            this.grpSumInfo.Controls.Add(this.SummationRunningBlnLabel);
            this.grpSumInfo.Controls.Add(this.SummationStatusLabel);
            this.grpSumInfo.Controls.Add(this.SumProcessingBlnLabel);
            this.grpSumInfo.Controls.Add(this.ProcessingStatusLabel);
            this.grpSumInfo.Controls.Add(this.SumInstalledBlnLabel);
            this.grpSumInfo.Controls.Add(this.SumInstalledLabel);
            this.grpSumInfo.Location = new System.Drawing.Point(12, 213);
            this.grpSumInfo.Name = "grpSumInfo";
            this.grpSumInfo.Size = new System.Drawing.Size(266, 102);
            this.grpSumInfo.TabIndex = 26;
            this.grpSumInfo.TabStop = false;
            this.grpSumInfo.Text = "Status";
            // 
            // SummationRunningBlnLabel
            // 
            this.SummationRunningBlnLabel.AutoSize = true;
            this.SummationRunningBlnLabel.Location = new System.Drawing.Point(116, 44);
            this.SummationRunningBlnLabel.Name = "SummationRunningBlnLabel";
            this.SummationRunningBlnLabel.Size = new System.Drawing.Size(0, 13);
            this.SummationRunningBlnLabel.TabIndex = 32;
            // 
            // SummationStatusLabel
            // 
            this.SummationStatusLabel.AutoSize = true;
            this.SummationStatusLabel.Location = new System.Drawing.Point(6, 44);
            this.SummationStatusLabel.Name = "SummationStatusLabel";
            this.SummationStatusLabel.Size = new System.Drawing.Size(105, 13);
            this.SummationStatusLabel.TabIndex = 31;
            this.SummationStatusLabel.Text = "Summation Running:";
            // 
            // SumProcessingBlnLabel
            // 
            this.SumProcessingBlnLabel.AutoSize = true;
            this.SumProcessingBlnLabel.Location = new System.Drawing.Point(116, 68);
            this.SumProcessingBlnLabel.Name = "SumProcessingBlnLabel";
            this.SumProcessingBlnLabel.Size = new System.Drawing.Size(0, 13);
            this.SumProcessingBlnLabel.TabIndex = 30;
            // 
            // ProcessingStatusLabel
            // 
            this.ProcessingStatusLabel.AutoSize = true;
            this.ProcessingStatusLabel.Location = new System.Drawing.Point(6, 68);
            this.ProcessingStatusLabel.Name = "ProcessingStatusLabel";
            this.ProcessingStatusLabel.Size = new System.Drawing.Size(95, 13);
            this.ProcessingStatusLabel.TabIndex = 29;
            this.ProcessingStatusLabel.Text = "Processing Status:";
            // 
            // SumInstalledBlnLabel
            // 
            this.SumInstalledBlnLabel.AutoSize = true;
            this.SumInstalledBlnLabel.Location = new System.Drawing.Point(116, 16);
            this.SumInstalledBlnLabel.Name = "SumInstalledBlnLabel";
            this.SumInstalledBlnLabel.Size = new System.Drawing.Size(0, 13);
            this.SumInstalledBlnLabel.TabIndex = 28;
            // 
            // SumInstalledLabel
            // 
            this.SumInstalledLabel.AutoSize = true;
            this.SumInstalledLabel.Location = new System.Drawing.Point(6, 16);
            this.SumInstalledLabel.Name = "SumInstalledLabel";
            this.SumInstalledLabel.Size = new System.Drawing.Size(104, 13);
            this.SumInstalledLabel.TabIndex = 27;
            this.SumInstalledLabel.Text = "Summation Installed:";
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // OutputOptionsGroupBox
            // 
            this.OutputOptionsGroupBox.Controls.Add(this.CreateImageFileCheckBox);
            this.OutputOptionsGroupBox.Controls.Add(this.CreateImageFileLabel);
            this.OutputOptionsGroupBox.Controls.Add(this.ExportTranscriptsOptionCheckBox);
            this.OutputOptionsGroupBox.Controls.Add(this.ExportTranscriptOptionLabel);
            this.OutputOptionsGroupBox.Controls.Add(this.OverwriteDirOptionCheckBox);
            this.OutputOptionsGroupBox.Controls.Add(this.OverwriteDirOptionLabel);
            this.OutputOptionsGroupBox.Controls.Add(this.ExportOCROptionCheckBox);
            this.OutputOptionsGroupBox.Controls.Add(this.ExportOCRBaseLabel);
            this.OutputOptionsGroupBox.Location = new System.Drawing.Point(386, 171);
            this.OutputOptionsGroupBox.Name = "OutputOptionsGroupBox";
            this.OutputOptionsGroupBox.Size = new System.Drawing.Size(205, 123);
            this.OutputOptionsGroupBox.TabIndex = 27;
            this.OutputOptionsGroupBox.TabStop = false;
            this.OutputOptionsGroupBox.Text = "Other Output Options";
            // 
            // ExportTranscriptsOptionCheckBox
            // 
            this.ExportTranscriptsOptionCheckBox.AutoSize = true;
            this.ExportTranscriptsOptionCheckBox.Location = new System.Drawing.Point(159, 67);
            this.ExportTranscriptsOptionCheckBox.Name = "ExportTranscriptsOptionCheckBox";
            this.ExportTranscriptsOptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.ExportTranscriptsOptionCheckBox.TabIndex = 8;
            this.ExportTranscriptsOptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExportTranscriptOptionLabel
            // 
            this.ExportTranscriptOptionLabel.AutoSize = true;
            this.ExportTranscriptOptionLabel.Location = new System.Drawing.Point(6, 67);
            this.ExportTranscriptOptionLabel.Name = "ExportTranscriptOptionLabel";
            this.ExportTranscriptOptionLabel.Size = new System.Drawing.Size(95, 13);
            this.ExportTranscriptOptionLabel.TabIndex = 7;
            this.ExportTranscriptOptionLabel.Text = "Export Transcripts:";
            // 
            // OverwriteDirOptionCheckBox
            // 
            this.OverwriteDirOptionCheckBox.AutoSize = true;
            this.OverwriteDirOptionCheckBox.Location = new System.Drawing.Point(159, 89);
            this.OverwriteDirOptionCheckBox.Name = "OverwriteDirOptionCheckBox";
            this.OverwriteDirOptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.OverwriteDirOptionCheckBox.TabIndex = 6;
            this.OverwriteDirOptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // OverwriteDirOptionLabel
            // 
            this.OverwriteDirOptionLabel.AutoSize = true;
            this.OverwriteDirOptionLabel.Location = new System.Drawing.Point(6, 89);
            this.OverwriteDirOptionLabel.Name = "OverwriteDirOptionLabel";
            this.OverwriteDirOptionLabel.Size = new System.Drawing.Size(147, 13);
            this.OverwriteDirOptionLabel.TabIndex = 5;
            this.OverwriteDirOptionLabel.Text = "Overwrite Existing Directories:";
            // 
            // ExportOCROptionCheckBox
            // 
            this.ExportOCROptionCheckBox.AutoSize = true;
            this.ExportOCROptionCheckBox.Location = new System.Drawing.Point(159, 45);
            this.ExportOCROptionCheckBox.Name = "ExportOCROptionCheckBox";
            this.ExportOCROptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.ExportOCROptionCheckBox.TabIndex = 4;
            this.ExportOCROptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExportOCRBaseLabel
            // 
            this.ExportOCRBaseLabel.AutoSize = true;
            this.ExportOCRBaseLabel.Location = new System.Drawing.Point(6, 45);
            this.ExportOCRBaseLabel.Name = "ExportOCRBaseLabel";
            this.ExportOCRBaseLabel.Size = new System.Drawing.Size(93, 13);
            this.ExportOCRBaseLabel.TabIndex = 3;
            this.ExportOCRBaseLabel.Text = "Export OCR Base:";
            // 
            // LoadFileOptionsGroupBox
            // 
            this.LoadFileOptionsGroupBox.Controls.Add(this.VolumeNamePanel);
            this.LoadFileOptionsGroupBox.Controls.Add(this.DelimiterTypePanel);
            this.LoadFileOptionsGroupBox.Controls.Add(this.LoadFileTypePanel);
            this.LoadFileOptionsGroupBox.Location = new System.Drawing.Point(386, 27);
            this.LoadFileOptionsGroupBox.Name = "LoadFileOptionsGroupBox";
            this.LoadFileOptionsGroupBox.Size = new System.Drawing.Size(416, 137);
            this.LoadFileOptionsGroupBox.TabIndex = 29;
            this.LoadFileOptionsGroupBox.TabStop = false;
            this.LoadFileOptionsGroupBox.Text = "Load File Options";
            // 
            // VolumeNamePanel
            // 
            this.VolumeNamePanel.Controls.Add(this.VolNameLabel);
            this.VolumeNamePanel.Controls.Add(this.DefaultVolNameRadioButton);
            this.VolumeNamePanel.Controls.Add(this.CustomVolNameTextBox);
            this.VolumeNamePanel.Controls.Add(this.CustomVolNameRadioButton);
            this.VolumeNamePanel.Location = new System.Drawing.Point(9, 98);
            this.VolumeNamePanel.Name = "VolumeNamePanel";
            this.VolumeNamePanel.Size = new System.Drawing.Size(395, 33);
            this.VolumeNamePanel.TabIndex = 31;
            // 
            // VolNameLabel
            // 
            this.VolNameLabel.AutoSize = true;
            this.VolNameLabel.Location = new System.Drawing.Point(7, 9);
            this.VolNameLabel.Name = "VolNameLabel";
            this.VolNameLabel.Size = new System.Drawing.Size(76, 13);
            this.VolNameLabel.TabIndex = 0;
            this.VolNameLabel.Text = "Volume Name:";
            // 
            // DefaultVolNameRadioButton
            // 
            this.DefaultVolNameRadioButton.AutoSize = true;
            this.DefaultVolNameRadioButton.Location = new System.Drawing.Point(99, 7);
            this.DefaultVolNameRadioButton.Name = "DefaultVolNameRadioButton";
            this.DefaultVolNameRadioButton.Size = new System.Drawing.Size(59, 17);
            this.DefaultVolNameRadioButton.TabIndex = 1;
            this.DefaultVolNameRadioButton.Text = "Default";
            this.DefaultVolNameRadioButton.UseVisualStyleBackColor = true;
            this.DefaultVolNameRadioButton.CheckedChanged += new System.EventHandler(this.rdoDefaultVolName_CheckedChanged);
            // 
            // CustomVolNameTextBox
            // 
            this.CustomVolNameTextBox.Location = new System.Drawing.Point(256, 6);
            this.CustomVolNameTextBox.Name = "CustomVolNameTextBox";
            this.CustomVolNameTextBox.Size = new System.Drawing.Size(125, 20);
            this.CustomVolNameTextBox.TabIndex = 29;
            // 
            // CustomVolNameRadioButton
            // 
            this.CustomVolNameRadioButton.AutoSize = true;
            this.CustomVolNameRadioButton.Location = new System.Drawing.Point(187, 7);
            this.CustomVolNameRadioButton.Name = "CustomVolNameRadioButton";
            this.CustomVolNameRadioButton.Size = new System.Drawing.Size(60, 17);
            this.CustomVolNameRadioButton.TabIndex = 2;
            this.CustomVolNameRadioButton.Text = "Custom";
            this.CustomVolNameRadioButton.UseVisualStyleBackColor = true;
            this.CustomVolNameRadioButton.CheckedChanged += new System.EventHandler(this.rdoCustomVolName_CheckedChanged);
            // 
            // DelimiterTypePanel
            // 
            this.DelimiterTypePanel.Controls.Add(this.DelimiterOptionLabel);
            this.DelimiterTypePanel.Controls.Add(this.DATRadioButton);
            this.DelimiterTypePanel.Controls.Add(this.PipeCaretRadioButton);
            this.DelimiterTypePanel.Location = new System.Drawing.Point(9, 19);
            this.DelimiterTypePanel.Name = "DelimiterTypePanel";
            this.DelimiterTypePanel.Size = new System.Drawing.Size(395, 33);
            this.DelimiterTypePanel.TabIndex = 32;
            // 
            // DelimiterOptionLabel
            // 
            this.DelimiterOptionLabel.AutoSize = true;
            this.DelimiterOptionLabel.Location = new System.Drawing.Point(3, 12);
            this.DelimiterOptionLabel.Name = "DelimiterOptionLabel";
            this.DelimiterOptionLabel.Size = new System.Drawing.Size(109, 13);
            this.DelimiterOptionLabel.TabIndex = 30;
            this.DelimiterOptionLabel.Text = "Delimiter Type (Data):";
            // 
            // DATRadioButton
            // 
            this.DATRadioButton.AutoSize = true;
            this.DATRadioButton.Location = new System.Drawing.Point(228, 10);
            this.DATRadioButton.Name = "DATRadioButton";
            this.DATRadioButton.Size = new System.Drawing.Size(50, 17);
            this.DATRadioButton.TabIndex = 32;
            this.DATRadioButton.Text = "DAT ";
            this.DATRadioButton.UseVisualStyleBackColor = true;
            // 
            // PipeCaretRadioButton
            // 
            this.PipeCaretRadioButton.AutoSize = true;
            this.PipeCaretRadioButton.Location = new System.Drawing.Point(127, 10);
            this.PipeCaretRadioButton.Name = "PipeCaretRadioButton";
            this.PipeCaretRadioButton.Size = new System.Drawing.Size(82, 17);
            this.PipeCaretRadioButton.TabIndex = 31;
            this.PipeCaretRadioButton.Text = "Pipe / Caret";
            this.PipeCaretRadioButton.UseVisualStyleBackColor = true;
            // 
            // LoadFileTypePanel
            // 
            this.LoadFileTypePanel.Controls.Add(this.LoadFileTypeOptionLabel);
            this.LoadFileTypePanel.Controls.Add(this.OPTOutputRadioButton);
            this.LoadFileTypePanel.Controls.Add(this.LFPOutputRadioButton);
            this.LoadFileTypePanel.Location = new System.Drawing.Point(9, 59);
            this.LoadFileTypePanel.Name = "LoadFileTypePanel";
            this.LoadFileTypePanel.Size = new System.Drawing.Size(395, 33);
            this.LoadFileTypePanel.TabIndex = 31;
            // 
            // LoadFileTypeOptionLabel
            // 
            this.LoadFileTypeOptionLabel.AutoSize = true;
            this.LoadFileTypeOptionLabel.Location = new System.Drawing.Point(3, 8);
            this.LoadFileTypeOptionLabel.Name = "LoadFileTypeOptionLabel";
            this.LoadFileTypeOptionLabel.Size = new System.Drawing.Size(115, 13);
            this.LoadFileTypeOptionLabel.TabIndex = 3;
            this.LoadFileTypeOptionLabel.Text = "Delimiter Type (Image):";
            // 
            // OPTOutputRadioButton
            // 
            this.OPTOutputRadioButton.AutoSize = true;
            this.OPTOutputRadioButton.Location = new System.Drawing.Point(127, 6);
            this.OPTOutputRadioButton.Name = "OPTOutputRadioButton";
            this.OPTOutputRadioButton.Size = new System.Drawing.Size(47, 17);
            this.OPTOutputRadioButton.TabIndex = 4;
            this.OPTOutputRadioButton.Text = "OPT";
            this.OPTOutputRadioButton.UseVisualStyleBackColor = true;
            this.OPTOutputRadioButton.CheckedChanged += new System.EventHandler(this.rdoOPTOutput_CheckedChanged);
            // 
            // LFPOutputRadioButton
            // 
            this.LFPOutputRadioButton.AutoSize = true;
            this.LFPOutputRadioButton.Location = new System.Drawing.Point(228, 6);
            this.LFPOutputRadioButton.Name = "LFPOutputRadioButton";
            this.LFPOutputRadioButton.Size = new System.Drawing.Size(44, 17);
            this.LFPOutputRadioButton.TabIndex = 5;
            this.LFPOutputRadioButton.Text = "LFP";
            this.LFPOutputRadioButton.UseVisualStyleBackColor = true;
            this.LFPOutputRadioButton.CheckedChanged += new System.EventHandler(this.rdoLFPOutput_CheckedChanged);
            // 
            // EmailOptionsGroupBox
            // 
            this.EmailOptionsGroupBox.Controls.Add(this.CreateMSGRefOptionCheckBox);
            this.EmailOptionsGroupBox.Controls.Add(this.CreateMSGRefOptionLabel);
            this.EmailOptionsGroupBox.Controls.Add(this.CreateMSGOptionCheckBox);
            this.EmailOptionsGroupBox.Controls.Add(this.CreateMSGOptionLabel);
            this.EmailOptionsGroupBox.Controls.Add(this.CopyEMBOptionCheckBox);
            this.EmailOptionsGroupBox.Controls.Add(this.CopyEMBOptionLabel);
            this.EmailOptionsGroupBox.Location = new System.Drawing.Point(597, 171);
            this.EmailOptionsGroupBox.Name = "EmailOptionsGroupBox";
            this.EmailOptionsGroupBox.Size = new System.Drawing.Size(205, 123);
            this.EmailOptionsGroupBox.TabIndex = 30;
            this.EmailOptionsGroupBox.TabStop = false;
            this.EmailOptionsGroupBox.Text = "Email Options";
            // 
            // CreateMSGRefOptionCheckBox
            // 
            this.CreateMSGRefOptionCheckBox.AutoSize = true;
            this.CreateMSGRefOptionCheckBox.Location = new System.Drawing.Point(158, 68);
            this.CreateMSGRefOptionCheckBox.Name = "CreateMSGRefOptionCheckBox";
            this.CreateMSGRefOptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.CreateMSGRefOptionCheckBox.TabIndex = 20;
            this.CreateMSGRefOptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateMSGRefOptionLabel
            // 
            this.CreateMSGRefOptionLabel.AutoSize = true;
            this.CreateMSGRefOptionLabel.Location = new System.Drawing.Point(6, 68);
            this.CreateMSGRefOptionLabel.Name = "CreateMSGRefOptionLabel";
            this.CreateMSGRefOptionLabel.Size = new System.Drawing.Size(140, 13);
            this.CreateMSGRefOptionLabel.TabIndex = 19;
            this.CreateMSGRefOptionLabel.Text = "Create MSG Reference File:";
            // 
            // CreateMSGOptionCheckBox
            // 
            this.CreateMSGOptionCheckBox.AutoSize = true;
            this.CreateMSGOptionCheckBox.Location = new System.Drawing.Point(158, 45);
            this.CreateMSGOptionCheckBox.Name = "CreateMSGOptionCheckBox";
            this.CreateMSGOptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.CreateMSGOptionCheckBox.TabIndex = 18;
            this.CreateMSGOptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateMSGOptionLabel
            // 
            this.CreateMSGOptionLabel.AutoSize = true;
            this.CreateMSGOptionLabel.Location = new System.Drawing.Point(6, 46);
            this.CreateMSGOptionLabel.Name = "CreateMSGOptionLabel";
            this.CreateMSGOptionLabel.Size = new System.Drawing.Size(143, 13);
            this.CreateMSGOptionLabel.TabIndex = 17;
            this.CreateMSGOptionLabel.Text = "Extract MSG files from PSTs:";
            // 
            // CopyEMBOptionCheckBox
            // 
            this.CopyEMBOptionCheckBox.AutoSize = true;
            this.CopyEMBOptionCheckBox.Location = new System.Drawing.Point(158, 22);
            this.CopyEMBOptionCheckBox.Name = "CopyEMBOptionCheckBox";
            this.CopyEMBOptionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.CopyEMBOptionCheckBox.TabIndex = 16;
            this.CopyEMBOptionCheckBox.UseVisualStyleBackColor = true;
            // 
            // CopyEMBOptionLabel
            // 
            this.CopyEMBOptionLabel.AutoSize = true;
            this.CopyEMBOptionLabel.Location = new System.Drawing.Point(6, 24);
            this.CopyEMBOptionLabel.Name = "CopyEMBOptionLabel";
            this.CopyEMBOptionLabel.Size = new System.Drawing.Size(108, 13);
            this.CopyEMBOptionLabel.TabIndex = 15;
            this.CopyEMBOptionLabel.Text = "Copy email body files:";
            // 
            // OpenOutputDirectoryButton
            // 
            this.OpenOutputDirectoryButton.Location = new System.Drawing.Point(808, 351);
            this.OpenOutputDirectoryButton.Name = "OpenOutputDirectoryButton";
            this.OpenOutputDirectoryButton.Size = new System.Drawing.Size(107, 37);
            this.OpenOutputDirectoryButton.TabIndex = 31;
            this.OpenOutputDirectoryButton.Text = "Open Output Directory";
            this.OpenOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.OpenOutputDirectoryButton.Click += new System.EventHandler(this.OpenOutputDirectoryButton_Click);
            // 
            // CreateImageFileCheckBox
            // 
            this.CreateImageFileCheckBox.AutoSize = true;
            this.CreateImageFileCheckBox.Location = new System.Drawing.Point(159, 23);
            this.CreateImageFileCheckBox.Name = "CreateImageFileCheckBox";
            this.CreateImageFileCheckBox.Size = new System.Drawing.Size(15, 14);
            this.CreateImageFileCheckBox.TabIndex = 10;
            this.CreateImageFileCheckBox.UseVisualStyleBackColor = true;
            this.CreateImageFileCheckBox.CheckedChanged += new System.EventHandler(this.CreateImageFileCheckBox_CheckedChanged);
            // 
            // CreateImageFileLabel
            // 
            this.CreateImageFileLabel.AutoSize = true;
            this.CreateImageFileLabel.Location = new System.Drawing.Point(6, 23);
            this.CreateImageFileLabel.Name = "CreateImageFileLabel";
            this.CreateImageFileLabel.Size = new System.Drawing.Size(92, 13);
            this.CreateImageFileLabel.TabIndex = 9;
            this.CreateImageFileLabel.Text = "Create Image File:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(932, 476);
            this.Controls.Add(this.OpenOutputDirectoryButton);
            this.Controls.Add(this.EmailOptionsGroupBox);
            this.Controls.Add(this.LoadFileOptionsGroupBox);
            this.Controls.Add(this.ClearAllCasesButton);
            this.Controls.Add(this.OutputOptionsGroupBox);
            this.Controls.Add(this.grpSumInfo);
            this.Controls.Add(this.CancelOperationButton);
            this.Controls.Add(this.RemoveCaseButton);
            this.Controls.Add(this.OutputDirectoryButton);
            this.Controls.Add(this.OutputPathTextBox);
            this.Controls.Add(this.FilePathLabel);
            this.Controls.Add(this.ProcessFilesButton);
            this.Controls.Add(this.grpOutputSummary);
            this.Controls.Add(this.AddCaseButton);
            this.Controls.Add(this.FileSourceGroupBox);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Simple Data Exporter";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.FileSourceGroupBox.ResumeLayout(false);
            this.grpOutputSummary.ResumeLayout(false);
            this.grpOutputSummary.PerformLayout();
            this.grpSumInfo.ResumeLayout(false);
            this.grpSumInfo.PerformLayout();
            this.OutputOptionsGroupBox.ResumeLayout(false);
            this.OutputOptionsGroupBox.PerformLayout();
            this.LoadFileOptionsGroupBox.ResumeLayout(false);
            this.VolumeNamePanel.ResumeLayout(false);
            this.VolumeNamePanel.PerformLayout();
            this.DelimiterTypePanel.ResumeLayout(false);
            this.DelimiterTypePanel.PerformLayout();
            this.LoadFileTypePanel.ResumeLayout(false);
            this.LoadFileTypePanel.PerformLayout();
            this.EmailOptionsGroupBox.ResumeLayout(false);
            this.EmailOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox FileSourceGroupBox;
        private System.Windows.Forms.Button AddCaseButton;
        private System.Windows.Forms.Button ProcessFilesButton;
        private System.Windows.Forms.Button OutputDirectoryButton;
        private System.Windows.Forms.TextBox OutputPathTextBox;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.Button RemoveCaseButton;
        private System.Windows.Forms.TextBox OutputSummaryTextBox;
        private System.Windows.Forms.GroupBox grpOutputSummary;
        private System.ComponentModel.BackgroundWorker MainBackGroundWorker;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.GroupBox grpSumInfo;
        private System.Windows.Forms.Label SumInstalledBlnLabel;
        private System.Windows.Forms.Label SumInstalledLabel;
        private System.Windows.Forms.Label SumProcessingBlnLabel;
        private System.Windows.Forms.Label ProcessingStatusLabel;
        private System.Windows.Forms.ListBox CasesListBox;
        private System.Windows.Forms.Label SummationRunningBlnLabel;
        private System.Windows.Forms.Label SummationStatusLabel;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.GroupBox OutputOptionsGroupBox;
        private System.Windows.Forms.Label ExportOCRBaseLabel;
        private System.Windows.Forms.CheckBox ExportOCROptionCheckBox;
        private System.Windows.Forms.CheckBox OverwriteDirOptionCheckBox;
        private System.Windows.Forms.Label OverwriteDirOptionLabel;
        private System.Windows.Forms.Button ClearAllCasesButton;
        private System.Windows.Forms.CheckBox ExportTranscriptsOptionCheckBox;
        private System.Windows.Forms.Label ExportTranscriptOptionLabel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instructionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox LoadFileOptionsGroupBox;
        private System.Windows.Forms.RadioButton DATRadioButton;
        private System.Windows.Forms.RadioButton PipeCaretRadioButton;
        private System.Windows.Forms.Label DelimiterOptionLabel;
        private System.Windows.Forms.TextBox CustomVolNameTextBox;
        private System.Windows.Forms.RadioButton CustomVolNameRadioButton;
        private System.Windows.Forms.RadioButton DefaultVolNameRadioButton;
        private System.Windows.Forms.Label VolNameLabel;
        private System.Windows.Forms.RadioButton LFPOutputRadioButton;
        private System.Windows.Forms.RadioButton OPTOutputRadioButton;
        private System.Windows.Forms.Label LoadFileTypeOptionLabel;
        private System.Windows.Forms.GroupBox EmailOptionsGroupBox;
        private System.Windows.Forms.CheckBox CreateMSGRefOptionCheckBox;
        private System.Windows.Forms.Label CreateMSGRefOptionLabel;
        private System.Windows.Forms.CheckBox CreateMSGOptionCheckBox;
        private System.Windows.Forms.Label CreateMSGOptionLabel;
        private System.Windows.Forms.CheckBox CopyEMBOptionCheckBox;
        private System.Windows.Forms.Label CopyEMBOptionLabel;
        private System.Windows.Forms.Panel LoadFileTypePanel;
        private System.Windows.Forms.Panel DelimiterTypePanel;
        private System.Windows.Forms.Panel VolumeNamePanel;
        private System.Windows.Forms.Button CancelOperationButton;
        private System.Windows.Forms.Button OpenOutputDirectoryButton;
        private System.Windows.Forms.CheckBox CreateImageFileCheckBox;
        private System.Windows.Forms.Label CreateImageFileLabel;
    }
}