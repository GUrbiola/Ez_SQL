namespace Ez_SQL.Custom_Controls
{
    partial class SideToSideLineComparer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Line1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.Line2 = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // Line1
            // 
            this.Line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Line1.IsReadOnly = false;
            this.Line1.Location = new System.Drawing.Point(2, 2);
            this.Line1.Name = "Line1";
            this.Line1.ShowLineNumbers = false;
            this.Line1.ShowVRuler = false;
            this.Line1.Size = new System.Drawing.Size(411, 32);
            this.Line1.TabIndex = 1;
            this.Line1.Text = "textEditorControl1";
            // 
            // Line2
            // 
            this.Line2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Line2.IsReadOnly = false;
            this.Line2.Location = new System.Drawing.Point(2, 34);
            this.Line2.Name = "Line2";
            this.Line2.ShowLineNumbers = false;
            this.Line2.ShowVRuler = false;
            this.Line2.Size = new System.Drawing.Size(411, 32);
            this.Line2.TabIndex = 2;
            this.Line2.Text = "textEditorControl1";
            // 
            // SideToSideLineComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Line2);
            this.Controls.Add(this.Line1);
            this.Name = "SideToSideLineComparer";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(415, 67);
            this.ResumeLayout(false);

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl Line1;
        private ICSharpCode.TextEditor.TextEditorControl Line2;
    }
}
