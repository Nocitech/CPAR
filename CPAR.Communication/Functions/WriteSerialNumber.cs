using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class WriteSerialNumber :
        Function
    {
        private static byte ResponseLength = 0;

        public WriteSerialNumber() : base(0x05, 2) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [XmlAttribute("serial-number")]
        public UInt16 SerialNumber
        {
            get
            {
                return request.GetUInt16(0);
            }
            set
            {
                request.InsertUInt16(0, value);
            }
        }

        public override string ToString()
        {
            return "[0x05] Write Serial Number";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("WRITE SERIAL NUMBER");
            builder.AppendLine("- Serial Number: " + SerialNumber.ToString());

            return builder.ToString();
        }
    }
}
