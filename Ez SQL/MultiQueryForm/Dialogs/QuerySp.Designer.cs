namespace Ez_SQL.MultiQueryForm.Dialogs
{
    partial class QuerySp
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
            this.wizardControl1 = new Crownwood.Magic.Controls.WizardControl();
            this.TypeOfReturnData = new Crownwood.Magic.Controls.WizardPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radList = new System.Windows.Forms.RadioButton();
            this.radOnlyOne = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.cmbPrimitives = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radObject = new System.Windows.Forms.RadioButton();
            this.radPrimitive = new System.Windows.Forms.RadioButton();
            this.FinalSettings = new Crownwood.Magic.Controls.WizardPage();
            this.chkRegion = new System.Windows.Forms.CheckBox();
            this.chkTimeElapsed = new System.Windows.Forms.CheckBox();
            this.chkUseTransaction = new System.Windows.Forms.CheckBox();
            this.chkSaveRowsAffected = new System.Windows.Forms.CheckBox();
            this.chkSaveRowsRead = new System.Windows.Forms.CheckBox();
            this.chkLogExc = new System.Windows.Forms.CheckBox();
            this.chkLogEnd = new System.Windows.Forms.CheckBox();
            this.chkLogStart = new System.Windows.Forms.CheckBox();
            this.radSPR = new System.Windows.Forms.RadioButton();
            this.TypeOfReturnData.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.FinalSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.SelectedIndex = 0;
            this.wizardControl1.Size = new System.Drawing.Size(709, 462);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.Title = "Settings for Method That Executes Store Procedure";
            this.wizardControl1.WizardPages.AddRange(new Crownwood.Magic.Controls.WizardPage[] {
            this.TypeOfReturnData,
            this.FinalSettings});
            this.wizardControl1.CancelClick += new System.EventHandler(this.wizardControl1_CancelClick);
            this.wizardControl1.FinishClick += new System.EventHandler(this.wizardControl1_FinishClick);
            // 
            // TypeOfReturnData
            // 
            this.TypeOfReturnData.CaptionTitle = "(Page Title)";
            this.TypeOfReturnData.Controls.Add(this.groupBox2);
            this.TypeOfReturnData.Controls.Add(this.groupBox1);
            this.TypeOfReturnData.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeOfReturnData.FullPage = false;
            this.TypeOfReturnData.Location = new System.Drawing.Point(0, 0);
            this.TypeOfReturnData.Name = "TypeOfReturnData";
            this.TypeOfReturnData.Size = new System.Drawing.Size(709, 333);
            this.TypeOfReturnData.SubTitle = "Define the type of return data you are expecting";
            this.TypeOfReturnData.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radSPR);
            this.groupBox2.Controls.Add(this.radList);
            this.groupBox2.Controls.Add(this.radOnlyOne);
            this.groupBox2.Location = new System.Drawing.Point(3, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(693, 134);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Return Count";
            // 
            // radList
            // 
            this.radList.AutoSize = true;
            this.radList.Location = new System.Drawing.Point(20, 64);
            this.radList.Name = "radList";
            this.radList.Size = new System.Drawing.Size(123, 23);
            this.radList.TabIndex = 1;
            this.radList.Text = "List of Records";
            this.radList.UseVisualStyleBackColor = true;
            // 
            // radOnlyOne
            // 
            this.radOnlyOne.AutoSize = true;
            this.radOnlyOne.Checked = true;
            this.radOnlyOne.Location = new System.Drawing.Point(20, 35);
            this.radOnlyOne.Name = "radOnlyOne";
            this.radOnlyOne.Size = new System.Drawing.Size(104, 23);
            this.radOnlyOne.TabIndex = 0;
            this.radOnlyOne.TabStop = true;
            this.radOnlyOne.Text = "First Record";
            this.radOnlyOne.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtClassName);
            this.groupBox1.Controls.Add(this.cmbPrimitives);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radObject);
            this.groupBox1.Controls.Add(this.radPrimitive);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(693, 130);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Return Type";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(355, 87);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(276, 27);
            this.txtClassName.TabIndex = 9;
            this.txtClassName.Text = "ClassX";
            // 
            // cmbPrimitives
            // 
            this.cmbPrimitives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrimitives.FormattingEnabled = true;
            this.cmbPrimitives.Items.AddRange(new object[] {
            "bool",
            "int",
            "string",
            "float",
            "double",
            "DateTime"});
            this.cmbPrimitives.Location = new System.Drawing.Point(20, 57);
            this.cmbPrimitives.Name = "cmbPrimitives";
            this.cmbPrimitives.Size = new System.Drawing.Size(276, 27);
            this.cmbPrimitives.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(351, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Class Name";
            // 
            // radObject
            // 
            this.radObject.AutoSize = true;
            this.radObject.Checked = true;
            this.radObject.Location = new System.Drawing.Point(355, 28);
            this.radObject.Name = "radObject";
            this.radObject.Size = new System.Drawing.Size(70, 23);
            this.radObject.TabIndex = 6;
            this.radObject.TabStop = true;
            this.radObject.Text = "Object";
            this.radObject.UseVisualStyleBackColor = true;
            // 
            // radPrimitive
            // 
            this.radPrimitive.AutoSize = true;
            this.radPrimitive.Location = new System.Drawing.Point(20, 28);
            this.radPrimitive.Name = "radPrimitive";
            this.radPrimitive.Size = new System.Drawing.Size(141, 23);
            this.radPrimitive.TabIndex = 5;
            this.radPrimitive.TabStop = true;
            this.radPrimitive.Text = "Primitive Variable";
            this.radPrimitive.UseVisualStyleBackColor = true;
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
            this.FinalSettings.Selected = false;
            this.FinalSettings.Size = new System.Drawing.Size(709, 333);
            this.FinalSettings.SubTitle = "(Page Description not defined)";
            this.FinalSettings.TabIndex = 2;
            // 
            // chkRegion
            // 
            this.chkRegion.AutoSize = true;
            this.chkRegion.Location = new System.Drawing.Point(12, 16);
            this.chkRegion.Name = "chkRegion";
            this.chkRegion.Size = new System.Drawing.Size(202, 23);
            this.chkRegion.TabIndex = 15;
            this.chkRegion.Text = "Method Inside a \"#Region\"";
            this.chkRegion.UseVisualStyleBackColor = true;
            // 
            // chkTimeElapsed
            // 
            this.chkTimeElapsed.AutoSize = true;
            this.chkTimeElapsed.Location = new System.Drawing.Point(12, 254);
            this.chkTimeElapsed.Name = "chkTimeElapsed";
            this.chkTimeElapsed.Size = new System.Drawing.Size(176, 23);
            this.chkTimeElapsed.TabIndex = 14;
            this.chkTimeElapsed.Text = "Measure Time Elapsed";
            this.chkTimeElapsed.UseVisualStyleBackColor = true;
            // 
            // chkUseTransaction
            // 
            this.chkUseTransaction.AutoSize = true;
            this.chkUseTransaction.Location = new System.Drawing.Point(12, 220);
            this.chkUseTransaction.Name = "chkUseTransaction";
            this.chkUseTransaction.Size = new System.Drawing.Size(221, 23);
            this.chkUseTransaction.TabIndex = 13;
            this.chkUseTransaction.Text = "Use Transaction for Execution";
            this.chkUseTransaction.UseVisualStyleBackColor = true;
            // 
            // chkSaveRowsAffected
            // 
            this.chkSaveRowsAffected.AutoSize = true;
            this.chkSaveRowsAffected.Enabled = false;
            this.chkSaveRowsAffected.Location = new System.Drawing.Point(12, 186);
            this.chkSaveRowsAffected.Name = "chkSaveRowsAffected";
            this.chkSaveRowsAffected.Size = new System.Drawing.Size(197, 23);
            this.chkSaveRowsAffected.TabIndex = 12;
            this.chkSaveRowsAffected.Text = "Save Rows Affected Count";
            this.chkSaveRowsAffected.UseVisualStyleBackColor = true;
            // 
            // chkSaveRowsRead
            // 
            this.chkSaveRowsRead.AutoSize = true;
            this.chkSaveRowsRead.Location = new System.Drawing.Point(12, 152);
            this.chkSaveRowsRead.Name = "chkSaveRowsRead";
            this.chkSaveRowsRead.Size = new System.Drawing.Size(176, 23);
            this.chkSaveRowsRead.TabIndex = 11;
            this.chkSaveRowsRead.Text = "Save Rows Read Count";
            this.chkSaveRowsRead.UseVisualStyleBackColor = true;
            // 
            // chkLogExc
            // 
            this.chkLogExc.AutoSize = true;
            this.chkLogExc.Location = new System.Drawing.Point(12, 118);
            this.chkLogExc.Name = "chkLogExc";
            this.chkLogExc.Size = new System.Drawing.Size(118, 23);
            this.chkLogExc.TabIndex = 10;
            this.chkLogExc.Text = "Log Exception";
            this.chkLogExc.UseVisualStyleBackColor = true;
            // 
            // chkLogEnd
            // 
            this.chkLogEnd.AutoSize = true;
            this.chkLogEnd.Location = new System.Drawing.Point(12, 84);
            this.chkLogEnd.Name = "chkLogEnd";
            this.chkLogEnd.Size = new System.Drawing.Size(146, 23);
            this.chkLogEnd.TabIndex = 9;
            this.chkLogEnd.Text = "Log End Execution";
            this.chkLogEnd.UseVisualStyleBackColor = true;
            // 
            // chkLogStart
            // 
            this.chkLogStart.AutoSize = true;
            this.chkLogStart.Location = new System.Drawing.Point(12, 50);
            this.chkLogStart.Name = "chkLogStart";
            this.chkLogStart.Size = new System.Drawing.Size(152, 23);
            this.chkLogStart.TabIndex = 8;
            this.chkLogStart.Text = "Log Start Execution";
            this.chkLogStart.UseVisualStyleBackColor = true;
            // 
            // radSPR
            // 
            this.radSPR.AutoSize = true;
            this.radSPR.Location = new System.Drawing.Point(20, 93);
            this.radSPR.Name = "radSPR";
            this.radSPR.Size = new System.Drawing.Size(124, 23);
            this.radSPR.TabIndex = 2;
            this.radSPR.Text = "SPR_Collection";
            this.radSPR.UseVisualStyleBackColor = true;
            // 
            // QuerySp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 462);
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuerySp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query Store Procedure";
            this.Load += new System.EventHandler(this.QuerySp_Load);
            this.TypeOfReturnData.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.FinalSettings.ResumeLayout(false);
            this.FinalSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Crownwood.Magic.Controls.WizardControl wizardControl1;
        private Crownwood.Magic.Controls.WizardPage TypeOfReturnData;
        private Crownwood.Magic.Controls.WizardPage FinalSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radList;
        private System.Windows.Forms.RadioButton radOnlyOne;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.ComboBox cmbPrimitives;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radObject;
        private System.Windows.Forms.RadioButton radPrimitive;
        private System.Windows.Forms.CheckBox chkRegion;
        private System.Windows.Forms.CheckBox chkTimeElapsed;
        private System.Windows.Forms.CheckBox chkUseTransaction;
        private System.Windows.Forms.CheckBox chkSaveRowsAffected;
        private System.Windows.Forms.CheckBox chkSaveRowsRead;
        private System.Windows.Forms.CheckBox chkLogExc;
        private System.Windows.Forms.CheckBox chkLogEnd;
        private System.Windows.Forms.CheckBox chkLogStart;
        private System.Windows.Forms.RadioButton radSPR;


    }
}