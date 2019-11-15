using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPAR.Core.Exporters;
using CPAR.Core.Results;
using System.Collections.Generic;
using CPAR.Core;

namespace CPAR.UnitTests.Core.Exporters
{
    [TestClass]
    public class OutputScriptTests
    {
        [TestMethod]
        public void CORE_EXPORTERS_BasicTest()
        {
            var R = new System.Collections.Generic.Dictionary<string, CPAR.Core.Result>() { };
            var engine = new SessionScriptEngine(R);

            Assert.AreEqual(2, (new OutputScript()
            {
                Name = "test-variable",
                Script = "2"
            }).Execute(engine));
            Assert.AreEqual(4, (new OutputScript()
            {
                Name = "test-variable",
                Script = "2+2"
            }).Execute(engine));
            Assert.AreEqual((new OutputScript()
            {
                Name = "test-variable",
                Script = "2/2"
            }).Execute(engine), 1);
        }

        [TestMethod]
        public void CORE_EXPORTERS_WithBasicVariables()
        {
            var R = new Dictionary<string, CPAR.Core.Result>()
            {
                {"T01", CreateSR()},

            };
            var engine = new SessionScriptEngine(R);

            Assert.AreEqual(0, (new OutputScript()
            {
                Name = "test-variable",
                Script = @"R['T01'].Index"
            }).Execute(engine));
            Assert.AreEqual(10, (new OutputScript()
            {
                Name = "test-variable",
                Script = @"R['T01'].PTL"
            }).Execute(engine));
            Assert.AreEqual(30, (new OutputScript()
            {
                Name = "test-variable",
                Script = @"R['T01'].PDT"
            }).Execute(engine));
            Assert.AreEqual(100, (new OutputScript()
            {
                Name = "test-variable",
                Script = @"R['T01'].PTT"
            }).Execute(engine));
            Assert.AreEqual(25, (new OutputScript()
            {
                Name = "test-variable",
                Script = @"R['T01'].GetPressureFromPerception(2.5)"
            }).Execute(engine));
        }

        private StimulusResponseResult CreateSR()
        {
            var result = new StimulusResponseResult()
            {
                ID = "T01",
                Index = 0,
                Name ="Stimulus-Response",
                Conditioned = false,
                Data = new List<CPAR.Core.Result.DataPoint>()
                {
                    new Result.DataPoint() { conditioning = 0, stimulating = 0, VAS = 0 },
                    new Result.DataPoint() { conditioning = 0, stimulating = 10, VAS = 1},
                    new Result.DataPoint() { conditioning = 0, stimulating = 20, VAS = 2},
                    new Result.DataPoint() { conditioning = 0, stimulating = 30, VAS = 3},
                    new Result.DataPoint() { conditioning = 0, stimulating = 40, VAS = 4},
                    new Result.DataPoint() { conditioning = 0, stimulating = 50, VAS = 5},
                    new Result.DataPoint() { conditioning = 0, stimulating = 60, VAS = 6},
                    new Result.DataPoint() { conditioning = 0, stimulating = 70, VAS = 7},
                    new Result.DataPoint() { conditioning = 0, stimulating = 80, VAS = 8},
                    new Result.DataPoint() { conditioning = 0, stimulating = 90, VAS = 9},
                    new Result.DataPoint() { conditioning = 0, stimulating = 100, VAS = 10}
                },
                VAS_PDT = 2
            };

            return result;
        }
    }
}
