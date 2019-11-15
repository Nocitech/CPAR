using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Communication;

namespace CPAR.Core.Results
{
    public class TemporalSummationResult :
        Result
    {
        [XmlAttribute("nominal-stimulating-pressure")]
        public double NominalStimulatingPressure { get; set; }

        public override string DisplayResult()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormatLine("TEMPORAL SUMMATION TEST [{0}]", Name);

            for (int i = 0; i < Responses.Length; ++i)
            {
                builder.AppendFormatLine("   PULSE [{0}]: {1:0.0}cm", i, Responses[i]);
            }

            builder.AppendFormatLine(" STIMULATING PRESSURE: {0:0.0}kPa", NominalStimulatingPressure);

            return builder.ToString();
        }

        public TemporalSummationResult() :
            base()
        {
            NumberOfStimuli = 0;
        }

        public TemporalSummationResult(int numberOfStimuli) :
            base()
        {
            this.NumberOfStimuli = numberOfStimuli;
        }

        [XmlAttribute("number-of-stimuli")]
        public int NumberOfStimuli { get; set; }

        [XmlAttribute(AttributeName = "t-on")]
        public double T_ON { get; set; }
        [XmlAttribute(AttributeName = "t-off")]
        public double T_OFF { get; set; }

        [XmlIgnore]
        public double[] Responses
        {
            get
            {
                double[] retValue = new double[NumberOfStimuli];
                int period = CPARDevice.TimeToRate(T_ON + T_OFF);

                for (int i = 0; i < NumberOfStimuli; ++i)
                {
                    int index = (i + 1) * period - 1;

                    if (index > Data.Count - 1)
                    {
                        retValue[i] = Data[Data.Count - 1].VAS;
                    }
                    else
                    {
                        retValue[i] = Data[(i + 1) * period - 1].VAS;
                    }
                }

                return retValue;
            }
        }
    }
}
