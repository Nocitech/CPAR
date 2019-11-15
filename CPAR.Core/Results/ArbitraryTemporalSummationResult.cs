using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Communication;

namespace CPAR.Core.Results
{
    public class ArbitraryTemporalSummationResult :
        Result
    {
        public class Response
        {
            [XmlAttribute("pressure")]
            public double Pressure { get; set; }
            [XmlAttribute("vas")]
            public double VAS { get; set; }
            [XmlAttribute("t-on")]
            public double T_ON { get; set; }
            [XmlAttribute("t-off")]
            public double T_OFF { get; set; }
        }
    
        public override string DisplayResult()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormatLine("ARBITRARY TEMPORAL SUMMATION TEST [{0}]", Name);

            for (int i = 0; i < Responses.Length; ++i)
            {
                builder.AppendFormatLine("   PULSE [{0}]: ({1:0.0}kPa / {2:0.0}cm", i, Responses[i].Pressure, Responses[i].VAS);
            }

            return builder.ToString();
        }

        public ArbitraryTemporalSummationResult() :
            base()
        {
            Responses = null;
        }

        public ArbitraryTemporalSummationResult(int numberOfStimuli) :
            base()
        {
            Responses = new Response[numberOfStimuli];
        }

        [XmlIgnore]
        public int NumberOfStimuli
        {
            get
            {
                int retValue = 0;

                if (Responses != null)
                {
                    retValue = Responses.Length;
                }

                return retValue;
            }
        }

        [XmlArray("responses")]
        [XmlArrayItem("response")]
        public Response[] Responses { get; set; }        
    }
}
