using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Communication.Messages;
using CPAR.Communication.Functions;
using CPAR.Logging;
using CPAR.Communication;
using CPAR.Core.Results;

namespace CPAR.Core.Tests
{
    public class StaticTemporalSummationTest :
        Test
    {
        [XmlAttribute("stimulus-duration")]
        public double StimulusDuration { get; set; }
        [XmlAttribute("tail-duration")]
        public double TailDuration { get; set; }
        [XmlElement(ElementName = "pressure")]
        public CalculatedParameter Pressure { get; set; }
        [XmlAttribute(AttributeName = "second-cuff")]
        public bool SECOND_CUFF { get; set; }
        [XmlAttribute(AttributeName = "primary-cuff")]
        public string PRIMARY_CUFF { get; set; }

        [XmlIgnore]
        private byte PrimaryChannel
        {
            get
            {
                byte retValue = 0;

                if (PRIMARY_CUFF != null)
                {
                    if (PRIMARY_CUFF == "2")
                    {
                        retValue = 1;
                    }
                }

                return retValue;
            }
        }

        [XmlIgnore]
        private byte SecondaryChannel
        {
            get
            {
                return (byte)(PrimaryChannel == 0 ? 1 : 0);
            }
        }


        public override bool IsBlocked()
        {
            return !Pressure.IsAvailable();
        }

        protected override Result GetResult()
        {
            return result;
        }

        protected override bool StartTest()
        {
            bool retValue = false;
            var stimulatingPressure = Pressure.Calculate();

            try
            {
                DeviceManager.Execute(CPARDevice.CreateStaticProgram(PrimaryChannel, StimulusDuration, stimulatingPressure));
                DeviceManager.Execute(SECOND_CUFF ? CPARDevice.CreateStaticProgram(SecondaryChannel, StimulusDuration, stimulatingPressure) :
                                                    CPARDevice.CreateEmptyProgram(SecondaryChannel));
                StartDevice(AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_PRESSED);
                Log.Debug("STATIC TS STARTED [Pressure: {0}, Duration: {1}, Both Cuffs: {2}", stimulatingPressure, StimulusDuration, SECOND_CUFF);

                result = new StaticTemporalSummationResult()
                {
                    NominalStimulatingPressure = stimulatingPressure
                };

                initializing = true;
                retValue = true;
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            return retValue;
        }

        protected override void Process(StatusMessage msg)
        {
            if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION || initializing)
            {
                var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 : PrimaryChannel == 0 ? msg.ActualPressure01 : msg.ActualPressure02;
                result.Add(force, 0, msg.VasScore);
                Visualizer.Update(force, 0, msg.VasScore);

                if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION)
                {
                    initializing = false;
                }
            }
            else if ((msg.Condition == StatusMessage.StopCondition.STOPCOND_STIMULATION_COMPLETED || msg.Condition == StatusMessage.StopCondition.STOPCOND_STOP_BUTTON_PRESSED)
                     && !initializing)
            {
                var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 : PrimaryChannel == 0 ? msg.ActualPressure01 : msg.ActualPressure02;
                result.Add(force, 0, msg.VasScore);
                Visualizer.Update(force, 0, msg.VasScore);

                if (result.AbortCount > 0)
                {
                    if (CPARDevice.CountToTime(result.Length - result.AbortCount) >= TailDuration)
                    {
                        Pending();
                    }
                }
                else
                {
                    result.AbortCount = result.Length;
                }
            }
            else
            {
                Abort();
            }
        }

        protected override void InitializeChart()
        {
            if (!Focused)
            {
                var stimulatingPressure = Pressure.IsAvailable() ? Pressure.Calculate() : 100;
                Visualizer.Pmax = 100;
                Visualizer.Tmax = StimulusDuration + TailDuration;
                Visualizer.Conditioning = false;
                Visualizer.SecondCuff = SECOND_CUFF;
                Visualizer.PrimaryChannel = PrimaryChannel + 1;
            }
        }

        #region External parameters
        [XmlIgnore]
        public override CalculatedParameter[] ExternalParameters
        {
            get
            {
                if (externalParameters == null)
                {
                    externalParameters = new List<CalculatedParameter>();

                    if (Pressure.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                        externalParameters.Add(Pressure);
                }

                return externalParameters.ToArray();
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                return Pressure.CalculationType == CalculatedParameter.PressureType.EXTERNAL ? 1 : 0;
            }
        }


        private List<CalculatedParameter> externalParameters = null;
        #endregion

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                return Pressure.IsDependent ? Pressure.Dependencies : new Test[] { };
            }
        }

        StaticTemporalSummationResult result = null;
        private bool initializing = false;
    }
}
