using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPAR.Communication;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class SetDigitalPin :
        Function
    {
        public enum PinType
        {
            PIN_LED_DEBUG01 = 0, ///< Debug Diode 1
            PIN_LED_DEBUG02, ///< Debug Diode 2
            PIN_LED_DEBUG03, ///< Debug Diode 3
            PIN_COMPRESSOR_POWER,
            PIN_STOP_BUTTON,
            PIN_VASMETER_CONNECTED,
            PIN_EMERGENCY_STOP,
            PIN_PWM_VALVE_OC3A,
            PIN_PWM_VALVE_OC3B,
            PIN_12V_POWER,
            PIN_EOL
        };

        private static byte ResponseLength = 0;

        public SetDigitalPin() : base(0x20, 2) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [XmlAttribute("pin")]
        public PinType Pin
        {
            get
            {
                return (PinType) request.GetByte(0);
            }
            set
            {
                request.InsertByte(0, (byte)value);
            }
        }

        [XmlAttribute("value")]
        public bool Value
        {
            get
            {
                return request.GetByte(1) != 0U;
            }
            set
            {
                request.InsertByte(1, (byte) (value ? 1U : 0U));
            }
        }

        public override string ToString()
        {
            return "[0x20] SetDigitalPin";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("WRITE DIGITAL PIN");
            builder.AppendLine("- PIN [ " + " ] : " + Value.ToString());

            return builder.ToString();
        }
    }
}
