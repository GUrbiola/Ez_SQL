namespace Ez_SQL.MultiQueryForm.Dialogs
{
    partial class NonQuerySp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NonQuerySp));
            this.wizardNonQuery = new Crownwood.Magic.Controls.WizardControl();
            this.FinalSettings = new Crownwood.Magic.Controls.WizardPage();
            this.chkRegion = new System.Windows.Forms.CheckBox();
            this.chkTimeElapsed = new System.Windows.Forms.CheckBox();
            this.chkUseTransaction = new System.Windows.Forms.CheckBox();
            this.chkSaveRowsAffected = new System.Windows.Forms.CheckBox();
            this.chkSaveRowsRead = new System.Windows.Forms.CheckBox();
            this.chkLogExc = new System.Windows.Forms.CheckBox();
            this.chkLogEnd = new System.Windows.Forms.CheckBox();
            this.chkLogStart = new System.Windows.Forms.CheckBox();
            this.FinalSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardNonQuery
            // 
            this.wizardNonQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardNonQuery.Location = new System.Drawing.Point(0, 0);
            this.wizardNonQuery.Name = "wizardNonQuery";
            this.wizardNonQuery.Picture = ((System.Drawing.Image)(resources.GetObject("wizardNonQuery.Picture")));
            this.wizardNonQuery.SelectedIndex = 0;
            this.wizardNonQuery.Size = new System.Drawing.Size(709, 462);
            this.wizardNonQuery.TabIndex = 0;
            this.wizardNonQuery.Title = "Settings for Method That Executes Store Procedure";
            this.wizardNonQuery.WizardPages.AddRange(new Crownwood.Magic.Controls.WizardPage[] {
            this.FinalSettings});
            this.wizardNonQuery.CancelClick += new System.EventHandler(this.wizardNonQuery_CancelClick);
            this.wizardNonQuery.FinishClick += new System.EventHandler(this.wizardNonQuery_FinishClick);
            // 
            // FinalSettings
            // 
            this.FinalSettings.CaptionTitle = "(Page Title)";
            this.FinalSettings.Controls.Add(this.chkRegion);
            this.FinalSettings.Controls.Add(this.chkTimeElapsed);
            this.FinalSettings.Controls.Add(this.chkUseTransaction);
            this.FinalSettings.Controls.Add(this.chkSaveRowsAffected);
            this.FinalSettings.Controls.Add(this.chkSaveRowsRead);
            this.FinalSettings.Controls.Add(this.chkLogExc);
            this.FinalSettings.Controls.Add(this.chkLogEnd);
            this.FinalSettings.Controls.Add(this.chkLogStart);
            this.FinalSettings.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinalSettings.FullPage = false;
            this.FinalSettings.Location = new System.Drawing.Point(0, 0);
            this.FinalSettings.Name = "FinalSettings";
            this.FinalSettings.Size = new System.Drawing.Size(709, 333);
            this.FinalSettings.SubTitle = "Determine the settings by which the method that executes the store procedure must" +
    " be created.";
            this.FinalSettings.TabIndex = 0;
            // 
            // chkRegion
            // 
            this.chkRegion.AutoSize = true;
            this.chkRegion.Location = new System.Drawing.Point(12, 17);
            this.chkRegion.Name = "chkRegion";
            this.chkRegion.Size = new System.Drawing.Size(202, 23);
            this.chkRegion.TabIndex = 7;
            this.chkRegion.Text = "Method Inside a \"#Region\"";
            this.chkRegion.UseVisualStyleBackColor = true;
            // 
            // chkTimeElapsed
            // 
            this.chkTimeElapsed.AutoSize = true;
            this.chkTimeElapsed.Location = new System.Drawing.Point(12, 255);
            this.chkTimeElapsed.Name = "chkTimeElapsed";
            this.chkTimeElapsed.Size = new System.Drawing.Size(176, 23);
            this.chkTimeElapsed.TabIndex = 6;
            this.chkTimeElapsed.Text = "Measure Time Elapsed";
            this.chkTimeElapsed.UseVisualStyleBackColor = true;
            // 
            // chkUseTransaction
            // 
            this.chkUseTransaction.AutoSize = true;
            this.chkUseTransaction.Location = new System.Drawing.Point(12, 221);
            this.chkUseTransaction.Name = "chkUseTransaction";
            this.chkUseTransaction.Size = new System.Drawing.Size(221, 23);
            this.chkUseTransaction.TabIndex = 5;
            this.chkUseTransaction.Text = "Use Transaction for Execution";
            this.chkUseTransaction.UseVisualStyleBackColor = true;
            // 
            // chkSaveRowsAffected
            // 
            this.chkSaveRowsAffected.AutoSize = true;
            this.chkSaveRowsAffected.Location = new System.Drawing.Point(12, 187);
            this.chkSaveRowsAffected.Name = "chkSaveRowsAffected";
            this.chkSaveRowsAffected.Size = new System.Drawing.Size(197, 23);
            this.chkSaveRowsAffected.TabIndex = 4;
            this.chkSaveRowsAffected.Text = "Save Rows Affected Count";
            this.chkSaveRowsAffected.UseVisualStyleBackColor = true;
            // 
            // chkSaveRowsRead
            // 
            this.chkSaveRowsRead.AutoSize = true;
            this.chkSaveRowsRead.Enabled = false;
            this.chkSaveRowsRead.Location = new System.Drawing.Point(12, 153);
            this.chkSaveRowsRead.Name = "chkSaveRowsRead";
            this.chkSaveRowsRead.Size = new System.Drawing.Size(176, 23);
            this.chkSaveRowsRead.TabIndex = 3;
            this.chkSaveRowsRead.Text = "Save Rows Read Count";
            this.chkSaveRowsRead.UseVisualStyleBackColor = true;
            // 
            // chkLogExc
            // 
            this.chkLogExc.AutoSize = true;
            this.chkLogExc.Location = new System.Drawing.Point(12, 119);
            this.chkLogExc.Name = "chkLogExc";
            this.chkLogExc.Size = new System.Drawing.Size(118, 23);
            this.chkLogExc.TabIndex = 2;
            this.chkLogExc.Text = "Log Exception";
            this.chkLogExc.UseVisualStyleBackColor = true;
            // 
            // chkLogEnd
            // 
            this.chkLogEnd.AutoSize = true;
            this.chkLogEnd.Location = new System.Drawing.Point(12, 85);
            this.chkLogEnd.Name = "chkLogEnd";
            this.chkLogEnd.Size = new System.Drawing.Size(146, 23);
            this.chkLogEnd.TabIndex = 1;
            this.chkLogEnd.Text = "Log End Execution";
            this.chkLogEnd.UseVisualStyleBackColor = true;
            // 
            // chkLogStart
            // 
            this.chkLogStart.AutoSize = true;
            this.chkLogStart.Location = new System.Drawing.Point(12, 51);
            this.chkLogStart.Name = "chkLogStart";
            this.chkLogStart.Size = new System.Drawing.Size(152, 23);
            this.chkLogStart.TabIndex = 0;
            this.chkLogStart.Text = "Log Start Execution";
            this.chkLogStart.UseVisualStyleBackColor = true;
            // 
            // NonQuerySp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 462);
            this.Controls.Add(this.wizardNonQuery);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NonQuerySp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Non Query Store Procedure";
            this.Load += new System.EventHandler(this.NonQuerySp_Load);
            this.FinalSettings.ResumeLayout(false);
            this.FinalSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Crownwood.Magic.Controls.WizardControl wizardNonQuery;
        private Crownwood.Magic.Controls.WizardPage FinalSettings;
        private System.Windows.Forms.CheckBox chkLogStart;
        private System.Windows.Forms.CheckBox chkTimeElapsed;
        private System.Windows.Forms.CheckBox chkUseTransaction;
        private System.Windows.Forms.CheckBox chkSaveRowsAffected;
        private System.Windows.Forms.CheckBox chkSaveRowsRead;
        private System.Windows.Forms.CheckBox chkLogExc;
        private System.Windows.Forms.CheckBox chkLogEnd;
        private System.Windows.Forms.CheckBox chkRegion;
    }
}