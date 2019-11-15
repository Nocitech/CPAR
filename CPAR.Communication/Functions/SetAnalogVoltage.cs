using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Communication;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class SetAnalogVoltage :
        Function
    {
        public enum PWMChannel
        {
            PWM_TARGET_PRESSURE01,
            PWM_TARGET_PRESSURE02
        };

        private static byte ResponseLength = 0;

        public SetAnalogVoltage() : base(0x22, 3) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [XmlAttribute("Channel")]
        public PWMChannel Channel
        {
            get
            {
                return (PWMChannel)request.GetByte(0);
            }
            set
            {
                request.InsertByte(0, (byte)value);
            }
        }

        [XmlAttribute("value")]
        public UInt16 Value
        {
            get
            {
                return request.GetUInt16(1);
            }
            set
            {
                request.InsertUInt16(1, value);
            }
        }


        public override string ToString()
        {
            return "[0x21] SetAnalogVoltage";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("SET ANALOG VOLTAGE");

            return builder.ToString();
        }
    }
}
