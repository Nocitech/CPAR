using CPAR.Core.Results;
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

namespace CPAR.Core.Tests
{
    public class TemporalSummationTest :
        Test
    {
        [XmlAttribute(AttributeName = "no-of-stimuli")]
        public byte NO_OF_STIMULI { get; set; }
        [XmlAttribute(AttributeName = "t-on")]
        public double T_ON { get; set; }
        [XmlAttribute(AttributeName = "t-off")]
        public double T_OFF { get; set; }
        [XmlElement(ElementName = "pressure-stimulate")]
        public CalculatedParameter P_STIMULATE { get; set; }
        [XmlAttribute(AttributeName = "pressure-static")]
        public double P_STATIC { get; set; }
        [XmlAttribute(AttributeName = "second-cuff")]
        public bool SECOND_CUFF { get; set; }

        public override bool IsBlocked()
        {
            return !P_STIMULATE.IsAvailable();
        }

        protected override Result GetResult()
        {
            return result;
        }

        protected override bool StartTest()
        {
            bool retValue = false;
            var stimulatingPressure = P_STIMULATE.Calculate();

            try
            {
                DeviceManager.Execute(CPARDevice.CreatePulseProgram(0, NO_OF_STIMULI, T_ON, T_OFF, stimulatingPressure, P_STATIC));
                DeviceManager.Execute(SECOND_CUFF ? CPARDevice.CreatePulseProgram(1, NO_OF_STIMULI, T_ON, T_OFF, stimulatingPressure, P_STATIC) :
                                                    CPARDevice.CreateEmptyProgram(1));
                StartDevice(AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_PRESSED);
                Log.Debug("TS STARTED [NO: {0}, T_ON: {1}, T_OFF: {2}", NO_OF_STIMULI, T_ON, T_OFF);

                result = new TemporalSummationResult(NO_OF_STIMULI)
                {
                    Name = Name,
                    ID = ID,
                    Index = Index,
                    Conditioned = false,
                    T_OFF = T_OFF,
                    T_ON = T_ON,
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
                var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 : msg.ActualPressure01;
                result.Add(force, 0, msg.VasScore);
                Visualizer.Update(force, 0, msg.VasScore);

                if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION)
                {
                    initializing = false;
                }
            }
            else if (msg.Condition == StatusMessage.StopCondition.STOPCOND_STIMULATION_COMPLETED && !initializing)
            {
                var force = SECOND_CUFF ? (msg.FinalPressure01 + msg.FinalPressure02) / 2 : msg.ActualPressure01;
                result.Add(force, 0, msg.FinalVasScore);
                Visualizer.Update(force, 0, msg.FinalVasScore);
                Pending();
            }
            else
            {
                Abort();
            }
        }

        private bool initializing = false;

        protected override void InitializeChart()
        {
            if (!Focused)
            {
                var stimulatingPressure = P_STIMULATE.IsAvailable() ? P_STIMULATE.Calculate() : 100;
                Visualizer.Pmax = 100;
                Visualizer.Tmax = NO_OF_STIMULI * (T_ON + T_OFF);
                Visualizer.Conditioning = false;
                Visualizer.SecondCuff = SECOND_CUFF;
                Visualizer.PrimaryChannel = 1;
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

                    if (P_STIMULATE.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                        externalParameters.Add(P_STIMULATE);
                }

                return externalParameters.ToArray();
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                return P_STIMULATE.CalculationType == CalculatedParameter.PressureType.EXTERNAL ? 1 : 0;
            }
        }


        private List<CalculatedParameter> externalParameters = null;
        #endregion

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                return P_STIMULATE.IsDependent ? P_STIMULATE.Dependencies : new Test[] { };
            }
        }

        TemporalSummationResult result = null;
    }
}
