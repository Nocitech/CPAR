using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Core.Results;
using CPAR.Communication.Messages;
using CPAR.Communication;
using CPAR.Logging;

namespace CPAR.Core.Tests
{
    public class StartleResponseTest : Test
    {
        public enum StartleState
        {
            READY,
            STIMULATING,
            WAITING
        }

        public enum ProbeType
        {
            [XmlEnum("normal-probe")]
            NORMAL,
            [XmlEnum("startle-probe")]
            STARTLE
        }

        public class ProbeParameters
        {
            [XmlAttribute("t_on")]
            public double T_ON { get; set; }
            [XmlAttribute("t_off")]
            public double T_OFF { get; set; }
            [XmlAttribute("t_startle")]
            public double T_STARTLE { get; set; }
            [XmlAttribute("t_response")]
            public double T_RESPONSE { get; set; }
            [XmlAttribute("t_delay")]
            public double T_DELAY { get; set; }
            [XmlAttribute("startle")]
            public ProbeType STARTLE { get; set; }
        }

        [XmlArray("probes")]
        [XmlArrayItem("probe")]
        public ProbeParameters[] Probes { get; set; }

        [XmlIgnore]
        public int NUMBER_OF_STARTLE_RESPONSES
        {
            get;
        }

        [XmlElement("startle-pressure")]
        public CalculatedParameter STARTLE_PRESSURE { get; set; }
        [XmlElement("stimulating-pressure")]
        public CalculatedParameter STIMULATING_PRESSURE { get; set; }
        [XmlAttribute(AttributeName = "second-cuff")]
        public bool SECOND_CUFF { get; set; }
        [XmlAttribute(AttributeName = "primary-cuff")]
        public int PRIMARY_CUFF { get; set; }

        public override bool IsBlocked()
        {
            return false;
        }

        [XmlIgnore]
        private byte SecondaryChannel
        {
            get
            {
                return (byte)(PRIMARY_CUFF == 1 ? 1 : 0);
            }
        }

        protected override Result GetResult()
        {
            return result;
        }

