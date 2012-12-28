namespace Ez_SQL.MultiQueryForm
{
    partial class QueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryForm));
            this.PopIList = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.TabIcons = new System.Windows.Forms.ImageList(this.components);
            this.FoldingRefresher = new System.Windows.Forms.Timer(this.components);
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.Query = new ICSharpCode.TextEditor.TextEditorControl();
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.goToDefinitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reservedWordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toUpperCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toLowerCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toUpperCseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toLowerCaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.outlinningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionsToolStrip = new System.Windows.Forms.ToolStrip();
            this.BtnExecute = new System.Windows.Forms.ToolStripButton();
            this.BtnStop = new System.Windows.Forms.ToolStripButton();
            this.BtnExtremeStop = new System.Windows.Forms.ToolStripButton();
            this.BtnComment = new System.Windows.Forms.ToolStripButton();
            this.BtnUncomment = new System.Windows.Forms.ToolStripButton();
            this.BtnSearch = new System.Windows.Forms.ToolStripButton();
            this.BtnBookmark = new System.Windows.Forms.ToolStripButton();
            this.BtnPrevious = new System.Windows.Forms.ToolStripButton();
            this.BtnNext = new System.Windows.Forms.ToolStripButton();
            this.BtnClearBookmarks = new System.Windows.Forms.ToolStripButton();
            this.BtnSave = new System.Windows.Forms.ToolStripButton();
            this.BtnLoad = new System.Windows.Forms.ToolStripButton();
            this.BtnShowHideResults = new System.Windows.Forms.ToolStripButton();
            this.BDTxt = new System.Windows.Forms.ToolStripTextBox();
            this.ServerTxt = new System.Windows.Forms.ToolStripTextBox();
            this.TabHolder = new System.Windows.Forms.TabControl();
            this.tabMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LockTabButton = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.popupMenu.SuspendLayout();
            this.ActionsToolStrip.SuspendLayout();
            this.tabMenu.SuspendLayout();
            this.SuspendLayout();
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
            // TabIcons
            // 
            this.TabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabIcons.ImageStream")));
            this.TabIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.TabIcons.Images.SetKeyName(0, "Table.png");
            this.TabIcons.Images.SetKeyName(1, "Clock.png");
            this.TabIcons.Images.SetKeyName(2, "Alert.png");
            this.TabIcons.Images.SetKeyName(3, "Edit.png");
            this.TabIcons.Images.SetKeyName(4, "LockedTab.png");
            // 
            // FoldingRefresher
            // 
            this.FoldingRefresher.Interval = 200;
            this.FoldingRefresher.Tick += new System.EventHandler(this.FoldingRefresher_Tick);
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.Query);
            this.MainContainer.Panel1.Controls.Add(this.ActionsToolStrip);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.TabHolder);
            this.MainContainer.Size = new System.Drawing.Size(954, 384);
            this.MainContainer.SplitterDistance = 212;
            this.MainContainer.TabIndex = 1;
            // 
            // Query
            // 
            this.Query.ContextMenuStrip = this.popupMenu;
            this.Query.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Query.IsIconBarVisible = true;
            this.Query.IsReadOnly = false;
            this.Query.Location = new System.Drawing.Point(0, 30);
            this.Query.Name = "Query";
            this.Query.ShowVRuler = false;
            this.Query.Size = new System.Drawing.Size(954, 182);
            this.Query.TabIndex = 0;
            // 
            // popupMenu
            // 
            this.popupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToDefinitionToolStripMenuItem,
            this.reservedWordsToolStripMenuItem,
            this.selectionToolStripMenuItem,
            this.outlinningToolStripMenuItem});
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.Size = new System.Drawing.Size(187, 92);
            // 
            // goToDefinitionToolStripMenuItem
            // 
            this.goToDefinitionToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.GoToDefinition;
            this.goToDefinitionToolStripMenuItem.Name = "goToDefinitionToolStripMenuItem";
            this.goToDefinitionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.goToDefinitionToolStripMenuItem.Text = "Go to definition (F12)";
            this.goToDefinitionToolStripMenuItem.Click += new System.EventHandler(this.goToDefinitionToolStripMenuItem_Click);
            // 
            // reservedWordsToolStripMenuItem
            // 
            this.reservedWordsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toUpperCaseToolStripMenuItem,
            this.toLowerCaseToolStripMenuItem});
            this.reservedWordsToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Words;
            this.reservedWordsToolStripMenuItem.Name = "reservedWordsToolStripMenuItem";
            this.reservedWordsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.reservedWordsToolStripMenuItem.Text = "Reserved words";
            // 
            // toUpperCaseToolStripMenuItem
            // 
            this.toUpperCaseToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.ToUpperCase;
            this.toUpperCaseToolStripMenuItem.Name = "toUpperCaseToolStripMenuItem";
            this.toUpperCaseToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.toUpperCaseToolStripMenuItem.Text = "To upper case";
            this.toUpperCaseToolStripMenuItem.Click += new System.EventHandler(this.toUpperCaseToolStripMenuItem_Click);
            // 
            // toLowerCaseToolStripMenuItem
            // 
            this.toLowerCaseToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.ToLowerCase;
            this.toLowerCaseToolStripMenuItem.Name = "toLowerCaseToolStripMenuItem";
            this.toLowerCaseToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.toLowerCaseToolStripMenuItem.Text = "To lower case";
            this.toLowerCaseToolStripMenuItem.Click += new System.EventHandler(this.toLowerCaseToolStripMenuItem_Click);
            // 
            // selectionToolStripMenuItem
            // 
            this.selectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toUpperCseToolStripMenuItem,
            this.toLowerCaseToolStripMenuItem1});
            this.selectionToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Selection;
            this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
            this.selectionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.selectionToolStripMenuItem.Text = "Selection";
            // 
            // toUpperCseToolStripMenuItem
            // 
            this.toUpperCseToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.ToUpperCase;
            this.toUpperCseToolStripMenuItem.Name = "toUpperCseToolStripMenuItem";
            this.toUpperCseToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.toUpperCseToolStripMenuItem.Text = "To upper case  (Ctrl + Shift + U)";
            this.toUpperCseToolStripMenuItem.Click += new System.EventHandler(this.toUpperCseToolStripMenuItem_Click);
            // 
            // toLowerCaseToolStripMenuItem1
            // 
            this.toLowerCaseToolStripMenuItem1.Image = global::Ez_SQL.Properties.Resources.ToLowerCase;
            this.toLowerCaseToolStripMenuItem1.Name = "toLowerCaseToolStripMenuItem1";
            this.toLowerCaseToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.toLowerCaseToolStripMenuItem1.Text = "To lower case (Ctrl + U)";
            this.toLowerCaseToolStripMenuItem1.Click += new System.EventHandler(this.toLowerCaseToolStripMenuItem1_Click);
            // 
            // outlinningToolStripMenuItem
            // 
            this.outlinningToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.toggleToolStripMenuItem});
            this.outlinningToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Outlining;
            this.outlinningToolStripMenuItem.Name = "outlinningToolStripMenuItem";
            this.outlinningToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.outlinningToolStripMenuItem.Text = "Outlining/Folding";
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Collapse;
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.collapseToolStripMenuItem.Text = "Collapse (Ctrl + O, C)";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Expand;
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.expandToolStripMenuItem.Text = "Expand (Ctrl + O, E)";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Image = global::Ez_SQL.Properties.Resources.Toggle;
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            this.toggleToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.toggleToolStripMenuItem.Text = "Toggle (Ctrl + O, T)";
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
            // 
            // ActionsToolStrip
            // 
            this.ActionsToolStrip.AutoSize = false;
            this.ActionsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ActionsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnExecute,
            this.BtnStop,
            this.BtnExtremeStop,
            this.BtnComment,
            this.BtnUncomment,
            this.BtnSearch,
            this.BtnBookmark,
            this.BtnPrevious,
            this.BtnNext,
            this.BtnClearBookmarks,
            this.BtnSave,
            this.BtnLoad,
            this.BtnShowHideResults,
            this.BDTxt,
            this.ServerTxt});
            this.ActionsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ActionsToolStrip.Name = "ActionsToolStrip";
            this.ActionsToolStrip.Size = new System.Drawing.Size(954, 30);
            this.ActionsToolStrip.TabIndex = 1;
            // 
            // BtnExecute
            // 
            this.BtnExecute.AutoSize = false;
            this.BtnExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnExecute.Image = global::Ez_SQL.Properties.Resources.Play;
            this.BtnExecute.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnExecute.Name = "BtnExecute";
            this.BtnExecute.Size = new System.Drawing.Size(28, 28);
            this.BtnExecute.ToolTipText = "Execute query (F5)";
            this.BtnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.AutoSize = false;
            this.BtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnStop.Enabled = false;
            this.BtnStop.Image = global::Ez_SQL.Properties.Resources.Stop;
            this.BtnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(28, 28);
            this.BtnStop.ToolTipText = "Stop execution(Shift + F5)";
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnExtremeStop
            // 
            this.BtnExtremeStop.AutoSize = false;
            this.BtnExtremeStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnExtremeStop.Enabled = false;
            this.BtnExtremeStop.Image = global::Ez_SQL.Properties.Resources.RedAlert;
            this.BtnExtremeStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnExtremeStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnExtremeStop.Name = "BtnExtremeStop";
            this.BtnExtremeStop.Size = new System.Drawing.Size(28, 28);
            this.BtnExtremeStop.ToolTipText = "Force the stop execution(Ctr + F5)";
            this.BtnExtremeStop.Click += new System.EventHandler(this.BtnExtremeStop_Click);
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
            this.BtnComment.ToolTipText = "Comment lines (Ctr + K, C)";
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
            this.BtnUncomment.ToolTipText = "Uncomment lines(Ctr + K, U)";
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
            this.BtnSearch.ToolTipText = "Search (Ctr + F)";
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
            // BtnShowHideResults
            // 
            this.BtnShowHideResults.AutoSize = false;
            this.BtnShowHideResults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnShowHideResults.Image = global::Ez_SQL.Properties.Resources.Link;
            this.BtnShowHideResults.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnShowHideResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnShowHideResults.Name = "BtnShowHideResults";
            this.BtnShowHideResults.Size = new System.Drawing.Size(28, 28);
            this.BtnShowHideResults.ToolTipText = "Show/hide results tab (Ctr + W)";
            this.BtnShowHideResults.Click += new System.EventHandler(this.BtnShowHideResults_Click);
            // 
            // BDTxt
            // 
            this.BDTxt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BDTxt.Name = "BDTxt";
            this.BDTxt.ReadOnly = true;
            this.BDTxt.Size = new System.Drawing.Size(180, 30);
            // 
            // ServerTxt
            // 
            this.ServerTxt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ServerTxt.Name = "ServerTxt";
            this.ServerTxt.ReadOnly = true;
            this.ServerTxt.Size = new System.Drawing.Size(180, 30);
            // 
            // TabHolder
            // 
            this.TabHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabHolder.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabHolder.ImageList = this.TabIcons;
            this.TabHolder.Location = new System.Drawing.Point(0, 0);
            this.TabHolder.Name = "TabHolder";
            this.TabHolder.SelectedIndex = 0;
            this.TabHolder.Size = new System.Drawing.Size(954, 168);
            this.TabHolder.TabIndex = 0;
            // 
            // tabMenu
            // 
            this.tabMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LockTabButton});
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.Size = new System.Drawing.Size(123, 26);
            // 
            // LockTabButton
            // 
            this.LockTabButton.Image = global::Ez_SQL.Properties.Resources.LockTab;
            this.LockTabButton.Name = "LockTabButton";
            this.LockTabButton.Size = new System.Drawing.Size(122, 22);
            this.LockTabButton.Text = "Lock Tab";
            this.LockTabButton.Click += new System.EventHandler(this.LockTabButton_Click);
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 384);
            this.Controls.Add(this.MainContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "QueryForm";
            this.Text = "QueryForm";
            this.Load += new System.EventHandler(this.QueryForm_Load);
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.popupMenu.ResumeLayout(false);
            this.ActionsToolStrip.ResumeLayout(false);
            this.ActionsToolStrip.PerformLayout();
            this.tabMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList PopIList;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList TabIcons;
        private System.Windows.Forms.Timer FoldingRefresher;
        private System.Windows.Forms.SplitContainer MainContainer;
        private ICSharpCode.TextEditor.TextEditorControl Query;
        private System.Windows.Forms.ToolStrip ActionsToolStrip;
        private System.Windows.Forms.ToolStripButton BtnExecute;
        private System.Windows.Forms.ToolStripButton BtnStop;
        private System.Windows.Forms.ToolStripButton BtnExtremeStop;
        private System.Windows.Forms.ToolStripButton BtnComment;
        private System.Windows.Forms.ToolStripButton BtnUncomment;
        private System.Windows.Forms.ToolStripButton BtnSearch;
        private System.Windows.Forms.ToolStripButton BtnBookmark;
        private System.Windows.Forms.ToolStripButton BtnPrevious;
        private System.Windows.Forms.ToolStripButton BtnNext;
        private System.Windows.Forms.ToolStripButton BtnClearBookmarks;
        private System.Windows.Forms.ToolStripButton BtnSave;
        private System.Windows.Forms.ToolStripButton BtnLoad;
        private System.Windows.Forms.ToolStripButton BtnShowHideResults;
        private System.Windows.Forms.ToolStripTextBox BDTxt;
        private System.Windows.Forms.ToolStripTextBox ServerTxt;
        private System.Windows.Forms.TabControl TabHolder;
        private System.Windows.Forms.ContextMenuStrip popupMenu;
        private System.Windows.Forms.ToolStripMenuItem goToDefinitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reservedWordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toUpperCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toLowerCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toUpperCseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toLowerCaseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem outlinningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip tabMenu;
        private System.Windows.Forms.ToolStripMenuItem LockTabButton;
    }
}