using CPAR.Core;
using ScienceFoundry.UI.Wizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPAR.Runner.Startup
{
    public partial class LoadExperimentPage : InternalWizardPage
    {
        public LoadExperimentPage()
        {
            InitializeComponent();
        }

        private void LoadExperimentPage_SetActive(object sender, CancelEventArgs e)
        {
            if (experiments == null)
            {
                experiments = Experiment.GetExperiments();
                if (experiments.Count > 0)
                {
                    experimentList.Items.AddRange(experiments.ToArray());
                }
            }
            experimentList.SelectedIndex = -1;
            SetWizardButtons(WizardButtons.None);
        }

        private void experimentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (experiments != null)
            {
                if ((experimentList.SelectedIndex >= 0) &&
                    (experimentList.SelectedIndex < experiments.Count))
                {
                    try
                    {
                        experimentDescription.Rtf = experiments[experimentList.SelectedIndex].Description.Trim();
                    }
                    catch
                    {
                        experimentDescription.Text = experiments[experimentList.SelectedIndex].Description.Trim();
                    }

                    Experiment.Active = experiments[experimentList.SelectedIndex];
                    SetWizardButtons(WizardButtons.Next);
                }
            }
        }

        private void LoadExperimentPage_WizardNext(object sender, WizardPageEventArgs e)
        {
            if (Experiment.Active != null)
            {
                if (Experiment.Active.UseExperimenters)
                {
                    e.NewPage = "SelectExperimenterPage";
                }
                else
                {
                    e.NewPage = "SelectSubjectPage";
                }
            }
        }

        private List<Experiment> experiments = null;

    }
}
