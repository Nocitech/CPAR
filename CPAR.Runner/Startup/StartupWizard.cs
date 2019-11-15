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
using CPAR.Core;

namespace CPAR.Runner.Startup
{
    /**
     * \brief Startup wizard
     * The startup wizard runs when the program is started, and guides an experimenter
     * through the initialization of an experimental session.
     * 
     * The startup wizard consists of the following steps:
     *   1. ConnectToDevicePage:
     *      On this page the system checks if a CPAR device is connected to the computer,
     *      and will only allow the wizard to go to the next page when it has detected a 
     *      CPAR device.
     *      
     *   2. LoadExperimentPage:
     *      On this page the experiments that are present on the computer is presented to
     *      the experimenter, and it will ask the experimenter to choose one of these experiments.
     *      
     *   3. SessionTypePage:
     *      On this page the experimenter is asked whether he or she wants to resume an allready
     *      existing session (b), or if he or she wants to start a new session (a). If there are no existing
     *      sessions then the option to resume a session will be greyed out.
     *   
     *   4a. 
     */
    public partial class StartupWizard : WizardSheet
    {
        public StartupWizard()
        {
            InitializeComponent();

            Pages.Add(new LoadExperimentPage());
            Pages.Add(new SelectExperimenterPage());
            Pages.Add(new SelectSubjectPage());
            Pages.Add(new SelectFactorsPage());
        }
    }
}
