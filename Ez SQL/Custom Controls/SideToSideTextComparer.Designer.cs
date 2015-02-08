namespace Ez_SQL.Custom_Controls
{
    partial class SideToSideTextComparer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Txt1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.LabTxt1 = new System.Windows.Forms.Label();
            this.Txt2 = new ICSharpCode.TextEditor.TextEditorControl();
            this.LabTxt2 = new System.Windows.Forms.Label();
            this.LineComparer = new Ez_SQL.Custom_Controls.SideToSideLineComparer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Txt1);
            this.splitContainer1.Panel1.Controls.Add(this.LabTxt1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Txt2);
            this.splitContainer1.Panel2.Controls.Add(this.LabTxt2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Size = new System.Drawing.Size(732, 520);
            this.splitContainer1.SplitterDistance = 358;
            this.splitContainer1.TabIndex = 0;
            // 
            // Txt1
            // 
            this.Txt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt1.IsReadOnly = false;
            this.Txt1.Location = new System.Drawing.Point(2, 25);
            this.Txt1.Name = "Txt1";
            this.Txt1.ShowVRuler = false;
            this.Txt1.Size = new System.Drawing.Size(354, 493);
            this.Txt1.TabIndex = 0;
            this.Txt1.Text = "textEditorControl1";
            // 
            // LabTxt1
            // 
            this.LabTxt1.AutoSize = true;
            this.LabTxt1.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabTxt1.Location = new System.Drawing.Point(2, 2);
            this.LabTxt1.Name = "LabTxt1";
            this.LabTxt1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LabTxt1.Size = new System.Drawing.Size(37, 23);
            this.LabTxt1.TabIndex = 1;
            this.LabTxt1.Text = "Text 1";
            // 
            // Txt2
            // 
            this.Txt2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Txt2.IsReadOnly = false;
            this.Txt2.Location = new System.Drawing.Point(2, 25);
            this.Txt2.Name = "Txt2";
            this.Txt2.ShowVRuler = false;
            this.Txt2.Size = new System.Drawing.Size(366, 493);
            this.Txt2.TabIndex = 2;
            this.Txt2.Text = "textEditorControl2";
            // 
            // LabTxt2
            // 
            this.LabTxt2.AutoSize = true;
            this.LabTxt2.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabTxt2.Location = new System.Drawing.Point(2, 2);
            this.LabTxt2.Name = "LabTxt2";
            this.LabTxt2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.LabTxt2.Size = new System.Drawing.Size(37, 23);
            this.LabTxt2.TabIndex = 3;
            this.LabTxt2.Text = "Text 2";
            // 
            // LineComparer
            // 
            this.LineComparer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LineComparer.Location = new System.Drawing.Point(0, 520);
            this.LineComparer.Name = "LineComparer";
            this.LineComparer.Padding = new System.Windows.Forms.Padding(2);
            this.LineComparer.Size = new System.Drawing.Size(732, 67);
            this.LineComparer.TabIndex = 2;
            // 
            // SideToSideTextComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.LineComparer);
            this.Name = "SideToSideTextComparer";
            this.Size = new System.Drawing.Size(732, 587);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ICSharpCode.TextEditor.TextEditorControl Txt1;
        private System.Windows.Forms.Label LabTxt1;
        private ICSharpCode.TextEditor.TextEditorControl Txt2;
        private System.Windows.Forms.Label LabTxt2;
        private SideToSideLineComparer LineComparer;
    }
}
