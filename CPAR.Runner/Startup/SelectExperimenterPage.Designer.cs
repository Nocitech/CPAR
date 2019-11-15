namespace CPAR.Runner.Startup
{
    partial class SelectExperimenterPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.experimentersList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(500, 64);
            this.Banner.Subtitle = "Please choose which experimenter that performs the experiment";
            this.Banner.Title = "Select experimenter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Experimenter:";
            // 
            // experimentersList
            // 
            this.experimentersList.FormattingEnabled = true;
            this.experimentersList.Location = new System.Drawing.Point(91, 71);
            this.experimentersList.Name = "experimentersList";
            this.experimentersList.ScrollAlwaysVisible = true;
            this.experimentersList.Size = new System.Drawing.Size(384, 264);
            this.experimentersList.TabIndex = 2;
            this.experimentersList.SelectedIndexChanged += new System.EventHandler(this.experimentersList_SelectedIndexChanged);
            // 
            // SelectExperimenterPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.experimentersList);
            this.Controls.Add(this.label1);
            this.Name = "SelectExperimenterPage";
            this.Size = new System.Drawing.Size(500, 350);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.SelectExperimenterPage_SetActive);
            this.WizardNext += new ScienceFoundry.UI.Wizard.WizardPageEventHandler(this.SelectExperimenterPage_WizardNext);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.experimentersList, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox experimentersList;
    }
}