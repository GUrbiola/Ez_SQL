namespace Ez_SQL.CSharp
{
    partial class SharpCodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharpCodeForm));
            this.ActionsToolStrip = new System.Windows.Forms.ToolStrip();
            this.BtnComment = new System.Windows.Forms.ToolStripButton();
            this.BtnUncomment = new System.Windows.Forms.ToolStripButton();
            this.BtnSearch = new System.Windows.Forms.ToolStripButton();
            this.BtnBookmark = new System.Windows.Forms.ToolStripButton();
            this.BtnPrevious = new System.Windows.Forms.ToolStripButton();
            this.BtnNext = new System.Windows.Forms.ToolStripButton();
            this.BtnClearBookmarks = new System.Windows.Forms.ToolStripButton();
            this.BtnSave = new System.Windows.Forms.ToolStripButton();
            this.BtnLoad = new System.Windows.Forms.ToolStripButton();
            this.BtnCopy = new System.Windows.Forms.ToolStripButton();
            this.MsgLabel = new System.Windows.Forms.ToolStripLabel();
            this.SharpText = new ICSharpCode.TextEditor.TextEditorControl();
            this.FoldingRefresher = new System.Windows.Forms.Timer(this.components);
            this.ThisTabMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionsToolStrip.SuspendLayout();
            this.ThisTabMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionsToolStrip
            // 
            this.ActionsToolStrip.AutoSize = false;
            this.ActionsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ActionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnComment,
            this.BtnUncomment,
            this.BtnSearch,
            this.BtnBookmark,
            this.BtnPrevious,
            this.BtnNext,
            this.BtnClearBookmarks,
            this.BtnSave,
            this.BtnLoad,
            this.BtnCopy,
            this.MsgLabel});
            this.ActionsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ActionsToolStrip.Name = "ActionsToolStrip";
            this.ActionsToolStrip.Size = new System.Drawing.Size(759, 30);
            this.ActionsToolStrip.TabIndex = 2;
            // 
            // BtnComment
            // 
            this.BtnComment.AutoSize = false;
            this.BtnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnComment.Image = global::Ez_SQL.Properties.Resources.Comment;
            this.BtnComment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnComment.Name = "BtnComment";
            this.BtnComment.Size = new System.Drawing.Size(28, 28);
            this.BtnComment.ToolTipText = "Comment Selected Lines (Ctrl+K)";
            this.BtnComment.Click += new System.EventHandler(this.BtnComment_Click);
            // 
            // BtnUncomment
            // 
            this.BtnUncomment.AutoSize = false;
            this.BtnUncomment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnUncomment.Image = global::Ez_SQL.Properties.Resources.UnComment;
            this.BtnUncomment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnUncomment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnUncomment.Name = "BtnUncomment";
            this.BtnUncomment.Size = new System.Drawing.Size(28, 28);
            this.BtnUncomment.ToolTipText = "Uncomment selected lines(Ctrl+U)";
            this.BtnUncomment.Click += new System.EventHandler(this.BtnUncomment_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.AutoSize = false;
            this.BtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnSearch.Image = global::Ez_SQL.Properties.Resources.Search;
            this.BtnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(28, 28);
            this.BtnSearch.ToolTipText = "Search or replace text(Ctrl+F)";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnBookmark
            // 
            this.BtnBookmark.AutoSize = false;
            this.BtnBookmark.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnBookmark.Image = global::Ez_SQL.Properties.Resources.Bookmark;
            this.BtnBookmark.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnBookmark.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnBookmark.Name = "BtnBookmark";
            this.BtnBookmark.Size = new System.Drawing.Size(28, 28);
            this.BtnBookmark.ToolTipText = "Toggle bookmark(F2)";
            this.BtnBookmark.Click += new System.EventHandler(this.BtnBookmark_Click);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.AutoSize = false;
            this.BtnPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnPrevious.Image = global::Ez_SQL.Properties.Resources.Previous;
            this.BtnPrevious.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(28, 28);
            this.BtnPrevious.ToolTipText = "Go to previous bookmark (Shift + F2)";
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.AutoSize = false;
            this.BtnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnNext.Image = ((System.Drawing.Image)(resources.GetObject("BtnNext.Image")));
            this.BtnNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(28, 28);
            this.BtnNext.ToolTipText = "Go to next bookmark (Ctr + F2)";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnClearBookmarks
            // 
            this.BtnClearBookmarks.AutoSize = false;
            this.BtnClearBookmarks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnClearBookmarks.Image = global::Ez_SQL.Properties.Resources.DelBookmark;
            this.BtnClearBookmarks.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnClearBookmarks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnClearBookmarks.Name = "BtnClearBookmarks";
            this.BtnClearBookmarks.Size = new System.Drawing.Size(28, 28);
            this.BtnClearBookmarks.ToolTipText = "Clear bookmarks (Ctr + Shift + F2)";
            this.BtnClearBookmarks.Click += new System.EventHandler(this.BtnClearBookmarks_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.AutoSize = false;
            this.BtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnSave.Image = global::Ez_SQL.Properties.Resources.Save;
            this.BtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(28, 28);
            this.BtnSave.ToolTipText = "Save script to file (Ctr + G)";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.AutoSize = false;
            this.BtnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnLoad.Image = global::Ez_SQL.Properties.Resources.Open;
            this.BtnLoad.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(28, 28);
            this.BtnLoad.ToolTipText = "Load file (Ctr + L)";
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnCopy
            // 
            this.BtnCopy.AutoSize = false;
            this.BtnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnCopy.Image = global::Ez_SQL.Properties.Resources.CopyConnectionString;
            this.BtnCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(28, 28);
            this.BtnCopy.ToolTipText = "Copy all text";
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // MsgLabel
            // 
            this.MsgLabel.AutoSize = false;
            this.MsgLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MsgLabel.Name = "MsgLabel";
            this.MsgLabel.Size = new System.Drawing.Size(200, 27);
            // 
            // SharpText
            // 
            this.SharpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SharpText.IsIconBarVisible = true;
            this.SharpText.IsReadOnly = false;
            this.SharpText.Location = new System.Drawing.Point(0, 30);
            this.SharpText.Name = "SharpText";
            this.SharpText.ShowVRuler = false;
            this.SharpText.Size = new System.Drawing.Size(759, 396);
            this.SharpText.TabIndex = 3;
            // 
            // FoldingRefresher
            // 
            this.FoldingRefresher.Interval = 200;
            this.FoldingRefresher.Tick += new System.EventHandler(this.FoldingRefresher_Tick);
            // 
            // ThisTabMenu
            // 
            this.ThisTabMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.closeAllButThisToolStripMenuItem});
            this.ThisTabMenu.Name = "ThisTabMenu";
            this.ThisTabMenu.Size = new System.Drawing.Size(167, 92);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Delete_24;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeAllToolStripMenuItem.Image")));
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // closeAllButThisToolStripMenuItem
            // 
            this.closeAllButThisToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeAllButThisToolStripMenuItem.Image")));
            this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
            this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.closeAllButThisToolStripMenuItem.Text = "Close All But This";
            this.closeAllButThisToolStripMenuItem.Click += new System.EventHandler(this.closeAllButThisToolStripMenuItem_Click);
            // 
            // SharpCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 426);
            this.Controls.Add(this.SharpText);
            this.Controls.Add(this.ActionsToolStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SharpCodeForm";
            this.TabPageContextMenuStrip = this.ThisTabMenu;
            this.TabText = "C# Code";
            this.Text = "SharpCodeForm";
            this.ToolTipText = "Ventana de Codigo de CSharp";
            this.Load += new System.EventHandler(this.SharpCodeForm_Load);
            this.Click += new System.EventHandler(this.BtnComment_Click);
            this.ActionsToolStrip.ResumeLayout(false);
            this.ActionsToolStrip.PerformLayout();
            this.ThisTabMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip ActionsToolStrip;
        private System.Windows.Forms.ToolStripButton BtnComment;
        private System.Windows.Forms.ToolStripButton BtnUncomment;
        private System.Windows.Forms.ToolStripButton BtnSearch;
        private System.Windows.Forms.ToolStripButton BtnBookmark;
        private System.Windows.Forms.ToolStripButton BtnPrevious;
        private System.Windows.Forms.ToolStripButton BtnNext;
        private System.Windows.Forms.ToolStripButton BtnClearBookmarks;
        private System.Windows.Forms.ToolStripButton BtnSave;
        private System.Windows.Forms.ToolStripButton BtnLoad;
        private ICSharpCode.TextEditor.TextEditorControl SharpText;
        private System.Windows.Forms.Timer FoldingRefresher;
        private System.Windows.Forms.ToolStripButton BtnCopy;
        private System.Windows.Forms.ToolStripLabel MsgLabel;
        private System.Windows.Forms.ContextMenuStrip ThisTabMenu;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllButThisToolStripMenuItem;
    }
}