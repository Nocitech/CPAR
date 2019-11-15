using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Communication.Functions;
using System.Xml.Serialization;
using CPAR.Communication;
using System.IO;

namespace CPAR.Tester
{
    [XmlRoot("script")]
    public class TestScript
    {
        public TestScript()
        {
            Name = "No name";
            Functions = new Function[] { };
        }

        [XmlAttribute("name")]
        public string Name { get; set;  }

        /*
        [XmlArrayItem("DeviceIdentification", typeof(DeviceIdentification))]
        [XmlArrayItem("KickWatchdog", typeof(KickWatchdog))]
        [XmlArrayItem("ReadCalibration", typeof(ReadCalibration))]
        [XmlArrayItem("ResetDevice", typeof(ResetDevice))]
        [XmlArrayItem("SetWaveformProgram", typeof(SetWaveformProgram))]
        [XmlArrayItem("StartStimulation", typeof(StartStimulation))]
        [XmlArrayItem("StopStimulation", typeof(StopStimulation))]
        [XmlArrayItem("WriteCalibration", typeof(WriteCalibration))]
        [XmlArrayItem("WriteSerialNumber", typeof(WriteSerialNumber))]
        */
        [XmlElement("DeviceIdentification", typeof(DeviceIdentification))]
        [XmlElement("KickWatchdog", typeof(KickWatchdog))]
        [XmlElement("ReadCalibration", typeof(ReadCalibration))]
        [XmlElement("ResetDevice", typeof(ResetDevice))]
        [XmlElement("SetWaveformProgram", typeof(SetWaveformProgram))]
        [XmlElement("StartStimulation", typeof(StartStimulation))]
        [XmlElement("StopStimulation", typeof(StopStimulation))]
        [XmlElement("WriteCalibration", typeof(WriteCalibration))]
        [XmlElement("WriteSerialNumber", typeof(WriteSerialNumber))]
        public Function[] Functions { get; set; }

        public static TestScript Load(string filename)
        {
            TestScript retValue = null;

            XmlSerializer serializer = new XmlSerializer(typeof(TestScript));

            using (var reader = new StreamReader(filename))
            {
                retValue = (TestScript)serializer.Deserialize(reader);
            }

            return retValue;
        }

        public void Start()
        {
            if (Functions != null)
            {
                index = 0;
                running = true;
            }
        }

        public Function Next()
        {
            Function retValue = null;

            if ((Functions != null) && running)
            {
                if (index < Functions.Length)
                {
                    retValue = Functions[index];
                    ++index;
                }
                else
                {
                    running = false;
                }
            }

            return retValue;
        }

        public bool Running
        {
            get
            {
                return running;
            }
        }

        private int index;
        private bool running;
    }
}
