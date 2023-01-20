using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class StartStimulation :
        Function
    {
        public enum StopCriterion
        {
            STOP_CRITERION_ON_BUTTON_VAS = 0,
            STOP_CRITERION_ON_BUTTON
        }

        private static byte ResponseLength = 0;

        public StartStimulation() : 
            base(0x11, 2)
        {
            Criterion = AlgometerStopCriterion.STOP_CRITERION_ON_BUTTON_VAS;
            CompressorMode = AlgometerCompressorMode.AUTO;
        }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [Category("Stop Criterion")]
        [Description("Stop criterion for the stimulation")]
        [XmlAttribute("stop-criterion")]
        public AlgometerStopCriterion Criterion
        {
            get => (AlgometerStopCriterion)request.GetByte(0);
            set => request.InsertByte(0, (byte)value);
        }

        [XmlAttribute("compressor-mode")]
        public AlgometerCompressorMode CompressorMode
        {
            get => (AlgometerCompressorMode)request.GetByte(1);
            set => request.InsertByte(1, (byte)value);
        }

        public override string ToString()
        {
            return "[0x11] Start Stimulation";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("START STIMULATION");

            return builder.ToString();

        }
    }
}
