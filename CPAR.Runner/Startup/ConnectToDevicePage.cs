using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScienceFoundry.UI.Wizard;
using CPAR.Communication;

namespace CPAR.Runner.Startup
{
    /**
     * \brief Check and waits until a device is connected
     * The purpose of this wizard page is to wait until a CPAR device is connected to the computer.
     * If and when a CPAR device is connected to the computer the page will automatically advance to the
     * next page in the wizard. If a CPAR is not connected it will instruct the experimenter to connect 
     * a CPAR device.
     */
    public partial class ConnectToDevicePage : InternalWizardPage
    {
        public ConnectToDevicePage()
        {
            InitializeComponent();
        }

        private void ConnectToDevicePage_SetActive(object sender, CancelEventArgs e)
        {
            if (DeviceManager.Connected)
            {
                SetWizardButtons(WizardButtons.Next);
            }
            else
            {
                SetWizardButtons(WizardButtons.None);
            }

            timer.Enabled = true;
            timer.Interval = 100;
        }

        private void ConnectToDevicePage_WizardNext(object sender, WizardPageEventArgs e)
        {
            timer.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (DeviceManager.Connected)
            {
                SetWizardButtons(WizardButtons.Next);
                PressButton(WizardButtons.Next);
            }
        }
    }
}
