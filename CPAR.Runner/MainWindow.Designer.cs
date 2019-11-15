namespace CPAR.Runner
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Test 1");
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 0D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(3D, 2D);
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.mMenuStrip = new System.Windows.Forms.MenuStrip();
            this.mRunMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mStartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAbortMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAcceptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mNextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPreviousMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mToolStrip = new System.Windows.Forms.ToolStrip();
            this.mStartBtn = new System.Windows.Forms.ToolStripButton();
            this.mAbortBtn = new System.Windows.Forms.ToolStripButton();
            this.mAcceptBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mPreviousBtn = new System.Windows.Forms.ToolStripButton();
            this.mNextBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mSetupBtn = new System.Windows.Forms.ToolStripButton();
            this.mVASScore = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mAdvice = new System.Windows.Forms.ToolStripLabel();
            this.mStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mConnectedStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mTestList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mTestListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mStartContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAbortContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAcceptContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mSetupContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mProtocolImageList = new System.Windows.Forms.ImageList(this.components);
            this.mChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.mTabInstructions = new System.Windows.Forms.TabPage();
            this.mInstructions = new System.Windows.Forms.RichTextBox();
            this.mTabLog = new System.Windows.Forms.TabPage();
            this.mLogSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mLogBox = new System.Windows.Forms.RichTextBox();
            this.mComment = new System.Windows.Forms.TextBox();
            this.mTabResults = new System.Windows.Forms.TabPage();
            this.mResultBox = new System.Windows.Forms.RichTextBox();
            this.mTimer = new System.Windows.Forms.Timer(this.components);
            this.mProtocolTimer = new System.Windows.Forms.Timer(this.components);
            this.msgTimer = new System.Windows.Forms.Timer(this.components);
            this.mMenuStrip.SuspendLayout();
            this.mToolStrip.SuspendLayout();
            this.mStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mSplitContainer)).BeginInit();
            this.mSplitContainer.Panel1.SuspendLayout();
            this.mSplitContainer.Panel2.SuspendLayout();
            this.mSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mViewSplitContainer)).BeginInit();
            this.mViewSplitContainer.Panel1.SuspendLayout();
            this.mViewSplitContainer.Panel2.SuspendLayout();
            this.mViewSplitContainer.SuspendLayout();
            this.mTestListContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).BeginInit();
            this.mTabControl.SuspendLayout();
            this.mTabInstructions.SuspendLayout();
            this.mTabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mLogSplitContainer)).BeginInit();
            this.mLogSplitContainer.Panel1.SuspendLayout();
            this.mLogSplitContainer.Panel2.SuspendLayout();
            this.mLogSplitContainer.SuspendLayout();
            this.mTabResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMenuStrip
            // 
            this.mMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mRunMenu,
            this.viewToolStripMenuItem});
            this.mMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mMenuStrip.Name = "mMenuStrip";
            this.mMenuStrip.Size = new System.Drawing.Size(1031, 24);
            this.mMenuStrip.TabIndex = 1;
            this.mMenuStrip.Text = "menuStrip1";
            // 
            // mRunMenu
            // 
            this.mRunMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mStartMenuItem,
            this.mAbortMenuItem,
            this.mAcceptMenuItem,
            this.toolStripSeparator5,
            this.mNextMenuItem,
            this.mPreviousMenuItem,
            this.toolStripSeparator3,
            this.mExitMenuItem});
            this.mRunMenu.Name = "mRunMenu";
            this.mRunMenu.Size = new System.Drawing.Size(40, 20);
            this.mRunMenu.Text = "Run";
            // 
            // mStartMenuItem
            // 
            this.mStartMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mStartMenuItem.Image")));
            this.mStartMenuItem.Name = "mStartMenuItem";
            this.mStartMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mStartMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mStartMenuItem.Text = "Start";
            this.mStartMenuItem.ToolTipText = "Start the selected test";
            this.mStartMenuItem.Click += new System.EventHandler(this.mStartBtn_Click);
            // 
            // mAbortMenuItem
            // 
            this.mAbortMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mAbortMenuItem.Image")));
            this.mAbortMenuItem.Name = "mAbortMenuItem";
            this.mAbortMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mAbortMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mAbortMenuItem.Text = "Abort";
            this.mAbortMenuItem.ToolTipText = "Abort the test";
            this.mAbortMenuItem.Click += new System.EventHandler(this.mAbortBtn_Click);
            // 
            // mAcceptMenuItem
            // 
            this.mAcceptMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mAcceptMenuItem.Image")));
            this.mAcceptMenuItem.Name = "mAcceptMenuItem";
            this.mAcceptMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mAcceptMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mAcceptMenuItem.Text = "Accept";
            this.mAcceptMenuItem.ToolTipText = "Accept result from the completed test";
            this.mAcceptMenuItem.Click += new System.EventHandler(this.mAcceptBtn_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(165, 6);
            // 
            // mNextMenuItem
            // 
            this.mNextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mNextMenuItem.Image")));
            this.mNextMenuItem.Name = "mNextMenuItem";
            this.mNextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.mNextMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mNextMenuItem.Text = "Next";
            this.mNextMenuItem.ToolTipText = "Select the next test";
            this.mNextMenuItem.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // mPreviousMenuItem
            // 
            this.mPreviousMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mPreviousMenuItem.Image")));
            this.mPreviousMenuItem.Name = "mPreviousMenuItem";
            this.mPreviousMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.mPreviousMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mPreviousMenuItem.Text = "Previous";
            this.mPreviousMenuItem.ToolTipText = "Select the previous test";
            this.mPreviousMenuItem.Click += new System.EventHandler(this.previousToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // mExitMenuItem
            // 
            this.mExitMenuItem.Name = "mExitMenuItem";
            this.mExitMenuItem.Size = new System.Drawing.Size(168, 22);
            this.mExitMenuItem.Text = "Exit";
            this.mExitMenuItem.Click += new System.EventHandler(this.mExitMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instructionsToolStripMenuItem,
            this.mLogMenuItem,
            this.mResultsMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.instructionsToolStripMenuItem.Text = "Instructions";
            this.instructionsToolStripMenuItem.Click += new System.EventHandler(this.instructionsToolStripMenuItem_Click);
            // 
            // mLogMenuItem
            // 
            this.mLogMenuItem.Name = "mLogMenuItem";
            this.mLogMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.mLogMenuItem.Size = new System.Drawing.Size(155, 22);
            this.mLogMenuItem.Text = "Log";
            this.mLogMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // mResultsMenuItem
            // 
            this.mResultsMenuItem.Name = "mResultsMenuItem";
            this.mResultsMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mResultsMenuItem.Size = new System.Drawing.Size(155, 22);
            this.mResultsMenuItem.Text = "Results";
            this.mResultsMenuItem.Click += new System.EventHandler(this.resultsToolStripMenuItem_Click);
            // 
            // mToolStrip
            // 
            this.mToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mStartBtn,
            this.mAbortBtn,
            this.mAcceptBtn,
            this.toolStripSeparator6,
            this.mPreviousBtn,
            this.mNextBtn,
            this.toolStripSeparator2,
            this.mSetupBtn,
            this.mVASScore,
            this.toolStripLabel2,
            this.toolStripSeparator7,
            this.mAdvice});
            this.mToolStrip.Location = new System.Drawing.Point(0, 24);
            this.mToolStrip.Name = "mToolStrip";
            this.mToolStrip.Size = new System.Drawing.Size(1031, 31);
            this.mToolStrip.TabIndex = 2;
            this.mToolStrip.Text = "toolStrip1";
            // 
            // mStartBtn
            // 
            this.mStartBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mStartBtn.Image = ((System.Drawing.Image)(resources.GetObject("mStartBtn.Image")));
            this.mStartBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mStartBtn.Name = "mStartBtn";
            this.mStartBtn.Size = new System.Drawing.Size(28, 28);
            this.mStartBtn.Text = "toolStripButton1";
            this.mStartBtn.ToolTipText = "Start the selected test";
            this.mStartBtn.Click += new System.EventHandler(this.mStartBtn_Click);
            // 
            // mAbortBtn
            // 
            this.mAbortBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mAbortBtn.Image = ((System.Drawing.Image)(resources.GetObject("mAbortBtn.Image")));
            this.mAbortBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mAbortBtn.Name = "mAbortBtn";
            this.mAbortBtn.Size = new System.Drawing.Size(28, 28);
            this.mAbortBtn.Text = "toolStripButton1";
            this.mAbortBtn.ToolTipText = "Abort the test";
            this.mAbortBtn.Click += new System.EventHandler(this.mAbortBtn_Click);
            // 
            // mAcceptBtn
            // 
            this.mAcceptBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mAcceptBtn.Image = ((System.Drawing.Image)(resources.GetObject("mAcceptBtn.Image")));
            this.mAcceptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mAcceptBtn.Name = "mAcceptBtn";
            this.mAcceptBtn.Size = new System.Drawing.Size(28, 28);
            this.mAcceptBtn.Text = "toolStripButton1";
            this.mAcceptBtn.ToolTipText = "Accept result from the completed test";
            this.mAcceptBtn.Click += new System.EventHandler(this.mAcceptBtn_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // mPreviousBtn
            // 
            this.mPreviousBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mPreviousBtn.Image = ((System.Drawing.Image)(resources.GetObject("mPreviousBtn.Image")));
            this.mPreviousBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mPreviousBtn.Name = "mPreviousBtn";
            this.mPreviousBtn.Size = new System.Drawing.Size(28, 28);
            this.mPreviousBtn.Text = "toolStripButton1";
            this.mPreviousBtn.ToolTipText = "Select the next test";
            this.mPreviousBtn.Click += new System.EventHandler(this.previousToolStripMenuItem_Click);
            // 
            // mNextBtn
            // 
            this.mNextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mNextBtn.Image = ((System.Drawing.Image)(resources.GetObject("mNextBtn.Image")));
            this.mNextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mNextBtn.Name = "mNextBtn";
            this.mNextBtn.Size = new System.Drawing.Size(28, 28);
            this.mNextBtn.Text = "toolStripButton1";
            this.mNextBtn.ToolTipText = "Select the previous test";
            this.mNextBtn.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // mSetupBtn
            // 
            this.mSetupBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mSetupBtn.Image = ((System.Drawing.Image)(resources.GetObject("mSetupBtn.Image")));
            this.mSetupBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mSetupBtn.Name = "mSetupBtn";
            this.mSetupBtn.Size = new System.Drawing.Size(28, 28);
            this.mSetupBtn.Text = "toolStripButton1";
            this.mSetupBtn.ToolTipText = "Setup external parameters";
            this.mSetupBtn.Click += new System.EventHandler(this.mSetupBtn_Click);
            // 
            // mVASScore
            // 
            this.mVASScore.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mVASScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mVASScore.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mVASScore.Name = "mVASScore";
            this.mVASScore.ReadOnly = true;
            this.mVASScore.Size = new System.Drawing.Size(60, 31);
            this.mVASScore.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 28);
            this.toolStripLabel2.Text = "VAS:";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 31);
            // 
            // mAdvice
            // 
            this.mAdvice.Name = "mAdvice";
            this.mAdvice.Size = new System.Drawing.Size(56, 28);
            this.mAdvice.Text = "Help Text";
            // 
            // mStatusStrip
            // 
            this.mStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mConnectedStatus});
            this.mStatusStrip.Location = new System.Drawing.Point(0, 626);
            this.mStatusStrip.Name = "mStatusStrip";
            this.mStatusStrip.Size = new System.Drawing.Size(1031, 22);
            this.mStatusStrip.TabIndex = 3;
            this.mStatusStrip.Text = "statusStrip1";
            // 
            // mConnectedStatus
            // 
            this.mConnectedStatus.Name = "mConnectedStatus";
            this.mConnectedStatus.Size = new System.Drawing.Size(183, 17);
            this.mConnectedStatus.Text = "CONNECTED | NOT CONNECTED";
            // 
            // mSplitContainer
            // 
            this.mSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSplitContainer.Location = new System.Drawing.Point(0, 55);
            this.mSplitContainer.Name = "mSplitContainer";
            this.mSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mSplitContainer.Panel1
            // 
            this.mSplitContainer.Panel1.Controls.Add(this.mViewSplitContainer);
            // 
            // mSplitContainer.Panel2
            // 
            this.mSplitContainer.Panel2.Controls.Add(this.mTabControl);
            this.mSplitContainer.Size = new System.Drawing.Size(1031, 571);
            this.mSplitContainer.SplitterDistance = 416;
            this.mSplitContainer.TabIndex = 4;
            // 
            // mViewSplitContainer
            // 
            this.mViewSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mViewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mViewSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mViewSplitContainer.Name = "mViewSplitContainer";
            // 
            // mViewSplitContainer.Panel1
            // 
            this.mViewSplitContainer.Panel1.Controls.Add(this.mTestList);
            // 
            // mViewSplitContainer.Panel2
            // 
            this.mViewSplitContainer.Panel2.Controls.Add(this.mChart);
            this.mViewSplitContainer.Size = new System.Drawing.Size(1031, 416);
            this.mViewSplitContainer.SplitterDistance = 267;
            this.mViewSplitContainer.TabIndex = 0;
            this.mViewSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mViewSplitContainer_SplitterMoved);
            // 
            // mTestList
            // 
            this.mTestList.AutoArrange = false;
            this.mTestList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.mTestList.ContextMenuStrip = this.mTestListContextMenu;
            this.mTestList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTestList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTestList.FullRowSelect = true;
            this.mTestList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mTestList.HideSelection = false;
            this.mTestList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.mTestList.LargeImageList = this.mProtocolImageList;
            this.mTestList.Location = new System.Drawing.Point(0, 0);
            this.mTestList.MultiSelect = false;
            this.mTestList.Name = "mTestList";
            this.mTestList.Scrollable = false;
            this.mTestList.ShowGroups = false;
            this.mTestList.Size = new System.Drawing.Size(265, 414);
            this.mTestList.SmallImageList = this.mProtocolImageList;
            this.mTestList.TabIndex = 0;
            this.mTestList.UseCompatibleStateImageBehavior = false;
            this.mTestList.View = System.Windows.Forms.View.Details;
            this.mTestList.SelectedIndexChanged += new System.EventHandler(this.mTestList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Protocol";
            this.columnHeader1.Width = 180;
            // 
            // mTestListContextMenu
            // 
            this.mTestListContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mTestListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mStartContextMenuItem,
            this.mAbortContextMenuItem,
            this.mAcceptContextMenuItem,
            this.toolStripSeparator1,
            this.mSetupContextMenuItem});
            this.mTestListContextMenu.Name = "mTestListContextMenu";
            this.mTestListContextMenu.Size = new System.Drawing.Size(116, 114);
            // 
            // mStartContextMenuItem
            // 
            this.mStartContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mStartContextMenuItem.Image")));
            this.mStartContextMenuItem.Name = "mStartContextMenuItem";
            this.mStartContextMenuItem.Size = new System.Drawing.Size(115, 26);
            this.mStartContextMenuItem.Text = "Start";
            this.mStartContextMenuItem.ToolTipText = "Start the selected test";
            this.mStartContextMenuItem.Click += new System.EventHandler(this.mStartBtn_Click);
            // 
            // mAbortContextMenuItem
            // 
            this.mAbortContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mAbortContextMenuItem.Image")));
            this.mAbortContextMenuItem.Name = "mAbortContextMenuItem";
            this.mAbortContextMenuItem.Size = new System.Drawing.Size(115, 26);
            this.mAbortContextMenuItem.Text = "Abort";
            this.mAbortContextMenuItem.ToolTipText = "Abort the test";
            this.mAbortContextMenuItem.Click += new System.EventHandler(this.mAbortBtn_Click);
            // 
            // mAcceptContextMenuItem
            // 
            this.mAcceptContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mAcceptContextMenuItem.Image")));
            this.mAcceptContextMenuItem.Name = "mAcceptContextMenuItem";
            this.mAcceptContextMenuItem.Size = new System.Drawing.Size(115, 26);
            this.mAcceptContextMenuItem.Text = "Accept";
            this.mAcceptContextMenuItem.ToolTipText = "Accept result from the completed test";
            this.mAcceptContextMenuItem.Click += new System.EventHandler(this.mAcceptBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(112, 6);
            // 
            // mSetupContextMenuItem
            // 
            this.mSetupContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mSetupContextMenuItem.Image")));
            this.mSetupContextMenuItem.Name = "mSetupContextMenuItem";
            this.mSetupContextMenuItem.Size = new System.Drawing.Size(115, 26);
            this.mSetupContextMenuItem.Text = "Setup";
            this.mSetupContextMenuItem.ToolTipText = "Setup external parameters";
            this.mSetupContextMenuItem.Click += new System.EventHandler(this.mSetupBtn_Click);
            // 
            // mProtocolImageList
            // 
            this.mProtocolImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mProtocolImageList.ImageStream")));
            this.mProtocolImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.mProtocolImageList.Images.SetKeyName(0, "1-test-blocked.png");
            this.mProtocolImageList.Images.SetKeyName(1, "2-test-unlocked.png");
            this.mProtocolImageList.Images.SetKeyName(2, "3-test-running.png");
            this.mProtocolImageList.Images.SetKeyName(3, "4-test-pending.png");
            this.mProtocolImageList.Images.SetKeyName(4, "5-test-complete.png");
            // 
            // mChart
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Title = "Time [s]";
            chartArea1.Name = "main";
            this.mChart.ChartAreas.Add(chartArea1);
            this.mChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.DockedToChartArea = "main";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend";
            this.mChart.Legends.Add(legend1);
            this.mChart.Location = new System.Drawing.Point(0, 0);
            this.mChart.Name = "mChart";
            series1.ChartArea = "main";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            this.mChart.Series.Add(series1);
            this.mChart.Size = new System.Drawing.Size(758, 414);
            this.mChart.TabIndex = 0;
            this.mChart.Text = "chart1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "TestName";
            title1.Text = "Test Name";
            this.mChart.Titles.Add(title1);
            // 
            // mTabControl
            // 
            this.mTabControl.Controls.Add(this.mTabInstructions);
            this.mTabControl.Controls.Add(this.mTabLog);
            this.mTabControl.Controls.Add(this.mTabResults);
            this.mTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mTabControl.Location = new System.Drawing.Point(0, 0);
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            this.mTabControl.Size = new System.Drawing.Size(1029, 149);
            this.mTabControl.TabIndex = 0;
            this.mTabControl.SelectedIndexChanged += new System.EventHandler(this.mTabControl_SelectedIndexChanged);
            // 
            // mTabInstructions
            // 
            this.mTabInstructions.Controls.Add(this.mInstructions);
            this.mTabInstructions.Location = new System.Drawing.Point(4, 24);
            this.mTabInstructions.Name = "mTabInstructions";
            this.mTabInstructions.Padding = new System.Windows.Forms.Padding(3);
            this.mTabInstructions.Size = new System.Drawing.Size(1021, 121);
            this.mTabInstructions.TabIndex = 0;
            this.mTabInstructions.Text = "Instructions (F1)";
            this.mTabInstructions.UseVisualStyleBackColor = true;
            // 
            // mInstructions
            // 
            this.mInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mInstructions.Location = new System.Drawing.Point(3, 3);
            this.mInstructions.Name = "mInstructions";
            this.mInstructions.ReadOnly = true;
            this.mInstructions.Size = new System.Drawing.Size(1015, 115);
            this.mInstructions.TabIndex = 0;
            this.mInstructions.Text = "";
            // 
            // mTabLog
            // 
            this.mTabLog.BackColor = System.Drawing.SystemColors.Control;
            this.mTabLog.Controls.Add(this.mLogSplitContainer);
            this.mTabLog.Location = new System.Drawing.Point(4, 24);
            this.mTabLog.Name = "mTabLog";
            this.mTabLog.Padding = new System.Windows.Forms.Padding(3);
            this.mTabLog.Size = new System.Drawing.Size(1021, 121);
            this.mTabLog.TabIndex = 1;
            this.mTabLog.Text = "Log (F2)";
            // 
            // mLogSplitContainer
            // 
            this.mLogSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLogSplitContainer.IsSplitterFixed = true;
            this.mLogSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.mLogSplitContainer.Name = "mLogSplitContainer";
            this.mLogSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mLogSplitContainer.Panel1
            // 
            this.mLogSplitContainer.Panel1.Controls.Add(this.mLogBox);
            // 
            // mLogSplitContainer.Panel2
            // 
            this.mLogSplitContainer.Panel2.Controls.Add(this.mComment);
            this.mLogSplitContainer.Size = new System.Drawing.Size(1015, 115);
            this.mLogSplitContainer.SplitterDistance = 80;
            this.mLogSplitContainer.TabIndex = 0;
            // 
            // mLogBox
            // 
            this.mLogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mLogBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLogBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mLogBox.Location = new System.Drawing.Point(0, 0);
            this.mLogBox.Name = "mLogBox";
            this.mLogBox.ReadOnly = true;
            this.mLogBox.Size = new System.Drawing.Size(1015, 80);
            this.mLogBox.TabIndex = 0;
            this.mLogBox.Text = "";
            // 
            // mComment
            // 
            this.mComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mComment.Location = new System.Drawing.Point(0, 8);
            this.mComment.Name = "mComment";
            this.mComment.Size = new System.Drawing.Size(1015, 23);
            this.mComment.TabIndex = 0;
            this.mComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mComment_KeyDown);
            // 
            // mTabResults
            // 
            this.mTabResults.Controls.Add(this.mResultBox);
            this.mTabResults.Location = new System.Drawing.Point(4, 24);
            this.mTabResults.Name = "mTabResults";
            this.mTabResults.Size = new System.Drawing.Size(1021, 121);
            this.mTabResults.TabIndex = 2;
            this.mTabResults.Text = "Results (F3)";
            this.mTabResults.UseVisualStyleBackColor = true;
            // 
            // mResultBox
            // 
            this.mResultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mResultBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mResultBox.Location = new System.Drawing.Point(0, 0);
            this.mResultBox.Name = "mResultBox";
            this.mResultBox.ReadOnly = true;
            this.mResultBox.Size = new System.Drawing.Size(1021, 121);
            this.mResultBox.TabIndex = 0;
            this.mResultBox.Text = "";
            // 
            // mTimer
            // 
            this.mTimer.Interval = 2000;
            this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
            // 
            // mProtocolTimer
            // 
            this.mProtocolTimer.Tick += new System.EventHandler(this.mProtocolTimer_Tick);
            // 
            // msgTimer
            // 
            this.msgTimer.Enabled = true;
            this.msgTimer.Interval = 50;
            this.msgTimer.Tick += new System.EventHandler(this.msgTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1031, 648);
            this.Controls.Add(this.mSplitContainer);
            this.Controls.Add(this.mStatusStrip);
            this.Controls.Add(this.mToolStrip);
            this.Controls.Add(this.mMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mMenuStrip;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CPAR Runner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.mMenuStrip.ResumeLayout(false);
            this.mMenuStrip.PerformLayout();
            this.mToolStrip.ResumeLayout(false);
            this.mToolStrip.PerformLayout();
            this.mStatusStrip.ResumeLayout(false);
            this.mStatusStrip.PerformLayout();
            this.mSplitContainer.Panel1.ResumeLayout(false);
            this.mSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mSplitContainer)).EndInit();
            this.mSplitContainer.ResumeLayout(false);
            this.mViewSplitContainer.Panel1.ResumeLayout(false);
            this.mViewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mViewSplitContainer)).EndInit();
            this.mViewSplitContainer.ResumeLayout(false);
            this.mTestListContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mChart)).EndInit();
            this.mTabControl.ResumeLayout(false);
            this.mTabInstructions.ResumeLayout(false);
            this.mTabLog.ResumeLayout(false);
            this.mLogSplitContainer.Panel1.ResumeLayout(false);
            this.mLogSplitContainer.Panel2.ResumeLayout(false);
            this.mLogSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mLogSplitContainer)).EndInit();
            this.mLogSplitContainer.ResumeLayout(false);
            this.mTabResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mRunMenu;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStrip mToolStrip;
        private System.Windows.Forms.StatusStrip mStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel mConnectedStatus;
        private System.Windows.Forms.ToolStripButton mStartBtn;
        private System.Windows.Forms.ToolStripButton mAbortBtn;
        private System.Windows.Forms.ToolStripButton mAcceptBtn;
        private System.Windows.Forms.ToolStripMenuItem mStartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAbortMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAcceptMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem instructionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mLogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mResultsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mExitMenuItem;
        private System.Windows.Forms.SplitContainer mSplitContainer;
        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.TabPage mTabInstructions;
        private System.Windows.Forms.TabPage mTabLog;
        private System.Windows.Forms.TabPage mTabResults;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mNextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mPreviousMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton mPreviousBtn;
        private System.Windows.Forms.ToolStripButton mNextBtn;
        private System.Windows.Forms.SplitContainer mViewSplitContainer;
        private System.Windows.Forms.ListView mTestList;
        private System.Windows.Forms.ImageList mProtocolImageList;
        private System.Windows.Forms.DataVisualization.Charting.Chart mChart;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton mSetupBtn;
        private System.Windows.Forms.ContextMenuStrip mTestListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mStartContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAbortContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAcceptContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mSetupContextMenuItem;
        private System.Windows.Forms.SplitContainer mLogSplitContainer;
        private System.Windows.Forms.RichTextBox mLogBox;
        private System.Windows.Forms.TextBox mComment;
        private System.Windows.Forms.RichTextBox mResultBox;
        private System.Windows.Forms.ToolStripTextBox mVASScore;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel mAdvice;
        private System.Windows.Forms.RichTextBox mInstructions;
        private System.Windows.Forms.Timer mTimer;
        private System.Windows.Forms.Timer mProtocolTimer;
        private System.Windows.Forms.Timer msgTimer;
    }
}

