﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Communication.Functions
{
    public class DeviceIdentification :
       Function
    {
        private static byte ResponseLength = 64;

        public DeviceIdentification() : base(0x01) { }

        protected override bool IsResponseValid()
        {
            return response?.Length == ResponseLength;
        }

        [XmlIgnore]
        [Category("Manufacturer")]
        [Description("The identifier of the manufacturer")]
        public UInt32 ManufactureID
        {
            get => response is object ? response.GetUInt32(0) : 0;
        }

        [XmlIgnore]
        [Category("Manufacturer")]
        [Description("The name of the manufacturer")]
        public string Manufacture
        {
            get => response is object ? response.GetString(16, 24) : "";
        }

        [XmlIgnore]
        [Category("Device")]
        [Description("The identifier of the device")]
        public UInt16 DeviceID
        {
            get => response is object ? response.GetUInt16(4) : (UInt16) 0;
        }

        [XmlIgnore]
        [Category("Device")]
        [Description("The name of the device")]
        public string Device
        {
            get => response is object ? response.GetString(40, 24) : "";
        }


        [XmlIgnore]
        [Category("Firmware")]
        [Description("Major Version")]
        public byte MajorVersion
        {
            get => response is object ? response.GetByte(10) : (byte) 0;
        }

        [XmlIgnore]
        [Category("Firmware")]
        [Description("Major Version")]
        public byte MinorVersion
        {
            get => response is object ? response.GetByte(11) : (byte) 0;
        }

        [XmlIgnore]
        [Category("Firmware")]
        [Description("Patch Version")]
        public byte PatchVersion
        {
            get => response is object ? response.GetByte(12) : (byte) 0;
        }

        [XmlIgnore]
        [Category("Firmware")]
        [Description("Engineering Version")]
        public byte EngineeringVersion
        {
            get =>  response is object ? response.GetByte(13) : (byte)0;
        }

        [XmlIgnore]
        [Category("Firmware")]
        [Description("Version")]
        public string Version => EngineeringVersion == 0 ?
                                 $"{MajorVersion}.{MinorVersion}.{PatchVersion}" :
                                 $"{MajorVersion}.{MinorVersion}.{PatchVersion}.r{EngineeringVersion}";

        [XmlIgnore]
        [Category("Device")]
        [Description("The serial number of device that is connected")]
        public UInt32 SerialNumber
        {
            get => response is object ? response.GetUInt32(6) : (UInt32) 0;
        }

        [XmlIgnore]
        [Category("Firmware")]
        [Description("The checksum number of device that is connected")]
        public UInt16 Checksum
        {
            get => response is object ? response.GetUInt16(14) : (UInt16) 0;
        }

        public override string ToString() => "[0x01] Device Identification";


        public override string SerializeResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("DEVICE IDENTIFICATION");
            builder.AppendFormat("- Identity          : {0}:{1}", Manufacture, Device); builder.AppendLine();
            builder.AppendFormat("- Software Revision : {0}", Version); builder.AppendLine();
            builder.AppendFormat("- Serial Number     : {0}", SerialNumber); builder.AppendLine();
            builder.AppendFormat("- Checksum          : {0}", Checksum); builder.AppendLine();

            return builder.ToString();

        }
    }
}
