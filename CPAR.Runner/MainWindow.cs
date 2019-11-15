using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPAR.Communication;
using CPAR.Runner.Startup;
using CPAR.Logging;
using CPAR.Core;
using CPAR.Communication.Messages;
using System.IO;
using CPAR.Core.Results;

namespace CPAR.Runner
{
    public partial class MainWindow : Form
    {
        private Visualizer visualizer;
        private Logger log;
        private PersistentLog persistentLog;
        private DebugWindow debugWnd = null;
        public delegate void InvokeDelegate();


        private class Logger :
            ILogger
        {

            public Logger(RichTextBox log)
            {
                this.log = log;
                this.log.VisibleChanged += (o, e) => ScrollToEnd();
            }

            private void ScrollToEnd()
            {
                if (log.Visible)
                {
                    log.SelectionStart = log.Text.Length;
                    log.ScrollToCaret();
                }
            }

            public void Add(DateTime time, LogCategory category, LogLevel level, string message)
            {
                if (log != null)
                {
                    if (log.InvokeRequired)
                    {
                        log.BeginInvoke(new InvokeDelegate(() =>
                        {
                            Add(time, category, level, message);
                        }));
                    }
                    else
                    {
                        LogText(FormatMessage(time, category, level, message) + System.Environment.NewLine, GetColor(level));
                    }
                }
            }

            private string FormatMessage(DateTime time, LogCategory category, LogLevel level, string message)
            {
                return String.Format("[{0}|{1}] {2}",
                    time,
                    category.ToString(),
                    message);
            }

            public void LogText(string text, Color color)
            {
                log.AppendText(text, color);
                ScrollToEnd();
            }

            public Color GetColor(LogLevel level)
            {
                switch (level)
                {
                    case LogLevel.DEBUG:
                        return Color.Blue;
                    case LogLevel.STATUS:
                        return Color.Black;
                    case LogLevel.ERROR:
                        return Color.Red;
                }

                return Color.Black;
            }

            RichTextBox log;
        }

