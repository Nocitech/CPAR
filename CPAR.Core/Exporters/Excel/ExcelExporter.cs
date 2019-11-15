using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClosedXML.Excel;

namespace CPAR.Core.Exporters.Excel
{
    public class ExcelExporter :
        Exporter.ExportMethod
    {
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        public string GetExperimentFile(string path)
        {
            return Path.Combine(path, FileName + ".xlsx");
        }

        public string GetSubjectFile(string path, string id)
        {
            return Path.Combine(path, FileName + "_" + id + ".xlsx");
        }

        public override void Export(string path)
        {
            Console.WriteLine("EXCEL EXPORTER [ {0} ]", FileName);

            ExportExperiment(path);
            ExportData(path);
        }

        #region EXPORT EXPERIMENT

        private void ExportExperiment(string path)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            Experiment exp = Experiment.Active;
            var workbook = new XLWorkbook();
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
            ws.Column(2).AdjustToContents();

            workbook.SaveAs(GetExperimentFile(path), false);

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
        #region EXPORT SUBJECTS
        private void ExportData(string path)
        {
            var subjects = Subject.GetSubjects();

            foreach (var subject in subjects)
            {
                ExportSubject(subject, GetSubjectFile(path, subject.SubjectID));
            }

        }

        private void ExportSubject(Subject subject, string filename)
        {
            Console.WriteLine("EXPORTING SUBJECT [ {0} ]", subject.SubjectID);
            var wb = new XLWorkbook();
            ExportSessions(wb, subject);
            wb.SaveAs(filename, false);
        }

        private void ExportSessions(XLWorkbook wb, Subject subject)
        {
            foreach (var session in subject.Sessions)
            {
                Console.WriteLine("   EXPORTING SESSION [ {0} ]", session.ID);

                foreach (var result in session.Results)
                {
                    var ws = wb.Worksheets.Add(session.ID.Length > 0 ?
                                               String.Format("{0}-{1} {2}", session.ID, result.ID, result.Name) :
                                               String.Format("{0} {1}", result.ID, result.Name));

                    ExportResult(ws, result);
                }
            }
        }

        private void ExportResult(IXLWorksheet ws, Result result)
        {
            double time = 0;
            int index = 2;
            Console.WriteLine("   - EXPORTING RESULT [ {0}: {1} ]", result.ID, result.Name);

            ws.Cell(1, 1).Value = "Time";
            ws.Cell(1, 2).Value = "Pressure";
            ws.Cell(1, 3).Value = "Conditioning";
            ws.Cell(1, 4).Value = "VAS";

            foreach (var p in result.Data)
            {
                ws.Cell(index, 1).Value = time;
                ws.Cell(index, 2).Value = p.stimulating;
                ws.Cell(index, 3).Value = p.conditioning;
                ws.Cell(index, 4).Value = p.VAS;
                ++index;
                time += 1.0 / 20.0;
            }

            ws.Column(2).AdjustToContents();
        }

        #endregion
    }
}
