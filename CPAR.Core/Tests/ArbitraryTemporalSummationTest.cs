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
using CPAR.Core;

namespace CPAR.Core.Tests
{
    public class ArbitraryTemporalSummationTest :
        Test
    {
        public class TemporalStimulus
        {
            [XmlAttribute(AttributeName = "t-on")]
            public double T_ON { get; set; }
            [XmlAttribute(AttributeName = "t-off")]
            public double T_OFF { get; set; }

            [XmlElement("intensity")]
            public CalculatedParameter Intensity { get; set; }
        }

        [XmlIgnore]
        public int NO_OF_STIMULI
        {
            get
            {
                int retValue = 0;

                if (Stimuli != null)
                {
                    retValue = Stimuli.Length;
                }

                return retValue;
            }
        }

        [XmlArray("stimuli")]
        [XmlArrayItem("stimulus")]
        public TemporalStimulus[] Stimuli { get; set; }

        [XmlAttribute(AttributeName = "pressure-static")]
        public double P_STATIC { get; set; }
        [XmlAttribute(AttributeName = "second-cuff")]
        public bool SECOND_CUFF { get; set; }

        public override bool IsBlocked()
        {
            if (NO_OF_STIMULI > 0)
            {
                bool retValue = false;

                foreach (var s in Stimuli)
                {
                    if (!s.Intensity.IsAvailable())
                    {
                        retValue = true;
                    }

                }

                return retValue;
            }
            else
            {
                return true;
            }
        }

        protected override Result GetResult()
        {
            return result;
        }

        private void Stimulate()
        {
            if (result != null)
            {
                if (currentStimulus < result.Responses.Length)
                {
                    var r = result.Responses[currentStimulus];

                    DeviceManager.Execute(CPARDevice.CreatePulseProgram(0, 1, r.T_ON, r.T_OFF, r.Pressure, P_STATIC));
                    DeviceManager.Execute(SECOND_CUFF ? CPARDevice.CreatePulseProgram(1, 1, r.T_ON, r.T_OFF, r.Pressure, P_STATIC) :
                                                        CPARDevice.CreateEmptyProgram(1));
                    ForceStartDevice(ForceStartStimulation.StopCriterion.STOP_CRITERION_ON_BUTTON);
                    Log.Debug("TS STARTED [NO: {0}, T_ON: {1}, T_OFF: {2}", currentStimulus, r.T_ON, r.T_OFF);
                    initializing = true;
                }
            }
        }


        protected override bool StartTest()
        {
            bool retValue = false;

            try
            {
                result = new ArbitraryTemporalSummationResult(NO_OF_STIMULI)
                {
                    Name = Name,
                    ID = ID,
                    Index = Index,
                    Conditioned = false
                };

                for (int n = 0; n < Stimuli.Length; ++n)
                {
                    result.Responses[n] = new ArbitraryTemporalSummationResult.Response()
                    {
                        Pressure = Stimuli[n].Intensity.Calculate(),
                        VAS = 0,
                        T_ON = Stimuli[n].T_ON,
                        T_OFF = Stimuli[n].T_OFF
                    };
                }
                currentStimulus = 0;
                Stimulate();

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
            var force = SECOND_CUFF ? (msg.ActualPressure01 + msg.ActualPressure02) / 2 : msg.ActualPressure01;
            result.Add(force, 0, msg.VasScore);
            Visualizer.Update(force, 0, msg.VasScore);

            if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION || initializing)
            {
                if (msg.Condition == StatusMessage.StopCondition.STOPCOND_NO_CONDITION)
                {
                    initializing = false;
                }
            }
            else if (msg.Condition == StatusMessage.StopCondition.STOPCOND_STIMULATION_COMPLETED && !initializing)
            {
                result.Responses[currentStimulus].VAS = msg.VasScore;
                ++currentStimulus;

                if (currentStimulus < result.NumberOfStimuli)
                {
                    try
                    {
                        Stimulate();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                        Abort();
                    }
                }
                else
                {
                    Pending();
                }
            }
            else
            {
                Abort();
            }
        }

        private bool initializing = false;

        private double TestDuration
        {
            get
            {
                return Stimuli.Sum((s) => s.T_ON + s.T_OFF);
            }
        }

        protected override void InitializeChart()
        {
            if (!Focused)
            {
                Visualizer.Pmax = 100;
                Visualizer.Tmax =TestDuration;
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

                    Stimuli.Foreach((s) =>
                    {
                        if (s.Intensity.CalculationType == CalculatedParameter.PressureType.EXTERNAL)
                            externalParameters.Add(s.Intensity);
                    });
                }

                return externalParameters.ToArray();
            }
        }

        [XmlIgnore]
        public override int NumberOfExternalParameters
        {
            get
            {
                return Stimuli.Sum((s) => s.Intensity.CalculationType == CalculatedParameter.PressureType.EXTERNAL ? 1 : 0);
            }
        }


        private List<CalculatedParameter> externalParameters = null;
        #endregion

        private bool IsTestIncluded(List<Test> tests, Test test)
        {
            return tests.Count((t) => t.ID == test.ID) > 0;
        }

        [XmlIgnore]
        public override Test[] Dependencies
        {
            get
            {
                List<Test> tests = new List<Test>();

                Stimuli.Foreach((s) =>
                {
                    if (s.Intensity.IsDependent)
                    {
                        s.Intensity.Dependencies.Foreach((d) =>
                        {
                            if (!IsTestIncluded(tests, d))
                            {
                                tests.Add(d);
                            }
                        });
                    }
                });

                return tests.ToArray();
            }
        }

        private ArbitraryTemporalSummationResult result = null;
        private int currentStimulus = 0;
    }
}
