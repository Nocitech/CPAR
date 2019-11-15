using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Logging
{
    public enum LogLevel
    {
        DEBUG = 0,
        STATUS,
        ERROR
    }

    public enum LogCategory
    {
        SYSTEM = 0,
        SUBJECT,
        SESSION
    }

    public interface ILogger
    {
        void Add(DateTime time, LogCategory category, LogLevel level, string str);
    }
}
