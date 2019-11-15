using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Core;
using System.Xml.Serialization;
using System.IO;

namespace CPAR.UnitTests.Core
{
    [TestClass]
    public class Serializing
    {
        [TestMethod]
        public void CORE_Serializing_TestSerializingSessions()
        {
            var session = new Session()
            {
                ID = "TestID",
                Levels = new Factor.Level[] { new Factor.Level() { Index = 0, Name = "Dummy" } },
                Results = new Result[1]                
            };


            XmlSerializer serializer = new XmlSerializer(typeof(Session));

            using (var writer = new StreamWriter("session.xml"))
            {
                serializer.Serialize(writer, session);
            }
        }
    }
}
