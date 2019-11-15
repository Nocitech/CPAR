using CPAR.Communication;
using CPAR.Communication.Functions;
using CPAR.Core.Results;
using CPAR.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Communication.Messages;

namespace CPAR.Core.Tests
{
    public class ConditionedPainTest :
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
        [XmlElement("cond-pressure")]
        public CalculatedParameter COND_PRESSURE { get; set; }
        [XmlAttribute(AttributeName = "delta-cond-pressure")]
        public double DELTA_COND_PRESSURE { get; set; }

        public override bool IsBlocked()
        {
            return !COND_PRESSURE.IsAvailable();
        }

        protected override Result GetResult()
        {
            return result;
        }

        protected override bool StartTest()
        {
            bool retValue = true;
            var conditioningPressure = COND_PRESSURE.Calculate();

            try
            {
                DeviceManager.Execute(CPARDevice.CreateDelayedRampProgram(0,DELTA_COND_PRESSURE, conditioningPressure, DELTA_PRESSURE, PRESSURE_LIMIT));
                DeviceManager.Execute(CPARDevice.CreateConditioningProgram(1, DELTA_COND_PRESSURE, conditioningPressure, DELTA_PRESSURE, PRESSURE_LIMIT));
                StartDevice(GetStopCriterion());

                result = new ConditionedPainResult()
                {
                    Name = Name,
                    ID = ID,
                    Index = Index,
                    Conditioned = true,
                    NominalConditioningPressure = conditioningPressure,
                    VAS_PDT = VAS_PDT
                };

                initializing = true;
                retValue = true;
            } catch (Exception e)
            {
                Log.Debug(e.Message);
            }

            return retValue;
        }

        private StartStimulation.StopCriterion GetStopCriterion()
        {
            switch (STOP_MODE)
            {
                case StopMode.STOP_ONLY_ON_BUTTON: return StartStimulation.StopCriterion.STOP_CRITERION_ON_BUTTON;
                case StopMode.STOP_ON_VAS_AND_BUTTON: return StartStimulation.StopCriterion.STOP_CRITERION_ON_BUTTON_VAS;
            }

            return StartStimulation.StopCriterion.STOP_CRITERION_ON_BUTTON;
        }

        protected override void InitializeChart()
        {
            var conditioningPressure = COND_PRESSURE.Calculate();
            Visualizer.Pmax = conditioningPressure > PRESSURE_LIMIT ? conditioningPressure : PRESSURE_LIMIT;
            Visualizer.Tmax = conditioningPressure / DELTA_COND_PRESSURE + PRESSURE_LIMIT / DELTA_PRESSURE;
            Visualizer.Conditioning = true;
            Visualizer.SecondCuff = false;
            Visualizer.PrimaryChannel = 1;
        }

        protected override void Process(StatusMessage msg)
        {
            if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION || initializing)
            {
                result.Add(msg.ActualPressure01, msg.ActualPressure02, msg.VasScore);
                Visualizer.Update(msg.ActualPressure01, msg.ActualPressure02, msg.VasScore);

                if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION)
                {
                    initializing = false;
                }
            }
            else if (IsValidStopCondition(msg) && !initializing)
            {
                result.Add(msg.FinalPressure01, msg.FinalPressure02, msg.FinalVasScore);
                Visualizer.Update(msg.FinalPressure01, msg.FinalPressure02, msg.FinalVasScore);
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
        private ConditionedPainResult result;
        private bool initializing = false;

        #region External parameters
        [XmlIgnore]
        public override CalculatedParameter[] ExternalParameters
        {
            get
            {
                if (externalParameters == null)
                {
                    externalParameters = new List<CalculatedParameter>();

                    if (COND_PRESSURE.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                        externalParameters.Add(COND_PRESSURE);
                }

                return externalParameters.ToArray();
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                return COND_PRESSURE.CalculationType == CalculatedParameter.PressureType.EXTERNAL ? 1 : 0;
            }
        }

        private List<CalculatedParameter> externalParameters = null;
        #endregion

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                return COND_PRESSURE.IsDependent ? COND_PRESSURE.Dependencies : new Test[] { };
            }
        }
    }
}
