namespace Ez_SQL.MultiQueryForm.Dialogs
{
    partial class GenerateClass
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
            this.labClassName = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkFieldType = new System.Windows.Forms.CheckBox();
            this.chkStringIndexer = new System.Windows.Forms.CheckBox();
            this.chkIntegerIndexer = new System.Windows.Forms.CheckBox();
            this.chkIndexers = new System.Windows.Forms.CheckBox();
            this.chkFieldCount = new System.Windows.Forms.CheckBox();
            this.chkDictionary = new System.Windows.Forms.CheckBox();
            this.chkObjectList = new System.Windows.Forms.CheckBox();
            this.chkObjectArray = new System.Windows.Forms.CheckBox();
            this.chkInstanceConverters = new System.Windows.Forms.CheckBox();
            this.chkPKAlias = new System.Windows.Forms.CheckBox();
            this.chkLoadFromDataTable = new System.Windows.Forms.CheckBox();
            this.chkPKConstructor = new System.Windows.Forms.CheckBox();
            this.chkDefaultConstructor = new System.Windows.Forms.CheckBox();
            this.chkDataMemberDecoration = new System.Windows.Forms.CheckBox();
            this.radVariableProperties = new System.Windows.Forms.RadioButton();
            this.radAutoImplementedProperties = new System.Windows.Forms.RadioButton();
            this.chkGenerateProperties = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labClassName
            // 
            this.labClassName.AutoSize = true;
            this.labClassName.Location = new System.Drawing.Point(1, 9);
            this.labClassName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labClassName.Name = "labClassName";
            this.labClassName.Size = new System.Drawing.Size(79, 18);
            this.labClassName.TabIndex = 0;
            this.labClassName.Text = "Class Name";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(4, 31);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(4);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(451, 26);
            this.txtClassName.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFieldType);
            this.groupBox1.Controls.Add(this.chkStringIndexer);
            this.groupBox1.Controls.Add(this.chkIntegerIndexer);
            this.groupBox1.Controls.Add(this.chkIndexers);
            this.groupBox1.Controls.Add(this.chkFieldCount);
            this.groupBox1.Controls.Add(this.chkDictionary);
            this.groupBox1.Controls.Add(this.chkObjectList);
            this.groupBox1.Controls.Add(this.chkObjectArray);
            this.groupBox1.Controls.Add(this.chkInstanceConverters);
            this.groupBox1.Controls.Add(this.chkPKAlias);
            this.groupBox1.Controls.Add(this.chkLoadFromDataTable);
            this.groupBox1.Controls.Add(this.chkPKConstructor);
            this.groupBox1.Controls.Add(this.chkDefaultConstructor);
            this.groupBox1.Controls.Add(this.chkDataMemberDecoration);
            this.groupBox1.Controls.Add(this.radVariableProperties);
            this.groupBox1.Controls.Add(this.radAutoImplementedProperties);
            this.groupBox1.Controls.Add(this.chkGenerateProperties);
            this.groupBox1.Location = new System.Drawing.Point(4, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 526);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Creation Options";
            // 
            // chkFieldType
            // 
            this.chkFieldType.AutoSize = true;
            this.chkFieldType.Location = new System.Drawing.Point(6, 501);
            this.chkFieldType.Name = "chkFieldType";
            this.chkFieldType.Size = new System.Drawing.Size(337, 22);
            this.chkFieldType.TabIndex = 17;
            this.chkFieldType.Text = "Create Method string FieldType(string FieldName)";
            this.chkFieldType.UseVisualStyleBackColor = true;
            // 
            // chkStringIndexer
            // 
            this.chkStringIndexer.AutoSize = true;
            this.chkStringIndexer.Location = new System.Drawing.Point(26, 473);
            this.chkStringIndexer.Name = "chkStringIndexer";
            this.chkStringIndexer.Size = new System.Drawing.Size(255, 22);
            this.chkStringIndexer.TabIndex = 16;
            this.chkStringIndexer.Text = "String Indexer(PropertyName Based)";
            this.chkStringIndexer.UseVisualStyleBackColor = true;
            this.chkStringIndexer.CheckedChanged += new System.EventHandler(this.chkIntegerIndexer_CheckedChanged);
            // 
            // chkIntegerIndexer
            // 
            this.chkIntegerIndexer.AutoSize = true;
            this.chkIntegerIndexer.Location = new System.Drawing.Point(26, 443);
            this.chkIntegerIndexer.Name = "chkIntegerIndexer";
            this.chkIntegerIndexer.Size = new System.Drawing.Size(262, 22);
            this.chkIntegerIndexer.TabIndex = 15;
            this.chkIntegerIndexer.Text = "Generate Integer Indexer(Zero Based)";
            this.chkIntegerIndexer.UseVisualStyleBackColor = true;
            this.chkIntegerIndexer.CheckedChanged += new System.EventHandler(this.chkIntegerIndexer_CheckedChanged);
            // 
            // chkIndexers
            // 
            this.chkIndexers.AutoSize = true;
            this.chkIndexers.Enabled = false;
            this.chkIndexers.Location = new System.Drawing.Point(6, 411);
            this.chkIndexers.Name = "chkIndexers";
            this.chkIndexers.Size = new System.Drawing.Size(142, 22);
            this.chkIndexers.TabIndex = 14;
            this.chkIndexers.Text = "Generate Indexers";
            this.chkIndexers.UseVisualStyleBackColor = true;
            // 
            // chkFieldCount
            // 
            this.chkFieldCount.AutoSize = true;
            this.chkFieldCount.Location = new System.Drawing.Point(26, 175);
            this.chkFieldCount.Name = "chkFieldCount";
            this.chkFieldCount.Size = new System.Drawing.Size(220, 22);
            this.chkFieldCount.TabIndex = 13;
            this.chkFieldCount.Text = "Create Property for Field Count";
            this.chkFieldCount.UseVisualStyleBackColor = true;
            // 
            // chkDictionary
            // 
            this.chkDictionary.AutoSize = true;
            this.chkDictionary.Location = new System.Drawing.Point(26, 383);
            this.chkDictionary.Name = "chkDictionary";
            this.chkDictionary.Size = new System.Drawing.Size(269, 22);
            this.chkDictionary.TabIndex = 12;
            this.chkDictionary.Text = "ThisClass.ToDictionary<string, Object>()";
            this.chkDictionary.UseVisualStyleBackColor = true;
            this.chkDictionary.CheckedChanged += new System.EventHandler(this.chkObjectArray_CheckedChanged);
            // 
            // chkObjectList
            // 
            this.chkObjectList.AutoSize = true;
            this.chkObjectList.Location = new System.Drawing.Point(26, 353);
            this.chkObjectList.Name = "chkObjectList";
            this.chkObjectList.Size = new System.Drawing.Size(197, 22);
            this.chkObjectList.TabIndex = 11;
            this.chkObjectList.Text = "ThisClass.ToLiistOfObjects()";
            this.chkObjectList.UseVisualStyleBackColor = true;
            this.chkObjectList.CheckedChanged += new System.EventHandler(this.chkObjectArray_CheckedChanged);
            // 
            // chkObjectArray
            // 
            this.chkObjectArray.AutoSize = true;
            this.chkObjectArray.Location = new System.Drawing.Point(26, 323);
            this.chkObjectArray.Name = "chkObjectArray";
            this.chkObjectArray.Size = new System.Drawing.Size(184, 22);
            this.chkObjectArray.TabIndex = 10;
            this.chkObjectArray.Text = "ThisClass.ToObjectArray()";
            this.chkObjectArray.UseVisualStyleBackColor = true;
            this.chkObjectArray.CheckedChanged += new System.EventHandler(this.chkObjectArray_CheckedChanged);
            // 
            // chkInstanceConverters
            // 
            this.chkInstanceConverters.AutoSize = true;
            this.chkInstanceConverters.Enabled = false;
            this.chkInstanceConverters.Location = new System.Drawing.Point(6, 293);
            this.chkInstanceConverters.Name = "chkInstanceConverters";
            this.chkInstanceConverters.Size = new System.Drawing.Size(211, 22);
            this.chkInstanceConverters.TabIndex = 9;
            this.chkInstanceConverters.Text = "Generate Instance Converters";
            this.chkInstanceConverters.UseVisualStyleBackColor = true;
            // 
            // chkPKAlias
            // 
            this.chkPKAlias.AutoSize = true;
            this.chkPKAlias.Location = new System.Drawing.Point(26, 145);
            this.chkPKAlias.Name = "chkPKAlias";
            this.chkPKAlias.Size = new System.Drawing.Size(291, 22);
            this.chkPKAlias.TabIndex = 8;
            this.chkPKAlias.Text = "Create Alias for PK, as property named \"Id\"";
            this.chkPKAlias.UseVisualStyleBackColor = true;
            // 
            // chkLoadFromDataTable
            // 
            this.chkLoadFromDataTable.AutoSize = true;
            this.chkLoadFromDataTable.Location = new System.Drawing.Point(6, 263);
            this.chkLoadFromDataTable.Name = "chkLoadFromDataTable";
            this.chkLoadFromDataTable.Size = new System.Drawing.Size(400, 22);
            this.chkLoadFromDataTable.TabIndex = 7;
            this.chkLoadFromDataTable.Text = "Generate static converters, from DataTable to List<ThisClass>";
            this.chkLoadFromDataTable.UseVisualStyleBackColor = true;
            // 
            // chkPKConstructor
            // 
            this.chkPKConstructor.AutoSize = true;
            this.chkPKConstructor.Location = new System.Drawing.Point(26, 235);
            this.chkPKConstructor.Name = "chkPKConstructor";
            this.chkPKConstructor.Size = new System.Drawing.Size(176, 22);
            this.chkPKConstructor.TabIndex = 5;
            this.chkPKConstructor.Text = "Primary Key Constructor";
            this.chkPKConstructor.UseVisualStyleBackColor = true;
            // 
            // chkDefaultConstructor
            // 
            this.chkDefaultConstructor.AutoSize = true;
            this.chkDefaultConstructor.Checked = true;
            this.chkDefaultConstructor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefaultConstructor.Enabled = false;
            this.chkDefaultConstructor.Location = new System.Drawing.Point(6, 205);
            this.chkDefaultConstructor.Name = "chkDefaultConstructor";
            this.chkDefaultConstructor.Size = new System.Drawing.Size(209, 22);
            this.chkDefaultConstructor.TabIndex = 4;
            this.chkDefaultConstructor.Text = "Generate Default Constructor";
            this.chkDefaultConstructor.UseVisualStyleBackColor = true;
            // 
            // chkDataMemberDecoration
            // 
            this.chkDataMemberDecoration.AutoSize = true;
            this.chkDataMemberDecoration.Location = new System.Drawing.Point(26, 115);
            this.chkDataMemberDecoration.Name = "chkDataMemberDecoration";
            this.chkDataMemberDecoration.Size = new System.Drawing.Size(177, 22);
            this.chkDataMemberDecoration.TabIndex = 3;
            this.chkDataMemberDecoration.Text = "DataMember decoration";
            this.chkDataMemberDecoration.UseVisualStyleBackColor = true;
            // 
            // radVariableProperties
            // 
            this.radVariableProperties.AutoSize = true;
            this.radVariableProperties.Location = new System.Drawing.Point(26, 85);
            this.radVariableProperties.Name = "radVariableProperties";
            this.radVariableProperties.Size = new System.Drawing.Size(278, 22);
            this.radVariableProperties.TabIndex = 2;
            this.radVariableProperties.Text = "Properties with a private variable behind";
            this.radVariableProperties.UseVisualStyleBackColor = true;
            // 
            // radAutoImplementedProperties
            // 
            this.radAutoImplementedProperties.AutoSize = true;
            this.radAutoImplementedProperties.Checked = true;
            this.radAutoImplementedProperties.Location = new System.Drawing.Point(26, 55);
            this.radAutoImplementedProperties.Name = "radAutoImplementedProperties";
            this.radAutoImplementedProperties.Size = new System.Drawing.Size(211, 22);
            this.radAutoImplementedProperties.TabIndex = 1;
            this.radAutoImplementedProperties.TabStop = true;
            this.radAutoImplementedProperties.Text = "Auto implemented properties";
            this.radAutoImplementedProperties.UseVisualStyleBackColor = true;
            // 
            // chkGenerateProperties
            // 
            this.chkGenerateProperties.AutoSize = true;
            this.chkGenerateProperties.Checked = true;
            this.chkGenerateProperties.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateProperties.Enabled = false;
            this.chkGenerateProperties.Location = new System.Drawing.Point(6, 25);
            this.chkGenerateProperties.Name = "chkGenerateProperties";
            this.chkGenerateProperties.Size = new System.Drawing.Size(152, 22);
            this.chkGenerateProperties.TabIndex = 0;
            this.chkGenerateProperties.Text = "Generate Properties";
            this.chkGenerateProperties.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(4, 596);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(198, 34);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(257, 596);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(198, 34);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // GenerateClass
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(461, 635);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.labClassName);
            this.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Class Generation Dialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labClassName;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkDataMemberDecoration;
        private System.Windows.Forms.RadioButton radVariableProperties;
        private System.Windows.Forms.RadioButton radAutoImplementedProperties;
        private System.Windows.Forms.CheckBox chkGenerateProperties;
        private System.Windows.Forms.CheckBox chkPKConstructor;
        private System.Windows.Forms.CheckBox chkDefaultConstructor;
        private System.Windows.Forms.CheckBox chkLoadFromDataTable;
        private System.Windows.Forms.CheckBox chkPKAlias;
        private System.Windows.Forms.CheckBox chkDictionary;
        private System.Windows.Forms.CheckBox chkObjectList;
        private System.Windows.Forms.CheckBox chkObjectArray;
        private System.Windows.Forms.CheckBox chkInstanceConverters;
        private System.Windows.Forms.CheckBox chkStringIndexer;
        private System.Windows.Forms.CheckBox chkIntegerIndexer;
        private System.Windows.Forms.CheckBox chkIndexers;
        private System.Windows.Forms.CheckBox chkFieldCount;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkFieldType;
    }
}