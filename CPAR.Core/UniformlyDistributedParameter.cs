using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public class UniformlyDistributedParameter
    {
        [XmlAttribute("min")]
        public double Minimum { get; set; }
        [XmlAttribute("max")]
        public double Maximum { get; set; }
        [XmlAttribute("seed")]
        public int Seed
        {
            get
            {
                return seed;
            }
            set
            {
                seed = value;
                random = new Random(seed);
            }
        }

        public UniformlyDistributedParameter()
        {
            seed = Guid.NewGuid().GetHashCode();
            random = new Random(seed);
        }

        [XmlIgnore]
        public double Value
        {
            get
            {
                return (Maximum-Minimum) * random.NextDouble() + Minimum;
            }
        }

        private Random random;
        private int seed;
    }
}
