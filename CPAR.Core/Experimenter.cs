using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public class Experimenter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("title")]
        public string Title { get; set; }
        [XmlAttribute("affiliation")]
        public string Affiliation { get; set; }
        [XmlAttribute("mail")]
        public string Mail { get; set; }
        [XmlAttribute("phone")]
        public string Phone { get; set; }


        public override string ToString()
        {
            return Name;
        }

        #region HANDLE ACTIVE EXPERIMENTER
        private static Experimenter activeExperimenter = null;

        public static Experimenter Active
        {
            get
            {
                return activeExperimenter;
            }
            set
            {
                activeExperimenter = value;
            }
        }
        #endregion
    }
}
