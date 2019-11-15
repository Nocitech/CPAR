using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CPAR.Communication;
using System.IO.Ports;

namespace CPAR.UnitTests.Communication
{
    [TestClass]
    public class PortSelectorTest
    {
        public string[] GetTestPorts()
        {
            return testPorts.ToArray();
        }

        [TestMethod]
        public void Communication_PortSelectorTest_TestObjectCreationWithRealEnumerator()
        {
            var manager = new PortSelector(SerialPort.GetPortNames);
        }

        [TestMethod]
        public void Communication_PortSelectorTest_TestNoPortsAvailable()
        {
            testPorts = new List<string>();
            var selector = new PortSelector(GetTestPorts);

            Assert.IsNull(selector.Next());
        }

        [TestMethod]
        public void Communication_PortSelectorTest_TestStaticScenario()
        {
            testPorts = new List<string>();
            testPorts.Add("COM0");
            testPorts.Add("COM1");
            testPorts.Add("COM2");
            var selector = new PortSelector(GetTestPorts);

            Assert.AreEqual("COM0", selector.Next());
            Assert.AreEqual("COM1", selector.Next());
            Assert.AreEqual("COM2", selector.Next());
            Assert.AreEqual("COM0", selector.Next());
        }

        [TestMethod]
        public void Communication_PortSelectorTest_TestDynamicPort()
        {
            testPorts = new List<string>();
            testPorts.Add("COM0");
            testPorts.Add("COM1");
            testPorts.Add("COM2");
            var selector = new PortSelector(GetTestPorts);

            Assert.AreEqual("COM0", selector.Next());
            Assert.AreEqual("COM1", selector.Next());
            Assert.AreEqual("COM2", selector.Next());
            Assert.AreEqual("COM0", selector.Next());
            testPorts.Add("COM3");

            Assert.AreEqual("COM1", selector.Next());
            Assert.AreEqual("COM2", selector.Next());
            Assert.AreEqual("COM3", selector.Next());
            Assert.AreEqual("COM0", selector.Next());

            testPorts.Remove("COM1");
            Assert.AreEqual("COM2", selector.Next());
            Assert.AreEqual("COM3", selector.Next());
            Assert.AreEqual("COM0", selector.Next());
            Assert.AreEqual("COM2", selector.Next());
            Assert.AreEqual("COM3", selector.Next());
            Assert.AreEqual("COM0", selector.Next());
            Assert.AreEqual("COM2", selector.Next());

            testPorts = new List<string>();
            Assert.IsNull(selector.Next());
        }

        List<string> testPorts = new List<string>();
    }
}
