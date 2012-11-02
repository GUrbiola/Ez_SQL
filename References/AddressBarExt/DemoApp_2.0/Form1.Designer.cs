namespace AddressBarExt
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.rtb_path = new System.Windows.Forms.RichTextBox();
            this.btn_c = new System.Windows.Forms.Button();
            this.btn_d = new System.Windows.Forms.Button();
            this.btn_fake = new System.Windows.Forms.Button();
            this.AdBar = new AddressBarExt.Controls.AddressBarExt();
            this.SuspendLayout();
            // 
            // rtb_path
            // 
            this.rtb_path.BackColor = System.Drawing.Color.White;
            this.rtb_path.Location = new System.Drawing.Point(12, 70);
            this.rtb_path.Name = "rtb_path";
            this.rtb_path.ReadOnly = true;
            this.rtb_path.Size = new System.Drawing.Size(544, 76);
            this.rtb_path.TabIndex = 1;
            this.rtb_path.Text = "C:\\";
            // 
            // btn_c
            // 
            this.btn_c.Location = new System.Drawing.Point(12, 12);
            this.btn_c.Name = "btn_c";
            this.btn_c.Size = new System.Drawing.Size(91, 23);
            this.btn_c.TabIndex = 2;
            this.btn_c.Text = "Reset to Root..";
            this.btn_c.UseVisualStyleBackColor = true;
            this.btn_c.Click += new System.EventHandler(this.btn_c_Click);
            // 
            // btn_d
            // 
            this.btn_d.Location = new System.Drawing.Point(109, 12);
            this.btn_d.Name = "btn_d";
            this.btn_d.Size = new System.Drawing.Size(204, 23);
            this.btn_d.TabIndex = 3;
            this.btn_d.Text = "Set Current Node to Program Files...";
            this.btn_d.UseVisualStyleBackColor = true;
            this.btn_d.Click += new System.EventHandler(this.btn_d_Click);
            // 
            // btn_fake
            // 
            this.btn_fake.Location = new System.Drawing.Point(319, 12);
            this.btn_fake.Name = "btn_fake";
            this.btn_fake.Size = new System.Drawing.Size(237, 23);
            this.btn_fake.TabIndex = 4;
            this.btn_fake.Text = "Set Current Node to Something Invalid..";
            this.btn_fake.UseVisualStyleBackColor = true;
            this.btn_fake.Click += new System.EventHandler(this.btn_fake_Click);
            // 
            // AdBar
            // 
            this.AdBar.BackColor = System.Drawing.Color.White;
            this.AdBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AdBar.CurrentNode = null;
            this.AdBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AdBar.Location = new System.Drawing.Point(12, 41);
            this.AdBar.MinimumSize = new System.Drawing.Size(331, 23);
            this.AdBar.Name = "AdBar";
            this.AdBar.RootNode = null;
            this.AdBar.SelectedStyle = ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline)));
            this.AdBar.Size = new System.Drawing.Size(544, 23);
            this.AdBar.TabIndex = 0;
            this.AdBar.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(this.AdBar_SelectionChange);
            this.AdBar.DoubleClick += new System.EventHandler(this.AdBar_DoubleClick);
            this.AdBar.NodeDoubleClick += new AddressBarExt.Controls.AddressBarExt.NodeDoubleClicked(this.AdBar_NodeDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 149);
            this.Controls.Add(this.btn_fake);
            this.Controls.Add(this.btn_d);
            this.Controls.Add(this.btn_c);
            this.Controls.Add(this.rtb_path);
            this.Controls.Add(this.AdBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(576, 185);
            this.MinimumSize = new System.Drawing.Size(576, 185);
            this.Name = "Form1";
            this.Text = "AddressBarExt";
            this.ResumeLayout(false);

        }

        #endregion

        private AddressBarExt.Controls.AddressBarExt AdBar;
        private System.Windows.Forms.RichTextBox rtb_path;
        private System.Windows.Forms.Button btn_c;
        private System.Windows.Forms.Button btn_d;
        private System.Windows.Forms.Button btn_fake;
    }
}

