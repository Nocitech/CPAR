using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CPAR.Core
{
    [XmlRoot("export")]
    public class Exporter
    {
        public abstract class ExportMethod
        {
            public abstract void Export(string path);
        }            

        [XmlAttribute("export-path")]
        public string ExportPath { get; set; }

        [XmlArray(ElementName = "exports")]
        [XmlArrayItem(Type = typeof(CPAR.Core.Exporters.Matlab.MatlabExporter), ElementName = "matlab")]
        [XmlArrayItem(Type = typeof(CPAR.Core.Exporters.SPSS.SPSSExporter), ElementName = "spss")]
        [XmlArrayItem(Type = typeof(CPAR.Core.Exporters.Excel.ExcelExporter), ElementName = "excel")]
        public ExportMethod[] Methods { get; set; }

        public void Execute()
        {
            Console.WriteLine("Exporting data from experiment [ {0} ] to directory [ {1} ]", Experiment.Active.Name, ExportPath);

            CheckExportPath();

            if (Methods != null)
            {
                foreach (var method in Methods)
                {
                    method.Export(ExportPath);
                }
            }
            else
            {
                Console.WriteLine("The export definition file [ {0} ] does not contain any exports", filename);
            }
        }

        private void CheckExportPath()
        {
            if (!Directory.Exists(ExportPath))
            {
                Directory.CreateDirectory(ExportPath);
            }
        }

        public static Exporter Load(string filename)
        {
            ThrowIf.Argument.IsNull(filename, "filename");
            ThrowIf.String.IsEmpty(filename, "filename");
            ThrowIf.File.DoesNotExists(filename);
            Exporter retValue = null;
            XmlSerializer serializer = new XmlSerializer(typeof(Exporter));

            using (var reader = new StreamReader(filename))
            {
                retValue = (Exporter)serializer.Deserialize(reader);
                retValue.path = Path.GetDirectoryName(filename);
                retValue.filename = Path.GetFileName(filename);
                Experiment.Active = Experiment.Load(Experiment.GetExperimentFile(retValue.path));
            }

            return retValue;
        }

        public static Exporter LoadFromDirectory(string workingPath)
        {
            ThrowIf.String.IsEmpty(workingPath, "workingPath");
            return Load(GetExperimentFile(workingPath));
        }

        public static string GetExperimentFile(string directory)
        {
            string retValue = null;
            var files = Directory.GetFiles(directory, "*" + SystemSettings.ExportExtension);

            if (files.Length == 1)
            {
                retValue = files[0];
            }
            else
            {
                Console.WriteLine("No export definition file found in working directory:");
                Console.WriteLine("- {0}",  directory);
                Console.WriteLine("");
                Console.WriteLine("Please check that:");
                Console.WriteLine("  1. That the directory contains a file with the extentision [ {0} ]", SystemSettings.ExportExtension);
                Console.WriteLine("  2. That this file is a valid export definition file");
                Console.WriteLine();
            }

            return retValue;
        }


        private string path = "";
        private string filename = "";
    }
}