        public MainWindow()
        {
            InitializeComponent();
            Text = String.Format("{0} ({1})", Text, SystemSettings.VersionInformation);

            StartupWizard wizard = new StartupWizard()
            {
                ShowIcon = false,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            if (wizard.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    UpdateViewStatus();
                    UpdateConnectedStatus();
                    InitializeLoggingSystem();
                    InitializeProtocol();
                    LogSessionStart();

                    DeviceManager.PortName = SystemSettings.SerialPort;
                    DeviceManager.Initialize();
                    DeviceManager.DeviceStateChanged += DeviceStateChanged;
                    DeviceManager.StatusReceived += OnStatusMessage;
                    mTimer.Enabled = true;

                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("UCE");
            }

            visualizer = new Visualizer(mChart);
            Visualizer.Active = visualizer;
            mTestList.Columns[0].Width = mViewSplitContainer.Panel1.Width;
        }

        private void InitializeWindow()
        {
            Width = SystemSettings.WindowWidth;
            Height = SystemSettings.WindowHeight;
            mSplitContainer.SplitterDistance = SystemSettings.SplitterDistance;
            mViewSplitContainer.SplitterDistance = SystemSettings.ViewSplitterDistance;
            Location = new Point(SystemSettings.StartX, SystemSettings.StartY);
        }

        private void LogSessionStart()
        {
            if (Experiment.Active.UseBetweenSubjectFactors)
                Log.Status("SESSION STARTED [ID: {0}  ]", Session.Active.ID);
            else
                Log.Status("SESSION STARTED");

            if (Experiment.Active.UseExperimenters)
            {
                Log.Status("EXPERIMENTER: [Name: {0}, Affiliation: {1}, Mail: {2}, Phone: {3} ]",
                    Experimenter.Active.Name,
                    Experimenter.Active.Affiliation,
                    Experimenter.Active.Mail,
                    Experimenter.Active.Phone);
            }
        }

        private void InitializeLoggingSystem()
        {
            var systemLog = Path.Combine(SystemSettings.BasePath, "system.log");
            var subjectLog = Path.Combine(SystemSettings.LoggingDirectory, Subject.Active.SubjectID + ".log");
            var sessionLog = Path.Combine(SystemSettings.LoggingDirectory, Subject.Active.SubjectID + "." + Session.Active.ID + ".log");
            log = new Logger(mLogBox);
            persistentLog = new PersistentLog(systemLog, subjectLog, sessionLog, log);
            Log.SetLogger(persistentLog);
            Log.Level = SystemSettings.LogLevel;

            if (Log.Level == LogLevel.DEBUG)
            {
                debugWnd = new DebugWindow();
                debugWnd.Show(this);
            }
        }

        private void InitializeProtocol()
        {
            mTestList.Items.Clear();
            Test.StateChanged += TestStateChanged;
            Experiment.Active.Protocol.Tests.Foreach((test) => mTestList.Items.Add(new ListViewItem(test.Name) { Tag = test }));
            Test.Initialize(Experiment.Active.Protocol.Tests);

            if (mTestList.Items.Count > 0)
            {
                SelectTest(0);
            }

            UpdateGUI();
            UpdateAdvice();
        }

        private void UpdateGUI()
        {
            UpdateTestList();
            UpdateGuiState();
            UpdateResult();
            UpdateInstructions();
            UpdateAdvice();
        }

        private void UpdateInstructions()
        {
            var test = GetSelectedTest();

            if (test != null)
            {
                try
                {
                    mInstructions.Rtf = test.Instruction.Trim();
                }
                catch
                {
                    mInstructions.Text = test.Instruction.Trim();
                }
            }
        }

        private void UpdateResult()
        {
            var test = GetSelectedTest();

            if (test != null)
            {
                var result = Session.Active.GetResult(test.ID);

                if (result != null)
                    mResultBox.Text = result.DisplayResult();
            }
        }

        private void UpdateTestList()
        {
            foreach (ListViewItem item in mTestList.Items)
            {
                if (item.Tag is Test)
                {
                    item.ImageIndex = (int)(item.Tag as Test).State;
                }
            }
        }

        private void UpdateGuiState()
        {
            if (SelectedIndex >= 0)
            {
                var test = GetSelectedTest();

                switch (test.State)
                {
                    case Test.TestState.READY:
                        PreviousEnabled = PreviousPossible;
                        NextEnabled = NextPossible;
                        StartEnabled = DeviceManager.StartPossible;
                        AbortEnabled = AcceptEnabled = false;
                        SetupEnabled = test.NumberOfExternalParameters > 0;
                        mTestList.Enabled = true;
                        break;
                    case Test.TestState.RUNNING:
                        PreviousEnabled = NextEnabled = StartEnabled = AcceptEnabled = SetupEnabled = false;
                        AbortEnabled = true;
                        mTestList.Enabled = false;
                        break;
                    case Test.TestState.PENDING:
                        PreviousEnabled = NextEnabled = StartEnabled = SetupEnabled = false;
                        AbortEnabled = AcceptEnabled = true;
                        mTestList.Enabled = false;
                        break;
                    case Test.TestState.COMPLETED:
                        PreviousEnabled = PreviousPossible;
                        NextEnabled = NextPossible;
                        StartEnabled = DeviceManager.StartPossible && !test.IsBlocked();
                        AbortEnabled = AcceptEnabled = SetupEnabled = false;
                        SetupEnabled = test.NumberOfExternalParameters > 0;
                        mTestList.Enabled = true;
                        break;
                    case Test.TestState.BLOCKED:
                        PreviousEnabled = PreviousPossible;
                        NextEnabled = NextPossible;
                        StartEnabled = AbortEnabled = AcceptEnabled = false;
                        mTestList.Enabled = true;
                        SetupEnabled = test.NumberOfExternalParameters > 0;
                        break;
                }
            }
        }

        private bool PreviousEnabled
        {
            get
            {
                return mPreviousBtn.Enabled;
            }
            set
            {
                if (mPreviousBtn.Enabled != value)
                    mPreviousBtn.Enabled = mPreviousMenuItem.Enabled = value;
            }
        }

        private bool PreviousPossible
        {
            get
            {
                return SelectedIndex > 0;
            }
        }

        private bool NextEnabled
        {
            get
            {
                return mNextBtn.Enabled;
            }
            set
            {
                if (mNextBtn.Enabled != value)
                    mNextBtn.Enabled = mNextMenuItem.Enabled = value;
            }
        }

        private bool NextPossible
        {
            get
            {
                return SelectedIndex < mTestList.Items.Count - 1;
            }
        }

        private bool StartEnabled
        {
            get
            {
                return mStartContextMenuItem.Enabled;
            }
            set
            {
                if (mStartBtn.Enabled != value)
                    mStartContextMenuItem.Enabled = mStartBtn.Enabled = mStartMenuItem.Enabled = value;
            }
        }

        private bool AbortEnabled
        {
            get
            {
                return mAbortContextMenuItem.Enabled;
            }
            set
            {
                if (mAbortBtn.Enabled != value)
                    mAbortContextMenuItem.Enabled = mAbortBtn.Enabled = mAbortMenuItem.Enabled = value;
            }
        }

        private bool AcceptEnabled
        {
            get
            {
                return mAcceptContextMenuItem.Enabled;
            }
            set
            {
                if (mAcceptBtn.Enabled != value)
                    mAcceptContextMenuItem.Enabled = mAcceptBtn.Enabled = mAcceptMenuItem.Enabled = value;
            }
        }

        private bool SetupEnabled
        {
            get
            {
                return mSetupBtn.Enabled;
            }
            set
            {
                if (mSetupBtn.Enabled != value)
                    mSetupBtn.Enabled = mSetupContextMenuItem.Enabled = value;
            }
        }

        private void SelectTest(int index)
        {
            if (SelectedIndex >= 0)
                mTestList.Items[SelectedIndex].Selected = false;

            if ((index >= 0) && (index < mTestList.Items.Count))
            {
                var test = mTestList.Items[index].Tag as Test;
                mTestList.Items[index].Selected = true;
                UpdateGUI();
            }
        }

        private Test GetSelectedTest()
        {
            Test retValue = null;

            if (SelectedIndex >= 0)
            {
                retValue = mTestList.Items[SelectedIndex].Tag as Test;
            }

            return retValue;
        }

        private int SelectedIndex
        {
            get
            {
                int retValue = -1;

                if (mTestList.SelectedIndices.Count == 1)
                {
                    retValue = mTestList.SelectedIndices[0];
                }

                return retValue;
            }
        }

        private void NextTest()
        {
            if (SelectedIndex < mTestList.Items.Count - 1)
            {
                SelectTest(SelectedIndex + 1);
            }
            else
            {
                GetSelectedTest().Focus();
            }
        }

        private void PreviousTest()
        {
            if (SelectedIndex > 0)
            {
                SelectTest(SelectedIndex - 1);
            }
        }

        #region UPDATE GUI STATE

        private void TestStateChanged(object sender, Test.TestState state)
        {
            switch (state)
            {
                case Test.TestState.BLOCKED:
                case Test.TestState.READY:
                case Test.TestState.RUNNING:
                    break;
                case Test.TestState.PENDING:
                    if (mProtocolTimer.Enabled)
                    {
                        Log.Debug("Protocol timer disabled");
                        mProtocolTimer.Enabled = false;
                    }
                    break;
                case Test.TestState.COMPLETED:
                    NextTest();
                    break;
            }

            Log.Debug("[MainWindow: State Changed {0}", state.ToString());
            UpdateGUI();
            UpdateAdvice();
        }


        private void DeviceStateChanged(object sender, bool state)
        {
            Log.Debug("MainWindow: DeviceStateChanged [ {0} ]", state);

            if (!state)
            {
                GetSelectedTest()?.Abort();
            }

            UpdateConnectedStatus();
            UpdateGuiState();
            UpdateAdvice();
        }

        private void UpdateAdvice()
        {
            if (DeviceManager.Connected)
            {
                if (SelectedIndex >= 0)
                {
                    var test = GetSelectedTest();

                    switch (test.State)
                    {
                        case Test.TestState.READY:
                        case Test.TestState.COMPLETED:
                            if (DeviceManager.DeviceState == StatusMessage.State.STATE_EMERGENCY)
                            {
                                SetAdviceText("Cannot start a test, EMERGENCY BUTTON is activated");
                            }
                            else
                            {
                                if (!DeviceManager.PowerOn)
                                {
                                    SetAdviceText("Please turn on the CPAR device.");
                                }
                                else if (!DeviceManager.VASMeterConnected)
                                {
                                    SetAdviceText("Please connect the VAS meter.");
                                }
                                else if (!DeviceManager.ScoreIsLow)
                                {
                                    SetAdviceText("Please set the VAS score to 0.0cm");
                                }
                                else if (DeviceManager.CompressorRunning)
                                {
                                    SetAdviceText("Please wait for the compressor to turn off.");
                                }
                                else
                                {
                                    if ((test.ExternalParameters.Length > 0) && test.IsBlocked())
                                    {
                                        SetAdviceText("Please enter external parameters (Setup buttom)");
                                    }
                                    else
                                    {
                                        SetAdviceText("");
                                    }
                                }
                            }
                            break;
                        case Test.TestState.BLOCKED:
                            {
                                SetDependenciesAdvice(test);
                            }
                            break;
                        case Test.TestState.PENDING:
                            SetAdviceText("Please accept or reject the result of the test.");
                            break;
                        case Test.TestState.RUNNING:
                            SetAdviceText("");
                            break;
                    }
                }
                else
                {
                    SetAdviceText("");
                }
            }
            else
            {
                SetAdviceText("Please connect an CPAR device");
            }
        }

        private int FirstUnmetDependency(Test[] test)
        {
            for (int i = 0; i < test.Length; ++i)
            {
                if (Session.Active.GetResult(test[i].ID) is NullResult)
                {
                    return i;
                }
            }

            return 0;
        }

        private void SetDependenciesAdvice(Test test)
        {
            var dependencies = test.Dependencies;
            var externals = test.ExternalParameters;

            if (dependencies.Length > 0)
            {
                int start = FirstUnmetDependency(dependencies);
                var builder = new StringBuilder("Please run test [");
                builder.Append(dependencies[start]);

                for (int i = start + 1; i < dependencies.Length; ++i)
                {
                    if (Session.Active.GetResult(dependencies[i].ID) is NullResult)
                    {
                        builder.Append(", " + dependencies[i]);
                    }
                }
                builder.Append("] first.");

                SetAdviceText(builder.ToString());
            }
            else if (externals.Length > 0)
            {
                SetAdviceText("Please enter external parameters");
            }
            else
            {
                Log.Debug("Test [{0}] is blocked, but it should not be blocked", test.Name);
            }
        }

        private void SetAdviceText(string text)
        {
            if (mAdvice.Text != text)
            {
                mAdvice.Text = text;
            }
        }

        private int statusUpdateCounter = 20;

        private void OnStatusMessage(object sender, StatusMessage msg)
        {
            ++statusUpdateCounter;

            if (statusUpdateCounter > 5)
            {
                UpdateVAS(msg.VasScore);
                statusUpdateCounter = 0;
            }

            UpdateGuiState();
            UpdateAdvice();

            if (debugWnd != null)
            {
                debugWnd.Data = msg;
            }

            LogDeviceState(msg);
        }

        private StatusMessage.StopCondition oldCondition = StatusMessage.StopCondition.STOPCOND_NO_CONDITION;

        private void LogDeviceState(StatusMessage msg)
        {
            if (oldCondition != msg.Condition)
            {
                switch (msg.Condition)
                {
                    case StatusMessage.StopCondition.STOPCOND_12V_POWER_OFF:
                        Log.Error("Power to the CPAR Device was turned off while running a test (STOPCOND_12V_POWER_OFF)");
                        break;
                    case StatusMessage.StopCondition.STOPCOND_COMM_WATCHDOG:
                        Log.Error("Internal communication failure, the communication watchdog was not service while running a test (STOPCOND_COMM_WATCHDOG)");
                        break;
                    case StatusMessage.StopCondition.STOPCOND_EMERGENCY_STOP_ACTIVATED:
                        Log.Error("Emergency button was activated while running a test (STOPCOND_EMERGENCY_STOP_ACTIVATED)");
                        break;
                    case StatusMessage.StopCondition.STOPCOND_MAXIMAL_TIME_EXCEEDED:
                        Log.Error("The maximal stimulation time (10min) was exceeded, please reduce the duration of the stimuli in order to run this protocol (STOPCOND_MAXIMAL_TIME_EXCEEDED)");
                        break;
                    case StatusMessage.StopCondition.STOPCOND_VASMETER_DISCONNECTED:
                        Log.Error("The VAS meter was accidently disconnected while running a test (STOPCOND_VASMETER_DISCONNECTED)");
                        break;
                    case StatusMessage.StopCondition.STOPCOND_OUT_OF_COMPLIANCE:
                        Log.Error("The CPAR device could not deliver the requested stimulation pressure (STOPCOND_OUT_OF_COMPLIANCE)");
                        break;
                    default:
                        break;
                }
            }

            oldCondition = msg.Condition;
        }

        private void UpdateVAS(double vas)
        {
            if (DeviceManager.VASMeterConnected)
            {
                mVASScore.Text = String.Format("{0:0.0}cm", vas);
            }
            else
            {
                mVASScore.Text = "N/A";
            }
        }

        private void UpdateViewStatus()
        {
            instructionsToolStripMenuItem.CheckState = mTabControl.SelectedIndex == 0 ? CheckState.Checked : CheckState.Unchecked;
            mLogMenuItem.CheckState = mTabControl.SelectedIndex == 1 ? CheckState.Checked : CheckState.Unchecked;
            mResultsMenuItem.CheckState = mTabControl.SelectedIndex == 2 ? CheckState.Checked : CheckState.Unchecked;
        }

        private void UpdateConnectedStatus()
        {
            mConnectedStatus.Text = DeviceManager.Connected ?
                                    String.Format("CONNECTED ({0} / Serial number: {1})", SystemSettings.SerialPort, DeviceManager.SerialNumber) :
                                    String.Format("NOT CONNECTED ({0})", SystemSettings.SerialPort);
        }

        #endregion
        #region EVENT HANDLERS

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTabControl.SelectedIndex = 0;
            mTestList.Focus();
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTabControl.SelectedIndex = 1;
            mComment.Focus();
        }

