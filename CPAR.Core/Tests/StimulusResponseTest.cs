using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Core.Results;
using CPAR.Communication.Functions;
using CPAR.Communication;
using CPAR.Logging;
using CPAR.Communication.Messages;

namespace CPAR.Core.Tests
{
    public enum StopMode
    {
        [XmlEnum("vas-and-button")]
        STOP_ON_VAS_AND_BUTTON,
        [XmlEnum("button")]
        STOP_ONLY_ON_BUTTON
    }

    public class StimulusResponseTest :
        Test
    {
        [XmlAttribute(AttributeName = "vas-pdt")]
        public double VAS_PDT { get; set; }
        [XmlAttribute(AttributeName = "delta-pressure")]
        public double DELTA_PRESSURE { get; set; }
        [XmlAttribute(AttributeName = "stop-mode")]
        public StopMode STOP_MODE { get; set; }
        [XmlAttribute(AttributeName = "pressure-limit")]
        public double PRESSURE_LIMIT { get; set; }
        [XmlAttribute(AttributeName = "second-cuff")]
        public bool SECOND_CUFF { get; set; }
        [XmlAttribute(AttributeName ="primary-cuff")]
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
                return (byte) (PrimaryChannel == 0 ? 1 : 0);
            }
        }

        public override bool IsBlocked()
        {
            return false;
        }

        protected override Result GetResult()
        {
            return result;
        }

        [XmlIgnore]
        public override CalculatedParameter[] ExternalParameters
        {
            get
            {
                return new CalculatedParameter[] { };
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                return 0;
            }
        }

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                return new Test[] { };
            }
        }

        protected override bool StartTest()
        {
            bool retValue = false;

            try
            {
                var program01 = CPARDevice.CreateRampProgram(PrimaryChannel, DELTA_PRESSURE, PRESSURE_LIMIT);
                var program02 = SECOND_CUFF ? CPARDevice.CreateRampProgram(SecondaryChannel, DELTA_PRESSURE, PRESSURE_LIMIT) :
                                              CPARDevice.CreateEmptyProgram(SecondaryChannel);
                DeviceManager.Execute(program01);
                DeviceManager.Execute(program02);
                StartDevice(GetStopCriterion());

                result = new StimulusResponseResult()
                {
                    Name = Name,
                    ID = ID,
                    Index = Index,
                    Conditioned = false,
                    VAS_PDT = VAS_PDT
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

        private AlgometerStopCriterion GetStopCriterion()
        {
            switch (STOP_MODE)
            {
                case StopMode.STOP_ONLY_ON_BUTTON: return AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_PRESSED;
                case StopMode.STOP_ON_VAS_AND_BUTTON: return AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_VAS;
            }

            return AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_PRESSED;
        }

        protected override void Process(StatusMessage msg)
        {
            if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION || initializing)
            {
                var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 : 
                            (PrimaryChannel == 0 ? msg.ActualPressure01 : msg.ActualPressure02);
                result.Add(force, 0, msg.VasScore);
                Visualizer.Update(force, 0, msg.VasScore);
                
                if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION)
                {
                    initializing = false;
                }
            }
            else if (IsValidStopCondition(msg) && !initializing)
            {
                var force = SECOND_CUFF ? (msg.FinalPressure01 + msg.FinalPressure02) / 2 : 
                            (PrimaryChannel == 0 ? msg.FinalPressure01 : msg.FinalPressure02);
                result.Add(force, 0, msg.FinalVasScore);
                Visualizer.Update(force, 0, msg.FinalVasScore);
                Pending();
            }
            else
            {
                Abort();
            }
        }

        private bool IsValidStopCondition(StatusMessage msg)
        {
            bool retValue = false;

            switch (STOP_MODE)
            {
                case StopMode.STOP_ONLY_ON_BUTTON:
                    retValue = (msg.Condition == StatusMessage.StopCondition.STOPCOND_STOP_BUTTON_PRESSED) ||
                               (msg.Condition == StatusMessage.StopCondition.STOPCOND_STIMULATION_COMPLETED);
                    break;

                case StopMode.STOP_ON_VAS_AND_BUTTON:
                    retValue = (msg.Condition == StatusMessage.StopCondition.STOPCOND_MAXIMAL_VAS_SCORED) ||
                               (msg.Condition == StatusMessage.StopCondition.STOPCOND_STIMULATION_COMPLETED);
                    break;
            }

            return retValue;
        }

        protected override void InitializeChart()
        {
            Visualizer.Pmax = PRESSURE_LIMIT;
            Visualizer.Tmax = PRESSURE_LIMIT / DELTA_PRESSURE;
            Visualizer.Conditioning = false;
            Visualizer.SecondCuff = SECOND_CUFF;
            Visualizer.PrimaryChannel = PrimaryChannel + 1;
        }

        StimulusResponseResult result = null;
        bool initializing = false;
    }
}
