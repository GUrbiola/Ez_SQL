using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Ez_SQL.ConnectionManagement;

namespace Ez_SQL
{
	public partial class ConxAdmin : Form
	{
		List<ConnectionGroup> CGs;
		int SelG, SelC;
		public ConxAdmin()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			CGs = Globals.GetConnections(MainForm.ConDataFileName);
			SelG = SelC = -1;
		}
		void AddGroupClick(object sender, EventArgs e)
		{
            InputBox dlg;
            dlg = new InputBox(true, "New connection group","Name for the new connection group");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ConnectionGroup aux = new ConnectionGroup();
                aux.Name = dlg.Input;
                CGs.Add(aux);
                LoadInfo();
            }
		}
		void RemoveGroupClick(object sender, EventArgs e)
		{

            if (SelG >= 0)
            {
                if(MessageBox.Show("Delete the selected connection group?", "Delte confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes )
                {
                    CGs.RemoveAt(SelG);
                    LoadInfo();
                }
            }			
		}		
		void AddConnectionClick(object sender, EventArgs e)
		{
            InputBox dlg;
            if (SelG >= 0)
            {
                ConnectionInfo NewCI = new ConnectionInfo();
                SQLConnectForm ConnectUI = new SQLConnectForm();
                if(ConnectUI.ShowDialog() == DialogResult.OK)
                {
                    dlg = new InputBox(true, "New connection", "Name for the new connection");
                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;
                    else
                        NewCI.Name = dlg.Input;
            		NewCI.ConnectionString = ConnectUI.ConnectionString;
                    CGs[SelG].Connections.Add(NewCI);
                    LoadInfo(CGs[SelG]);
                }
            }
            else
            {
                MessageBox.Show("Select the connection group to whuch the new connection will be added", "Add connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

		}
		void RemoveConnectionClick(object sender, EventArgs e)
		{
            if (SelG >= 0)
            {
                if (SelC >= 0)
                {
                    if(MessageBox.Show("Delete selected connection?", "Delecte confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        CGs[SelG].Connections.RemoveAt(SelC);
                        LoadInfo(CGs[SelG]);
                    }
                }
                else
                {
                    MessageBox.Show("A connection must be selected", "Delete connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Select connection group", "Delete connection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }			
		}
		void BtnEndClick(object sender, EventArgs e)
		{
            Globals.SaveConnections(MainForm.ConDataFileName, CGs);
            DialogResult = DialogResult.OK;
		}
        private void LoadInfo(ConnectionGroup Current = null)
        {
            LGroup.Items.Clear();
            foreach (ConnectionGroup gc in CGs)
            	LGroup.Items.Add(gc.Name, 0);
            if (Current != null)
            {
                LGroup.SelectedIndices.Clear();
                LGroup.SelectedIndices.Add(CGs.IndexOf(Current));
            }
        }		
		void LGroupSelectedIndexChanged(object sender, EventArgs e)
		{
            int index;
            index = LGroup.SelectedIndices.Count > 0 ? LGroup.SelectedIndices[0] : -1;
            if (index >= 0)
            {
            	SelG = index;
            	SelC = -1;
            	label1.Text = "Connection groups: " + CGs[index].Name;
            	label2.Text = "Connections";
	            LConx.Items.Clear();
                foreach (ConnectionInfo con in CGs[index].Connections)
                    LConx.Items.Add(con.Name, 1);
            }
            else
            {
            	label1.Text = "Connection groups";
            	SelG = -1;
            }
		}
		void LConxSelectedIndexChanged(object sender, EventArgs e)
		{
            int indexc;
            indexc = LConx.SelectedIndices.Count > 0 ? LConx.SelectedIndices[0] : -1;
            BtnConStr.Tag = "";
            if (SelG >= 0)
            {
                if (indexc >= 0)
                {
                	SelC = indexc;
                	label2.Text = "Connections: " + CGs[SelG].Connections[SelC].Name;
                    BtnConStr.Tag = CGs[SelG].Connections[SelC].ConnectionString;
                }
                else
                {
                	SelC = -1;
                    label2.Text = "Connections";
               }
            }			
		}
		void BtnConStrClick(object sender, EventArgs e)
		{
            if (BtnConStr.Tag != null && BtnConStr.Tag.ToString().Trim().Length > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText(BtnConStr.Tag.ToString());
                MessageBox.Show(BtnConStr.Tag.ToString(), "Connection string copied to the clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }			
		}
		void ConxAdminLoad(object sender, EventArgs e)
		{
			LoadInfo();
		}
	}
}