        private void resultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTabControl.SelectedIndex = 2;
            mTestList.Focus();
        }

        private void mTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateViewStatus();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextTest();
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviousTest();
        }

        private void mStartBtn_Click(object sender, EventArgs e)
        {
            var test = GetSelectedTest();

            if (test != null)
            {
                var dependents = test.Dependents;

                if ((test.State == Test.TestState.COMPLETED) && (dependents.Length > 0))
                {
                    if (MessageBox.Show(CreateInvalidationMessage(dependents),
                                        "Rerun test: " + test.Name,
                                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        StartTest(test);
                    }
                }
                else if (test.State == Test.TestState.COMPLETED)
                {
                    if (MessageBox.Show("Rerunning the test will delete its previously measured results, if and when the results are accepted",
                                        "Rerun test: " + test.Name,
                                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        StartTest(test);
                    }
                }
                else
                {
                    StartTest(test);
                }
            }
        }

        private void StartTest(Test test)
        {
            DeviceManager.ProcessMessages();
            test.Start();

            if (test.RequireTimer)
            {
                mProtocolTimer.Interval = test.Period;
                mProtocolTimer.Enabled = true;
                Log.Debug("Protocol timer enabled: Interval: {0}", test.Period);
            }
        }

        private string CreateInvalidationMessage(Test[] dependents)
        {
            StringBuilder builder = new StringBuilder("Rerunning the test will invalidate and delete the results of the following tests: ");

            for (int i = 0; i < dependents.Length; ++i)
            {
                builder.Append(dependents[i].Name);

                if (i != dependents.Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(" if the results are accpeted");

            return builder.ToString();
        }


        private void mAbortBtn_Click(object sender, EventArgs e)
        {
            GetSelectedTest()?.Abort();
        }

        private void mAcceptBtn_Click(object sender, EventArgs e)
        {
            GetSelectedTest()?.Accept();
        }

        private void mComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (mComment.Text.Length > 0)
                {
                    Log.Status("[ {0} ] {1}", Experimenter.Active.Name, mComment.Text);
                }

                mComment.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void mTestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSelectedTest()?.Focus();
            UpdateGUI();
        }

        private void mSetupBtn_Click(object sender, EventArgs e)
        {
            var test = GetSelectedTest();

            if (test.NumberOfExternalParameters > 0)
            {
                var dialog = new SetupParametersForm(test);

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateGUI();
                }
            }
        }

        #endregion

        private void mTimer_Tick(object sender, EventArgs e)
        {
            DeviceManager.Heartbeat();
        }

        private void mExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Status("SESSION TERMINATED");
        }

        private void mViewSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            mTestList.Columns[0].Width = mViewSplitContainer.Panel1.Width;
        }

        private void mProtocolTimer_Tick(object sender, EventArgs e)
        {
            var test = GetSelectedTest();

            if (test != null)
            {
                test.Run();
                mProtocolTimer.Interval = test.Period;
            }
        }

        private void msgTimer_Tick(object sender, EventArgs e)
        {
            DeviceManager.ProcessMessages();
        }
    }
}
