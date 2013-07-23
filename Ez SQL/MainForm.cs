using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Ez_SQL.ConnectionBarNodes;
using Ez_SQL.DataBaseObjects;
using System.IO;
using Ez_SQL.Extensions;
using Ez_SQL.Snippets;

namespace Ez_SQL
{
    public partial class MainForm : Form
    {
        private AddressBarExt.Controls.AddressBarExt AdBar = null;
        private string _ConxGroup;
        public string ConxGroup
        {
            get { return _ConxGroup; }
        }
        private string _ConxName;
        public string ConxName
        {
            get { return _ConxName; }
        }
        private static string _DataStorageDir;
        public static string DataStorageDir
        {
            get { return _DataStorageDir; }
            set { _DataStorageDir = value; }
        }
        private static string _ConDataFileName;
        public static string ConDataFileName
        {
            get { return _ConDataFileName; }
            set { _ConDataFileName = value; }
        }
        private static string _CurConStr;
        public static string CurConStr
        {
            get { return _CurConStr; }
            set { _CurConStr = value; }
        }
        private SqlConnector _CurrentConnection;
        public SqlConnector CurrentConnection
        {
            get { return _CurrentConnection; }
            set { _CurrentConnection = value; }
        }
        private Point LoadingDialogPosition
        {
            get
            {
                int X, Y;
                X = SideMenu.Width + 10;
                Y = MainMenu.Height + 10;
                return new Point(this.Location.X + X, this.Location.Y + Y);
            }
        }
        private List<SqlConnector> _Connectors;
        public List<SqlConnector> Connectors
        {
            get { return _Connectors; }
            set { _Connectors = value; }
        }
        SnippetEditor Sform;
        //EzConfig.SyntaxColorsConfigurator ColorConfigTab;
        private List<Snippet> Snippets;

