using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Core;
using CPAR.Core.Results;

namespace CPAR.UnitTests.Core
{
    [TestClass]
    public class ResultTest
    {
        private StimulusResponseResult CreateSR_Result()
        {
            var result = new StimulusResponseResult()
            {
                Conditioned = false,
                VAS_PDT = 0.1,
                ID = "T01",
                Name = "Test 1"
            };

            return result;
        }

        private void AddData(StimulusResponseResult result)
        {
            result.Add(0, 0, 0);
            result.Add(1, 0, 0);
            result.Add(2, 0, 0);
            result.Add(3, 0, 1);
            result.Add(4, 0, 2);
            result.Add(5, 0, 3);
            result.Add(6, 0, 4);
            result.Add(7, 0, 5);
            result.Add(8, 0, 6);
            result.Add(9, 0, 7);
            result.Add(10, 0, 8);
            result.Add(11, 0, 9);
            result.Add(12, 0, 10);
        }

        [TestMethod]
        public void CORE_ResultTest_TestPerceptionScore()
        {
            var result = CreateSR_Result();

            result.Add(0, 0, 0);
            result.Add(1, 0, 0);
            result.Add(2, 0, 0);
            result.Add(3, 0, 1);
            Assert.AreEqual(3, result.PTT);
            Assert.AreEqual(1, result.PTL);
            result.Add(4, 0, 2);
            result.Add(5, 0, 3);
            result.Add(6, 0, 4);
            result.Add(7, 0, 5);
            Assert.AreEqual(3, result.PDT);

            Assert.IsTrue(result.IsScoreAvailable(1), "VAS Score 1 should be present");
            Assert.IsTrue(result.IsScoreAvailable(1.5), "VAS Score 1.5 should be present");
            Assert.IsFalse(result.IsScoreAvailable(6), "VAS Score of 6 should not be present");

            Assert.AreEqual(3.5, result.GetPressureFromPerception(1.5), "VAS score of 1.5 should be a pressure of 3.5");
            Assert.AreEqual(-1, result.GetPressureFromPerception(6), "Pressure should be -1 if the score is not available");
        }
    }
}
