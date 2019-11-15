using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPAR.Core;
using System.IO;

namespace CPAR.UnitTests.Core
{
    [TestClass]
    public class FileSystemManagerTests
    {
        public static readonly string ExperimentFilename = "sample-experiment" +  SystemSettings.ExperimentExtension;
        public static readonly string ProtocolFilename = "sample-protocol" + SystemSettings.ProtocolExtension;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            try
            {
                File.Delete(ExperimentFilename);
                File.Delete(ProtocolFilename);
            }
            catch { }

            File.Copy("schemas//sample-experiment.xml", ExperimentFilename);
            File.Copy("schemas//sample-protocol.xml", ProtocolFilename);

            CreateDevelopmentEnvironment();
        }

        public static void CreateDevelopmentEnvironment()
        {
            string exp1Path = Path.Combine(SystemSettings.BasePath, "exp1");
            string exp2Path = Path.Combine(SystemSettings.BasePath, "exp2");

            CreateExperiment(exp1Path, "Experiment 1", "Experiment 1 description");
            CreateExperiment(exp2Path, "Experiment 2", "Experiment 2 description");
        }

        public static void CreateExperiment(string expPath, string name, string description)
        {
            if (!Directory.Exists(expPath))
            {
                if (!File.Exists("schemas//sample-protocol.prtx"))
                {
                    File.Copy("schemas//sample-protocol.xml", "schemas//sample-protocol.prtx");
                }

                Directory.CreateDirectory(expPath);
                var exp = Experiment.Load("schemas//sample-experiment.xml");
                exp.Name = name;
                exp.Description = description;
                exp.Save(Path.Combine(expPath, ExperimentFilename));
                File.Copy("schemas//sample-protocol.xml", Path.Combine(expPath, ProtocolFilename));
            }
        }

        [TestMethod]
        public void CORE_FileSystemManagerTests_TestBasePath()
        {
            Assert.IsTrue(Directory.Exists(SystemSettings.BasePath));
        }

        [TestMethod]
        public void CORE_FileSystemManagerTests_TestLoadExperiment()
        {
            var experiment = Experiment.Load(ExperimentFilename);
            Assert.IsNotNull(experiment);
            Assert.AreEqual("Sample Experiment", experiment.Name);
            Assert.IsNotNull(experiment.Protocol);
            Assert.AreEqual("Sample Protocol", experiment.Protocol.Name);
        }

        [TestMethod]
        public void CORE_FileSystemManagerTests_LoadExperiments()
        {
            var experiments = Experiment.GetExperiments();
            Assert.IsNotNull(experiments);
        }

        [TestMethod]
        public void CORE_EnumerateSessions()
        {
            var experiments = Experiment.GetExperiments();
            Assert.IsNotNull(experiments);
            Assert.AreEqual(1, experiments[0].NumberOfSessions);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            //File.Delete(ExperimentFilename);
            //File.Delete(ProtocolFilename);
        }
    }
}
