using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public class Factor : IArrayIndex
    {
        public const int MAX_FACTORS = 7;
        private static Level[] activeLevels = null;

        public class Level : IArrayIndex
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlIgnore]
            public int Index { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public Factor()
        {
            Levels = new Level[] { };
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("level")]
        public Level[] Levels { get; set; }

        [XmlIgnore]
        public int Count
        {
            get
            {
                int retValue = 0;

                if (Levels != null)
                    retValue = Levels.Length;

                return retValue;
            }
        }

        [XmlIgnore]
        public int Index { get; set; }

        public static string CreateID(Factor.Level[] factors)
        {
            ThrowIf.Argument.IsNull(factors, "levels");
            ThrowIf.Array.IsEmpty(factors, "levels");
            var builder = new StringBuilder(factors[0].Name);

            for (int i = 1; i < factors.Length; ++i)
            {
                builder.AppendFormat(".{0}", factors[i].Name);
            }

            return builder.ToString();
        }

        public static Level[] Active
        {
            get
            {
                return activeLevels;
            }
            set
            {
                activeLevels = value;
            }
        }
    }
}
