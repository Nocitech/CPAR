using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScienceFoundry.IO.Matlab;
using System.IO;
using System.Xml.Serialization;
using CPAR.Core.Results;
using CPAR.Core;

namespace CPAR.Core.Exporters.Matlab
{
    public class MatlabExporter :
        Exporter.ExportMethod
    {
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        public override void Export(string path)
        {
            var fullname = Path.Combine(path, FileName + ".mat");
            Console.WriteLine("MATLAB EXPORTER [ {0} ]", fullname);

            MatlabFile file = new MatlabFile(fullname, true);
            ExportExperiment(file);
            ExportData(file);

        }

        #region EXPORT EXPERIMENT
        private void ExportExperiment(MatlabFile file)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            Experiment experiment = Experiment.Active;

            file.Write(new Struct("experiment", new IMatrix[]
            {
                new StringArray("name", experiment.Name),
                new StringArray("protocol", experiment.ProtocolName),
                new StringArray("description", experiment.Description),
                new StringArray("path", experiment.ExperimentPath),
                experiment.WithinSubjectFactors != null ? new Cell("wsf", ExportFactors(experiment.WithinSubjectFactors)) : new Cell("within-subject-factors", new Matrix[] { new Matrix("t", 1)}),
                experiment.BetweenSubjectFactors != null ? new Cell("bsf", ExportFactors(experiment.BetweenSubjectFactors)) : new Cell("between-subject-factors", new Matrix[] { new Matrix("t", 1)}),
            }));
        }

        private IMatrix[] ExportFactors(Factor[] factors)
        {
            return (from f in factors
                    select new Struct(f.Name, new IMatrix[]
                    {
                        new StringArray("name", f.Name),
                        ExportLevels(f.Levels)
                    })).ToArray();
        }

        private Cell ExportLevels(Factor.Level[] levels)
        {
            return new Cell("levels", (from l in levels
                                       select new StringArray(l.Name, l.Name)).ToArray());
        }
        #endregion
        #region EXPORT SUBJECTS
        private void ExportData(MatlabFile file)
        {
            var subjects = Subject.GetSubjects();
            List<IMatrix> list = new List<IMatrix>();

            foreach (var subject in subjects)
            {
                list.Add(ExportSubject(subject));
            }

            file.Write(new Cell("subjects", list.ToArray()));
        }

        private Struct ExportSubject(Subject subject)
        {
            Console.WriteLine("EXPORTING SUBJECT [ {0} ]", subject.SubjectID);
            return new Struct("subject", new IMatrix[]
            {
                new StringArray("ID", subject.SubjectID),
                ExportSessions(subject)
            });
        }

        private IMatrix ExportSessions(Subject subject)
        {
            List<IMatrix> list = new List<IMatrix>();

            foreach (var session in subject.Sessions)
            {
                Console.WriteLine("   EXPORTING SESSION [ {0} ]", session.ID);
                list.Add(new Struct("session", new IMatrix[]
                {
                    new StringArray("ID", session.ID),
                    new Cell("levels", (from level in session.Levels
                                       select new StringArray("", level.Name)).ToArray()),
                    new Cell("data", (from result in session.Results
                                      select ExportResult(result)).ToArray())
                }));
            }

            return new Cell("sessions", list.ToArray());
        }

