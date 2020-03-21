using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Functions
{
    public class ResetDevice :
        Function
    {
        private static byte ResponseLength = 0;

        public ResetDevice() : base(0x09, 0) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        public override string ToString()
        {
            return "[0x09] Reset Devce";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("RESET DEVICE");

            return builder.ToString();
        }
    }
}
