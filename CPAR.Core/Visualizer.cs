using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using CPAR.Communication;
using CPAR.Logging;

namespace CPAR.Core
{
    public class Visualizer
    {
        private Chart chart;
        private ChartArea chartArea;


        #region Construction and initial configuration
        public Visualizer(Chart chart)
        {
            ThrowIf.Argument.IsNull(chart, "chart");
            this.chart = chart;
            SetupChartArea();
        }

        #region Properties
        public static bool Conditioning { get; set; }

        public static string Title { get; set; }

        public static double Pmax { get; set; }

        public static double Tmax { get; set; }

        public static bool SecondCuff { get; set; }

        public static int PrimaryChannel { get; set; }

        #endregion

        private void SetupChartArea()
        {
            chartArea = chart.ChartAreas[0];
            chartArea.AxisX.Title = "Time [s]";
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MinorGrid.Enabled = false;
            chartArea.AxisY.Title = "Pressure [kPA]";
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MinorGrid.Enabled = false;
            chartArea.AxisY2.Title = "VAS [cm]";
            chartArea.AxisY2.Maximum = CPARDevice.MAX_SCORE;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MinorGrid.Enabled = false;
        }

        #endregion
        public static void Initialize(double[] stim = null, double[] cond = null, double[] score = null)
        {
            ThrowIf.Argument.IsNull(Active, "Active");
            Active.Setup(stim, cond, score);
        }

        private void Setup(double[] stim = null, double[] cond = null, double[] score = null)
        {
            chart.Titles[0].Text = Title;

            chartArea.AxisX.Maximum = Tmax;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.IsStartedFromZero = true;
            chartArea.AxisY.Maximum = Pmax;
            chartArea.AxisX.LabelStyle.Format = "{0:0}";

            chart.Series.Clear();

            if (Conditioning)
            {
                chart.Series.Add(new Series()
                {
                    ChartArea = chartArea.Name,
                    ChartType = SeriesChartType.FastLine,
                    Name = String.Format("Stimulation ({0})", PrimaryChannel),
                    XAxisType = AxisType.Primary,
                    YAxisType = AxisType.Primary,
                    Color = System.Drawing.Color.Red
                });
                chart.Series.Add(new Series()
                {
                    ChartArea = chartArea.Name,
                    ChartType = SeriesChartType.FastLine,
                    Name = String.Format("Conditioning ({0})",PrimaryChannel == 1 ? 2 : 1),
                    XAxisType = AxisType.Primary,
                    YAxisType = AxisType.Primary,
                    Color = System.Drawing.Color.Yellow
                });
                chart.Series.Add(new Series()
                {
                    ChartArea = chartArea.Name,
                    ChartType = SeriesChartType.FastLine,
                    Name = "VAS",
                    XAxisType = AxisType.Primary,
                    YAxisType = AxisType.Secondary,
                    Color = System.Drawing.Color.Blue
                });

                if (stim != null)
                {
                    Add(chart.Series[0], stim);
                }
                if (cond != null)
                {
                    Add(chart.Series[1], cond);
                }
                if (score != null)
                {
                    Add(chart.Series[2], score);
                }
            }
            else
            {
                chart.Series.Add(new Series()
                {
                    ChartArea = chartArea.Name,
                    ChartType = SeriesChartType.FastLine,
                    Name = SecondCuff ? "Stimulation (1+2)" : String.Format("Stimulation ({0})", PrimaryChannel),
                    XAxisType = AxisType.Primary,
                    YAxisType = AxisType.Primary,
                    Color = System.Drawing.Color.Red 
                });
                chart.Series.Add(new Series()
                {
                    ChartArea = chartArea.Name,
                    ChartType = SeriesChartType.FastLine,
                    Name = "VAS",
                    XAxisType = AxisType.Primary,
                    YAxisType = AxisType.Secondary,
                    Color = System.Drawing.Color.Blue
                });

                if (stim != null)
                {
                    Add(chart.Series[0], stim);
                }
                if (score != null)
                {
                    Add(chart.Series[1], score);
                }
            }
        }

        public delegate void InvokeDelegate();

        public static void Update(double stim, double cond, double score)
        {
            ThrowIf.Argument.IsNull(Active, "Active");

            if (Active.chart.InvokeRequired)
            {
                Active.chart.BeginInvoke(new InvokeDelegate(() => Active.AddData(stim, cond, score)));
            }
            else
            {
                Active.AddData(stim, cond, score);
            }
        }

        private void Add(Series series, double[] y)
        {
            for (int i = 0; i < y.Length; ++i)
            {
                series.Points.Add(new DataPoint()
                {
                    XValue = CPARDevice.CountToTime(i),
                    YValues = new double[] { y[i] }
                });
            }
        }

        private void Add(Series series, double y)
        {
            double time = CPARDevice.CountToTime(series.Points.Count);

            series.Points.Add(new DataPoint()
            {
                XValue = time,
                YValues = new double[] { y }
            });

            if (time > chartArea.AxisX.Maximum)
            {
                chartArea.AxisX.Maximum = time;
            }
        }
        private void AddData(double stim, double cond, double score)
        {
            if (Conditioning)
            {
                Add(chart.Series[0], stim);
                Add(chart.Series[1], cond);
                Add(chart.Series[2], score);
            }
            else
            {
                Add(chart.Series[0], stim);
                Add(chart.Series[1], score);
            }

        }

        public static Visualizer Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        private static Visualizer active = null;
    }
}
