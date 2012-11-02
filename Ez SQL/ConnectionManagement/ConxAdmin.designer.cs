namespace Ez_SQL
{
	partial class ConxAdmin
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConxAdmin));
            this.label1 = new System.Windows.Forms.Label();
            this.LGroup = new System.Windows.Forms.ListView();
            this.GroupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.IList = new System.Windows.Forms.ImageList(this.components);
            this.LConx = new System.Windows.Forms.ListView();
            this.ConxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnConStr = new System.Windows.Forms.Button();
            this.BtnEnd = new System.Windows.Forms.Button();
            this.AddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupMenu.SuspendLayout();
            this.ConxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection groups";
            // 
            // LGroup
            // 
            this.LGroup.ContextMenuStrip = this.GroupMenu;
            this.LGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.LGroup.LargeImageList = this.IList;
            this.LGroup.Location = new System.Drawing.Point(0, 20);
            this.LGroup.Name = "LGroup";
            this.LGroup.Size = new System.Drawing.Size(364, 167);
            this.LGroup.SmallImageList = this.IList;
            this.LGroup.StateImageList = this.IList;
            this.LGroup.TabIndex = 1;
            this.LGroup.UseCompatibleStateImageBehavior = false;
            this.LGroup.View = System.Windows.Forms.View.List;
            this.LGroup.SelectedIndexChanged += new System.EventHandler(this.LGroupSelectedIndexChanged);
            // 
            // GroupMenu
            // 
            this.GroupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddGroup,
            this.RemoveGroup});
            this.GroupMenu.Name = "GroupMenu";
            this.GroupMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.GroupMenu.Size = new System.Drawing.Size(206, 48);
            // 
            // IList
            // 
            this.IList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IList.ImageStream")));
            this.IList.TransparentColor = System.Drawing.Color.Transparent;
            this.IList.Images.SetKeyName(0, "DBGroup.png");
            this.IList.Images.SetKeyName(1, "Database.png");
            // 
            // LConx
            // 
            this.LConx.ContextMenuStrip = this.ConxMenu;
            this.LConx.Dock = System.Windows.Forms.DockStyle.Top;
            this.LConx.LargeImageList = this.IList;
            this.LConx.Location = new System.Drawing.Point(0, 207);
            this.LConx.Name = "LConx";
            this.LConx.Size = new System.Drawing.Size(364, 167);
            this.LConx.SmallImageList = this.IList;
            this.LConx.StateImageList = this.IList;
            this.LConx.TabIndex = 3;
            this.LConx.UseCompatibleStateImageBehavior = false;
            this.LConx.View = System.Windows.Forms.View.List;
            this.LConx.SelectedIndexChanged += new System.EventHandler(this.LConxSelectedIndexChanged);
            // 
            // ConxMenu
            // 
            this.ConxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddConnection,
            this.RemoveConnection});
            this.ConxMenu.Name = "ConxMenu";
            this.ConxMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ConxMenu.Size = new System.Drawing.Size(276, 48);
            // 
            // AddConnection
            // 
            this.AddConnection.Image = global::Ez_SQL.Properties.Resources.Add_22;
            this.AddConnection.Name = "AddConnection";
            this.AddConnection.Size = new System.Drawing.Size(275, 22);
            this.AddConnection.Text = "Add nex connection to selected group";
            this.AddConnection.Click += new System.EventHandler(this.AddConnectionClick);
            // 
            // RemoveConnection
            // 
            this.RemoveConnection.Image = global::Ez_SQL.Properties.Resources.Remove_22;
            this.RemoveConnection.Name = "RemoveConnection";
            this.RemoveConnection.Size = new System.Drawing.Size(275, 22);
            this.RemoveConnection.Text = "Delete selected connection";
            this.RemoveConnection.Click += new System.EventHandler(this.RemoveConnectionClick);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(364, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Connections";
            // 
            // BtnConStr
            // 
            this.BtnConStr.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnConStr.Location = new System.Drawing.Point(0, 374);
            this.BtnConStr.Name = "BtnConStr";
            this.BtnConStr.Size = new System.Drawing.Size(364, 36);
            this.BtnConStr.TabIndex = 4;
            this.BtnConStr.Text = "Get connection string";
            this.BtnConStr.UseVisualStyleBackColor = true;
            this.BtnConStr.Click += new System.EventHandler(this.BtnConStrClick);
            // 
            // BtnEnd
            // 
            this.BtnEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnEnd.Location = new System.Drawing.Point(0, 410);
            this.BtnEnd.Name = "BtnEnd";
            this.BtnEnd.Size = new System.Drawing.Size(364, 36);
            this.BtnEnd.TabIndex = 5;
            this.BtnEnd.Text = "Save changes";
            this.BtnEnd.UseVisualStyleBackColor = true;
            this.BtnEnd.Click += new System.EventHandler(this.BtnEndClick);
            // 
            // AddGroup
            // 
            this.AddGroup.Image = global::Ez_SQL.Properties.Resources.Add_22;
            this.AddGroup.Name = "AddGroup";
            this.AddGroup.Size = new System.Drawing.Size(205, 22);
            this.AddGroup.Text = "Add connection group";
            this.AddGroup.Click += new System.EventHandler(this.AddGroupClick);
            // 
            // RemoveGroup
            // 
            this.RemoveGroup.Image = global::Ez_SQL.Properties.Resources.Remove_22;
            this.RemoveGroup.Name = "RemoveGroup";
            this.RemoveGroup.Size = new System.Drawing.Size(205, 22);
            this.RemoveGroup.Text = "Delete connection group";
            this.RemoveGroup.Click += new System.EventHandler(this.RemoveGroupClick);
            // 
            // ConxAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 446);
            this.Controls.Add(this.BtnEnd);
            this.Controls.Add(this.BtnConStr);
            this.Controls.Add(this.LConx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LGroup);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConxAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrar Conexiones";
            this.Load += new System.EventHandler(this.ConxAdminLoad);
            this.GroupMenu.ResumeLayout(false);
            this.ConxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ImageList IList;
		private System.Windows.Forms.ListView LGroup;
		private System.Windows.Forms.ListView LConx;
		private System.Windows.Forms.ToolStripMenuItem RemoveConnection;
		private System.Windows.Forms.ToolStripMenuItem AddConnection;
		private System.Windows.Forms.ContextMenuStrip ConxMenu;
		private System.Windows.Forms.ToolStripMenuItem RemoveGroup;
		private System.Windows.Forms.ToolStripMenuItem AddGroup;
		private System.Windows.Forms.ContextMenuStrip GroupMenu;
		private System.Windows.Forms.Button BtnEnd;
		private System.Windows.Forms.Button BtnConStr;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
