using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CPAR.Communication;

namespace CPAR.UnitTests.Communication
{
    [TestClass]
    public class CRC8
    {
        [TestMethod]
        public void Communication_CRC8_CRCTest()
        {
            byte[] testdata1 = new byte[] { 0x01, 0x02, 0x04, 0x05 };
            byte[] testdata2 = new byte[] { 0x01, 0x02, 0x04, 0x06 };
            byte crcValue1 = CRC8CCITT.Calculate(testdata1);
            byte crcValue2 = CRC8CCITT.Calculate(testdata2);
            Assert.AreNotEqual(crcValue2, crcValue1);
        }
    }
}
