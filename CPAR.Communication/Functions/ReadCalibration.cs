using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class ReadCalibration : 
        Function
    {
        public static byte FUNCTION_CODE = 0x07;
        private static byte CALIBRATION_RECORD_SIZE = 10;
        private static byte VALID_MARKER = 0xC9;
        private static byte ResponseLength = CALIBRATION_RECORD_SIZE;

        public ReadCalibration() : 
            base(FUNCTION_CODE, 1)
        {
            Calibrator = CalibratorID.ID_VAS_SCORE_CALIBRATOR;
        }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [Category("Calibrator")]
        [XmlAttribute("calibrator")]
        public CalibratorID Calibrator
        {
            get
            {
                return (CalibratorID)request.GetByte(0);
            }
            set
            {
                request.InsertByte(0, (byte)value);
            }
        }

        [Category("Calibration Record")]
        public bool ValidMarker
        {
            get
            {
                if (response != null)
                    return response.GetByte(0) == VALID_MARKER;
                else
                    return false;
            }
        }

        [Category("Calibration Record")]
        public double A
        {
            get
            {
                if (response != null)
                    return ((double)response.GetInt32(1)) / 256;
                else
                    return 0;
            }
        }

        [Category("Calibration Record")]
        public double B
        {
            get
            {
                if (response != null)
                    return ((double)response.GetInt32(5)) / 256;
                else
                    return 0;
            }
        }

        [Category("Calibration Record")]
        public byte Checksum
        {
            get
            {
                if (response != null)
                    return response.GetByte(CALIBRATION_RECORD_SIZE - 1);
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            return "[0x07] Read Calibration Record";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("READ CALIBRATION");
            builder.AppendFormat("Record      : {0} ({1})", Calibrator.ToString(), ValidMarker);
            builder.AppendLine();
            if (ValidMarker)
            {
                builder.AppendFormat("Calibration : {0}*x + {1}", A, B);
                builder.AppendLine();
                builder.AppendFormat("Checksum    : {0}", Checksum);
                builder.AppendLine();
            }

            return builder.ToString();

        }
    }
}
