using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Core;
using System.Xml.Serialization;
using System.IO;
using ClosedXML.Excel;

namespace CPAR.Core.Exporters.SPSS
{
    /**
     * \brief Export data in the format required by SPSS
     */
    public class SPSSExporter :
        Exporter.ExportMethod
    {
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        [XmlElement("output-variable")]
        public OutputScript[] OutputVariables { get; set; }

        public override void Export(string path)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            var fullname = Path.Combine(path, FileName + ".xlsx");
            Console.WriteLine("SPSS EXPORTER [ {0} ]", fullname);

            var workbook = new XLWorkbook();
            SerializeExperiment(workbook);
            SerializeData(workbook);
            workbook.SaveAs(fullname, false);
        }

        #region EXPORT EXPERIMENT INFORMATION
        private void SerializeExperiment(XLWorkbook workbook)
        {
            var exp = Experiment.Active;
            var ws = workbook.Worksheets.Add("Experiment");
            ws.Cell(1, 1).Value = "Name:";
            ws.Cell(2, 1).Value = "Protocol:";
            ws.Cell(3, 1).Value = "Path:";
            ws.Cell(4, 1).Value = "Within Subject Factors:";
            ws.Cell(5, 1).Value = "Between Subject Factors:";
            ws.Cell(6, 1).Value = "Subjects:";
            ws.Cell(7, 1).Value = "Completed subjects:";
            ws.Column(1).AdjustToContents();

            ws.Cell(1, 2).Value = exp.Name;
            ws.Cell(2, 2).Value = exp.Protocol.Name;
            ws.Cell(3, 2).Value = exp.ExperimentPath;
            ws.Cell(4, 2).Value = exp.UseWithinSubjectFactors ? SerializeFactors(exp.WithinSubjectFactors) : "none";
            ws.Cell(5, 2).Value = exp.UseBetweenSubjectFactors ? SerializeFactors(exp.BetweenSubjectFactors) : "none";
            ws.Cell(6, 2).Value = Subject.GetSubjects().Count((s) => true);
            ws.Cell(7, 2).Value = Subject.GetSubjects().Count((s) => s.SubjectComplete());
        }

        private string SerializeFactors(Factor[] factors)
        {
            StringBuilder builder = new StringBuilder();

            if (factors.Length > 0)
            {
                AddFactor(builder, factors[0]);

                for (int i = 1; i < factors.Length; ++i)
                {
                    builder.Append("|");
                    AddFactor(builder, factors[i]);
                }
            }
            else
            {
                builder.Append("none");
            }

            return builder.ToString();
        }

        private void AddFactor(StringBuilder builder, Factor factor)
        {
            builder.Append(factor.Name);
            
            if (factor.Levels.Length > 0)
            {
                builder.Append("(" + factor.Levels[0].Name);

                for (int i = 1; i < factor.Levels.Length; ++i)
                {
                    builder.Append(", ");
                    builder.Append(factor.Levels[i].Name);
                }
                builder.Append(")");
            }
        }
        #endregion
        #region EXPORT DATA
        void SerializeData(XLWorkbook workbook)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            var exp = Experiment.Active;
            var ws = workbook.Worksheets.Add("Data");

            CreateDataHeaders(ws);
            ExportSubjects(ws);
        }

        private void CreateDataHeaders(IXLWorksheet ws)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            var exp = Experiment.Active;
            int offset = 1;

            ws.Cell(1, 1).Value = "SUBJECT";
            offset += 1;

            if (exp.UseBetweenSubjectFactors)
            {
                for (int i = 0; i < exp.BetweenSubjectFactors.Length; ++i)
                {
                    ws.Cell(1, i + offset).Value = exp.BetweenSubjectFactors[i].Name;
                }

                offset += exp.BetweenSubjectFactors.Length;
            }

            if (exp.UseWithinSubjectFactors)
            {
                var sessionIDs = exp.EnumerateSessions();

                foreach (var id in sessionIDs)
                {
                    foreach (var script in OutputVariables)
                    {
                        ws.Cell(1, offset).Value = id + "." + script.Name;
                        ++offset;
                    }
                }
            }
            else
            {
                foreach (var script in OutputVariables)
                {
                    ws.Cell(1, offset).Value = script.Name;
                    ++offset;
                }
            }
        }

        private void ExportSubjects(IXLWorksheet ws)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            var exp = Experiment.Active;
            var subjects = Subject.GetSubjects();
            int row = 2;

            foreach (var subject in subjects)
            {
                int col = 2;

                #region  EXPORT SUBJECT ID
                ws.Cell(row, 1).Value = subject.SubjectID;
                Console.WriteLine("Exporting subject [ {0} ]", subject.SubjectID);
                #endregion
                #region EXPORT BETWEEN SUBJECT FACTORS
                if (exp.UseBetweenSubjectFactors)
                {
                    subject.BetweenSubjectFactors.Foreach((level) =>
                    {
                        ws.Cell(row, col).Value = level.Name;
                        ++col;
                    });
                }
                #endregion
                #region EXPORT WITHIN SUBJECT FACTORS
                if (exp.UseWithinSubjectFactors)
                {
                    var sessionID = exp.EnumerateSessions();

                    foreach (var id in sessionID)
                    {
                        if (subject.SessionExists(id))
                        {
                            var session = subject.FindSession(id);
                            col = ExportSession(ws, session, row, col);
                        }
                        else
                        {
                            col += OutputVariables.Length;
                        }
                    }
                }
                else
                {
                    if (subject.SessionExists(Subject.DummySessionID))
                    {
                        var session = subject.FindSession(Subject.DummySessionID);
                        col = ExportSession(ws, session, row, col);
                    }
                    else
                    {
                        col += OutputVariables.Length;
                    }
                }
                #endregion

                ++row;
            }
        }

        private int ExportSession(IXLWorksheet ws, Session session, int row, int col)
        {
            var engine = new SessionScriptEngine(session.GetResultCollection());

            foreach (var script in OutputVariables)
            {
                try
                {
                    var value = script.Execute(engine);
                    ws.Cell(row, col).Value = value;
                    Console.WriteLine("Data exported for  [ {0} ] = {1}", session.ID + "." + script.Name, value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("No data available for [ {0} ] => {1}", session.ID + "." + script.Name, e.Message);
                }

                ++col;
            }

            return col;
        }


        #endregion
    }
}
