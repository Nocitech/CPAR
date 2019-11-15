using CPAR.Communication.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPAR.Core
{
    public abstract class Result :
        IArrayIndex
    {
        public class DataPoint
        {
            [XmlAttribute("s")]
            public double stimulating { get; set; }
            [XmlAttribute("c")]
            public double conditioning { get; set; }
            [XmlAttribute("vas")]
            public double VAS { get; set; }
        }

        public Result()
        {
            Data = new List<DataPoint>();
            VAS_PDT = 0.05;
        }

        [XmlAttribute(AttributeName = "ID")]
        /**
         * \brief ID 
         * ID is the identifier (ID) of the test that has produced the results.
         */
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "name")]
        /**
         * \brief Mnemotechnic name of the test that has produced the results.
         */
        public string Name { get; set; }

        [XmlAttribute("conditioned")]
        public bool Conditioned { get; set; }

        [XmlAttribute(AttributeName = "vas-pdt")]
        public double VAS_PDT { get; set; }

        [XmlIgnore]
        /**
         * \brief Pain Tolerance Threshold (PTT)
         * The Pain Tolerance Threshold is the maximal pressure a person can endure. Depending on 
         * the setup of the test this is either when 10cm is scored on the VAS scale or when the
         * person presses the stop button.
         */
        public double PTT
        {
            get
            {
                return Data.Count > 0 ? Data[Data.Count - 1].stimulating : -1;
            }
        }

        [XmlIgnore]
        /**
         * \brief Pain Tolerance Limit (PTL)
         * The Pain Tolerance Limit is the maximal VAS score that a person can endure. This does not 
         * actually make any sense, as a VAS of 10cm is defined as the maximal pain a person can endure.
         * However, some scientists has been using the VAS scale in this way and defining two "maximal pain"
         * definitions, even though it is per definition an erroneous scoring by subject rather than a valid
         * measurement if PTL is anything else than 10cm.
         * 
         * However, the customer get what the customer wants.
         */
        public double PTL
        {
            get
            {
                return Data.Count > 0 ? Data[Data.Count - 1].VAS : -1;
            }
        }

        [XmlIgnore]
        /**
         * \brief Pain Detection Threshold (PDT)
         * The Pain Detection Threshold is the minimal pressure that is perceived as pain by the 
         * test subject.
         */
        public double PDT
        {
            get
            {
                double retValue = -1;
                int index;

                if (FindIndexDownwards(VAS_PDT, out index))
                {
                    retValue = Data[index].stimulating;
                }

                return retValue;
            }
        }

        [XmlIgnore]
        public int Index { get; set; }

        /**
         * \brief Add a new data point to the result
         */
        public void Add(double s, double c, double vas)
        {
            Data.Add(new DataPoint()
            {
                stimulating = s,
                conditioning = c,
                VAS = vas
            });
        }

        public bool IsScoreAvailable(double score)
        {
            bool retValue = false;
            int upIndex, downIndex;

            if (FindIndexDownwards(score, out downIndex) && FindIndexUpwards(score, out upIndex))
            {
                retValue = true;
            }

            return retValue;
        }

        public double GetPressureFromPerception(double score)
        {
            double retValue = -1;
            int upIndex, downIndex;

            if (FindIndexDownwards(score, out downIndex) && FindIndexUpwards(score, out upIndex))
            {
                double x2 = Data[downIndex].VAS;
                double y2 = Data[downIndex].stimulating;
                double x1 = Data[upIndex].VAS;
                double y1 = Data[upIndex].stimulating;
                double a = (y2 - y1) / (x2 - x1);
                double b = y2 - a * x2;
                retValue = a * score + b;
            }

            return retValue;
        }

        private bool FindIndexDownwards(double score, out int index)
        {
            bool retValue = false;
            index = -1;

            for (int n = Data.Count - 2; n >= 0; --n)
            {
                if ((Data[n].VAS <= score) && (Data[n+1].VAS >= score))
                {
                    index = n + 1;
                    retValue = true;
                    break;
                }
            }            

            return retValue;
        }

        private bool FindIndexUpwards(double score, out int index)
        {
            bool retValue = false;
            index = -1;

            for (int n = 1; n < Data.Count; ++n)
            {
                if ((Data[n-1]. VAS <= score) && (Data[n].VAS >= score))
                {
                    index = n - 1;
                    retValue = true;
                    break;
                }
            }

            return retValue;
        }

        [XmlIgnore]
        public double[] StimulationPressure
        {
            get
            {
                return (from p in Data
                        select p.stimulating).ToArray();
            }
        }

        [XmlIgnore]
        public double[] ConditioningPressure
        {
            get
            {
                return (from p in Data
                        select p.conditioning).ToArray();
            }
        }

        [XmlIgnore]
        public double[] VAS
        {
            get
            {
                return (from p in Data
                        select p.VAS).ToArray();
            }
        }

        [XmlIgnore]
        public int Length
        {
            get
            {
                return Data.Count;
            }
        }

        /**
         * \brief Display the results
         * This function will format the results so they can be displayed in a 
         * text control.
         * 
         * \return the formatted results
         */
        public abstract string DisplayResult();

        [XmlArray("data")]
        [XmlArrayItem("p")]
        public List<DataPoint> Data { get; set; }
    }
}
