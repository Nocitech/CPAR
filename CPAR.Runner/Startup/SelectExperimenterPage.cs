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
using CPAR.Core;

namespace CPAR.Runner.Startup
{
    public partial class SelectExperimenterPage : InternalWizardPage
    {
        public SelectExperimenterPage()
        {
            InitializeComponent();
        }


        private void SelectExperimenterPage_SetActive(object sender, CancelEventArgs e)
        {
            experimentersList.Items.Clear();
            experimentersList.Items.AddRange(Experiment.Active.Experimenters.ToArray());
            SetWizardButtons(WizardButtons.Back);
        }

        private void experimentersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (experimentersList.SelectedIndex >= 0)
            {
                SetWizardButtons(WizardButtons.Next | WizardButtons.Back);
            }
        }

        private void SelectExperimenterPage_WizardNext(object sender, WizardPageEventArgs e)
        {
            if (experimentersList.SelectedIndex >= 0)
            {
                Experimenter.Active = (Experimenter)experimentersList.Items[experimentersList.SelectedIndex];
            }
        }
    }
}
