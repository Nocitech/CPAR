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
using CPAR.Logging;

namespace CPAR.Runner
{
    public partial class SetupParametersForm : Form
    {
        private TextBox[] valueBoxes;
        private Test test;
        private CalculatedParameter[] parameters;

        private Label[] labels;

        public SetupParametersForm(Test test)
        {
            ThrowIf.Argument.IsZero(test.NumberOfExternalParameters, "test.NumberOfExternalParameters");
            InitializeComponent();
            this.test = test;
            this.parameters = test.ExternalParameters;
            valueBoxes =  new TextBox[] { value1, value2, value3 };
            labels = new Label[] { label1, label2, label3 };
            SetupDataView();
            ValidateData();
        }

        private void SetupDataView()
        {
            for (int i = 0; i < valueBoxes.Length; ++i)
            {
                if (i < parameters.Length)
                {
                    var p = parameters[i];

                    labels[i].Visible = true;
                    labels[i].Text = p.Description;
                    valueBoxes[i].Visible = true;
                    valueBoxes[i].Text = p.Value.ToString();
                    valueBoxes[i].Tag = p;
                }
                else
                {
                    labels[i].Visible = false;
                    valueBoxes[i].Visible = false;
                    valueBoxes[i].Tag = null;
                }
            }
        }

        private void ValidateData()
        {
            bool dataValid = true;

            errorProvider.Clear();

            for (int i = 0; i < parameters.Length; ++i)
            {
                double value = 0;

                if (!double.TryParse(valueBoxes[i].Text, out value))
                {
                    errorProvider.SetError(valueBoxes[i], "Please enter a number");
                    dataValid = false;
                }
            }

            mOkBtn.Enabled = dataValid;
        }

        private void mOkBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < parameters.Length; ++i)
            {
                double value = 0;

                if (double.TryParse(valueBoxes[i].Text, out value))
                {
                    parameters[i].Value = value;
                    parameters[i].ExternallySpecified = true;

                    Log.Status("Test [ {0} ] {1} set to: {2}", 
                        test.Name, 
                        parameters[i].Description, 
                        value);
                }
                else
                {
                    Log.Error("Invalid value in SetupParametersForm.mOkBtn_Click: " + valueBoxes[i].Text);
                }
            }
        }

        private void ParameterChanged(object sender, EventArgs e)
        {
            ValidateData();
        }
    }
}
