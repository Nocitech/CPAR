namespace CPAR.Runner.Startup
{
    partial class LoadExperimentPage
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
            this.experimentList = new System.Windows.Forms.ListBox();
            this.experimentDescription = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(500, 64);
            this.Banner.Subtitle = "Select the experiment to run ...";
            this.Banner.Title = "Load experiment";
            // 
            // experimentList
            // 
            this.experimentList.FormattingEnabled = true;
            this.experimentList.Location = new System.Drawing.Point(4, 67);
            this.experimentList.Name = "experimentList";
            this.experimentList.ScrollAlwaysVisible = true;
            this.experimentList.Size = new System.Drawing.Size(184, 277);
            this.experimentList.TabIndex = 1;
            this.experimentList.SelectedIndexChanged += new System.EventHandler(this.experimentList_SelectedIndexChanged);
            // 
            // experimentDescription
            // 
            this.experimentDescription.BackColor = System.Drawing.Color.White;
            this.experimentDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.experimentDescription.Location = new System.Drawing.Point(194, 67);
            this.experimentDescription.Name = "experimentDescription";
            this.experimentDescription.ReadOnly = true;
            this.experimentDescription.Size = new System.Drawing.Size(303, 277);
            this.experimentDescription.TabIndex = 2;
            this.experimentDescription.Text = "";
            // 
            // LoadExperimentPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.experimentDescription);
            this.Controls.Add(this.experimentList);
            this.Name = "LoadExperimentPage";
            this.Size = new System.Drawing.Size(500, 350);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.LoadExperimentPage_SetActive);
            this.WizardNext += new ScienceFoundry.UI.Wizard.WizardPageEventHandler(this.LoadExperimentPage_WizardNext);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.Controls.SetChildIndex(this.experimentList, 0);
            this.Controls.SetChildIndex(this.experimentDescription, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox experimentList;
        private System.Windows.Forms.RichTextBox experimentDescription;
    }
}