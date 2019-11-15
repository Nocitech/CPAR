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
    public partial class SelectSubjectPage : InternalWizardPage
    {
        public SelectSubjectPage()
        {
            InitializeComponent();
        }

        private void ExistingSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExistingSubjects.SelectedIndex >= 0)
            {
                ActiveSubject.Text = (string) ExistingSubjects.Items[ExistingSubjects.SelectedIndex];
            }

            UpdateButtons();
        }

        private void ActiveSubject_TextChanged(object sender, EventArgs e)
        {
            UpdateExistingIndex();
            UpdateButtons();
        }

        private void UpdateExistingIndex()
        {
            int index = ExistingSubjects.Items.IndexOf(ActiveSubject.Text);

            if (index >= 0)
            {
                ExistingSubjects.SelectedIndex = index;
            }
            else
            {
                ExistingSubjects.SelectedIndex = -1;
            }
        }

        private void SelectSubjectPage_SetActive(object sender, CancelEventArgs e)
        {
            var subjects = Subject.GetSubjects();
            var activeSubject = Subject.Active;
            ExistingSubjects.Items.Clear();
            ActiveSubject.AutoCompleteCustomSource.Clear();
            ActiveSubject.AutoCompleteMode = AutoCompleteMode.Suggest;
            ActiveSubject.AutoCompleteSource = AutoCompleteSource.CustomSource;

            if (subjects != null)
            {
                foreach (var subject in subjects)
                {
                    ExistingSubjects.Items.Add(subject.ToString());
                    ActiveSubject.AutoCompleteCustomSource.Add(subject.ToString());
                }
            }

            if (activeSubject != null)
            {
                ActiveSubject.Text = activeSubject.SubjectID;
            }
            else
            {
                ActiveSubject.Text = "";
            }

            UpdateExistingIndex();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (IsIDValid())
            {
                if (Experiment.Active.UseBetweenSubjectFactors || Experiment.Active.UseWithinSubjectFactors)
                {
                    SetWizardButtons(WizardButtons.Back | WizardButtons.Next);
                }
                else
                {
                    SetWizardButtons(WizardButtons.Back | WizardButtons.Finish);
                }
            }
            else
            {
                SetWizardButtons(WizardButtons.Back);
            }
        }

        private bool IsIDValid()
        {
            if (ActiveSubject.Text == "")
                return false;

            if (ActiveSubject.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
                return false;

            return true;
        }

        private void SelectSubjectPage_WizardNext(object sender, WizardPageEventArgs e)
        {
            InitializeSubject();
        }

        private void SelectSubjectPage_WizardFinish(object sender, CancelEventArgs e)
        {
            InitializeSubject();
            Subject.Active.InitializeSession(Subject.DummySessionID, 
                                             new Factor.Level[] { });
        }

        private void InitializeSubject()
        {
            var subjectID = ActiveSubject.Text;

            if (subjectID != "")
            {
                if (Subject.Exists(subjectID))
                {
                    Subject.Active = Subject.Find(subjectID);
                }
                else
                {
                    Subject.Active = Subject.Create(subjectID);
                }
            }
        }

        private void ActiveSubject_KeyPress(object sender, KeyPressEventArgs e)
        {
            UpdateExistingIndex();
            UpdateButtons();
        }

    }
}
