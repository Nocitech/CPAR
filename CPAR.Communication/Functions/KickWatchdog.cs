using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Functions
{
    public class KickWatchdog :
        Function
    {
        private static byte ResponseLength = 4;

        public KickWatchdog() : base(0x08, 0) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        public UInt32 Counter
        {
            get
            {
                if (response != null)
                    return response.GetUInt32(0);
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            return "[0x08] Kick Watchdog";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("KICK WATCHDOG");
            builder.AppendFormat("- Counter: {0}", Counter);
            builder.AppendLine();

            return builder.ToString();

        }
    }
}
