namespace Ez_SQL.AdditionalForms
{
    partial class ObjectSearcher
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectSearcher));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbSearchType = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ResultsList = new System.Windows.Forms.ListView();
            this.LonelyHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PopIList = new System.Windows.Forms.ImageList(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.GridFields = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.Contains = new System.Windows.Forms.RadioButton();
            this.StartsWith = new System.Windows.Forms.RadioButton();
            this.TxtFilter = new Ez_SQL.Custom_Controls.AnimatedWaitTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridFields)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(484, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(2, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(484, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Object type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CmbSearchType
            // 
            this.CmbSearchType.Dock = System.Windows.Forms.DockStyle.Top;
            this.CmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbSearchType.FormattingEnabled = true;
            this.CmbSearchType.Items.AddRange(new object[] {
            "Any database object",
            "Tables",
            "Views",
            "Stored procedures",
            "Scalar functions",
            "Table functions",
            "Search in code(Views, Procedures, Functions)",
            "Search for field(Tables, Views, Table Functions)",
            "Search for parameter(Procedures, Functions)"});
            this.CmbSearchType.Location = new System.Drawing.Point(2, 64);
            this.CmbSearchType.Name = "CmbSearchType";
            this.CmbSearchType.Size = new System.Drawing.Size(484, 21);
            this.CmbSearchType.TabIndex = 3;
            this.CmbSearchType.SelectedIndexChanged += new System.EventHandler(this.CmbSearchType_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 119);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ResultsList);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GridFields);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Size = new System.Drawing.Size(484, 325);
            this.splitContainer1.SplitterDistance = 187;
            this.splitContainer1.TabIndex = 10;
            // 
            // ResultsList
            // 
            this.ResultsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LonelyHeader});
            this.ResultsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsList.FullRowSelect = true;
            this.ResultsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ResultsList.Location = new System.Drawing.Point(0, 18);
            this.ResultsList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ResultsList.MultiSelect = false;
            this.ResultsList.Name = "ResultsList";
            this.ResultsList.ShowItemToolTips = true;
            this.ResultsList.Size = new System.Drawing.Size(484, 169);
            this.ResultsList.SmallImageList = this.PopIList;
            this.ResultsList.TabIndex = 9;
            this.ResultsList.UseCompatibleStateImageBehavior = false;
            this.ResultsList.View = System.Windows.Forms.View.Details;
            this.ResultsList.SelectedIndexChanged += new System.EventHandler(this.ResultsList_SelectedIndexChanged);
            this.ResultsList.DoubleClick += new System.EventHandler(this.ResultsList_DoubleClick);
            this.ResultsList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ResultsList_KeyPress);
            // 
            // LonelyHeader
            // 
            this.LonelyHeader.Text = "No se debe de ver";
            this.LonelyHeader.Width = 700;
            // 
            // PopIList
            // 
            this.PopIList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("PopIList.ImageStream")));
            this.PopIList.TransparentColor = System.Drawing.Color.Transparent;
            this.PopIList.Images.SetKeyName(0, "Schema.png");
            this.PopIList.Images.SetKeyName(1, "Table.png");
            this.PopIList.Images.SetKeyName(2, "View.png");
            this.PopIList.Images.SetKeyName(3, "Procedure.png");
            this.PopIList.Images.SetKeyName(4, "Function.png");
            this.PopIList.Images.SetKeyName(5, "Field.png");
            this.PopIList.Images.SetKeyName(6, "Variable.png");
            this.PopIList.Images.SetKeyName(7, "Snippet.png");
            this.PopIList.Images.SetKeyName(8, "Alias.png");
            this.PopIList.Images.SetKeyName(9, "TableFunction.png");
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(484, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Search results";
            // 
            // GridFields
            // 
            this.GridFields.AllowUserToAddRows = false;
            this.GridFields.AllowUserToDeleteRows = false;
            this.GridFields.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.GridFields.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GridFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GridFields.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridFields.Location = new System.Drawing.Point(0, 18);
            this.GridFields.Name = "GridFields";
            this.GridFields.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridFields.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GridFields.Size = new System.Drawing.Size(484, 116);
            this.GridFields.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(484, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "Selected object fields and/or parameters";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Contains
            // 
            this.Contains.AutoSize = true;
            this.Contains.Dock = System.Windows.Forms.DockStyle.Top;
            this.Contains.Location = new System.Drawing.Point(2, 102);
            this.Contains.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Contains.Name = "Contains";
            this.Contains.Size = new System.Drawing.Size(484, 17);
            this.Contains.TabIndex = 9;
            this.Contains.Text = "Contains filter";
            this.Contains.UseVisualStyleBackColor = true;
            // 
            // StartsWith
            // 
            this.StartsWith.AutoSize = true;
            this.StartsWith.Checked = true;
            this.StartsWith.Dock = System.Windows.Forms.DockStyle.Top;
            this.StartsWith.Location = new System.Drawing.Point(2, 85);
            this.StartsWith.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartsWith.Name = "StartsWith";
            this.StartsWith.Size = new System.Drawing.Size(484, 17);
            this.StartsWith.TabIndex = 8;
            this.StartsWith.TabStop = true;
            this.StartsWith.Text = "Starts by filter";
            this.StartsWith.UseVisualStyleBackColor = true;
            // 
            // TxtFilter
            // 
            this.TxtFilter.DefaultImage = global::Ez_SQL.Properties.Resources.Filter;
            this.TxtFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFilter.Location = new System.Drawing.Point(2, 20);
            this.TxtFilter.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(484, 26);
            this.TxtFilter.TabIndex = 1;
            this.TxtFilter.WaitInterval = 5;
            this.TxtFilter.TextWaitEnded += new Ez_SQL.Custom_Controls.OnTextWaitEnded(this.TxtFilter_TextWaitEnded);
            // 
            // ObjectSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 446);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.Contains);
            this.Controls.Add(this.StartsWith);
            this.Controls.Add(this.CmbSearchType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ObjectSearcher";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "ObjectSearcher";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Custom_Controls.AnimatedWaitTextBox TxtFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbSearchType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView ResultsList;
        private System.Windows.Forms.ColumnHeader LonelyHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView GridFields;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton Contains;
        private System.Windows.Forms.RadioButton StartsWith;
        private System.Windows.Forms.ImageList PopIList;
    }
}