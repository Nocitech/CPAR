namespace CPAR.Tester
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mPortList = new System.Windows.Forms.ToolStripComboBox();
            this.mConnectBtn = new System.Windows.Forms.ToolStripButton();
            this.mDisconnectBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.mAutoKick = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.scriptButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.DeviceType = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.logBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.autoStartBtn = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainerVertical = new System.Windows.Forms.SplitContainer();
            this.mSplitContainerVertical = new System.Windows.Forms.SplitContainer();
            this.mFunctionList = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.updateView = new System.Windows.Forms.PropertyGrid();
            this.mLogWindow = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.scriptTimer = new System.Windows.Forms.Timer(this.components);
            this.msgTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mSplitContainerVertical)).BeginInit();
            this.mSplitContainerVertical.Panel1.SuspendLayout();
            this.mSplitContainerVertical.Panel2.SuspendLayout();
            this.mSplitContainerVertical.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mPortList,
            this.mConnectBtn,
            this.mDisconnectBtn,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.mAutoKick,
            this.toolStripSeparator9,
            this.scriptButton,
            this.toolStripSeparator11,
            this.DeviceType,
            this.toolStripSeparator12,
            this.toolStripLabel2,
            this.logBtn,
            this.toolStripSeparator13,
            this.toolStripLabel3,
            this.autoStartBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(428, 47);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // mPortList
            // 
            this.mPortList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mPortList.Name = "mPortList";
            this.mPortList.Size = new System.Drawing.Size(80, 47);
            // 
            // mConnectBtn
            // 
            this.mConnectBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mConnectBtn.Image = ((System.Drawing.Image)(resources.GetObject("mConnectBtn.Image")));
            this.mConnectBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mConnectBtn.Name = "mConnectBtn";
            this.mConnectBtn.Size = new System.Drawing.Size(44, 44);
            this.mConnectBtn.Text = "Connect";
            this.mConnectBtn.Click += new System.EventHandler(this.mConnectBtn_Click);
            // 
            // mDisconnectBtn
            // 
            this.mDisconnectBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mDisconnectBtn.Image = ((System.Drawing.Image)(resources.GetObject("mDisconnectBtn.Image")));
            this.mDisconnectBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mDisconnectBtn.Name = "mDisconnectBtn";
            this.mDisconnectBtn.Size = new System.Drawing.Size(44, 44);
            this.mDisconnectBtn.Text = "Disconnect";
            this.mDisconnectBtn.Click += new System.EventHandler(this.mDisconnectBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(61, 44);
            this.toolStripLabel1.Text = "Auto Kick:";
            // 
            // mAutoKick
            // 
            this.mAutoKick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mAutoKick.Items.AddRange(new object[] {
            "Off",
            "On"});
            this.mAutoKick.Name = "mAutoKick";
            this.mAutoKick.Size = new System.Drawing.Size(75, 47);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 47);
            // 
            // scriptButton
            // 
            this.scriptButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scriptButton.Image = ((System.Drawing.Image)(resources.GetObject("scriptButton.Image")));
            this.scriptButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptButton.Name = "scriptButton";
            this.scriptButton.Size = new System.Drawing.Size(44, 44);
            this.scriptButton.Text = "Script";
            this.scriptButton.Click += new System.EventHandler(this.scriptButton_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 47);
            // 
            // DeviceType
            // 
            this.DeviceType.Name = "DeviceType";
            this.DeviceType.Size = new System.Drawing.Size(45, 44);
            this.DeviceType.Text = "Device:";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(30, 44);
            this.toolStripLabel2.Text = "Log:";
            // 
            // logBtn
            // 
            this.logBtn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.logBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.logBtn.Image = ((System.Drawing.Image)(resources.GetObject("logBtn.Image")));
            this.logBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.logBtn.Name = "logBtn";
            this.logBtn.Size = new System.Drawing.Size(32, 44);
            this.logBtn.Text = "OFF";
            this.logBtn.ToolTipText = "Click to turn logging on off";
            this.logBtn.Click += new System.EventHandler(this.logBtn_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(56, 15);
            this.toolStripLabel3.Text = "Autostart";
            // 
            // autoStartBtn
            // 
            this.autoStartBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.autoStartBtn.Image = ((System.Drawing.Image)(resources.GetObject("autoStartBtn.Image")));
            this.autoStartBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoStartBtn.Name = "autoStartBtn";
            this.autoStartBtn.Size = new System.Drawing.Size(32, 19);
            this.autoStartBtn.Text = "OFF";
            this.autoStartBtn.Click += new System.EventHandler(this.autoStartBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(428, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVertical.Location = new System.Drawing.Point(0, 47);
            this.splitContainerVertical.Name = "splitContainerVertical";
            this.splitContainerVertical.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.mSplitContainerVertical);
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.mLogWindow);
            this.splitContainerVertical.Size = new System.Drawing.Size(428, 369);
            this.splitContainerVertical.SplitterDistance = 257;
            this.splitContainerVertical.TabIndex = 2;
            // 
            // mSplitContainerVertical
            // 
            this.mSplitContainerVertical.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mSplitContainerVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mSplitContainerVertical.Location = new System.Drawing.Point(0, 0);
            this.mSplitContainerVertical.Name = "mSplitContainerVertical";
            // 
            // mSplitContainerVertical.Panel1
            // 
            this.mSplitContainerVertical.Panel1.Controls.Add(this.mFunctionList);
            // 
            // mSplitContainerVertical.Panel2
            // 
            this.mSplitContainerVertical.Panel2.Controls.Add(this.tabControl1);
            this.mSplitContainerVertical.Size = new System.Drawing.Size(428, 257);
            this.mSplitContainerVertical.SplitterDistance = 74;
            this.mSplitContainerVertical.TabIndex = 0;
            // 
            // mFunctionList
            // 
            this.mFunctionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFunctionList.FormattingEnabled = true;
            this.mFunctionList.Location = new System.Drawing.Point(0, 0);
            this.mFunctionList.Name = "mFunctionList";
            this.mFunctionList.Size = new System.Drawing.Size(70, 253);
            this.mFunctionList.TabIndex = 0;
            this.mFunctionList.SelectedIndexChanged += new System.EventHandler(this.mFunctionList_SelectedIndexChanged);
            this.mFunctionList.DoubleClick += new System.EventHandler(this.mFunctionList_DoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(346, 253);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.mPropertyGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(1);
            this.tabPage1.Size = new System.Drawing.Size(338, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Function (F1)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // mPropertyGrid
            // 
            this.mPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.mPropertyGrid.Location = new System.Drawing.Point(1, 1);
            this.mPropertyGrid.Name = "mPropertyGrid";
            this.mPropertyGrid.Size = new System.Drawing.Size(336, 225);
            this.mPropertyGrid.TabIndex = 0;
            this.mPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.mPropertyGrid_PropertyValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.updateView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(1);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(1);
            this.tabPage2.Size = new System.Drawing.Size(336, 234);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Status Update (F2)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // updateView
            // 
            this.updateView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.updateView.Location = new System.Drawing.Point(1, 1);
            this.updateView.Margin = new System.Windows.Forms.Padding(1);
            this.updateView.Name = "updateView";
            this.updateView.Size = new System.Drawing.Size(334, 232);
            this.updateView.TabIndex = 0;
            // 
            // mLogWindow
            // 
            this.mLogWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLogWindow.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mLogWindow.Location = new System.Drawing.Point(0, 0);
            this.mLogWindow.Multiline = true;
            this.mLogWindow.Name = "mLogWindow";
            this.mLogWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mLogWindow.Size = new System.Drawing.Size(424, 104);
            this.mLogWindow.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // scriptTimer
            // 
            this.scriptTimer.Tick += new System.EventHandler(this.scriptTimer_Tick);
            // 
            // msgTimer
            // 
            this.msgTimer.Interval = 50;
            this.msgTimer.Tick += new System.EventHandler(this.msgTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 438);
            this.Controls.Add(this.splitContainerVertical);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainWindow";
            this.Text = "CPAR Tester";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            this.splitContainerVertical.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).EndInit();
            this.splitContainerVertical.ResumeLayout(false);
            this.mSplitContainerVertical.Panel1.ResumeLayout(false);
            this.mSplitContainerVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mSplitContainerVertical)).EndInit();
            this.mSplitContainerVertical.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainerVertical;
        private System.Windows.Forms.ToolStripComboBox mPortList;
        private System.Windows.Forms.ToolStripButton mConnectBtn;
        private System.Windows.Forms.ToolStripButton mDisconnectBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TextBox mLogWindow;
        private System.Windows.Forms.SplitContainer mSplitContainerVertical;
        private System.Windows.Forms.ListBox mFunctionList;
        private System.Windows.Forms.PropertyGrid mPropertyGrid;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox mAutoKick;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton scriptButton;
        private System.Windows.Forms.Timer scriptTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripLabel DeviceType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton logBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton autoStartBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid updateView;
        private System.Windows.Forms.Timer msgTimer;
    }
}

