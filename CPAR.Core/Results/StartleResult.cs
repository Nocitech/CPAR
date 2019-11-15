using CPAR.Core.Tests;
using CPAR.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core.Results
{
    public class StartleResult :
        Result
    {
         public class Probe
        {
            [XmlAttribute("type")]
            public StartleResponseTest.ProbeType ProbeType { get; set; }
            [XmlAttribute("vas")]
            public double VAS { get; set; }

            public bool IsStartle
            {
                get
                {
                    return ProbeType == StartleResponseTest.ProbeType.STARTLE;
                }
            }
        }

        public override string DisplayResult()
        {
            StringBuilder retValue = new StringBuilder();
            int n = 1;

            retValue.AppendFormatLine("STARTLE RESULT (Stimulating Pressure: {0:0.00}kPa, Startle Pressure {1:0.00})kPa", 
                StimulatingPressure, 
                StartlePressure);

            foreach (var p in Probes)
            {
                if (p.ProbeType == StartleResponseTest.ProbeType.STARTLE)
                {
                    retValue.AppendFormatLine("STARTLE PROBE [ {0} ] VAS: {1:0.00}cm, TYPE: {2}",
                        n, p.VAS, p.ProbeType);
                }
                else
                {
                    retValue.AppendFormatLine("NORMAL PROBE [ {0} ] VAS: {1:0.00}cm, TYPE: {2}",
                        n, p.VAS, p.ProbeType);
                }
                ++n;
            }

            return retValue.ToString();
        }

        public Probe[] GetStartleProbes()
        {
            return (from p in Probes
                    where p.ProbeType == StartleResponseTest.ProbeType.STARTLE
                    select p).ToArray();
        }

        public Probe[] GetNormalProbes()
        {
            return (from p in Probes
                    where p.ProbeType == StartleResponseTest.ProbeType.NORMAL
                    select p).ToArray();
        }

        public void CreateProbes(StartleResponseTest.ProbeParameters[] probes)
        {
            StringBuilder str = new StringBuilder();
            Probes = new List<Probe>();

            str.Append("PROBES: ");

            foreach (var probe in probes)
            {
                var element = new Probe()
                {
                    ProbeType = probe.STARTLE
                };
                Probes.Add(element);
                str.AppendFormat("{0} ", element.ProbeType.ToString());
            }

            Log.Debug(str.ToString());
        }

        [XmlAttribute("startle-pressure")]
        public double StartlePressure { get; set; }

        [XmlAttribute("stimulating-pressure")]
        public double StimulatingPressure { get; set; }

        [XmlArray("probes")]
        [XmlArrayItem("probe")]
        public List<Probe> Probes { get; set; }
    }
}
