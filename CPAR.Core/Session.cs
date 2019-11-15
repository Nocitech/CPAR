using CPAR.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public class Session
    {
        public static Session Create(Factor.Level[] factors)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            ThrowIf.Argument.IsNull(Experiment.Active.Protocol, "Experiment.Active.Protocol");

            return new Session()
            {
                ID = Factor.CreateID(factors),
                Results = CreateDefaultResults(),
                Levels = factors
            };
        }

        public static Result[] CreateDefaultResults()
        {
            var retValue = new Result[Experiment.Active.Protocol.Tests.Length];

            foreach (var test in Experiment.Active.Protocol.Tests)
            {
                retValue[test.Index] = new NullResult()
                {
                    ID = test.ID,
                    Name = test.Name,
                    Index = test.Index
                };
            }
            retValue.IndexArray();

            return retValue;
        }

        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlArray("factors")]
        [XmlArrayItem("level")]
        public Factor.Level[] Levels { get; set; }

        [XmlArray("results")]
        [XmlArrayItem(Type = typeof(ConditionedPainResult), ElementName = "conditioned-pain-modulation"),
         XmlArrayItem(Type = typeof(TemporalSummationResult), ElementName = "temporal-summation"),
         XmlArrayItem(Type = typeof(StimulusResponseResult), ElementName = "stimulus-response"),
         XmlArrayItem(Type = typeof(StartleResult), ElementName = "startle-response"),        
         XmlArrayItem(Type = typeof(StaticTemporalSummationResult), ElementName = "static-temporal-summation"),
         XmlArrayItem(Type = typeof(ArbitraryTemporalSummationResult), ElementName = "arbitrary-temporal-summation"),
         XmlArrayItem(Type = typeof(NullResult), ElementName = "null-result")]
        public Result[] Results { get; set; }

        public void Add(Result result)
        {
            ThrowIf.Argument.OutOfBounds(result.Index, Results);
            if (result.ID != Results[result.Index].ID)
                throw new ArgumentException("ID of result did not match the expected result");
            if (result.Name != Results[result.Index].Name)
                throw new ArgumentException("Name of result did not match the expected result");

            var test = Experiment.Active.Protocol.GetTest(result.ID);
            var dependents = test.Dependents;

            foreach (var d in dependents)
            {
                Results[d.Index] = new NullResult()
                {
                    ID = d.ID,
                    Name = d.Name,
                    Index = d.Index
                };
            }

            Results[result.Index] = result;
            Subject.Active.Save();
        }

        public bool Complete()
        {
            var retValue = true;
            Results.Foreach<Result>((e) => retValue = e is NullResult ? false : retValue);
            return retValue;
        }

        public bool Complete(int index)
        {
            ThrowIf.Argument.OutOfBounds(index, Active.Results);
            return !(Active.Results[index] is NullResult);
        }

        public bool Complete(string id)
        {
            var result = GetResult(id);
            bool retValue = false;

            if (result != null)
            {
                retValue = !(result is NullResult);
            }
            else
            {
                throw new ArgumentException(String.Format("No test exists with id ( {0} )", id));
            }

            return retValue;
        }

        public Result GetResult(string id)
        {
            return Results.First((r) => r.ID == id);
        }

        private static Session activeSession;

        public static Session Active
        {
            get
            {
                return activeSession;
            }
            set
            {
                activeSession = value;
            }
        }

        internal Dictionary<string, Result> GetResultCollection()
        {
            var retValue = new Dictionary<string, Result>();
            Results.Foreach((r) => retValue.Add(r.ID, r));
            return retValue;
        }
    }
}