        protected override bool StartTest()
        {
            bool retValue = true;
            result = new StartleResult();

            try
            {
                result.CreateProbes(Probes);
                startlePressure = STARTLE_PRESSURE.Calculate();
                stimulatingPressure = STIMULATING_PRESSURE.Calculate();
                result.StartlePressure = startlePressure;
                result.StimulatingPressure = stimulatingPressure;
                Log.Status("STIM.PRESSURE = {0:0.0}, STARTLE.PRESSURE: {1:0.0}", stimulatingPressure, startlePressure);

                state = StartleState.READY;
                probeNumber = 0;
                Stimulate();
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            return retValue;
        }

        private void Stimulate()
        {
            if (state == StartleState.READY)
            {
                if (probeNumber < Probes.Length)
                {
                    var probe = Probes[probeNumber];

                    try
                    {
                        double Tdelay = probe.T_DELAY;

                        if (result.Probes[probeNumber].IsStartle)
                        {
                            var program01 = CPARDevice.CreateStartleProbe(0,
                                                                           stimulatingPressure,
                                                                           startlePressure,
                                                                           probe.T_ON,
                                                                           probe.T_DELAY,
                                                                           probe.T_STARTLE,
                                                                           probe.T_RESPONSE);
                            var program02 = SECOND_CUFF ? CPARDevice.CreateStartleProbe(1, 
                                                                                        stimulatingPressure, 
                                                                                        startlePressure, 
                                                                                        probe.T_ON, 
                                                                                        probe.T_DELAY, 
                                                                                        probe.T_STARTLE, 
                                                                                        probe.T_RESPONSE) :
                                                          CPARDevice.CreateEmptyProgram(1);
                            DeviceManager.Execute(program01);
                            DeviceManager.Execute(program02);
                            Log.Status("STARTLE PROBE: Tdelay {0}, Pressure: {1:0.0}kPa/{2:0.0}kPa", Tdelay, stimulatingPressure, startlePressure);
                        }
                        else
                        {
                            var program01 = CPARDevice.CreateNormalProbe(0, stimulatingPressure, probe.T_ON, probe.T_RESPONSE);
                            var program02 = SECOND_CUFF ? CPARDevice.CreateNormalProbe(1, stimulatingPressure, probe.T_ON, probe.T_RESPONSE) :
                                                          CPARDevice.CreateEmptyProgram(1);
                            DeviceManager.Execute(program01);
                            DeviceManager.Execute(program02);
                            Log.Status("NORMAL PROBE Pressure: {0:0.0}kPa", stimulatingPressure);
                        }

                        StartDevice(Communication.AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_PRESSED);
                        state = StartleState.STIMULATING;
                        count = 0;
                    }
                    catch (Exception e)
                    {
                        Log.Debug(e.Message);
                    }
                }
                else
                {
                    Log.Debug("Attempted to start a probe that does not exists, ending test");
                    Pending();
                }
            }
            else
            {
                Log.Debug("StartleTest: Attempted to start a stimulation while not in the READY state [State = {0}]", state.ToString());
            }
        }

        private double RunTime
        {
            get
            {
                return CPARDevice.CountToTime(count);
            }
        }

        private bool Complete
        {
            get
            {
                return probeNumber == Probes.Length;
            }
        }

        private void DisplaySignal(StatusMessage msg)
        {
            var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 :
                        msg.ActualPressure01;
            result.Add(force, 0, msg.VasScore);
            Visualizer.Update(force, 0, msg.VasScore);
        }

        private void Next()
        {
            ++probeNumber;

            if (!Complete)
            {
                state = StartleState.WAITING;
                Log.Debug("STIMULATING: STATE CHANGED => WAITING (RunTime: {0:0.00}s, For probe: {1})", RunTime, probeNumber);
            }
            else
            {
                Pending();
            }
        }

        protected override void Process(StatusMessage msg)
        {
            ++count;
            DisplaySignal(msg);

            if (msg.Condition == StatusMessage.StopCondition.STOPCOND_STOP_BUTTON_PRESSED ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_12V_POWER_OFF ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_COMM_WATCHDOG ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_MAXIMAL_TIME_EXCEEDED ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_OUT_OF_COMPLIANCE ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_VASMETER_DISCONNECTED ||
                msg.Condition == StatusMessage.StopCondition.STOPCOND_EMERGENCY_STOP_ACTIVATED)
            {
                Abort();
            }           
            else
            {
                var probe = Probes[probeNumber];

                switch (state)
                {
                    case StartleState.STIMULATING:
                        if (RunTime > probe.T_ON + probe.T_RESPONSE)
                        {
                            result.Probes[probeNumber].VAS = msg.FinalVasScore;
                            Log.Debug("STIMULATING: PROBE RECORDED [{0}] TIME = {1:0.00}s VAS = {2:0.00}", probeNumber, RunTime, msg.FinalVasScore);
                            Log.Status("PROBE RECORDED [{0}] VAS: {1:0.00}, Please set the VAS to zero.", probeNumber, msg.FinalVasScore);
                            Next();
                            count = 0;
                        }
                        break;
                    case StartleState.WAITING:
                        if (!msg.StartPossible)
                        {
                            count = 0;
                        }

                        if (RunTime >= probe.T_OFF)
                        {
                            state = StartleState.READY;
                            Log.Debug("WAITING: Time: {0:0.00}: STATE CHANGED => READY", RunTime);
                        }
                        break;
                    case StartleState.READY:
                        Log.Debug("STARTING NEXT PROBE [{0}]", probeNumber);
                        Stimulate();
                        break;
                }
            }
        }

        protected override void InitializeChart()
        {
            double Tmax = 0;
            Probes.Foreach((p) => Tmax += p.T_ON + p.T_OFF + p.T_RESPONSE + 2);
            Visualizer.Pmax = 100;
            Visualizer.Tmax = Tmax;
            Visualizer.Conditioning = false;
            Visualizer.SecondCuff = SECOND_CUFF;
            Visualizer.PrimaryChannel = 1;
        }

        #region External parameters
        private void BuildExternalParametersList()
        {
            externalParameters = new List<CalculatedParameter>();

            if (STARTLE_PRESSURE.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                externalParameters.Add(STARTLE_PRESSURE);

            if (STIMULATING_PRESSURE.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                externalParameters.Add(STIMULATING_PRESSURE);
        }

        [XmlIgnore]
        public override CalculatedParameter[] ExternalParameters
        {
            get
            {
                if (externalParameters == null)
                {
                    BuildExternalParametersList();
                }

                return externalParameters.ToArray();
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                if (externalParameters == null)
                {
                    BuildExternalParametersList();
                }

                return externalParameters.Count; 
            }
        }

        private List<CalculatedParameter> externalParameters = null;
        #endregion

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                List<Test> dependencies = new List<Test>();

                if (STARTLE_PRESSURE.IsDependent)
                    dependencies.AddRange(STARTLE_PRESSURE.Dependencies);
                if (STIMULATING_PRESSURE.IsDependent)
                    dependencies.AddRange(STIMULATING_PRESSURE.Dependencies);

                return dependencies.ToArray();
            }
        }

        private StartleResult result = null;
        private StartleState state;
        private int probeNumber;
        private int count;
        private double startlePressure;
        private double stimulatingPressure;
    }
}
