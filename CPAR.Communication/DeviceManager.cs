using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CPAR.Communication;
using CPAR.Communication.Functions;
using System.IO.Ports;
using CPAR.Communication.Messages;
using CPAR.Logging;

namespace CPAR.Communication
{
    public class DeviceManager : IMessageVisitor
    {
        public static event EventHandler<bool> DeviceStateChanged;
        public static event EventHandler<EventMessage.EventID> EventReceived;
        public static event EventHandler<StatusMessage> StatusReceived;

        #region Construction and singleton handling
        public static string PortName { get; set; }

        static DeviceManager()
        {
            PortName = "COM4";
        }

        private DeviceManager(string portName)
        {
            master = new DeviceMaster()
            {
                Timeout = 500,
                PortName = portName,
                BaudRate = 38400
            };
        }

        public static void Initialize()
        {
            if (instance == null)
            {
                instance = new DeviceManager(PortName);
            }
        }

        public static void Execute(Function function)
        {
            if (Connected)
            {
                Instance.master.Execute(function);
            }
        }

        private static DeviceManager Instance
        {
            get
            {
                Initialize();
                return instance;
            }
        }

        #endregion
        #region Properties
        #region Device & Connection Properties
        public static bool Connected
        {
            get
            {
                return Instance.connected;
            }
            private set
            {
                Instance.connected = value;
            }
        }

        public static uint CommCount
        {
            get
            {
                return Instance.kickWatchdog != null ? Instance.kickWatchdog.Counter : 0;
            }
        }

        public static int CommAttempts
        {
            get
            {
                return Instance.commAttemptCount;
            }
        }

        public static StatusMessage.State DeviceState
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.SystemState;
                }
                else
                    return StatusMessage.State.STATE_IDLE;
            }
        }

        public static bool PowerOn
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.PowerOn;
                }
                else
                    return false;
            }
        }

        public static bool ScoreIsLow
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.VasIsLow;
                }
                else
                    return true;
            }
        }

        public static bool VASMeterConnected
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.VasConnected;
                }
                else
                    return false;
            }
        }

        public static bool CompressorRunning
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.CompressorRunning;
                }
                else
                    return false;
            }
        }

        public static bool StartPossible
        {
            get
            {
                if (Instance.statusMessage != null)
                {
                    return Instance.statusMessage.StartPossible;
                }
                else
                    return false;
            }
        }

        public static string SerialNumber
        {
            get
            {
                if (Instance.devIdentification != null)
                {
                    return Instance.devIdentification.SerialNumber.ToString();
                }
                else
                    return "UNKNOWN";
            }
        }

        #endregion
        #endregion
        #region Handle device watchdog and device discovery
        public static void Heartbeat()
        {
            Instance.DoHeartbeat();
        }

        private void DoHeartbeat()
        {
            if (!Connected)
            {
                ScanPort();
            }
            else
            {
                KickDevice();
            }
        }

        public static void ProcessMessages()
        {
            Instance.DoProcessMessages();
        }

        private void DoProcessMessages()
        {
            while (master.IsMessagePending)
            {
                var message = Message.CreateMessage(master.GetMessage());
                message?.Visit(this);
            }
        }

        private void ScanPort()
        {
            try
            {
                if (!initComm)
                {
                    Log.Debug("Opening port: {0}", PortName);
                    master.Open(PortName);
                    initComm = true;

                    if (!master.IsOpen)
                        throw new ArgumentException("port is not open");
                }
                else
                {
                    Log.Debug("Scanning port: {0}", PortName);
                    master.Execute(devIdentification);
                    Connected = true;
                    initComm = false;
                    Log.Debug("Connected to port {0}", PortName);
                    DeviceStateChanged?.Invoke(this, Connected);
                }
            }
            catch (Exception e)
            {
                ++commAttemptCount;
                Log.Debug("SCANPORT FAILED: {0}", e.Message);
                initComm = false;

                if (master.IsOpen)
                    master.Close();
            }
        }

        private void KickDevice()
        {
            if (master.IsOpen)
            {
                try
                {
                    master.Execute(kickWatchdog);

                    if (wdCounter > kickWatchdog.Counter)
                    {
                        wdCounter = kickWatchdog.Counter;
                        Log.Error("The CPAR device has experienced a malfunction and has restarted. Please check the device for abnormal behaviour before continuing with its use");
                    }
                    commAttemptCount = 0;
                }
                catch { }
            }
            else
            {
                if (Connected)
                {
                    Log.Debug("DEVICE DISCONNECTED");
                    Connected = false;
                    wdCounter = 0;
                    DeviceStateChanged?.Invoke(this, Connected);
                }
            }
        }

        #endregion

        public void Accept(EventMessage msg)
        {
            EventReceived?.Invoke(this, msg.Event);
        }

        public void Accept(StatusMessage msg)
        {
            statusMessage = msg;
            StatusReceived?.Invoke(this, msg);
        }

        private DeviceMaster master;
        private bool connected = false;
        private KickWatchdog kickWatchdog = new KickWatchdog();
        private DeviceIdentification devIdentification = new DeviceIdentification();
        private StatusMessage statusMessage = null;
        private int commAttemptCount = 0;
        private uint wdCounter = 0;
        private static DeviceManager instance;
        private bool initComm = false;
    }
}
