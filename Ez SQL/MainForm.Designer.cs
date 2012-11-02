namespace Ez_SQL
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.MainMenu = new System.Windows.Forms.ToolStrip();
            this.BtnRefreshConnection = new System.Windows.Forms.ToolStripButton();
            this.BtnAddConnection = new System.Windows.Forms.ToolStripButton();
            this.BtnCopyConnectionString = new System.Windows.Forms.ToolStripButton();
            this.BtnClose = new System.Windows.Forms.ToolStripButton();
            this.BtnMax = new System.Windows.Forms.ToolStripButton();
            this.BtnMin = new System.Windows.Forms.ToolStripButton();
            this.BtnMoveWindow = new System.Windows.Forms.ToolStripButton();
            this.SideMenu = new System.Windows.Forms.ToolStrip();
            this.BtnNewQuery = new System.Windows.Forms.ToolStripButton();
            this.BtnSearch = new System.Windows.Forms.ToolStripButton();
            this.BtnHistoric = new System.Windows.Forms.ToolStripButton();
            this.BtnImport = new System.Windows.Forms.ToolStripButton();
            this.BtnSnippetEditor = new System.Windows.Forms.ToolStripButton();
            this.WorkPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BgWorker = new System.ComponentModel.BackgroundWorker();
            this.ResizeIcon = new Ez_SQL.Custom_Controls.StatusStripIcon();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.MainMenu.SuspendLayout();
            this.SideMenu.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.AutoSize = false;
            this.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnRefreshConnection,
            this.BtnAddConnection,
            this.BtnCopyConnectionString,
            this.BtnClose,
            this.BtnMax,
            this.BtnMin,
            this.BtnMoveWindow});
            this.MainMenu.Location = new System.Drawing.Point(44, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenu.Size = new System.Drawing.Size(880, 32);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "toolStrip1";
            this.MainMenu.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainMenu_MouseDoubleClick);
            // 
            // BtnRefreshConnection
            // 
            this.BtnRefreshConnection.AutoSize = false;
            this.BtnRefreshConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnRefreshConnection.Image = global::Ez_SQL.Properties.Resources.Refresh;
            this.BtnRefreshConnection.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnRefreshConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnRefreshConnection.Name = "BtnRefreshConnection";
            this.BtnRefreshConnection.Size = new System.Drawing.Size(28, 28);
            this.BtnRefreshConnection.Text = "Refresh connection";
            this.BtnRefreshConnection.Click += new System.EventHandler(this.BtnRefreshConnection_Click);
            // 
            // BtnAddConnection
            // 
            this.BtnAddConnection.AutoSize = false;
            this.BtnAddConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnAddConnection.Image = global::Ez_SQL.Properties.Resources.AddConnection;
            this.BtnAddConnection.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnAddConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnAddConnection.Name = "BtnAddConnection";
            this.BtnAddConnection.Size = new System.Drawing.Size(28, 28);
            this.BtnAddConnection.Text = "Add new connection";
            this.BtnAddConnection.Click += new System.EventHandler(this.BtnAddConnection_Click);
            // 
            // BtnCopyConnectionString
            // 
            this.BtnCopyConnectionString.AutoSize = false;
            this.BtnCopyConnectionString.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnCopyConnectionString.Image = global::Ez_SQL.Properties.Resources.CopyConnectionString;
            this.BtnCopyConnectionString.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnCopyConnectionString.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnCopyConnectionString.Name = "BtnCopyConnectionString";
            this.BtnCopyConnectionString.Size = new System.Drawing.Size(28, 28);
            this.BtnCopyConnectionString.Text = "Copy connection string";
            this.BtnCopyConnectionString.Click += new System.EventHandler(this.BtnCopyConnectionString_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnClose.AutoSize = false;
            this.BtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnClose.Image = global::Ez_SQL.Properties.Resources.Exit;
            this.BtnClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(28, 28);
            this.BtnClose.Text = "Close";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnMax
            // 
            this.BtnMax.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnMax.AutoSize = false;
            this.BtnMax.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnMax.Image = ((System.Drawing.Image)(resources.GetObject("BtnMax.Image")));
            this.BtnMax.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnMax.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnMax.Name = "BtnMax";
            this.BtnMax.Size = new System.Drawing.Size(28, 28);
            this.BtnMax.Text = "Maximize";
            this.BtnMax.Click += new System.EventHandler(this.BtnMax_Click);
            // 
            // BtnMin
            // 
            this.BtnMin.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnMin.AutoSize = false;
            this.BtnMin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnMin.Image = global::Ez_SQL.Properties.Resources.Minimize;
            this.BtnMin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnMin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnMin.Name = "BtnMin";
            this.BtnMin.Size = new System.Drawing.Size(28, 28);
            this.BtnMin.Text = "Minimize";
            this.BtnMin.Click += new System.EventHandler(this.BtnMin_Click);
            // 
            // BtnMoveWindow
            // 
            this.BtnMoveWindow.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnMoveWindow.AutoSize = false;
            this.BtnMoveWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnMoveWindow.Image = global::Ez_SQL.Properties.Resources.MoveWindow;
            this.BtnMoveWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnMoveWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnMoveWindow.Name = "BtnMoveWindow";
            this.BtnMoveWindow.Size = new System.Drawing.Size(28, 28);
            this.BtnMoveWindow.Text = "Move window";
            this.BtnMoveWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnMoveWindow_MouseDown);
            // 
            // SideMenu
            // 
            this.SideMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.SideMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SideMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.SideMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnNewQuery,
            this.BtnSearch,
            this.BtnHistoric,
            this.BtnImport,
            this.BtnSnippetEditor});
            this.SideMenu.Location = new System.Drawing.Point(0, 0);
            this.SideMenu.Name = "SideMenu";
            this.SideMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.SideMenu.Size = new System.Drawing.Size(44, 459);
            this.SideMenu.TabIndex = 2;
            this.SideMenu.Text = "toolStrip2";
            // 
            // BtnNewQuery
            // 
            this.BtnNewQuery.AutoSize = false;
            this.BtnNewQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnNewQuery.Image = global::Ez_SQL.Properties.Resources.New_Window32;
            this.BtnNewQuery.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnNewQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnNewQuery.Name = "BtnNewQuery";
            this.BtnNewQuery.Size = new System.Drawing.Size(43, 36);
            this.BtnNewQuery.Text = "New query";
            this.BtnNewQuery.Click += new System.EventHandler(this.BtnNewQuery_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.AutoSize = false;
            this.BtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnSearch.Image = global::Ez_SQL.Properties.Resources.DB_Search32;
            this.BtnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(43, 36);
            this.BtnSearch.Text = "Search objects";
            // 
            // BtnHistoric
            // 
            this.BtnHistoric.AutoSize = false;
            this.BtnHistoric.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnHistoric.Image = global::Ez_SQL.Properties.Resources.Historic;
            this.BtnHistoric.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnHistoric.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnHistoric.Name = "BtnHistoric";
            this.BtnHistoric.Size = new System.Drawing.Size(43, 36);
            this.BtnHistoric.Text = "Query execution historic";
            this.BtnHistoric.Click += new System.EventHandler(this.BtnHistoric_Click);
            // 
            // BtnImport
            // 
            this.BtnImport.AutoSize = false;
            this.BtnImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnImport.Image = global::Ez_SQL.Properties.Resources.ImportToDB;
            this.BtnImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(43, 36);
            this.BtnImport.Text = "Import to database";
            // 
            // BtnSnippetEditor
            // 
            this.BtnSnippetEditor.AutoSize = false;
            this.BtnSnippetEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnSnippetEditor.Image = global::Ez_SQL.Properties.Resources.SnippetEditor;
            this.BtnSnippetEditor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnSnippetEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSnippetEditor.Name = "BtnSnippetEditor";
            this.BtnSnippetEditor.Size = new System.Drawing.Size(43, 36);
            this.BtnSnippetEditor.Text = "Import to database";
            this.BtnSnippetEditor.Click += new System.EventHandler(this.BtnSnippetEditor_Click);
            // 
            // WorkPanel
            // 
            this.WorkPanel.BackgroundImage = global::Ez_SQL.Properties.Resources.Background;
            this.WorkPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WorkPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.WorkPanel.Location = new System.Drawing.Point(44, 32);
            this.WorkPanel.Name = "WorkPanel";
            this.WorkPanel.Size = new System.Drawing.Size(880, 427);
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
            autoHideStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
            tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
            dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
            tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
            dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
            dockPaneStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
            tabGradient12.EndColor = System.Drawing.SystemColors.Control;
            tabGradient12.StartColor = System.Drawing.SystemColors.Control;
            tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
            dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
            tabGradient13.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
            tabGradient14.EndColor = System.Drawing.Color.Transparent;
            tabGradient14.StartColor = System.Drawing.Color.Transparent;
            tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
            dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
            dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
            this.WorkPanel.Skin = dockPanelSkin2;
            this.WorkPanel.TabIndex = 3;
            // 
            // BgWorker
            // 
            this.BgWorker.WorkerReportsProgress = true;
            this.BgWorker.WorkerSupportsCancellation = true;
            // 
            // ResizeIcon
            // 
            this.ResizeIcon.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ResizeIcon.AutoSize = false;
            this.ResizeIcon.BackColor = System.Drawing.Color.Transparent;
            this.ResizeIcon.BackgroundImage = global::Ez_SQL.Properties.Resources.ResizeIcon;
            this.ResizeIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ResizeIcon.Margin = new System.Windows.Forms.Padding(0);
            this.ResizeIcon.Name = "ResizeIcon";
            this.ResizeIcon.Size = new System.Drawing.Size(25, 28);
            this.ResizeIcon.ToolTipText = "Resize window";
            this.ResizeIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResizeIcon_MouseDown);
            // 
            // StatusBar
            // 
            this.StatusBar.AutoSize = false;
            this.StatusBar.GripMargin = new System.Windows.Forms.Padding(0);
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResizeIcon});
            this.StatusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.StatusBar.Location = new System.Drawing.Point(0, 459);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.StatusBar.Size = new System.Drawing.Size(924, 29);
            this.StatusBar.SizingGrip = false;
            this.StatusBar.TabIndex = 6;
            this.StatusBar.Text = "StatusBar";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 488);
            this.Controls.Add(this.WorkPanel);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.SideMenu);
            this.Controls.Add(this.StatusBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.SideMenu.ResumeLayout(false);
            this.SideMenu.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainMenu;
        private System.Windows.Forms.ToolStripButton BtnAddConnection;
        private System.Windows.Forms.ToolStrip SideMenu;
        private System.Windows.Forms.ToolStripButton BtnRefreshConnection;
        private System.Windows.Forms.ToolStripButton BtnCopyConnectionString;
        private System.Windows.Forms.ToolStripButton BtnClose;
        private System.Windows.Forms.ToolStripButton BtnMax;
        private System.Windows.Forms.ToolStripButton BtnMin;
        private System.Windows.Forms.ToolStripButton BtnMoveWindow;
        private WeifenLuo.WinFormsUI.Docking.DockPanel WorkPanel;
        private System.Windows.Forms.ToolStripButton BtnNewQuery;
        private System.Windows.Forms.ToolStripButton BtnSearch;
        private System.Windows.Forms.ToolStripButton BtnHistoric;
        private System.Windows.Forms.ToolStripButton BtnImport;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker BgWorker;
        private Custom_Controls.StatusStripIcon ResizeIcon;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripButton BtnSnippetEditor;
    }
}