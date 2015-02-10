namespace Ez_SQL.DbComparer
{
    partial class DbComparer
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
            this.panelSource = new System.Windows.Forms.Panel();
            this.SourceMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.panelDestination = new System.Windows.Forms.Panel();
            this.DestinationMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripDestination = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.labProgressStatus = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAdd = new System.Windows.Forms.TabPage();
            this.tabUpdate = new System.Windows.Forms.TabPage();
            this.tabRemove = new System.Windows.Forms.TabPage();
            this.sideToSideTextComparer1 = new Ez_SQL.Custom_Controls.SideToSideTextComparer();
            this.panelSource.SuspendLayout();
            this.SourceMenu.SuspendLayout();
            this.panelDestination.SuspendLayout();
            this.DestinationMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSource
            // 
            this.panelSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSource.Controls.Add(this.SourceMenu);
            this.panelSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSource.Location = new System.Drawing.Point(0, 0);
            this.panelSource.Name = "panelSource";
            this.panelSource.Padding = new System.Windows.Forms.Padding(1);
            this.panelSource.Size = new System.Drawing.Size(635, 34);
            this.panelSource.TabIndex = 0;
            // 
            // SourceMenu
            // 
            this.SourceMenu.AutoSize = false;
            this.SourceMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SourceMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel});
            this.SourceMenu.Location = new System.Drawing.Point(1, 1);
            this.SourceMenu.Name = "SourceMenu";
            this.SourceMenu.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.SourceMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.SourceMenu.Size = new System.Drawing.Size(629, 32);
            this.SourceMenu.TabIndex = 2;
            this.SourceMenu.Text = "toolStrip1";
            // 
            // toolStripLabel
            // 
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(94, 29);
            this.toolStripLabel.Text = "Source Database";
            // 
            // panelDestination
            // 
            this.panelDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDestination.Controls.Add(this.DestinationMenu);
            this.panelDestination.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDestination.Location = new System.Drawing.Point(0, 0);
            this.panelDestination.Name = "panelDestination";
            this.panelDestination.Padding = new System.Windows.Forms.Padding(1);
            this.panelDestination.Size = new System.Drawing.Size(637, 34);
            this.panelDestination.TabIndex = 1;
            // 
            // DestinationMenu
            // 
            this.DestinationMenu.AutoSize = false;
            this.DestinationMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.DestinationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDestination});
            this.DestinationMenu.Location = new System.Drawing.Point(1, 1);
            this.DestinationMenu.Name = "DestinationMenu";
            this.DestinationMenu.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.DestinationMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.DestinationMenu.Size = new System.Drawing.Size(631, 32);
            this.DestinationMenu.TabIndex = 2;
            this.DestinationMenu.Text = "toolStrip1";
            // 
            // toolStripDestination
            // 
            this.toolStripDestination.Name = "toolStripDestination";
            this.toolStripDestination.Size = new System.Drawing.Size(118, 29);
            this.toolStripDestination.Text = "Destination Database";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelSource);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelDestination);
            this.splitContainer1.Size = new System.Drawing.Size(1273, 34);
            this.splitContainer1.SplitterDistance = 635;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.SizeChanged += new System.EventHandler(this.splitContainer1_SizeChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1273, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Start Comparison";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // labProgressStatus
            // 
            this.labProgressStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.labProgressStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressStatus.Location = new System.Drawing.Point(0, 64);
            this.labProgressStatus.Name = "labProgressStatus";
            this.labProgressStatus.Size = new System.Drawing.Size(1273, 23);
            this.labProgressStatus.TabIndex = 5;
            this.labProgressStatus.Text = "Heere Im going to show the information about the current status";
            this.labProgressStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 87);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.sideToSideTextComparer1);
            this.splitContainer2.Size = new System.Drawing.Size(1273, 481);
            this.splitContainer2.SplitterDistance = 210;
            this.splitContainer2.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAdd);
            this.tabControl1.Controls.Add(this.tabUpdate);
            this.tabControl1.Controls.Add(this.tabRemove);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1273, 210);
            this.tabControl1.TabIndex = 0;
            // 
            // tabAdd
            // 
            this.tabAdd.Location = new System.Drawing.Point(4, 22);
            this.tabAdd.Name = "tabAdd";
            this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdd.Size = new System.Drawing.Size(1265, 184);
            this.tabAdd.TabIndex = 0;
            this.tabAdd.Text = "Objects Only Found in Source";
            this.tabAdd.UseVisualStyleBackColor = true;
            // 
            // tabUpdate
            // 
            this.tabUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabUpdate.Name = "tabUpdate";
            this.tabUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpdate.Size = new System.Drawing.Size(1265, 184);
            this.tabUpdate.TabIndex = 1;
            this.tabUpdate.Text = "Objects With Differences";
            this.tabUpdate.UseVisualStyleBackColor = true;
            // 
            // tabRemove
            // 
            this.tabRemove.Location = new System.Drawing.Point(4, 22);
            this.tabRemove.Name = "tabRemove";
            this.tabRemove.Size = new System.Drawing.Size(1265, 184);
            this.tabRemove.TabIndex = 2;
            this.tabRemove.Text = "Objects Only Found in Destination";
            this.tabRemove.UseVisualStyleBackColor = true;
            // 
            // sideToSideTextComparer1
            // 
            this.sideToSideTextComparer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideToSideTextComparer1.Location = new System.Drawing.Point(0, 0);
            this.sideToSideTextComparer1.Name = "sideToSideTextComparer1";
            this.sideToSideTextComparer1.Size = new System.Drawing.Size(1273, 267);
            this.sideToSideTextComparer1.TabIndex = 0;
            this.sideToSideTextComparer1.Text1Label = "Object from Source Database";
            this.sideToSideTextComparer1.Text2Label = "Object from Destination Database";
            // 
            // DbComparer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 568);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.labProgressStatus);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DbComparer";
            this.Text = "DbComparer";
            this.Load += new System.EventHandler(this.DbComparer_Load);
            this.panelSource.ResumeLayout(false);
            this.SourceMenu.ResumeLayout(false);
            this.SourceMenu.PerformLayout();
            this.panelDestination.ResumeLayout(false);
            this.DestinationMenu.ResumeLayout(false);
            this.DestinationMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSource;
        private System.Windows.Forms.ToolStrip SourceMenu;
        private System.Windows.Forms.ToolStripLabel toolStripLabel;
        private System.Windows.Forms.Panel panelDestination;
        private System.Windows.Forms.ToolStrip DestinationMenu;
        private System.Windows.Forms.ToolStripLabel toolStripDestination;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labProgressStatus;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAdd;
        private System.Windows.Forms.TabPage tabUpdate;
        private System.Windows.Forms.TabPage tabRemove;
        private Custom_Controls.SideToSideTextComparer sideToSideTextComparer1;
    }
}