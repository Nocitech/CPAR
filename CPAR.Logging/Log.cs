using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Logging
{
    public class Log
    {
        public static void Debug(string format, params object[] args)
        {
            if (level == LogLevel.DEBUG)
                AddToLogger(LogCategory.SYSTEM, LogLevel.DEBUG, String.Format(format, args));
        }

        public static void Status(string format, params object[] args)
        {
            if (level <= LogLevel.STATUS)
                AddToLogger(LogCategory.SYSTEM, LogLevel.STATUS, String.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            AddToLogger(LogCategory.SYSTEM, LogLevel.ERROR, String.Format(format, args));
        }

        public static void Debug(LogCategory category, string format, params object[] args)
        {
            if (level == LogLevel.DEBUG)
                AddToLogger(category, LogLevel.DEBUG, String.Format(format, args));
        }

        public static void Status(LogCategory category, string format, params object[] args)
        {
            if (level <= LogLevel.STATUS)
                AddToLogger(category, LogLevel.STATUS, String.Format(format, args));
        }

        public static void Error(LogCategory category, string format, params object[] args)
        {
            AddToLogger(category, LogLevel.ERROR, String.Format(format, args));
        }

        public static void SetLogger(ILogger newLogger)
        {
            logger = newLogger;
        }

        public static LogLevel Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        private static void AddToLogger(LogCategory category, LogLevel level, string str)
        {
            if (logger != null)
            {
                logger.Add(DateTime.Now, category, level, str);
            }
        }

        private static ILogger logger = null;
        private static LogLevel level = LogLevel.DEBUG;
    }
}
