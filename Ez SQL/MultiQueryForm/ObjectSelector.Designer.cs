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
            this.CmbObjects = new System.Windows.Forms.ComboBox();
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
            // CmbObjects
            // 
            this.CmbObjects.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CmbObjects.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.CmbObjects.Dock = System.Windows.Forms.DockStyle.Top;
            this.CmbObjects.FormattingEnabled = true;
            this.CmbObjects.Location = new System.Drawing.Point(0, 23);
            this.CmbObjects.Name = "CmbObjects";
            this.CmbObjects.Size = new System.Drawing.Size(363, 21);
            this.CmbObjects.TabIndex = 1;
            this.CmbObjects.SelectedIndexChanged += new System.EventHandler(this.CmbObjects_SelectedIndexChanged);
            // 
            // ObjectSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 44);
            this.Controls.Add(this.CmbObjects);
            this.Controls.Add(this.LabDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectSelector";
            this.Text = "ObjectSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabDescription;
        private System.Windows.Forms.ComboBox CmbObjects;
    }
}