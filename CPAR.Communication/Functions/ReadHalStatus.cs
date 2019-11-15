using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class ReadHalStatus :
        Function
    {
        private static byte ResponseLength = 18;

        public ReadHalStatus() : base(0x21, 0) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        #region DIGITAL PINS
        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_LED_DEBUG01
        {
            get
            {
                return response.GetByte(0) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_LED_DEBUG02
        {
            get
            {
                return response.GetByte(1) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_LED_DEBUG03
        {
            get
            {
                return response.GetByte(2) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_COMPRESSOR_POWER
        {
            get
            {
                return response.GetByte(3) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_STOP_BUTTON
        {
            get
            {
                return response.GetByte(4) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_VASMETER_CONNECTED
        {
            get
            {
                return response.GetByte(5) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_EMERGENCY_STOP
        {
            get
            {
                return response.GetByte(6) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_PWM_VALVE_OC3A
        {
            get
            {
                return response.GetByte(7) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_PWM_VALVE_OC3B
        {
            get
            {
                return response.GetByte(8) != 0;
            }
        }

        [Category("Digital Pins")]
        [XmlIgnore]
        public bool PIN_12V_POWER
        {
            get
            {
                return response.GetByte(9) != 0;
            }
        }
        #endregion
        #region ANALOG VOLTAGES
        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_EMERGENCY_STOP
        {
            get
            {
                return response.GetByte(10);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_TEMPERATURE
        {
            get
            {
                return response.GetByte(11);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_SUPPLY_PRESSURE
        {
            get
            {
                return response.GetByte(12);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_ACTUAL_PRESSURE01
        {
            get
            {
                return response.GetByte(13);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_ACTUAL_PRESSURE02
        {
            get
            {
                return response.GetByte(14);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_VAS_SCORE
        {
            get
            {
                return response.GetByte(15);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_VAS_BUTTON
        {
            get
            {
                return response.GetByte(16);
            }
        }

        [Category("Analog Voltages")]
        [XmlIgnore]
        public Byte CHAN_12V_POWER
        {
            get
            {
                return response.GetByte(17);
            }
        }

        #endregion

        public override string ToString()
        {
            return "[0x21] ReadHalStatus";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("READ HAL STATUS");
            builder.AppendLine("- PIN_LED_DEBUG01: " + PIN_LED_DEBUG01);
            builder.AppendLine("- PIN_LED_DEBUG02: " + PIN_LED_DEBUG02);
            builder.AppendLine("- PIN_LED_DEBUG03: " + PIN_LED_DEBUG03);
            builder.AppendLine("- PIN_COMPRESSOR_POWER: " + PIN_COMPRESSOR_POWER);
            builder.AppendLine("- PIN_STOP_BUTTON: " + PIN_STOP_BUTTON);
            builder.AppendLine("- PIN_VASMETER_CONNECTED: " + PIN_VASMETER_CONNECTED);
            builder.AppendLine("- PIN_EMERGENCY_STOP: " + PIN_EMERGENCY_STOP);
            builder.AppendLine("- PIN_PWM_VALVE_OC3A: " + PIN_PWM_VALVE_OC3A);
            builder.AppendLine("- PIN_PWM_VALVE_OC3B: " + PIN_PWM_VALVE_OC3B);
            builder.AppendLine("- PIN_12V_POWER: " + PIN_12V_POWER);

            builder.AppendLine("- CHAN_EMERGENCY_STOP: " + CHAN_EMERGENCY_STOP);
            builder.AppendLine("- CHAN_TEMPERATURE: " + CHAN_TEMPERATURE);
            builder.AppendLine("- CHAN_SUPPLY_PRESSURE: " + CHAN_SUPPLY_PRESSURE);
            builder.AppendLine("- CHAN_ACTUAL_PRESSURE01: " + CHAN_ACTUAL_PRESSURE01);
            builder.AppendLine("- CHAN_ACTUAL_PRESSURE02: " + CHAN_ACTUAL_PRESSURE02);
            builder.AppendLine("- CHAN_VAS_SCORE: " + CHAN_VAS_SCORE);
            builder.AppendLine("- CHAN_VAS_BUTTON: " + CHAN_VAS_BUTTON);
            builder.AppendLine("- CHAN_12V_POWER: " + CHAN_12V_POWER);

            return builder.ToString();
        }
    }
}
