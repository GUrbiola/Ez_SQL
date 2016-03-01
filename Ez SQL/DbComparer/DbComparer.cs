using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.ConnectionBarNodes;
using Ez_SQL.Custom_Controls;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.DbComparer
{
    public partial class DbComparer : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private AddressBarExt.Controls.AddressBarExt AdBarSource = null;
        private AddressBarExt.Controls.AddressBarExt AdBarDestination = null;
        private SqlConnector sourceConx;
        private SqlConnector destinationConx;
        private List<DifferenceModel> differencesFound;
        private List<SqlConnector> _Connectors;
        public List<SqlConnector> Connectors
        {
            get { return _Connectors; }
            set { _Connectors = value; }
        }

        public DbComparer()
        {
            InitializeComponent();

            Connectors = new List<SqlConnector>();

            #region Connection Bar (Source)
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
            this.AdBarSource.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(SourceConnectionChange);

            ToolStripControlHost Helper = new ToolStripControlHost(AdBarSource);
            Helper.Width = 350;
            SourceMenu.Items.Insert(1, Helper);
            #endregion

            #region Connection Bar (Destination)
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
            this.AdBarDestination.SelectionChange += new AddressBarExt.Controls.AddressBarExt.SelectionChanged(DestinationConnectionChange);


            ToolStripControlHost Helper1 = new ToolStripControlHost(AdBarDestination);
            Helper1.Width = 350;
            DestinationMenu.Items.Insert(1, Helper1);
            #endregion

        }

        #region Form Events
        private void DbComparer_Load(object sender, EventArgs e)
        {
            LoadConnectionsInfo();
        }
        #endregion

        #region Control Events
        private void DestinationConnectionChange(object sender, AddressBarExt.Controls.NodeChangedArgs nca)
        {
            string[] Data;
            if (nca.OUniqueID.ToString().Contains("Name"))
            {
                Data = nca.OUniqueID.ToString().Split('|');
                string CurConStr = Data[3].Split(':')[1];
                //ConStrCad.Text = CurConStr;
                if (Connectors == null)
                    Connectors = new List<SqlConnector>();
                foreach (SqlConnector _DbConx in Connectors)
                {
                    if (_DbConx.ConnectionString.Equals(CurConStr, StringComparison.CurrentCultureIgnoreCase))
                    {
                        destinationConx = _DbConx;
                        return;
                    }
                }
                Connectors.Add(new SqlConnector(CurConStr));
                destinationConx = Connectors[Connectors.Count - 1];
            }
            else
            {
                destinationConx = null;
            }
        }

        private void SourceConnectionChange(object sender, AddressBarExt.Controls.NodeChangedArgs nca)
        {
            string[] Data;
            if (nca.OUniqueID.ToString().Contains("Name"))
            {
                Data = nca.OUniqueID.ToString().Split('|');
                string CurConStr = Data[3].Split(':')[1];
                //ConStrCad.Text = CurConStr;
                if (Connectors == null)
                    Connectors = new List<SqlConnector>();
                foreach (SqlConnector _DbConx in Connectors)
                {
                    if (_DbConx.ConnectionString.Equals(CurConStr, StringComparison.CurrentCultureIgnoreCase))
                    {
                        sourceConx = _DbConx;
                        return;
                    }
                }
                Connectors.Add(new SqlConnector(CurConStr));
                sourceConx = Connectors[Connectors.Count - 1];
            }
            else
            {
                sourceConx = null;
            }
        }

        private void btnGOOO_Click(object sender, EventArgs e)
        {
            if (sourceConx == null)
            {
                MessageBox.Show("Source connection must be selected", "Source Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (destinationConx == null)
            {
                MessageBox.Show("Destination connection must be selected", "Destination Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (sourceConx.ConnectionString.Equals(destinationConx.ConnectionString, StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("Both source and destination are pointing to the same data base, comparison is not needed.", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (differencesFound == null)
            {
                differencesFound = new List<DifferenceModel>();
            }
            else
            {
                differencesFound.Clear();
            }

            //this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            bgWorker.RunWorkerAsync();
        }

        #region Background worker events, allows threaded execution of the comparison
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker bw = sender as BackgroundWorker;
            //e.Result = LoadInfo();
            //if (bw.CancellationPending)
            //{
            //    e.Cancel = true;
            //}

            int curCount;
            
            if(sourceConx.DbObjects != null && sourceConx.DbObjects.Count > 0)
                sourceConx.DbObjects.Clear();
            if (destinationConx.DbObjects != null && destinationConx.DbObjects.Count > 0)
                destinationConx.DbObjects.Clear();

            //1.- Read schema from Source
            bgWorker.ReportProgress(0, "Step 1 of 4: Reading Objects from Source - (Tables: 0)");
            curCount = sourceConx.LoadTables();
            bgWorker.ReportProgress(0, String.Format("Step 1 of 4: Reading Objects from Source - (Tables: {0})", curCount));
            
            bgWorker.ReportProgress(0, "Step 1 of 4: Reading Objects from Source - (Views: 0)");
            curCount = sourceConx.LoadViews();
            bgWorker.ReportProgress(0, String.Format("Step 1 of 4: Reading Objects from Source - (Views: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 1 of 4: Reading Objects from Source - (Stored Procedures: 0)");
            curCount = sourceConx.LoadProcedures();
            bgWorker.ReportProgress(0, String.Format("Step 1 of 4: Reading Objects from Source - (Stored Procedures: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 1 of 4: Reading Objects from Source - (Scalar Functions: 0)");
            curCount = sourceConx.LoadScalarFunctions();
            bgWorker.ReportProgress(0, String.Format("Step 1 of 4: Reading Objects from Source - (Scalar Functions: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 1 of 4: Reading Objects from Source - (Table Functions: 0)");
            curCount = sourceConx.LoadTableFunctions();
            bgWorker.ReportProgress(0, String.Format("Step 1 of 4: Reading Objects from Source - (Table Functions: {0})", curCount));
            bgWorker.ReportProgress(0, "Step 1: Completed!");

            //2.- Read schema from Destination
            bgWorker.ReportProgress(0, "Step 2 of 4: Reading Objects from Destination - (Tables: 0)");
            curCount = destinationConx.LoadTables();
            bgWorker.ReportProgress(0, String.Format("Step 2 of 4: Reading Objects from Destination - (Tables: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 2 of 4: Reading Objects from Destination - (Views: 0)");
            curCount = destinationConx.LoadViews();
            bgWorker.ReportProgress(0, String.Format("Step 2 of 4: Reading Objects from Destination - (Views: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 2 of 4: Reading Objects from Destination - (Stored Procedures: 0)");
            curCount = destinationConx.LoadProcedures();
            bgWorker.ReportProgress(0, String.Format("Step 2 of 4: Reading Objects from Destination - (Stored Procedures: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 2 of 4: Reading Objects from Destination - (Scalar Functions: 0)");
            curCount = destinationConx.LoadScalarFunctions();
            bgWorker.ReportProgress(0, String.Format("Step 2 of 4: Reading Objects from Destination - (Scalar Functions: {0})", curCount));

            bgWorker.ReportProgress(0, "Step 2 of 4: Reading Objects from Destination - (Table Functions: 0)");
            curCount = destinationConx.LoadTableFunctions();
            bgWorker.ReportProgress(0, String.Format("Step 2 of 4: Reading Objects from Destination - (Table Functions: {0})", curCount));
            bgWorker.ReportProgress(0, "Step 2: Completed!");

            //3.- Do the comparison
            bgWorker.ReportProgress(0, "Step 3 of 4: Comparing Tables.");
            FindDifferencesBetweenObjects(ObjectType.Table);
            bgWorker.ReportProgress(0, "Step 3 of 4: Table Comparison Complete.");

            bgWorker.ReportProgress(0, "Step 3 of 4: Comparing Views.");
            FindDifferencesBetweenObjects(ObjectType.View);
            bgWorker.ReportProgress(0, "Step 3 of 4: View Comparison Complete.");

            bgWorker.ReportProgress(0, "Step 3 of 4: Comparing Stored Procedures.");
            FindDifferencesBetweenObjects(ObjectType.Procedure);
            bgWorker.ReportProgress(0, "Step 3 of 4: Stored Procedure Comparison Complete.");

            bgWorker.ReportProgress(0, "Step 3 of 4: Comparing Scalar Function.");
            FindDifferencesBetweenObjects(ObjectType.ScalarFunction);
            bgWorker.ReportProgress(0, "Step 3 of 4: Table Function Comparison Complete.");

            bgWorker.ReportProgress(0, "Step 3 of 4: Comparing Table Functions.");
            FindDifferencesBetweenObjects(ObjectType.TableFunction);
            bgWorker.ReportProgress(0, "Step 3 of 4: Table Function Comparison Complete.");
            bgWorker.ReportProgress(0, "Step 3: Completed!");

            //4.- Load results to UI
            bgWorker.ReportProgress(0, "Step 4 of 4: Loading Results to Controls.");

        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labProgressStatus.Text = e.UserState.ToString();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadGrid(DifferenceType.None);
            LoadGrid(DifferenceType.Add);
            LoadGrid(DifferenceType.Update);
            LoadGrid(DifferenceType.Delete);

            //this.Enabled = true;
            this.Cursor = Cursors.Default;
            
            labProgressStatus.Text = "Step 4: Completed!";
        }
        #endregion

        private void GridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) //click on check box
            {
                if (grid != null)// && !grid.Rows[e.RowIndex].Cells[0].ReadOnly)
                {
                    grid.Rows[e.RowIndex].Cells[0].Value = !((bool)grid.Rows[e.RowIndex].Cells[0].Value);
                }
            }
            else if(e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                DifferenceType dt = DifferenceType.None;
                DifferenceModel dm;
                string name;
                if (grid == gridNone)
                {
                    dt = DifferenceType.None;
                }
                else if (grid == gridAdd)
                {
                    dt = DifferenceType.Add;
                }
                else if (grid == gridUpdate)
                {
                    dt = DifferenceType.Update;
                }
                else if (grid == gridDelete)
                {
                    dt = DifferenceType.Delete;
                }
                name = grid.Rows[e.RowIndex].Cells[2].Value.ToString();


                dm = differencesFound.FirstOrDefault(x => x.DiffType == dt && x.Name == name);
                if (dm != null)
                {
                    sideToSideTextComparer1.LoadTexts(dm.SourceScript, dm.DestinationScript);
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            TabPage p = e.TabPage;
            DataGridView grid = null;
            DifferenceType dt = DifferenceType.None;
            DifferenceModel dm;
            string name;

            if (p != null)
            {
                if (p == tabNone)
                {
                    grid = gridNone;
                    dt = DifferenceType.None;
                }
                else if (p == tabAdd)
                {
                    grid = gridAdd;
                    dt = DifferenceType.Add;
                }
                else if (p == tabUpdate)
                {
                    grid = gridUpdate;
                    dt = DifferenceType.Update;
                }
                else if (p == tabRemove)
                {
                    grid = gridDelete;
                    dt = DifferenceType.Delete;
                }

                if(grid != null && grid.SelectedRows != null && grid.SelectedRows.Count > 0)
                {
                    name = grid.SelectedRows[0].Cells[2].Value.ToString();
                    dm = differencesFound.FirstOrDefault(x => x.DiffType == dt && x.Name == name);
                    if (dm != null)
                    {
                        sideToSideTextComparer1.LoadTexts(dm.SourceScript, dm.DestinationScript);
                    }
                }
                
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DifferenceType dt = DifferenceType.Add;
            DifferenceModel dm;
            string name;
            List<string> pendings = new List<string>();
            bool applyChange;

            if (MessageBox.Show("Please confirm, would you like to create all the missing objects at the Destination Db?", "Synchronization Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (gridAdd.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in gridAdd.Rows)
                {
                    applyChange = (bool)row.Cells[0].Value;
                    name = row.Cells[2].Value.ToString();
                    dm = differencesFound.FirstOrDefault(x => x.DiffType == dt && x.Name == name);
                    if (dm != null && applyChange)
                    {
                        string query = dm.SourceScript;
                        if (!destinationConx.ExecuteNonQuery(query))
                        {
                            pendings.Add(query);
                        }
                    }
                }

                int lupe = 0;
                while (pendings.Count > 0 && lupe < 3)
                {
                    for (int i = pendings.Count - 1; i >= 0; i--)
                    {
                        if (destinationConx.ExecuteNonQuery(pendings[i]))
                        {
                            pendings.RemoveAt(i);
                        }
                    }
                    lupe++;
                }


                if (pendings.Count > 0)
                {
                    MessageBox.Show(
                        "Synchronization incomplete, failed creation of some objects. Refresh comparison to find out differences.",
                        "Db Object Creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Creation of objects successful!", "Db Object Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DifferenceType dt = DifferenceType.Update;
            DifferenceModel dm;
            string name;
            List<string> pendings = new List<string>();
            bool applyChange;

            if (MessageBox.Show("Please confirm, would you like to update all the objects(TABLES WILL BE SKIPPED) with differences at the Destination Db?", "Synchronization Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (gridUpdate.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in gridUpdate.Rows)
                {
                    applyChange = (bool)row.Cells[0].Value;
                    name = row.Cells[2].Value.ToString();
                    dm = differencesFound.FirstOrDefault(x => x.DiffType == dt && x.Name == name && x.ObjectKind != ObjectType.Table);
                    if (dm != null && applyChange)
                    {
                        string query = dm.SourceScript;
                        query = "ALTER" + query.Remove(0, 6);
                        if (!destinationConx.ExecuteNonQuery(query))
                        {
                            pendings.Add(query);
                        }
                    }
                }

                int lupe = 0;
                while (pendings.Count > 0 && lupe < 3)
                {
                    for (int i = pendings.Count - 1; i >= 0; i--)
                    {
                        if (!destinationConx.ExecuteNonQuery(pendings[i]))
                        {
                            pendings.RemoveAt(i);
                        }
                    }
                    lupe++;
                }


                if (pendings.Count > 0)
                {
                    MessageBox.Show(
                        "Synchronization incomplete, failed update of some objects. Refresh comparison to find out differences.",
                        "Db Object Creation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Update of objects successful!", "Db Object Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion

        #region Auxiliar functions
        /// <summary>
        /// Based on the type object(table, view, procedure,...) returns the icon that should be shown on the grid
        /// </summary>
        /// <param name="type">Enum variable to identify the type of object</param>
        /// <returns></returns>
        private Image GetIconFor(ObjectType type)
        {
            switch (type)
            {
                case ObjectType.Table:
                    return PopIList.Images[0];
                case ObjectType.View:
                    return PopIList.Images[1];
                case ObjectType.Procedure:
                    return PopIList.Images[2];
                case ObjectType.ScalarFunction:
                    return PopIList.Images[3];
                case ObjectType.TableFunction:
                    return PopIList.Images[4];
                default:
                    return null;
            }
        }

        /// <summary>
        /// Initialize the BreadCrumb Controls(control used to select db connection)
        /// </summary>
        private void LoadConnectionsInfo()
        {
            AdBarSource.InitializeRoot(new RootConxNode("Start"));
            AdBarDestination.InitializeRoot(new RootConxNode("Start"));
        }

        /// <summary>
        /// Auxiliar function that does a comparison for the specified object type between the source and destination
        /// </summary>
        /// <param name="type">Enum variable to identify the type of object</param>
        private void FindDifferencesBetweenObjects(ObjectType type)
        {
            Dictionary<string, ISqlObject> sourceObjs, destinationObjs;
            
            //Convert source/destination objects to dictionary for the comparison
            sourceObjs = sourceConx.DbObjects
                .Where(x => x.Kind == type)
                .Select(y => y)
                .ToDictionary(key => key.Schema + "." + key.Name, value => value);
            destinationObjs = destinationConx.DbObjects
                .Where(x => x.Kind == type)
                .Select(y => y)
                .ToDictionary(key => key.Schema + "." + key.Name, value => value);

            foreach (KeyValuePair<string, ISqlObject> ts in sourceObjs)
            {
                ts.Value.LoadScript(new SqlCommand("", sourceConx.Connection));
                if (destinationObjs.ContainsKey(ts.Key))
                {
                    destinationObjs[ts.Key].LoadScript(new SqlCommand("", destinationConx.Connection));
                    
                    //Seems like sometimes indentation(tabs) are saved as 4 spaces and sometimes as tabs(\t)
                    //to avoid this causing false difference detection, both scripts have their tabs replaced 
                    //by 4 spaces, not the "best" solution but is effective with the default configuration
                    string buffSource, buffDestiny;
                    buffSource = ts.Value.Script.Replace("\t", "    ");
                    buffDestiny = destinationObjs[ts.Key].Script.Replace("\t", "    ");

                    if (String.Compare(buffSource, buffDestiny, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        differencesFound.Add(new DifferenceModel()
                        {
                            Name = ts.Key,
                            SourceScript = buffSource,
                            DestinationScript = buffDestiny,
                            DiffType = DifferenceType.None,
                            ObjectKind = type
                        });
                    }
                    else
                    {
                        differencesFound.Add(new DifferenceModel()
                        {
                            Name = ts.Key,
                            SourceScript = ts.Value.Script,
                            DestinationScript = destinationObjs[ts.Key].Script,
                            DiffType = DifferenceType.Update,
                            ObjectKind = type
                        });
                    }

                }
                else
                {
                    differencesFound.Add(new DifferenceModel()
                    {
                        Name = ts.Key,
                        SourceScript = ts.Value.Script,
                        DestinationScript = "",
                        DiffType = DifferenceType.Add,
                        ObjectKind = type
                    });
                }
            }

            foreach (KeyValuePair<string, ISqlObject> td in destinationObjs)
            {
                if (!sourceObjs.ContainsKey(td.Key))
                {
                    td.Value.LoadScript(new SqlCommand("", destinationConx.Connection));
                    differencesFound.Add(new DifferenceModel()
                    {
                        Name = td.Key,
                        SourceScript = "",
                        DestinationScript = td.Value.Script,
                        DiffType = DifferenceType.Delete,
                        ObjectKind = type
                    });
                }
            }
        }

        /// <summary>
        /// Load the results of the comparison for the specified difference type
        /// </summary>
        /// <param name="diffType">Enum variable to identify the type of difference(None, Add, Update, Remove)</param>
        private void LoadGrid(DifferenceType diffType)
        {
            int count = 0;
            DataGridView grid;
            TabPage tab;
            switch (diffType)
            {
                case DifferenceType.Add:
                    grid = gridAdd;
                    tab = tabAdd;
                    break;
                case DifferenceType.Update:
                    grid = gridUpdate;
                    tab = tabUpdate;
                    break;
                case DifferenceType.Delete:
                    grid = gridDelete;
                    tab = tabRemove;
                    break;
                case DifferenceType.None:
                    grid = gridNone;
                    tab = tabNone;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("diffType");
            }


            grid.Rows.Clear();
            grid.Columns.Clear();

            DataGridViewColumn chkColumn = new CustomGridViewCheckBoxColumn();
            chkColumn.MinimumWidth = 26;
            grid.Columns.Add(chkColumn);

            DataGridViewColumn iconColumn = new DataGridViewImageColumn();
            iconColumn.MinimumWidth = 26;
            grid.Columns.Add(iconColumn);

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.HeaderText = "Object Name";
            grid.Columns.Add(nameColumn);

            foreach (DifferenceModel dm in differencesFound.Where(x => x.DiffType == diffType).OrderBy(y => y.ObjectKind).ThenBy(z => z.Name))
            {
                grid.Rows.Add(false, GetIconFor(dm.ObjectKind), dm.Name);
                count++;
            }

            tab.Text = String.Format("{0}({1})", tab.Text.IndexOf('(') >= 0 ? tab.Text.Substring(0, tab.Text.IndexOf('(')) : tab.Text, count);
        }
        #endregion 
    }
}
