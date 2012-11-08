namespace Ez_SQL.MultiQueryForm
{
    partial class ChildSelector
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
            this.LabDescription = new System.Windows.Forms.Label();
            this.ChkList = new System.Windows.Forms.CheckedListBox();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.TxtFilter = new Ez_SQL.Custom_Controls.AnimatedWaitTextBox();
            this.SuspendLayout();
            // 
            // LabDescription
            // 
            this.LabDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabDescription.Location = new System.Drawing.Point(0, 0);
            this.LabDescription.Name = "LabDescription";
            this.LabDescription.Size = new System.Drawing.Size(353, 23);
            this.LabDescription.TabIndex = 3;
            this.LabDescription.Text = "label1";
            this.LabDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChkList
            // 
            this.ChkList.CheckOnClick = true;
            this.ChkList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChkList.FormattingEnabled = true;
            this.ChkList.Location = new System.Drawing.Point(0, 66);
            this.ChkList.Name = "ChkList";
            this.ChkList.Size = new System.Drawing.Size(353, 268);
            this.ChkList.TabIndex = 5;
            this.ChkList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChkList_KeyDown);
            this.ChkList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChkList_KeyPress);
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChkAll.Location = new System.Drawing.Point(0, 49);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(353, 17);
            this.ChkAll.TabIndex = 6;
            this.ChkAll.Text = "Check all current items";
            this.ChkAll.UseVisualStyleBackColor = true;
            this.ChkAll.CheckedChanged += new System.EventHandler(this.ChkAll_CheckedChanged);
            // 
            // TxtFilter
            // 
            this.TxtFilter.DefaultImage = global::Ez_SQL.Properties.Resources.Filter;
            this.TxtFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFilter.Location = new System.Drawing.Point(0, 23);
            this.TxtFilter.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(353, 26);
            this.TxtFilter.TabIndex = 4;
            this.TxtFilter.WaitInterval = 1;
            this.TxtFilter.TextWaitEnded += new Ez_SQL.Custom_Controls.OnTextWaitEnded(this.TxtFilter_TextWaitEnded);
            this.TxtFilter.TextSecured += new Ez_SQL.Custom_Controls.OnTextSecured(this.TxtFilter_TextSecured);
            this.TxtFilter.KeyDowned += new System.Windows.Forms.KeyEventHandler(this.TxtFilter_KeyDowned);
            // 
            // ChildSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 334);
            this.Controls.Add(this.ChkList);
            this.Controls.Add(this.ChkAll);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LabDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChildSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChildSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Custom_Controls.AnimatedWaitTextBox TxtFilter;
        private System.Windows.Forms.Label LabDescription;
        private System.Windows.Forms.CheckedListBox ChkList;
        private System.Windows.Forms.CheckBox ChkAll;
    }
}