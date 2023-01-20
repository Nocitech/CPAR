using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Functions
{
    public class StopStimulation : 
        Function
    {
        private static byte ResponseLength = 0;

        public StopStimulation() : base(0x13, 0) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        public override string ToString()
        {
            return "[0x13] Stop Stimulation";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("STOP STIMULATION");

            return builder.ToString();

        }
    }
}
