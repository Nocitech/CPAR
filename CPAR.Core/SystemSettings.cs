using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace CPAR.Core
{
    /**
     * \brief The file system of the software
     * This class maintains and provides access to the filesystem for the software.
     */
    public static class SystemSettings
    {
        public class WindowSettings
        {
            [XmlAttribute("height")]
            public int Height { get; set; }
            [XmlAttribute("width")]
            public int Width { get; set; }
            [XmlAttribute("splitter-distance")]
            public int SplitterDistance { get; set; }
            [XmlAttribute("view-splitter-distance")]
            public int ViewSplitterDistance { get; set; }
            [XmlAttribute("start-x")]
            public int X { get; set; }
            [XmlAttribute("start-y")]
            public int Y { get; set; }

            public WindowSettings()
            {
                Height = 610;
                Width = 908;
                SplitterDistance = 331;
                ViewSplitterDistance = 300;
                X = 50;
                Y = 100;
            }
        }

        public class SettingsFile
        {
            [XmlAttribute("base-path")]
            public string BasePath { get; set; }
            [XmlAttribute("log-level")]
            public Logging.LogLevel LogLevel { get; set; }
            [XmlAttribute("port")]
            public string SerialPort { get; set; }
            [XmlElement("window-properties")]
            public WindowSettings WindowSettings { get; set; }

            [XmlIgnore]
            public string ExperimentExtension { get; private set; }
            [XmlIgnore]
            public string ProtocolExtension { get; private set; }
            [XmlIgnore]
            public string SubjectExtension { get; private set; }
            [XmlIgnore]
            public string ExportExtension { get; private set; }

            public SettingsFile()
            {
                BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CPAR");
                LogLevel = Logging.LogLevel.STATUS;
                ExperimentExtension = ".expx";
                ProtocolExtension = ".prtx";
                SubjectExtension = ".subx";
                ExportExtension = ".edfx";
                SerialPort = GetDefaultPort();
                WindowSettings = new WindowSettings();
            } 

            private string GetDefaultPort()
            {
                var retValue = "COM4";
                var ports = System.IO.Ports.SerialPort.GetPortNames();

                if (ports.Length > 0)
                {
                    retValue = ports[0];
                }

                return retValue;
            }

            public static SettingsFile Load()
            {
                SettingsFile retValue = new SettingsFile();

                if (File.Exists(StateFile))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

                    using (var reader = new StreamReader(StateFile))
                    {
                        retValue = (SettingsFile)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    retValue.Save();
                }

                return retValue;
            }

            public void Save()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

                using (var writer = new StreamWriter(StateFile))
                {
                    serializer.Serialize(writer, this);
                }
            }

            public static string SystemDirectory
            {
                get
                {
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cpar");
                }
            }

            private static string StateFile
            {
                get
                {
                    return Path.Combine(SystemDirectory, "cpar.xml");
                }
            }
        }

        private static SettingsFile settings = null;

        // Constants for paths

        static SystemSettings()
        {
            if (!Directory.Exists(SettingsFile.SystemDirectory))
            {
                Directory.CreateDirectory(SettingsFile.SystemDirectory);
            }
            settings = SettingsFile.Load();

            if (!Directory.Exists(settings.BasePath))
            {
                Directory.CreateDirectory(settings.BasePath);
            }
        }

        public static void Sync()
        {
            if (settings != null)
            {
                settings.Save();
            }
        }

        #region Paths and Files
        public static string SubjectDirectory
        {
            get
            {
                return Path.Combine(Experiment.Active.ExperimentPath, "subjects");
            }
        }

        public static string LoggingDirectory
        {
            get
            {
                return Path.Combine(Experiment.Active.ExperimentPath, "logs");
            }
        }

        public static string ExperimentExtension
        {
            get
            {
                return settings.ExperimentExtension;
            }
        } 

        public static string ProtocolExtension
        {
            get
            {
                return settings.ProtocolExtension;
            }
        } 

        public static string SubjectExtension
        {
            get
            {
                return settings.SubjectExtension;
            }
        }

        public static string ExportExtension
        {
            get
            {
                return settings.ExportExtension;
            }
        }

        public static string BasePath
        {
            get
            {
                return settings.BasePath;
            }
        }

        public static Logging.LogLevel LogLevel
        {
            get
            {
                return settings.LogLevel;
            }
        }

        public static string SerialPort
        {
            get
            {
                return settings.SerialPort;
            }
        }

        public static int WindowHeight
        {
            get
            {
                return settings.WindowSettings.Height;
            }
            set
            {
                settings.WindowSettings.Height = value;
            }
        }

        public static int WindowWidth
        {
            get
            {
                return settings.WindowSettings.Width;
            }
            set
            {
                settings.WindowSettings.Width = value;
            }
        }

        public static int SplitterDistance
        {
            get
            {
                return settings.WindowSettings.SplitterDistance;
            }
            set
            {
                settings.WindowSettings.SplitterDistance = value;
            }
        }

        public static int ViewSplitterDistance
        {
            get
            {
                return settings.WindowSettings.ViewSplitterDistance;
            }
            set
            {
                settings.WindowSettings.ViewSplitterDistance = value;
            }
        }

        public static int StartX
        {
            get
            {
                return settings.WindowSettings.X;
            }
            set
            {
                settings.WindowSettings.X = value;
            }
        }

        public static int StartY
        {
            get
            {
                return settings.WindowSettings.Y;
            }
            set
            {
                settings.WindowSettings.Y = value;
            }
        }

        public static string VersionInformation
        {
            get
            {
                return Assembly.GetAssembly(typeof(SystemSettings)).GetName().Version.ToString();
            }
        }

        #endregion
    }
}
