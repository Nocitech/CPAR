using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core.Results
{
    public class StimulusResponseResult :
        Result
    {
        public StimulusResponseResult()
        {
        }

        public override string DisplayResult()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormatLine("STIMUS-RESPONSE TEST [{0}]", Name);
            builder.AppendFormatLine("   Pain Detection Threshold (PDT0): {0:0.0}kPa (VAS Threshold: {1:0.0})", GetPressureFromPerception(0.1), 0.1);
            builder.AppendFormatLine("   Pain Detection Threshold (PDT) : {0:0.0}kPa (VAS Threshold: {1:0.0})", PDT, VAS_PDT);
            builder.AppendFormatLine("   Pain Tolerance Threshold (PTT) : {0:0.0}kPa", PTT);
            builder.AppendFormatLine("   Pain Tolerance Limit (PTL)     : {0:0.0}cm", PTL);

            return builder.ToString();
        }
    }
}
