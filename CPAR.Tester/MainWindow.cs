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
using CPAR.Communication.Functions;
using CPAR.Communication.Messages;
using System.IO.Ports;
using CPAR.Logging;
using System.IO;
using CPAR.Core;

namespace CPAR.Tester
{
    public partial class MainWindow : 
        Form,
        IMessageVisitor
    {
        #region Data Members
        private static readonly string BasePath = @"c:\\CPAR";
        private static readonly string LogPath = Path.Combine(BasePath, "recordings");
        private string logFilename;
        private bool doLogging = false;

        private DeviceMaster master;
        private Logger logger;
        private bool msgReceived = false;
        private TestScript script = null;
        private bool initComm = false;
        private DeviceID devID = DeviceID.UNKNOWN;
        #endregion
        #region Create Object
        public MainWindow()
        {
            InitializeComponent();
            Text = String.Format("{0} ({1})", Text, SystemSettings.VersionInformation);

            SetupComm();
            SetupUserInterface();
            SetupStates();

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            logger = new Logger() { Box = mLogWindow };
            Log.SetLogger(logger);
            Log.Level = LogLevel.DEBUG;
            Width = 1000;
            Height = 900;
            StartPosition = FormStartPosition.CenterScreen;
            mSplitContainerVertical.SplitterDistance = 200;
        }

        private void SetupUserInterface()
        {
            var names = SerialPort.GetPortNames();
            mPortList.Items.AddRange(names);

            if (names.Length > 0)
            {
                mPortList.SelectedIndex = 0;
                master.PortName = names[0];
            }

            mAutoKick.Text = mAutoKick.Items[1].ToString();
        }

        private void SetupComm()
        {
            master = new DeviceMaster()
            {
                Timeout = 500,
                PortName = "COM1",
                BaudRate = 38400
            };
        }

        private void SetupStates()
        {
            if (master.IsOpen)
            {
                mConnectBtn.Enabled = false;
                mDisconnectBtn.Enabled = true;
                scriptButton.Enabled = true;
            }
            else
            {
                mConnectBtn.Enabled = true;
                mDisconnectBtn.Enabled = false;
                scriptButton.Enabled = false;
            }
        }

        #endregion
        #region Data Logging
        private string GenerateFileName()
        {
            var time = DateTime.Now;
            var filename = String.Format("D{0}-{1}-{2}_T{3}h{4}m{5}s", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            return Path.Combine(LogPath, filename + ".csv");
        }

        private void DoLogging(StatusMessage msg)
        {
            if (doLogging)
            {
                var line = String.Format("{0};{1};{2};{3};{4};{5}",
                                         msg.VasScore,
                                         msg.ActualPressure01,
                                         msg.ActualPressure02,
                                         msg.SupplyPressure,
                                         msg.TargetPressure01,
                                         msg.TargetPressure02) + System.Environment.NewLine;
                File.AppendAllText(logFilename, line);
            }
        }

        private void logBtn_Click(object sender, EventArgs e)
        {
            doLogging = !doLogging;

            if (doLogging)
            {
                logFilename = GenerateFileName();
                File.AppendAllText(logFilename, "VAS;ActualPressure01;ActualPressure02;SupplyPressure;TargetPressure01;TargetPressure02" + System.Environment.NewLine);
            }
            UpdateLoggingBtn();
        }

        private void UpdateLoggingBtn()
        {
            logBtn.Text = doLogging ? "ON" : "OFF";
        }
        #endregion
        #region Handle Connections

        private void Connect()
        {
            try
            {
                master.PortName = mPortList.Items[mPortList.SelectedIndex].ToString();
                Log.Debug("Opening port: {0}", master.PortName);
                master.Open();
                msgReceived = false;
                timer.Start();
                initComm = true;
                devID = DeviceID.UNKNOWN;
                msgTimer.Enabled = true;
                Log.Status("Trying to connect");
            }
            catch (Exception ex)
            {
                Log.Error("Problem opening port: " + ex.Message);
            }

            SetupStates();
        }

        private void Disconnect()
        {
            mFunctionList.Items.Clear();
            master.Close();
            SetupStates();
            Log.Status("Disconnected");
            timer.Stop();
            msgTimer.Enabled = false;
        }

        private void SetupFunctions()
        {
            mFunctionList.Items.Clear();
            mFunctionList.Items.AddRange(FunctionFactory.GetFunctions());

            if (mFunctionList.Items.Count > 0)
            {
                mFunctionList.SelectedIndex = 0;
                mPropertyGrid.SelectedObject = mFunctionList.SelectedItem;
            }
        }

        #endregion
        #region Execution of Functions

        private bool Execute(Function function, bool doLogging)
        {
            bool retValue = false;

            try
            {
                if (function != null)
                {
                    if (doLogging)
                        Log.Status("Transmitting {0}", function.ToString());

                    master.Execute(function);

                    if (doLogging)
                    {
                        Log.Status("Completed");
                        mPropertyGrid.Refresh();
                    }
                }

                retValue = true;
            }
            catch (Exception e)
            {
                if (doLogging)
                    Log.Error("EXCEPTION [" + e.ToString() + " ] ");
            }

            return retValue;
        }

        #endregion
        #region Handle UI Events

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
                tabControl1.SelectedIndex = 0;
            if (keyData == Keys.F2)
                tabControl1.SelectedIndex = 1;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void mConnectBtn_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void mDisconnectBtn_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void mFunctionList_DoubleClick(object sender, EventArgs e)
        {
            if (mDisconnectBtn.Enabled)
                Execute((Function)mFunctionList.SelectedItem, true);
            else
                Log.Status("Please connect first to a device");
        }

        private void mFunctionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mFunctionList.SelectedItem != null)
            {
                mPropertyGrid.SelectedObject = mFunctionList.SelectedItem;
            }

        }

