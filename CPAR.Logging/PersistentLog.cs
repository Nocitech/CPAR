using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPAR.Logging
{
    public class PersistentLog :
        ILogger
    {
        public PersistentLog(string systemLog, string subjectLog, string sessionLog, ILogger inner = null)
        {
            this.systemLog = systemLog;
            this.subjectLog = subjectLog;
            this.sessionLog = sessionLog;
            this.inner = inner;

            Initialize();
        }

        public void Add(DateTime time, LogCategory category, LogLevel level, string str)
        {
            var entry = CreateEntry(time, category, level, str);

            if (category == LogCategory.SYSTEM)
            {
                Write(systemLog, entry);
            }
            if ((category == LogCategory.SYSTEM) || (category == LogCategory.SUBJECT))
            {
                Write(subjectLog, entry);
            }

            Write(sessionLog, entry);

            if (inner != null)
            {
                inner.Add(time, category, level, str);
            }
        }

        private string CreateEntry(DateTime time, LogCategory category, LogLevel level, string str)
        {
            return String.Format("{0}|{1}|{2}|{3}" + System.Environment.NewLine,
                time.ToUniversalTime().ToString(), category, level, str);
        }

        private void Write(string file, string entry)
        {
            if (file != null)
            {
                lock (lockObject)
                {
                    File.AppendAllText(file, entry);
                }
            }
        }

        private void Initialize()
        {
            if (sessionLog != null)
            {
                if (File.Exists(sessionLog))
                {
                    var delimiter = new char[] { '|' };
                    string[] entries = File.ReadAllLines(sessionLog);

                    foreach (var entry in entries)
                    {
                        string[] parts = entry.Split(delimiter);

                        if (parts.Length == 4)
                        {
                            try
                            {
                                var time = Convert.ToDateTime(parts[0]);
                                var category = (LogCategory) Enum.Parse(typeof(LogCategory), parts[1]);
                                var level = (LogLevel) Enum.Parse(typeof(LogLevel), parts[2]);

                                if (inner != null)
                                {
                                    inner.Add(time, category, level, parts[3]);
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Debug("Exception while parsing log entry: {0} => {1}", entry, e.Message);
                            }
                        }
                        else
                        {
                            Log.Debug("Log entry: {0}, did not parse correctly", entry);
                        }
                    }
                }
            }
        }

        private ILogger inner;
        private string systemLog;
        private string subjectLog;
        private string sessionLog;
        private object lockObject = new object();
    }
}
