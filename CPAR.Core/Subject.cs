using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CPAR.Core.Results;

namespace CPAR.Core
{
    public class Subject
    {
        private string filename;

        [XmlAttribute("id")]
        public string SubjectID { get; set; }

        [XmlArray("sessions")]
        [XmlArrayItem(Type = typeof(Session), ElementName = "session")]
        public List<Session> Sessions { get; set; }

        [XmlArray("between-subject-factors")]
        [XmlArrayItem("level")]
        public Factor.Level[] BetweenSubjectFactors { get; set; }

        #region Constructor
        public Subject()
        {
            Sessions = new List<Session>();
            BetweenSubjectFactors = new Factor.Level[0];
        }
        #endregion
        #region Handling managing subjects in an experiment
        private static Subject activeSubject;
        private static List<Subject> subjects = null;

        internal static void CacheSubjects()
        {
            subjects = new List<Subject>();

            foreach (var filename in Directory.GetFiles(SystemSettings.SubjectDirectory, "*" + SystemSettings.SubjectExtension))
            {
                try
                {
                    subjects.Add(Subject.Load(filename));
                }
                catch { }
            }
        }

        public static List<Subject> GetSubjects()
        {
            return subjects;
        }

        public static Subject Find(string id)
        {
            Subject retValue = null;

            if (Exists(id))
            {
                retValue = subjects.Find((s) => s.SubjectID == id);
            }

            return retValue;
        }

        public static bool Exists(string id)
        {
            bool retValue = false;

            if (subjects != null)
            {
                retValue = subjects.Count((s) => s.SubjectID == id) > 0;
            }

            return retValue;
        }

        public static Subject Active
        {
            get
            {
                return activeSubject;
            }
            set
            {
                activeSubject = value;
            }
        }

        public static Subject Create(string id)
        {
            if (id.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                throw new ArgumentException("ID is not a valid filename");
            }

            Subject retValue = null;

            if (!Exists(id))
            {
                var filename = Path.Combine(SystemSettings.SubjectDirectory, 
                                            id + SystemSettings.SubjectExtension);

                retValue = new Subject();
                retValue.SubjectID = id;

                retValue.Save(filename);
            }
            else
            {
                throw new ArgumentException(String.Format("Subject [ {0} ] allready exists", id));
            }

            CacheSubjects();

            return retValue;
        }

        public static Subject Load(string filename)
        {
            Subject retValue = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Subject));

            using (var reader = new StreamReader(filename))
            {
                retValue = (Subject)serializer.Deserialize(reader);
                retValue.filename = filename;
            }

            return retValue;
        }

        private void Save(string filename)
        {
            this.filename = filename;
            Save();
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Subject));

            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }
        }
        #endregion
        #region Handling managing sessions for a subject
        public static Factor.Level[] DummySessionID
        {
            get
            {
                return new Factor.Level[] { new Factor.Level() { Index = 0, Name = "" } };
            }
        }

        public void InitializeSession(Factor.Level[] withinSubjectFactors,
                                      Factor.Level[] betweenSubjectFactors)
        {
            if (BetweenSubjectFactors.Length == 0)
            {
                BetweenSubjectFactors = betweenSubjectFactors;
                Save();
            }

            if (SessionExists(withinSubjectFactors))
            {
                Session.Active = FindSession(withinSubjectFactors);
            }
            else
            {
                Session.Active = CreateSession(withinSubjectFactors);
                Sessions.Add(Session.Active);
                Save();
            }
        }

        public Session FindSession(Factor.Level[] factors)
        {
            return FindSession(Factor.CreateID(factors));
        }

        public Session FindSession(string sessionID)
        {
            return Sessions.Find((s) => s.ID == sessionID);
        }

        private Session CreateSession(Factor.Level[] factors)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Active experiment");

            if (Experiment.Active.WithinSubjectFactors != null)
            {
                ThrowIf.Array.IsIncorrectSize(factors, Experiment.Active.WithinSubjectFactors.Length, "factors");
            }

            return Session.Create(factors);
        }

        public bool SessionExists(Factor.Level[] factors)
        {
            ThrowIf.Argument.IsNull(Sessions, "Subject.Sessions");
            return SessionExists(Factor.CreateID(factors));
        }

        public bool SessionExists(string sessionID)
        {
            return Sessions.Count((s) => s.ID == sessionID) > 0;
        }

        public bool FactorExists(int index, Factor.Level factor)
        {
            ThrowIf.Argument.IsNull(Sessions, "Subject.Sessions");
            return Sessions.Count((s) => s.Levels[index].Name == factor.Name) > 0;
        }

        public bool SubjectEmpty()
        {
            return Sessions.Count == 0;
        }

        public bool SubjectIncomplete()
        {
            return !SubjectComplete();
        }

        public bool SubjectComplete()
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            return NumberOfCompletedSessions() == Experiment.Active.NumberOfSessions;
        }

        private int NumberOfCompletedSessions()
        {
            return Sessions.Count((s) => s.Complete());
        }

         #endregion

        public override string ToString()
        {
            return SubjectID;
        }
    }
}