        #endregion
        #region IMessageVisitor

        public void Accept(StatusMessage statusMsg)
        {
            DoLogging(statusMsg);
            updateView.SelectedObject = statusMsg;
        }

        public void Accept(EventMessage eventMsg)
        {
            if (eventMsg.Event == EventID.EVT_WAVEFORMS_COMPLETED & autoStart)
            {
                Execute(FunctionFactory.GetFunction(typeof(StartStimulation)), true);
            }
        }

        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!msgReceived)
            {
                Log.Debug("Message Not Received");
                if (devID == DeviceID.CPAR)
                {
                    Disconnect();
                }
            }

            if (initComm)
            {
                try
                {
                    Log.Debug("Initializing connection");
                    DeviceIdentification devFunction = (DeviceIdentification) FunctionFactory.GetFunction(typeof(DeviceIdentification));
                    Execute(devFunction, true);
                    devID = DeviceID.CPAR;
                    SetupFunctions();
                    DeviceType.Text = "Device: " + devID.ToString();
                    Log.Debug(DeviceType.Text);

                    initComm = false;
                    Log.Status("Connected");

                }
                catch
                {
                    Disconnect();
                }
            }
            else
            {
                msgReceived = false;

                if (mAutoKick.Text == mAutoKick.Items[1].ToString())
                {
                    var kickWatchdog = new KickWatchdog();
                    Execute(kickWatchdog, false);

                    if (wdCounter > kickWatchdog.Counter)
                    {
                        Log.Error("CPAR Device has reset");
                        wdCounter = kickWatchdog.Counter;
                    }
                }
            }
        }

        private uint wdCounter = 0;

        private void mPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            mPropertyGrid.Refresh();
        }

        private void scriptTimer_Tick(object sender, EventArgs e)
        {
            if (script != null)
            {
                if (script.Running)
                {
                    var function = script.Next();

                    if (function != null)
                    {
                        try
                        {
                            if (Execute(function, true))
                            {
                                Log.Status(function.SerializeResponse());
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    scriptTimer.Enabled = false;
                }
            }
        }

        private void scriptButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                DefaultExt = "xml",
                Filter = "Script Files (*.xml)|*.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    script = TestScript.Load(dialog.FileName);
                    script.Start();
                    scriptTimer.Enabled = true;
                }
                catch
                {

                }
            }
        }

        private bool autoStart = false;

        private void autoStartBtn_Click(object sender, EventArgs e)
        {
            autoStart = !autoStart;
            autoStartBtn.Text = autoStart ? "ON" : "OFF";
        }

        private void msgTimer_Tick(object sender, EventArgs e)
        {
            while (master.IsMessagePending)
            {
                var message = CPAR.Communication.Message.CreateMessage(master.GetMessage());
                message?.Visit(this);
                msgReceived = true;
            }
        }
    }
}
