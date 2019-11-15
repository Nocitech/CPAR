using CPAR.Core.Results;
using CPAR.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public class CalculatedParameter
    {
        public enum PressureType
        {
            [XmlEnum("constant")]
            CONSTANT,
            [XmlEnum("vas")]
            VAS,
            [XmlEnum("range")]
            RANGE,
            [XmlEnum("external")]
            EXTERNAL,
            [XmlEnum("ptt-percentile")]
            PTT_PERCENTILE,
            [XmlEnum("pdt-percentile")]
            PDT_PERCENTIKE,
        }

        private double value;
        private bool externallySpecified = false;

        [XmlAttribute(AttributeName = "test-id")]
        public string TestID { get; set; }

        [XmlAttribute(AttributeName = "calculation-type")]        
        public PressureType CalculationType { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        [XmlIgnore]
        public bool ExternallySpecified
        {
            get
            {
                return externallySpecified;
            }
            set
            {
                externallySpecified = value;
            }
        }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        public bool IsAvailable()
        {
            bool retValue = false;

            switch (CalculationType)
            {
                case PressureType.CONSTANT:
                    retValue = true;
                    break;
                case PressureType.EXTERNAL:
                    retValue = externallySpecified;
                    break;
                case PressureType.RANGE:
                    {
                        var result = GetResults();
                        retValue = true;
                        result.Foreach((r) =>
                        {
                            if (retValue)
                            {
                                if (r is StimulusResponseResult)
                                {
                                    if ((r.PDT == 0) || (r.PTT == 0))
                                    {
                                        retValue = false;
                                    }
                                }
                                else
                                {
                                    retValue = false;
                                }
                            }
                        });
                    }
                    break;               
                case PressureType.VAS:
                    {
                        var result = GetResults();
                        retValue = true;
                        result.Foreach((r) =>
                        {
                            if (r is StimulusResponseResult)
                            {
                                retValue = retValue ? r.IsScoreAvailable(Value) : false;
                            }
                        });
                    }
                    break;
                case PressureType.PDT_PERCENTIKE:
                    {
                        var result = GetResults();
                        retValue = true;
                        result.Foreach((r) =>
                        {
                            if (retValue)
                            {
                                if (r is StimulusResponseResult)
                                {
                                    if (r.PDT == 0)
                                    {
                                        retValue = false;
                                    }
                                }
                                else
                                {
                                    retValue = false;
                                }
                            }
                        });
                    }
                    break;
                case PressureType.PTT_PERCENTILE:
                    {
                        var result = GetResults();
                        retValue = true;
                        result.Foreach((r) =>
                        {
                            if (retValue)
                            {
                                if (r is StimulusResponseResult)
                                {
                                    if (r.PTT == 0)
                                    {
                                        retValue = false;
                                    }
                                }
                                else
                                {
                                    retValue = false;
                                }
                            }
                        });
                    }
                    break;
            }

            return retValue;
        }

        public double Calculate()
        {
            double retValue = 0;

            if (IsAvailable())
            {
                switch (CalculationType)
                {
                    case PressureType.CONSTANT:
                        retValue = Value;
                        break;
                    case PressureType.EXTERNAL:
                        retValue = Value;
                        break;
                    case PressureType.RANGE:
                        {
                            var result = GetResults();

                            if (result.Length > 0)
                            {
                                retValue = result.Average((r) =>
                                {
                                    if (r!= null)
                                    {
                                        if (r is StimulusResponseResult)
                                        {
                                            return Value * (r.PTT - r.PDT) + r.PDT;
                                        }
                                    }

                                    return 0;
                                });
                            }
                        }
                        break;
                    case PressureType.PDT_PERCENTIKE:
                        {
                            var result = GetResults();

                            if (result.Length > 0)
                            {
                                retValue = result.Average((r) =>
                                {
                                    if (r != null)
                                    {
                                        if (r is StimulusResponseResult)
                                        {
                                            return Value * r.PDT;
                                        }
                                    }

                                    return 0;
                                });
                            }
                        }
                        break;
                    case PressureType.PTT_PERCENTILE:
                        {
                            var result = GetResults();

                            if (result.Length > 0)
                            {
                                retValue = result.Average((r) =>
                                {
                                    if (r != null)
                                    {
                                        if (r is StimulusResponseResult)
                                        {
                                            return Value * r.PTT;
                                        }
                                    }

                                    return 0;
                                });
                            }
                        }
                        break;
                    case PressureType.VAS:
                        {
                            var result = GetResults();

                            if (result.Length > 0)
                            {
                                retValue = result.Average((r) =>
                                {
                                    if (r != null)
                                    {
                                        if (r is StimulusResponseResult)
                                        {
                                            return r.GetPressureFromPerception(Value);
                                        }
                                    }

                                    return 0;
                                });
                            }
                        }
                        break;
                }
            }

            return retValue;
        }

        public bool IsDependent
        {
            get
            {
                bool retValue = false;

                switch (CalculationType)
                {
                    case PressureType.CONSTANT:
                    case PressureType.EXTERNAL:
                        retValue = false;
                        break;
                    case PressureType.RANGE:
                    case PressureType.VAS:
                        retValue = true;
                        break;
                }

                return retValue;
            }
        }

        public Test[] Dependencies
        {
            get
            {
                ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
                ThrowIf.Argument.IsNull(Experiment.Active.Protocol, "Experiment.Active.Protocol");
                var IDs = GetDependencies();
                Test[] retValue = new Test[IDs.Length];

                for (int n = 0; n < IDs.Length; ++n)
                {
                    retValue[n] = Experiment.Active.Protocol.GetTest(IDs[n]);
                }

                return retValue;
            }
        }

        private string[] GetDependencies()
        {
            if (TestID != null)
            {
                return TestID.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
                return new string[] { };
        }

        private Result[] GetResults()
        {
            var IDs = GetDependencies();
            List<Result> retValue = new List<Result>();

            foreach (var id in IDs)
            {
                var result = Session.Active.GetResult(id);

                if (result != null)
                {
                    retValue.Add(result);
                }
            }

            return retValue.ToArray();
        }
    }
}
