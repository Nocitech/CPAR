using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Core
{
    public static class ClassExtensions
    {
        #region StringBuilder
        public static void AppendFormatLine(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format + System.Environment.NewLine, args);
        }
        #endregion
    }
}
