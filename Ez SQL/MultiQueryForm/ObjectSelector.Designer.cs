namespace Ez_SQL.MultiQueryForm
{
    partial class ObjectSelector
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
            this.ItemList = new System.Windows.Forms.ListBox();
            this.TxtFilter = new Ez_SQL.Custom_Controls.AnimatedWaitTextBox();
            this.SuspendLayout();
            // 
            // LabDescription
            // 
            this.LabDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabDescription.Location = new System.Drawing.Point(0, 0);
            this.LabDescription.Name = "LabDescription";
            this.LabDescription.Size = new System.Drawing.Size(363, 23);
            this.LabDescription.TabIndex = 0;
            this.LabDescription.Text = "label1";
            this.LabDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ItemList
            // 
            this.ItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemList.FormattingEnabled = true;
            this.ItemList.Location = new System.Drawing.Point(0, 49);
            this.ItemList.Name = "ItemList";
            this.ItemList.Size = new System.Drawing.Size(363, 142);
            this.ItemList.TabIndex = 2;
            this.ItemList.DoubleClick += new System.EventHandler(this.ItemList_DoubleClick);
            // 
            // TxtFilter
            // 
            this.TxtFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtFilter.ImagenInicial = null;
            this.TxtFilter.Location = new System.Drawing.Point(0, 23);
            this.TxtFilter.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.TxtFilter.Name = "TxtFilter";
            this.TxtFilter.Size = new System.Drawing.Size(363, 26);
            this.TxtFilter.TabIndex = 1;
            this.TxtFilter.WaitInterval = 1;
            this.TxtFilter.TextWaitEnded += new Ez_SQL.Custom_Controls.OnTextWaitEnded(this.TxtFilter_TextWaitEnded);
            this.TxtFilter.TextSecured += new Ez_SQL.Custom_Controls.OnTextSecured(this.TxtFilter_TextSecured);
            this.TxtFilter.KeyDowned += new System.Windows.Forms.KeyEventHandler(this.TxtFilter_KeyDowned);
            // 
            // ObjectSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 191);
            this.Controls.Add(this.ItemList);
            this.Controls.Add(this.TxtFilter);
            this.Controls.Add(this.LabDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ObjectSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabDescription;
        private Custom_Controls.AnimatedWaitTextBox TxtFilter;
        private System.Windows.Forms.ListBox ItemList;
    }
}