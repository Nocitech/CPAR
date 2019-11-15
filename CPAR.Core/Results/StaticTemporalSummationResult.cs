using CPAR.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core.Results
{
    public class StaticTemporalSummationResult :
        Result
    {
        [XmlAttribute("nominal-stimulating-pressure")]
        public double NominalStimulatingPressure { get; set; }

        [XmlIgnore]
        public double AbortTime
        {
            get
            {
                return AbortCount > 0 ? CPARDevice.CountToTime(AbortCount) : 0;
            }
        }

        [XmlAttribute("abort-count")]
        public int AbortCount { get; set; }



        public override string DisplayResult()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormatLine("STATIC TEMPORAL SUMMATION TEST [{0}]", Name);
            builder.AppendFormatLine("   MaximalVAS            : {0:0.00}cm", MaximalVAS);
            builder.AppendFormatLine("   MaximalTime           : {0:0.00}s", MaximalTime);
            builder.AppendFormatLine("   AverageVASStimulation : {0:0.00}cm", AverageVASStimulation);
            builder.AppendFormatLine("   AverageVASTail        : {0:0.00}cm", AverageVASTail);
            builder.AppendFormatLine("   AverageVAS            : {0:0.00}cm", AverageVAS);
            builder.AppendFormatLine("   VASEndOfStimulation   : {0:0.00}cm", VASEndOfStimulation);
            builder.AppendFormatLine("   VASEndOfTail          : {0:0.00}cm", VASEndOfTail);
            builder.AppendFormatLine("   VASAreaStimulation    : {0:0.00}cm", VASAreaStimulation);
            builder.AppendFormatLine("   VASAreaTail           : {0:0.00}cm", VASAreaTail);
            builder.AppendFormatLine("   VASArea               : {0:0.00}cm", VASArea);
            builder.AppendFormatLine("   AbortTime             : {0:0.00}s", AbortTime);

            builder.AppendFormatLine(" STIMULATING PRESSURE: {0:0.0}kPa", NominalStimulatingPressure);

            return builder.ToString();
        }

        [XmlIgnore]
        public double MaximalVAS
        {
            get
            {
                return Data.Count > 0 ? VAS.Max() : 0;
            }
        }

        [XmlIgnore]
        public double MaximalTime
        {
            get
            {
                int count = 0;
                int ctime = 0;
                double max = 0;

                if (Data.Count > 0)
                {
                    Data.ForEach((p) =>
                    {
                        if (p.VAS > max)
                        {
                            max = p.VAS;
                            ctime = count;
                        }
                        ++count;
                    });

                }

                return CPARDevice.CountToTime(ctime);
            }
        }

        [XmlIgnore]
        public double AverageVASStimulation
        {
            get 
            {
                return AbortCount > 0 ? VAS.Skip(0).Take(AbortCount).ToArray().Average() : 0;
            }
        }

        [XmlIgnore]
        public double AverageVASTail
        {
            get
            {
                return AbortCount > 0 ? VAS.Skip(AbortCount).Take(VAS.Length - AbortCount).ToArray().Average() : 0;
            }
        }

        [XmlIgnore]
        public double AverageVAS
        {
            get
            {
                return AbortCount > 0 ? VAS.Average() : 0;
            }
        }

        [XmlIgnore]
        public double VASEndOfStimulation
        {
            get
            {
                return VAS.Length > AbortCount ? VAS[AbortCount] : 0.0;                
            }
        }

        [XmlIgnore]
        public double VASEndOfTail
        {
            get
            {
                return VAS.Length > 0 ? VAS[VAS.Length - 1] : 0.0;
            }
        }

        [XmlIgnore]
        public double VASAreaStimulation
        {
            get
            {
                return AbortCount > 0 ? VAS.Skip(0).Take(AbortCount).ToArray().Sum() * CPARDevice.CountToTime(1) : 0;
            }
        }

        [XmlIgnore]
        public double VASAreaTail
        {
            get
            {
                return AbortCount > 0 ? VAS.Skip(AbortCount).Take(VAS.Length - AbortCount).ToArray().Sum() * CPARDevice.CountToTime(1) : 0;
            }
        }

        [XmlIgnore]
        public double VASArea
        {
            get
            {
                return VAS.Length > 0 ? VAS.Sum() * CPARDevice.CountToTime(1) : 0;
            }
        }
    }
}
