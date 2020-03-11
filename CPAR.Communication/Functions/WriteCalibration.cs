using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class WriteCalibration :
        Function
    {
        public static byte FUNCTION_CODE = 0x16;
        private static byte CALIBRATION_RECORD_SIZE = 10;
        private static byte VALID_MARKER = 0xC9;
        private static byte ResponseLength = 0;

        public WriteCalibration() : 
            base(FUNCTION_CODE, (byte) (CALIBRATION_RECORD_SIZE + 1))
        {
            Calibrator = CalibratorID.ID_VAS_SCORE_CALIBRATOR;
            request.InsertByte(1, VALID_MARKER);
            A = 1;
            B = 0;
        }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

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

        [XmlAttribute("A")]
        public double A
        {
            get
            {
                return ((double)request.GetInt32(2)) / 256;
            }
            set
            {
                request.InsertInt32(2, (Int32)Math.Truncate(value * 256));
                CreateChecksum();
            }
        }

        [XmlAttribute("B")]
        public double B
        {
            get
            {
                return ((double)request.GetInt32(6)) / 256;
            }
            set
            {
                request.InsertInt32(6, (Int32)Math.Truncate(value * 256));
                CreateChecksum();
            }
        }

        public byte Checksum
        {
            get
            {
                return request.GetByte(CALIBRATION_RECORD_SIZE);
            }
        }

        private void CreateChecksum()
        {
            byte checksum = 0;

            for (int n = 1; n < CALIBRATION_RECORD_SIZE; ++n)
            {
                checksum = CRC8CCITT.Update(checksum, request.GetByte(n));
            }

            request.InsertByte(CALIBRATION_RECORD_SIZE, checksum);
        }

        public override string ToString()
        {
            return "[0x06] Write Calibration Record";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("WRITE CALIBRATION");
            builder.AppendFormat("Record      : {0}", Calibrator.ToString());
            builder.AppendLine();
            builder.AppendFormat("Calibration : {0}*x + {1}", A, B);
            builder.AppendLine();
            builder.AppendFormat("Checksum    : {0}", Checksum);
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