        public MainForm()
        {
            InitializeComponent();

            StatusBar.Padding = new Padding(StatusBar.Padding.Left, StatusBar.Padding.Top, StatusBar.Padding.Left, StatusBar.Padding.Bottom);

            #region Connection Bar
            AdBar = new AddressBarExt.Controls.AddressBarExt();
            this.AdBar.BackColor = System.Drawing.Color.White;
            this.AdBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AdBar.CurrentNode = null;
            this.AdBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.AdBar.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdBar.ForeColor = System.Drawing.Color.Navy;
            this.AdBar.Location = new System.Drawing.Point(0, 0);
            this.AdBar.MinimumSize = new System.Drawing.Size(385, 26);
            this.AdBar.Name = "AddressBar";
            this.AdBar.RootNode = null;
            this.AdBar.SelectedStyle = ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline)));
            this.AdBar.Size = new System.Drawing.Size(482, 31);
            this.AdBar.TabIndex = 2;
            this.AdBar.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(this.AddressBarSelectionChange);

            ToolStripControlHost Helper = new ToolStripControlHost(AdBar);
            Helper.Width = 350;
            MainMenu.Items.Insert(0, Helper);
            #endregion

            //not using tha app path anymore, better yet is to use a diferent folder for each winuser that uses the app,
            //this also makes unnecesary the manifest, and the problems it causes
            //DataStorageDir = Application.StartupPath;
            DataStorageDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ez SQL";
            if (!Directory.Exists(DataStorageDir))
            {
                Directory.CreateDirectory(DataStorageDir);
            }

            ConDataFileName = String.Format("{0}\\ConnectionGroups.xml", DataStorageDir);


            if (!Directory.Exists(String.Format("{0}\\Snippets", DataStorageDir)))//create snippets dir, and create default snippets 
            {
                Directory.CreateDirectory(String.Format("{0}\\Snippets", DataStorageDir));

                #region Create the default snippets files
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Begin - end.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Begin_end, 0, Properties.Resources.Begin_end.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Complex Snippet.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Complex_Snippet, 0, Properties.Resources.Complex_Snippet.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Create procedure.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Create_procedure, 0, Properties.Resources.Create_procedure.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Cross join.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Cross_join, 0, Properties.Resources.Cross_join.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Cursor.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Cursor, 0, Properties.Resources.Cursor.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\declare.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.declare, 0, Properties.Resources.declare.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Inner join.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Inner_join, 0, Properties.Resources.Inner_join.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Insert into.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Insert_into, 0, Properties.Resources.Insert_into.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Insert into all.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Insert_into_all, 0, Properties.Resources.Insert_into_all.Length);
                    writer.Flush();
                    writer.Close();
                }
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Left outer join.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Left_outer_join, 0, Properties.Resources.Left_outer_join.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\order by.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.order_by, 0, Properties.Resources.order_by.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Outer apply.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Outer_apply, 0, Properties.Resources.Outer_apply.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Right outer join.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Right_outer_join, 0, Properties.Resources.Right_outer_join.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\select.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.select, 0, Properties.Resources.select.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Select all.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Select_All, 0, Properties.Resources.Select_All.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Select some from.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Select_some_from, 0, Properties.Resources.Select_some_from.Length);
                    writer.Flush();
                    writer.Close();
                } 
                using (FileStream writer = new FileStream(String.Format("{0}\\Snippets\\Select top records.snp", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    writer.Write(Properties.Resources.Select_top_records, 0, Properties.Resources.Select_top_records.Length);
                    writer.Flush();
                    writer.Close();
                } 
                #endregion
            }
            if (!Directory.Exists(String.Format("{0}\\QueriesLog", DataStorageDir)))
            {
                Directory.CreateDirectory(String.Format("{0}\\QueriesLog", DataStorageDir));
            }
            if (!Directory.Exists(MainForm.DataStorageDir + "\\SintaxHighLight"))
            {
                Directory.CreateDirectory(MainForm.DataStorageDir + "\\SintaxHighLight\\");
            }
            if (!File.Exists(String.Format("{0}\\SintaxHighLight\\SQL.xshd", DataStorageDir)))
            {
                using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\SQL.xshd", DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    Writer.Write(Properties.Resources.SQL, 0, Properties.Resources.SQL.Length);
                    Writer.Close();
                }
            }
            LoadSnippets();
        }

        #region Basic Functionality, minimize, maximize, restore, move dialog and resizing
        private bool _IsMinimized = false;
        private bool _IsMaximized;
        private Size _LastSize, _ScreenSize;
        private Point _LastLocation, _ZeroZero;
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void BtnMax_Click(object sender, EventArgs e)
        {
            if (this.Size == _ScreenSize)
            {//is maximized
                _IsMaximized = false;
                this.Location = _LastLocation;
                this.Size = _LastSize;

                BtnMax.Image = Properties.Resources.Maximize;
                BtnMax.Text = "Maximize";
            }
            else
            {
                _IsMaximized = true;
                _LastSize = this.Size;
                _LastLocation = this.Location;
                this.Location = _ZeroZero;
                this.Size = _ScreenSize;

                BtnMax.Image = Properties.Resources.Restore;
                BtnMax.Text = "Restore";
            }
        }
        private void BtnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void MainMenu_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnMax_Click(null, null);
        }
        const int WM_NCHITTEST = 0x0084;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_SIZE = 0xF000;
        const int SC_SIZE_HTBOTTOMRIGHT = 8;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void BtnMoveWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal && e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0x00A1, HTCAPTION, 0);
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if (m.Result == (IntPtr)HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    break;
            }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            _ScreenSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            _ZeroZero = new Point(0, 0);
            _LastLocation = this.Location;
            this.Location = _ZeroZero;
            _LastSize = this.Size;
            this.Size = _ScreenSize;

            _IsMaximized = true;
            BtnMax.Image = Properties.Resources.Restore;
            BtnMax.Text = "Restore";
        }
        private void ResizeIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_IsMaximized && e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_SYSCOMMAND, SC_SIZE + 8, 0);
            }
        }
        #endregion

        private void LoadConnectionsInfo()
        {
            AdBar.InitializeRoot(new RootConxNode("Start"));
        }
        private void AddressBarSelectionChange(object sender, AddressBarExt.Controls.NodeChangedArgs nca)
        {
            string[] Data;
            if (nca.OUniqueID.ToString().Contains("Name"))
            {//si contiene toda la informacion, grupo de conexiones, nombre de conexion y connectionstring
                Data = nca.OUniqueID.ToString().Split('|');
                _ConxGroup = Data[1].Split(':')[1];
                _ConxName = Data[2].Split(':')[1];
                CurConStr = Data[3].Split(':')[1];
                //ConStrCad.Text = CurConStr;
                if (Connectors == null)
                    Connectors = new List<SqlConnector>();
                foreach (SqlConnector _DbConx in Connectors)
                {
                    if (_DbConx.ConnectionString.Equals(CurConStr, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _CurrentConnection = _DbConx;
                        return;
                    }
                }
                Connectors.Add(new SqlConnector(CurConStr));
                _CurrentConnection = Connectors[Connectors.Count - 1];
            }
            else
            {//If there is no connection selected clean connection variables
                _ConxGroup = "";
                _ConxName = "";
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConnectionsInfo();
        }

        #region Top tool bar methods
        private void BtnRefreshConnection_Click(object sender, EventArgs e)
        {
            if (CurrentConnection != null && !CurrentConnection.Busy)
                CurrentConnection.Initialize(LoadingDialogPosition, CurrentConnection.FullLoaded);
            LoadSnippets();
        }
        private void BtnAddConnection_Click(object sender, EventArgs e)
        {
            ConxAdmin Ac = new ConxAdmin();
            if (Ac.ShowDialog() == DialogResult.OK)
                AdBar.InitializeRoot(new RootConxNode("Start"));
        }
        private void BtnCopyConnectionString_Click(object sender, EventArgs e)
        {
            if (CurrentConnection != null && CurrentConnection.ConnectionString.Length > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText(CurrentConnection.ConnectionString);
                MessageBox.Show("Connection string copied to clipboard:" + Environment.NewLine + CurrentConnection.ConnectionString, "Connection string copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
        #region Side tool bar methods
        private void BtnNewQuery_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CurConStr))
            {
                MessageBox.Show("Select a database connection");
                return;
            }

            if (CurrentConnection.IsBusy)
            {
                return;
            }

            if (!CurrentConnection.Loaded)
            {
                CurrentConnection.Initialize(LoadingDialogPosition, false);
            }

            MultiQueryForm.QueryForm dlg = new MultiQueryForm.QueryForm(this, CurrentConnection);
            dlg.ToolTipText = String.Format("{0} - {1}", CurrentConnection.Server, CurrentConnection.DataBase);
            dlg.Text = String.Format("{0} - {1}", ConxGroup, ConxName);
            dlg.ShowIcon = true;
            dlg.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CurConStr))
            {
                MessageBox.Show("Select a database connection");
                return;
            }

            if (CurrentConnection.IsBusy)
            {
                return;
            }

            if (!CurrentConnection.Loaded)
            {
                CurrentConnection.Initialize(LoadingDialogPosition, false);
            }

            AdditionalForms.ObjectSearcher dlg = new AdditionalForms.ObjectSearcher(this, CurrentConnection);
            dlg.ToolTipText = String.Format("{0} - {1}", CurrentConnection.Server, CurrentConnection.DataBase);
            dlg.Text = String.Format("{0} - {1}", ConxGroup, ConxName);
            dlg.ShowIcon = true;
            dlg.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
        }
        private void BtnHistoric_Click(object sender, EventArgs e)
        {
            QueryLog.HistoricForm Hform = new Ez_SQL.QueryLog.HistoricForm();
            Hform.TabText = "Historic";
            Hform.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }
        private void BtnSnippetEditor_Click(object sender, EventArgs e)
        {
            if (Sform == null)
            {
                Sform = new SnippetEditor();
                Sform.TabText = "Snippet Editor";
                Sform.Width = 400;
            }
            if (!Sform.Visible)
            {
                Sform.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
            }
        }
        private void BtnConfigColors_Click(object sender, EventArgs e)
        {
            EzConfig.SyntaxColorsConfigurator ColorConfigTab;
            ColorConfigTab = new EzConfig.SyntaxColorsConfigurator();
            ColorConfigTab.TabText = "Syntax Highlighting Config";
            ColorConfigTab.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }

        #endregion
        #region Method to add a new tab query from an already opened form(can be a query form(F12 or ctrl + click) or can be from a search dialog)
        public void AddQueryForm(string title, string text, SqlConnector DataProvider)
        {
            MultiQueryForm.QueryForm dlg = new MultiQueryForm.QueryForm(this, DataProvider, text);
            dlg.ToolTipText = String.Format("{0} - {1} / {2} - {3}", DataProvider.Server, DataProvider.DataBase, ConxGroup, ConxName);
            dlg.Text = String.Format("{0}", title);
            dlg.ShowIcon = true;
            dlg.Show(WorkPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }
        #endregion

        #region Methods to determine if shortcut is a snippet and to load them from file
        internal string IsInsertSnippet(string Shortcut)
        {
            Snippet Got = Snippets.FindAll(X => X.ShortCut.Equals(Shortcut, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (Got != null)
                return Got.Script;
            return "";
        }
        private void LoadSnippets()
        {
            if (Snippets == null)
                Snippets = new List<Snippet>();
            else
                Snippets.Clear();

            if (Directory.Exists(String.Format("{0}\\Snippets", MainForm.DataStorageDir)))
            {
                string[] Files = Directory.GetFiles(String.Format("{0}\\Snippets", MainForm.DataStorageDir), "*.snp");
                foreach (string f in Files)
                {
                    try
                    {
                        Snippets.Add((Snippet)f.DeserializeFromXmlFile());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show
                        (
                            String.Format("Reading {0} file, raised exception: {1}", f.Substring(f.LastIndexOf('\\') + 1), ex.Message),
                            "Error while reading a snippet file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
                Snippets.Sort((X, Y) => String.Compare(X.Name, Y.Name));
            }
        }
        public List<Snippet> GetSnippetList()
        {
            return Snippets;
        }
        #endregion
    }


}
