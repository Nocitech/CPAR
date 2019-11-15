using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Functions
{
    public class DeviceIdentification :
       Function
    {
        private static byte ResponseLength = 6;

        public DeviceIdentification() : base(0x01) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [Category("Device")]
        [Description("The type of device that is connected")]
        public DeviceID Identity
        {
            get
            {
                if (response != null)
                {
                    Int32 value = response.GetByte(0);

                    if (Enum.IsDefined(typeof(DeviceID), value))
                        return (DeviceID)value;
                    else
                        return DeviceID.UNKNOWN;
                }
                else
                    return 0;
            }
        }

        [Category("Revision")]
        [Description("Major Version")]
        public byte MajorRevision
        {
            get
            {
                if (response != null)
                {
                    return response.GetByte(1);
                }
                else
                    return 0;
            }
        }

        [Category("Revision")]
        [Description("Engineering Version")]
        public byte EngineeringRevision
        {
            get
            {
                if (response != null)
                {
                    return response.GetByte(2);
                }
                else
                    return 0;
            }
        }

        [Category("Device")]
        [Description("The serial number of device that is connected")]
        public UInt16 SerialNumber
        {
            get
            {
                if (response != null)
                {
                    return response.GetUInt16(4);
                }
                else
                    return 0;
            }
        }

        [Category("Device")]
        [Description("The serial number of device that is connected")]
        public byte Checksum
        {
            get
            {
                if (response != null)
                {
                    return response.GetByte(3);
                }
                else
                    return 0;
            }
        }

        public override string ToString()
        {
            return "[0x01] Device Identification";
        }

        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("DEVICE IDENTIFICATION");
            builder.AppendFormat("- Identity          : {0}", Identity.ToString()); builder.AppendLine();
            builder.AppendFormat("- Software Revision : {0}rc{1}", MajorRevision, EngineeringRevision); builder.AppendLine();
            builder.AppendFormat("- Serial Number     : {0}", SerialNumber); builder.AppendLine();
            builder.AppendFormat("- Checksum          : {0}", Checksum); builder.AppendLine();

            return builder.ToString();

        }
    }
}
