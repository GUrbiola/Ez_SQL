namespace Ez_SQL.EzConfig
{
    partial class SyntaxColorsConfigurator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyntaxColorsConfigurator));
            this.txtEditorPreview = new ICSharpCode.TextEditor.TextEditorControl();
            this.btnRestoreToDefault = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.syntaxTreeView = new System.Windows.Forms.TreeView();
            this.treeIcons = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStyles = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEditorPreview
            // 
            this.txtEditorPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditorPreview.IsReadOnly = false;
            this.txtEditorPreview.Location = new System.Drawing.Point(0, 0);
            this.txtEditorPreview.Name = "txtEditorPreview";
            this.txtEditorPreview.Padding = new System.Windows.Forms.Padding(3);
            this.txtEditorPreview.Size = new System.Drawing.Size(838, 792);
            this.txtEditorPreview.TabIndex = 1;
            this.txtEditorPreview.Text = resources.GetString("txtEditorPreview.Text");
            this.txtEditorPreview.VRulerRow = 0;
            // 
            // btnRestoreToDefault
            // 
            this.btnRestoreToDefault.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRestoreToDefault.Location = new System.Drawing.Point(0, 764);
            this.btnRestoreToDefault.Name = "btnRestoreToDefault";
            this.btnRestoreToDefault.Size = new System.Drawing.Size(420, 28);
            this.btnRestoreToDefault.TabIndex = 7;
            this.btnRestoreToDefault.Text = "Restore Default Values";
            this.btnRestoreToDefault.UseVisualStyleBackColor = true;
            this.btnRestoreToDefault.Click += new System.EventHandler(this.btnRestoreToDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Location = new System.Drawing.Point(0, 736);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(420, 28);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save Changes";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // syntaxTreeView
            // 
            this.syntaxTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxTreeView.ImageIndex = 0;
            this.syntaxTreeView.ImageList = this.treeIcons;
            this.syntaxTreeView.Location = new System.Drawing.Point(0, 34);
            this.syntaxTreeView.Name = "syntaxTreeView";
            this.syntaxTreeView.SelectedImageIndex = 8;
            this.syntaxTreeView.Size = new System.Drawing.Size(420, 702);
            this.syntaxTreeView.TabIndex = 9;
            this.syntaxTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.syntaxTreeView_BeforeCollapse);
            this.syntaxTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.syntaxTreeView_BeforeExpand);
            this.syntaxTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.syntaxTreeView_NodeMouseDoubleClick);
            this.syntaxTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.syntaxTreeView_MouseDown);
            // 
            // treeIcons
            // 
            this.treeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeIcons.ImageStream")));
            this.treeIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.treeIcons.Images.SetKeyName(0, "TreeRoot.png");
            this.treeIcons.Images.SetKeyName(1, "Environment.png");
            this.treeIcons.Images.SetKeyName(2, "Digits.png");
            this.treeIcons.Images.SetKeyName(3, "Gears.png");
            this.treeIcons.Images.SetKeyName(4, "Gear.png");
            this.treeIcons.Images.SetKeyName(5, "Span.png");
            this.treeIcons.Images.SetKeyName(6, "KeyWords.png");
            this.treeIcons.Images.SetKeyName(7, "Letter.png");
            this.treeIcons.Images.SetKeyName(8, "CurrentNode.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.syntaxTreeView);
            this.splitContainer1.Panel1.Controls.Add(this.cmbStyles);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnRestoreToDefault);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtEditorPreview);
            this.splitContainer1.Size = new System.Drawing.Size(1262, 792);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Predefined styles";
            // 
            // cmbStyles
            // 
            this.cmbStyles.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStyles.FormattingEnabled = true;
            this.cmbStyles.Items.AddRange(new object[] {
            "Current",
            "Default",
            "Son of Obsidian",
            "Selenitic",
            "Old Timer(Turbo C)"});
            this.cmbStyles.Location = new System.Drawing.Point(0, 13);
            this.cmbStyles.Name = "cmbStyles";
            this.cmbStyles.Size = new System.Drawing.Size(420, 21);
            this.cmbStyles.TabIndex = 11;
            this.cmbStyles.SelectedIndexChanged += new System.EventHandler(this.cmbStyles_SelectedIndexChanged);
            // 
            // SyntaxColorsConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 792);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SyntaxColorsConfigurator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Color Configuration for the Text Editor in the Query Form";
            this.Load += new System.EventHandler(this.SyntaxColorsConfigurator_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl txtEditorPreview;
        private System.Windows.Forms.Button btnRestoreToDefault;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView syntaxTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList treeIcons;
        private System.Windows.Forms.ComboBox cmbStyles;
        private System.Windows.Forms.Label label1;

    }
}