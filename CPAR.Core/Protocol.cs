using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Core.Tests;
using System.Xml.Serialization;
using System.IO;

namespace CPAR.Core
{
    [XmlRoot("protocol")]
    public class Protocol
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlArray("tests")]
        [XmlArrayItem(Type=typeof(StimulusResponseTest), ElementName = "stimulus-response"),
         XmlArrayItem(Type = typeof(TemporalSummationTest), ElementName = "temporal-summation"),
         XmlArrayItem(Type = typeof(ConditionedPainTest), ElementName = "conditioned-pain-modulation"),
         XmlArrayItem(Type = typeof(StartleResponseTest), ElementName = "startle-response"),
         XmlArrayItem(Type = typeof(StaticTemporalSummationTest), ElementName = "static-temporal-summation"),
         XmlArrayItem(Type = typeof(ArbitraryTemporalSummationTest), ElementName = "arbitrary-temporal-summation")]
        public Test[] Tests { get; set; }

        public static Protocol Load(string filename)
        {
            Protocol retValue = null;

            XmlSerializer serializer = new XmlSerializer(typeof(Protocol));

            using (var reader = new StreamReader(filename))
            {
                retValue = (Protocol)serializer.Deserialize(reader);
                ThrowIf.Argument.IsNull(retValue.Tests, "Protocol.Tests");
                ThrowIf.Array.IsEmpty(retValue.Tests, "Protocol.Tests");
                retValue.Tests.IndexArray();
            }

            return retValue;
        }

        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Protocol));

            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }
        }

        public Test GetTest(string id)
        {
            return Tests.First((t) => t.ID == id);
        }
    }
}
