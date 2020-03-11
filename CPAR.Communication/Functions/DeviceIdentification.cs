using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication.Functions
{
    public class DeviceIdentification :
       Function
    {
        private static byte ResponseLength = 64;

        public DeviceIdentification() : base(0x01) { }

        protected override bool IsResponseValid()
        {
            return response.Length == ResponseLength;
        }

        [Category("Manufacturer")]
        [Description("The identifier of the manufacturer")]
        public UInt32 ManufactureID
        {
            get
            {
                return response.GetUInt32(0);
            }
        }

        [Category("Manufacturer")]
        [Description("The name of the manufacturer")]
        public string Manufacture
        {
            get
            {
                return response.GetString(16, 24);
            }
        }

        [Category("Device")]
        [Description("The type of device that is connected")]
        public DeviceID Identity
        {
            get
            {
                if (response != null)
                {
                    Int32 value = response.GetUInt16(4);

                    if (Enum.IsDefined(typeof(DeviceID), value))
                        return (DeviceID)value;
                    else
                        return DeviceID.UNKNOWN;
                }
                else
                    return 0;
            }
        }

        [Category("Device")]
        [Description("The name of the device")]
        public string Device
        {
            get
            {
                return response.GetString(40, 24);
            }
        }

        [Category("Firmware")]
        [Description("Major Version")]
        public byte MajorRevision
        {
            get
            {
                return response.GetByte(10);
            }
        }

        [Category("Firmware")]
        [Description("Major Version")]
        public byte MinorVersion
        {
            get
            {
                return response.GetByte(11);
            }
        }

        [Category("Firmware")]
        [Description("Patch Version")]
        public byte PatchVersion
        {
            get
            {
                return response.GetByte(12);
            }
        }

        [Category("Firmware")]
        [Description("Engineering Version")]
        public byte EngineeringRevision
        {
            get
            {
                return response.GetByte(13);
            }
        }

        [Category("Firmware")]
        [Description("Version")]
        public string Version
        {
            get
            {
                if (response != null)
                {
                    return EngineeringRevision == 0 ?
                           String.Format(CultureInfo.CurrentCulture, "{0}.{1}.{2}", MajorRevision, MinorVersion, PatchVersion) :
                           String.Format(CultureInfo.CurrentCulture, "{0}.{1}.{2}.r{3}", MajorRevision, MinorVersion, PatchVersion, EngineeringRevision);
                }
                else
                    return "";
            }
        }

        [Category("Device")]
        [Description("The serial number of device that is connected")]
        public UInt32 SerialNumber
        {
            get
            {
                return response.GetUInt32(6);
            }
        }

        [Category("Firmware")]
        [Description("The checksum number of device that is connected")]
        public UInt16 Checksum
        {
            get
            {
                return response.GetUInt16(14);
            }
        }


        /*
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
        */
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
