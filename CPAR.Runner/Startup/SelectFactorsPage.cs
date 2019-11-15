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
    public partial class SelectFactorsPage : InternalWizardPage
    {
        private ComboBox[] factors;
        private Label[] labels;

        public SelectFactorsPage()
        {
            InitializeComponent();

            factors = new ComboBox[] { factor01, factor02, factor03, factor04, factor05, factor06, factor07 };
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7 };
        }

        private void SelectFactorsPage_SetActive(object sender, CancelEventArgs e)
        {
            ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");
            ThrowIf.Argument.IsNull(Subject.Active, "Subject.Active");
            var experiment = Experiment.Active;
            var subject = Subject.Active;

            for (int i = 0; i < factors.Length; ++i)
            {
                factors[i].Visible = false;
                labels[i].Visible = false;
            }

            SetBetweenFactors(experiment, subject);
            SetupWithinFactors(experiment);
            SetButtons();
        }

        private void SetupWithinFactors(Experiment experiment)
        {
            var noOfBetweenSubjectFactors = experiment.BetweenSubjectFactors != null ?
                                            experiment.BetweenSubjectFactors.Length :
                                            0;

            foreach (var factor in experiment.WithinSubjectFactors)
            {
                var i = factor.Index + noOfBetweenSubjectFactors;
                factors[i].Items.Clear();
                factors[i].Visible = factors[i].Enabled = labels[i].Visible = true;
                factors[i].Items.AddRange(factor.Levels);
                labels[i].Text = factor.Name + ":";
            }
        }

        private void SetBetweenFactors(Experiment experiment, Subject subject)
        {
            bool isEmpty = subject.BetweenSubjectFactors.Length == 0;

            if (experiment.BetweenSubjectFactors != null)
            {
                foreach (var factor in experiment.BetweenSubjectFactors)
                {
                    var i = factor.Index;
                    factors[i].Items.Clear();
                    factors[i].Visible = labels[i].Visible = true;
                    labels[factor.Index].Text = factor.Name + ":";
                    factors[i].Enabled = isEmpty ? true : false;

                    if (isEmpty)
                    {
                        factors[factor.Index].Items.AddRange(factor.Levels);
                    }
                    else
                    {
                        factors[i].Items.Add(subject.BetweenSubjectFactors[i]);
                        factors[i].SelectedIndex = 0;
                    }
                }
            }
        }

        private Factor.Level[] WithinSubjectFactors
        {
            get
            {
                ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");

                if (AllFactorsSelected)
                {
                    var retValue = new Factor.Level[Experiment.Active.WithinSubjectFactors.Length];
                    var offset = Experiment.Active.BetweenSubjectFactors != null ? Experiment.Active.BetweenSubjectFactors.Length : 0;

                    for (int i = 0; i < retValue.Length; ++i)
                    {
                        retValue[i] = new Factor.Level() {
                            Name = factors[i + offset].Text
                        };
                    }
                    retValue.IndexArray();
                    return retValue;
                }
                else
                {
                    return null;
                }
            }
        }

        /**
         * \brief Returns the between subject factos.
         * The between subject factors are the first factors in the 
         * list, where the number can be obtained from the number of 
         * between subject factors in the experiment. 
         */
        private Factor.Level[] BetweenSubjectFactors
        {
            get
            {
                ThrowIf.Argument.IsNull(Experiment.Active, "Experiment.Active");

                if (Experiment.Active.BetweenSubjectFactors != null)
                {
                    if (AllFactorsSelected)
                    {
                        var retValue = new Factor.Level[Experiment.Active.BetweenSubjectFactors.Length];

                        for (int i = 0; i < retValue.Length; ++i)
                        {
                            retValue[i] = new Factor.Level() {
                                Name = factors[i].Text
                            };
                        }

                        retValue.IndexArray();
                        return retValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private void factor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtons();
        }

        private void SetButtons()
        {
            if (AllFactorsSelected)
            {
                SetWizardButtons(WizardButtons.Back | WizardButtons.Finish);
            }
            else
            {
                SetWizardButtons(WizardButtons.Back);
            }
        }

        private bool AllFactorsSelected
        {
            get
            {
                var retValue = true;

                foreach (var box in factors)
                {
                    if (box.Visible)
                    {
                        if (box.SelectedIndex < 0)
                        {
                            retValue = false;
                        }
                    }
                }

                return retValue;
            }
        }

        private void SelectFactorsPage_WizardFinish(object sender, CancelEventArgs e)
        {
            if (!AllFactorsSelected)
                throw new InvalidOperationException("Not all factors is selected, should not be possible");

            Subject.Active.InitializeSession(WithinSubjectFactors, BetweenSubjectFactors);
        }
    }
}