        private IMatrix ExportResult(Result result)
        {
            IMatrix retValue = null;

            if (result is ConditionedPainResult)
            {
                var cpResult = result as ConditionedPainResult;

                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "ConditionedPainResult"),
                    new StringArray("ID", result.ID),
                    new StringArray("TestName", result.Name),
                    new Matrix("VAS_PDT", result.VAS_PDT),
                    new Matrix("PDT", result.PDT),
                    new Matrix("PTT", result.PTT),
                    new Matrix("PTL", result.PTL),
                    new Matrix("Conditioned", result.Conditioned ? 1 : 0),
                    new Matrix("StimulatingPressure", result.StimulationPressure),
                    new Matrix("ConditioningPressure", result.ConditioningPressure),
                    new Matrix("VAS", result.VAS)
                });

                Console.WriteLine("  -- Conditioned Pain Result [ {0} ]", cpResult.ID);
            }
            else if (result is StimulusResponseResult)
            {
                var srResult = result as StimulusResponseResult;

                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "StimulusResponseResult"),
                    new StringArray("ID", result.ID),
                    new StringArray("TestName", result.Name),
                    new Matrix("VAS_PDT", result.VAS_PDT),
                    new Matrix("PDT", result.PDT),
                    new Matrix("PTT", result.PTT),
                    new Matrix("PTL", result.PTL),
                    new Matrix("Conditioned", result.Conditioned ? 1 : 0),
                    new Matrix("StimulatingPressure", result.StimulationPressure),
                    new Matrix("ConditioningPressure", result.ConditioningPressure),
                    new Matrix("VAS", result.VAS)
                });

                Console.WriteLine("  -- SR Result [ {0} ]", srResult.ID);
            }
            else if (result is TemporalSummationResult)
            {
                var tsResult = result as TemporalSummationResult;

                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "TemporalSummationResult"),
                    new StringArray("ID", result.ID),
                    new StringArray("TestName", result.Name),
                    new Matrix("VAS_PDT", result.VAS_PDT),
                    new Matrix("PDT", result.PDT),
                    new Matrix("PTT", result.PTT),
                    new Matrix("PTL", result.PTL),
                    new Matrix("Conditioned", result.Conditioned ? 1 : 0),
                    new Matrix("StimulatingPressure", result.StimulationPressure),
                    new Matrix("ConditioningPressure", result.ConditioningPressure),
                    new Matrix("VAS", result.VAS),
                    new Matrix("Responses", tsResult.Responses)
                });

                Console.WriteLine("  -- TS Result [ {0} ]", tsResult.ID);
            }
            else if (result is StartleResult)
            {
                StartleResult startleResult = result as StartleResult;

                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "StartleResult"),
                    new StringArray("ID", startleResult.ID),
                    new StringArray("TestName", startleResult.Name),
                    new Matrix("VAS_PDT", startleResult.VAS_PDT),
                    new Matrix("PDT", startleResult.PDT),
                    new Matrix("PTT", startleResult.PTT),
                    new Matrix("PTL", startleResult.PTL),
                    new Matrix("Conditioned", startleResult.Conditioned ? 1 : 0),
                    new Matrix("StimulatingPressure", startleResult.StimulationPressure),
                    new Matrix("ConditioningPressure", startleResult.ConditioningPressure),
                    new Matrix("StartlePressure", startleResult.StartlePressure),
                    new Matrix("VAS", startleResult.VAS),
                    new Cell("probes", (from p in startleResult.Probes
                                        select new Struct("probe", new IMatrix[]
                                        {
                                            new Matrix("startle", p.IsStartle ? 1 : 0),
                                            new Matrix("VAS", p.VAS)
                                         })
                                        ).ToArray())
                });

                Console.WriteLine("  -- Startle Result [ {0} ]", startleResult.ID);
            }
            else if (result is StaticTemporalSummationResult)
            {
                var tsResult = result as StaticTemporalSummationResult;

                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "StartleResult"),
                    new StringArray("ID", result.ID),
                    new StringArray("TestName", result.Name),
                    new Matrix("VAS_PDT", result.VAS_PDT),
                    new Matrix("PDT", result.PDT),
                    new Matrix("PTT", result.PTT),
                    new Matrix("PTL", result.PTL),
                    new Matrix("Conditioned", result.Conditioned ? 1 : 0),
                    new Matrix("StimulatingPressure", result.StimulationPressure),
                    new Matrix("ConditioningPressure", result.ConditioningPressure),
                    new Matrix("VAS", result.VAS),
                    new Matrix("MaximalVAS", tsResult.MaximalVAS),
                    new Matrix("MaximalTime", tsResult.MaximalTime),
                    new Matrix("AverageVAS", tsResult.AverageVASStimulation),
                    new Matrix("AbortTime", tsResult.AbortTime),
                    new Matrix("VASTail", tsResult.VASEndOfStimulation)
                });

                Console.WriteLine("  -- Static TS Result [ {0} ]", tsResult.ID);
            }
            else
            {
                retValue = new Struct("data", new IMatrix[] {
                    new StringArray("type", "NULL")

                });

                Console.WriteLine("  -- Null Result [ {0} ]", result.ID);

            }

            return retValue;
        }

        #endregion
    }
}
