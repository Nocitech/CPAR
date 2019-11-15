using CPAR.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CPAR.Core
{
    [XmlRoot("experiment")]
    public class Experiment
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("protocol")]
        public string ProtocolName { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlArray("experimenters")]
        [XmlArrayItem("experimenter")]
        public Experimenter[] Experimenters { get; set; }

        [XmlArray("within-subject-factors")]
        [XmlArrayItem("factor")]
        public Factor[] WithinSubjectFactors { get; set; }

        [XmlArray("between-subject-factors")]
        [XmlArrayItem("factor")]
        public Factor[] BetweenSubjectFactors { get; set; }

        [XmlIgnore]
        public Protocol Protocol
        {
            get
            {
                return protocol;
            }
        }
        [XmlIgnore]
        public bool UseExperimenters { get; private set; }

        [XmlIgnore]
        public bool UseWithinSubjectFactors { get; private set; }

        [XmlIgnore]
        public bool UseBetweenSubjectFactors { get; private set; }

        [XmlIgnore]
        public int NumberOfSessions
        {
            get
            {
                if (UseWithinSubjectFactors)
                {
                    int count = WithinSubjectFactors[0].Count;

                    for (int i = 1; i < WithinSubjectFactors.Length; ++i)
                    {
                        count = count * WithinSubjectFactors[i].Count;
                    }

                    return count;
                }
                else
                {
                    return 1;
                }
            }
        }

        [XmlIgnore]
        public string ExperimentPath
        {
            get
            {
                return path;
            }
        }

        public Experiment()
        {
            UseExperimenters = false;
            UseWithinSubjectFactors = false;
            UseBetweenSubjectFactors = false;
        }

        #region LOAD EXPERIMENTS
        public static Experiment Load(string filename)
        {
            Experiment retValue = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Experiment));

            using (var reader = new StreamReader(filename))
            {
                retValue = (Experiment)serializer.Deserialize(reader);
                retValue.path = Path.GetDirectoryName(filename);
                retValue.protocol = Protocol.Load(Path.Combine(retValue.path, retValue.ProtocolName) + SystemSettings.ProtocolExtension);
                retValue.Initialize();
            }

            return retValue;
        }

        private void Initialize()
        {
            CheckWithinSubjectFactors();
            CheckBetweenSubjectFactors();
            CheckExperimenters();
        }

        private void CheckWithinSubjectFactors()
        {
            if (WithinSubjectFactors != null)
            {
                WithinSubjectFactors.IndexArray();
                UseWithinSubjectFactors = true;
            }
            else
            {
                UseWithinSubjectFactors = false;
            }
        }

        private void CheckBetweenSubjectFactors()
        {
            if (BetweenSubjectFactors != null)
            {
                BetweenSubjectFactors.IndexArray();
                UseBetweenSubjectFactors = true;
            }
            else
            {
                UseBetweenSubjectFactors = false;
            }
        }

        private void CheckExperimenters()
        {
            if (Experimenters != null)
            {
                UseExperimenters = true;
            }
            else
            {
                Experimenters = new Experimenter[]
                {
                    new Experimenter() { Name = "EXPERIMENTER" }
                };
                UseExperimenters = false;
            }
        }
        #endregion
        #region SAVE EXPERIMENTS
        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Experiment));

            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }
        }
        #endregion
        #region ENUMERATE SESSIONS

        public List<string> EnumerateSessions()
        {
            return UseWithinSubjectFactors ? EnumerateLevel(0) : new List<string>(); 
        }

        private List<string> EnumerateLevel(int no)
        {
            List<string> retValue = new List<string>();

            if (no < WithinSubjectFactors.Length)
            {
                foreach (var level in WithinSubjectFactors[no].Levels)
                {
                    var str = no == 0 ? level.Name : "." + level.Name;

                    if (no + 1 < WithinSubjectFactors.Length)
                    {
                        foreach (var line in EnumerateLevel(no+1))
                        {
                            retValue.Add(str + line);
                        }
                    }
                    else
                    {
                        retValue.Add(str);
                    }
                }
            }

            return retValue;
        }


        #endregion
        #region OVERRIDING FUNCTIONS

        public override string ToString()
        {
            return Name;
        }

        #endregion
        #region HANDLING LOADING OF EXPERIMENTS
        public static List<Experiment> GetExperiments()
        {
            var retValue = new List<Experiment>();

            foreach (var directory in Directory.GetDirectories(SystemSettings.BasePath))
            {
                var experimentFile = GetExperimentFile(directory);

                if (experimentFile != null)
                {
                    try
                    {
                        var experiment = Experiment.Load(experimentFile);

                        if (experiment != null)
                        {
                            retValue.Add(experiment);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Debug(e.Message);
                    }
                }
            }

            return retValue;
        }

        public static string GetExperimentFile(string directory)
        {
            string retValue = null;
            var files = Directory.GetFiles(directory, "*" + SystemSettings.ExperimentExtension);

            if (files.Length == 1)
            {
                retValue = files[0];
            }

            return retValue;
        }

        private static void CheckExperimentDirectory()
        {
            if (!Directory.Exists(SystemSettings.SubjectDirectory))
            {
                Directory.CreateDirectory(SystemSettings.SubjectDirectory);
            }
            if (!Directory.Exists(SystemSettings.LoggingDirectory))
            {
                Directory.CreateDirectory(SystemSettings.LoggingDirectory);
            }
        }

        public static Experiment Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                CheckExperimentDirectory();
                Subject.CacheSubjects();
            }
        }

        private static Experiment active = null;
        #endregion

        private string path = "";
        private Protocol protocol;
    }
}
