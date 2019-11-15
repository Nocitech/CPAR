using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Core.Results
{
    public class NullResult :
        Result
    {
        public override string DisplayResult()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("TEST [{0}]", Name);
            builder.AppendLine();
            builder.AppendLine("  No results available");
            return builder.ToString();
        }
    }
}
