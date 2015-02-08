namespace Ez_SQL
{
    partial class SideToSideTester
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
            this.stsComparer = new Ez_SQL.Custom_Controls.SideToSideTextComparer();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stsComparer
            // 
            this.stsComparer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stsComparer.Location = new System.Drawing.Point(0, 23);
            this.stsComparer.Name = "stsComparer";
            this.stsComparer.Size = new System.Drawing.Size(994, 585);
            this.stsComparer.TabIndex = 0;
            this.stsComparer.Text1Label = "Text 1";
            this.stsComparer.Text2Label = "Text 2";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(994, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SideToSideTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 608);
            this.Controls.Add(this.stsComparer);
            this.Controls.Add(this.button1);
            this.Name = "SideToSideTester";
            this.Text = "SideToSideTester";
            this.Load += new System.EventHandler(this.SideToSideTester_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Custom_Controls.SideToSideTextComparer stsComparer;
        private System.Windows.Forms.Button button1;

    }
}