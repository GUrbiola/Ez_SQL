using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.ConnectionBarNodes;

namespace Ez_SQL.DbComparer
{
    public partial class DbComparer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private AddressBarExt.Controls.AddressBarExt AdBarSource = null;
        private AddressBarExt.Controls.AddressBarExt AdBarDestination = null;

        public DbComparer()
        {
            InitializeComponent();

            #region Connection Bar
            AdBarSource = new AddressBarExt.Controls.AddressBarExt();
            this.AdBarSource.BackColor = System.Drawing.Color.White;
            this.AdBarSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AdBarSource.CurrentNode = null;
            this.AdBarSource.Dock = System.Windows.Forms.DockStyle.Left;
            this.AdBarSource.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdBarSource.ForeColor = System.Drawing.Color.Navy;
            this.AdBarSource.Location = new System.Drawing.Point(0, 0);
            this.AdBarSource.MinimumSize = new System.Drawing.Size(385, 26);
            this.AdBarSource.Name = "AddressBar";
            this.AdBarSource.RootNode = null;
            this.AdBarSource.SelectedStyle = ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline)));
            this.AdBarSource.Size = new System.Drawing.Size(482, 31);
            this.AdBarSource.TabIndex = 2;
            //this.AdBarSource.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(this.AddressBarSelectionChange);

            ToolStripControlHost Helper = new ToolStripControlHost(AdBarSource);
            Helper.Width = 350;
            SourceMenu.Items.Insert(1, Helper);
            #endregion

            #region Connection Bar
            AdBarDestination = new AddressBarExt.Controls.AddressBarExt();
            this.AdBarDestination.BackColor = System.Drawing.Color.White;
            this.AdBarDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AdBarDestination.CurrentNode = null;
            this.AdBarDestination.Dock = System.Windows.Forms.DockStyle.Left;
            this.AdBarDestination.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdBarDestination.ForeColor = System.Drawing.Color.Navy;
            this.AdBarDestination.Location = new System.Drawing.Point(0, 0);
            this.AdBarDestination.MinimumSize = new System.Drawing.Size(385, 26);
            this.AdBarDestination.Name = "AddressBar";
            this.AdBarDestination.RootNode = null;
            this.AdBarDestination.SelectedStyle = ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline)));
            this.AdBarDestination.Size = new System.Drawing.Size(482, 31);
            this.AdBarDestination.TabIndex = 2;
            //this.AdBarDestination.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(this.AddressBarSelectionChange);

            ToolStripControlHost Helper1 = new ToolStripControlHost(AdBarDestination);
            Helper1.Width = 350;
            DestinationMenu.Items.Insert(1, Helper1);
            #endregion

        }

        private void LoadConnectionsInfo()
        {
            AdBarSource.InitializeRoot(new RootConxNode("Start"));
            AdBarDestination.InitializeRoot(new RootConxNode("Start"));

        }

        private void DbComparer_Load(object sender, EventArgs e)
        {
            LoadConnectionsInfo();
        }

        private void splitContainer1_SizeChanged(object sender, EventArgs e)
        {

        }

        //private void AddressBarSelectionChange(object sender, AddressBarExt.Controls.NodeChangedArgs nca)
        //{
        //    string[] Data;
        //    if (nca.OUniqueID.ToString().Contains("Name"))
        //    {
        //        Data = nca.OUniqueID.ToString().Split('|');
        //        _ConxGroup = Data[1].Split(':')[1];
        //        _ConxName = Data[2].Split(':')[1];
        //        CurConStr = Data[3].Split(':')[1];
        //        //ConStrCad.Text = CurConStr;
        //        if (Connectors == null)
        //            Connectors = new List<SqlConnector>();
        //        foreach (SqlConnector _DbConx in Connectors)
        //        {
        //            if (_DbConx.ConnectionString.Equals(CurConStr, StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                _CurrentConnection = _DbConx;
        //                return;
        //            }
        //        }
        //        Connectors.Add(new SqlConnector(CurConStr));
        //        _CurrentConnection = Connectors[Connectors.Count - 1];
        //    }
        //    else
        //    {//If there is no connection selected clean connection variables
        //        _ConxGroup = "";
        //        _ConxName = "";
        //    }
        //}


    }
}
