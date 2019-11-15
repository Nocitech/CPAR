using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
    /**
     * \brief Manages the list of port names
     * This class handles the available port names when the system is scannning for a CPAR
     * device.
     */
    public class PortSelector
    {
        public delegate string[] PortEnumerator();

        public PortSelector(PortEnumerator enumerator)
        {
            this.enumerator = enumerator;
            index = 0;
        }

        /**
         * \brief Get the next port to be tested
         * 
         * \return the name of the next port to be tested.
         */
        public string Next()
        {
            string retValue = null;
            var ports = enumerator();

            if (ports.Length > 0)
            {
                if (index < ports.Length)
                {
                    retValue = ports[index];
                }
                else
                {
                    index = 0;
                    retValue = ports[index];
                }

                ++index;
            }

            return retValue;
        }

        private PortEnumerator enumerator;
        private int index;
    }
}
