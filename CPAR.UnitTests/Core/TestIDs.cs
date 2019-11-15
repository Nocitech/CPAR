using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.UnitTests.Core
{
    [TestClass]
    public class TestIDs
    {
        [TestMethod]
        public void CORE_Multiple_ID_Test()
        {
            string ids = "T01;T02;T03;";
            string[] tokens = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(3, tokens.Length);
            Assert.AreEqual("T01", tokens[0]);
            Assert.AreEqual("T02", tokens[1]);
            Assert.AreEqual("T03", tokens[2]);
        }

        [TestMethod]
        public void CORE_Single_ID_Test()
        {
            string ids = "T01";
            string[] tokens = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(1, tokens.Length);
            Assert.AreEqual("T01", tokens[0]);
        }

        [TestMethod]
        public void CORE_No_ID_Test()
        {
            string ids = "";
            string[] tokens = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(0, tokens.Length);
        }
    }
}