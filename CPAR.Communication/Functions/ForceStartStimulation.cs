using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class ForceStartStimulation :
        Function
    {
        public enum StopCriterion
        {
            STOP_CRITERION_ON_BUTTON_VAS = 0,
            STOP_CRITERION_ON_BUTTON
        }

        private static byte ResponseLength = 0;

        public ForceStartStimulation() :
            base(0x1A, 1)
        {
            Criterion = StopCriterion.STOP_CRITERION_ON_BUTTON_VAS;
        }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [Category("Stop Criterion")]
        [Description("Stop criterion for the stimulation")]
        [XmlAttribute("stop-criterion")]
        public StopCriterion Criterion
        {
            get
            {
                return (StopCriterion)request.GetByte(0);
            }
            set
            {
                request.InsertByte(0, (byte)value);
            }
        }

        public override string ToString()
        {
            return "[0x0A] Force Start Stimulation";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("FORCE START STIMULATION");

            return builder.ToString();

        }
    }
}
