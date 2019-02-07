using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using Ez_SQL.Common_Code;

namespace Ez_SQL.AdditionalForms
{
    public partial class ObjectSearcher : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private ISqlObject SelectedObject;
        private SqlConnector DataProvider;
        private MainForm Parent;
        private List<ISqlObject> FilteredObjs; 
        public ObjectSearcher(MainForm Parent, SqlConnector DataProvider)
        {
            InitializeComponent();
            this.DataProvider = DataProvider;
            this.Parent = Parent;
            CmbSearchType.SelectedIndex = 0;
            SelectedObject = null;
        }

        private void TxtFilter_TextWaitEnded(string Text, int Decimals)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            SelectedObject = null;
            string filter = TxtFilter.Text;
            if (DataProvider.DbObjects == null || DataProvider.DbObjects.Count == 0)
                return;
            switch (CmbSearchType.SelectedIndex)
            {
                case 0://Any database object
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind != ObjectType.Schema && X.Kind != ObjectType.Alias).ToList();
                    break;
                case 1://Tables
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.Table).ToList();
                    break;
                case 2://Views
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.View).ToList();
                    break;
                case 3://Stored procedures
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.Procedure).ToList();
                    break;
                case 4://Scalar functions
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.ScalarFunction).ToList();
                    break;
                case 5://Table functions
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.TableFunction).ToList();
                    break;
                case 6://Search in code(Views, Procedures, Functions)
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.View || X.Kind == ObjectType.Procedure || X.Kind == ObjectType.TableFunction || X.Kind == ObjectType.ScalarFunction).ToList();
                    break;
                case 7://Search for field(Tables, Views, Table Functions)
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.View || X.Kind == ObjectType.Table || X.Kind == ObjectType.TableFunction).ToList();
                    break;
                case 8://Search for parameter(Procedures, Functions)
                    FilteredObjs = DataProvider.DbObjects.Where(X => X.Kind == ObjectType.Procedure || X.Kind == ObjectType.TableFunction || X.Kind == ObjectType.ScalarFunction).ToList();
                    break;
            }
            CleanSearchResults();
            if (FilteredObjs == null || FilteredObjs.Count == 0)
            {
                return;
            }

            if (StartsWith.Checked)
            {
                if(CmbSearchType.SelectedIndex == 6)//search in code
                {//in this mode it is always contains, start with makes no sense
                    FilteredObjs = FilteredObjs.Where(X => X.Script.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else if (CmbSearchType.SelectedIndex == 7)//search for fields
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Childs.Any(Z => Z.Kind == ChildType.Field && Z.Name.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))).ToList();
                }
                else if (CmbSearchType.SelectedIndex == 8)//search for parameter
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Childs.Any(Z => Z.Kind == ChildType.Parameter && Z.Name.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))).ToList();
                }
                else
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Name.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
            }
            else
            {
                if (CmbSearchType.SelectedIndex == 6)//search in code
                {//in this mode it is always contains, start with makes no sense
                    FilteredObjs = FilteredObjs.Where(X => X.Script.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else if (CmbSearchType.SelectedIndex == 7)//search for fields
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Childs.Any(Z => Z.Kind == ChildType.Field && Z.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase))).ToList();
                }
                else if (CmbSearchType.SelectedIndex == 8)//search for parameter
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Childs.Any(Z => Z.Kind == ChildType.Parameter && Z.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase))).ToList();
                }
                else
                {
                    FilteredObjs = FilteredObjs.Where(X => X.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
            }

            ResultsList.Items.Clear();
            ResultsList.Columns[0].Width = ResultsList.Width - 10;

            if (FilteredObjs == null || FilteredObjs.Count == 0)
                return;
            //Filtered.Sort((obj1, obj2) => obj1.Name.CompareTo(obj2.Name));
            FilteredObjs = FilteredObjs.Sort(X => X.Name, SortDirection.Ascending).ToList();
            foreach (ISqlObject dbobj in FilteredObjs)
            {
                ResultsList.Items.Add(new ListViewItem(dbobj.Name, dbobj.ImageIndex) { ToolTipText = String.Format("{0}.{1}", dbobj.Schema, dbobj.Name) });
            }
            label3.Text = String.Format("Objects found: {0}", ResultsList.Items.Count.ToString());
        }
        private void CleanSearchResults()
        {
            ResultsList.Items.Clear();
        }
        private void CmbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        private void ResultsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Tab;
            ISqlObject Obj;
            if (ResultsList.SelectedIndices.Count > 0)
            {
                Obj = FilteredObjs[ResultsList.SelectedIndices[0]];
                SelectedObject = Obj;
                Tab = new DataTable("AuxTable");
                Tab.Columns.Add(new DataColumn("Type"));
                Tab.Columns.Add(new DataColumn("Name"));
                Tab.Columns.Add(new DataColumn("Datatype"));
                Tab.Columns.Add(new DataColumn("Length"));
                Tab.Columns.Add(new DataColumn("Nullable"));
                Tab.Columns.Add(new DataColumn("PKey"));
                Tab.Columns.Add(new DataColumn("FKey"));
                Tab.Columns.Add(new DataColumn("Computed"));
                Tab.Columns.Add(new DataColumn("Formula"));
                Tab.Columns.Add(new DataColumn("Default"));

                foreach (ISqlChild Ch in Obj.Childs.Sort(X => X.Kind, SortDirection.Ascending))
                {
                    DataRow NR = Tab.NewRow();
                    NR["Type"] = Ch.Kind.ToString();
                    NR["Name"] = Ch.Name;
                    NR["Datatype"] = Ch.Type;
                    NR["Length"] = Ch.Precision.ToString();
                    NR["Nullable"] = Ch.Nullable ? "Yes" : "No";
                    NR["PKey"] = Ch.IsPrimaryKey ? "Yes" : "No";
                    NR["FKey"] = Ch.IsForeignKey ? "Yes" : "No";
                    if (Ch.Computed)
                    {
                        NR["Computed"] = "Yes";
                        NR["Formula"] = Ch.DefaultValue;
                        NR["Default"] = "";
                    }
                    else
                    {
                        NR["Computed"] = "No";
                        NR["Formula"] = "";
                        NR["Default"] = Ch.DefaultValue;
                    }
                    Tab.Rows.Add(NR);
                }
                GridFields.DataSource = Tab;
            }
            else
            {
                SelectedObject = null;
                GridFields.DataSource = null;
            }
        }
        private void ResultsList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' && ResultsList.SelectedIndices.Count > 0)
            {
                ISqlObject obj = FilteredObjs[ResultsList.SelectedIndices[0]];
                Parent.AddQueryForm(obj.Name, obj.Script, DataProvider);
            }
        }
        private void ResultsList_DoubleClick(object sender, EventArgs e)
        {
            if (ResultsList.SelectedIndices.Count > 0)
            {
                ISqlObject obj = FilteredObjs[ResultsList.SelectedIndices[0]];
                Parent.AddQueryForm(obj.Name, obj.Script, DataProvider);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Parent.CloseAllTabs();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Parent.CloseAllTabsButMe(this);
        }

        private void ResultsList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (ResultsList.SelectedIndices != null && ResultsList.SelectedIndices.Count > 0)
                {
                    ISqlObject Obj = FilteredObjs[ResultsList.SelectedIndices[0]];
                    if (Obj.Kind == ObjectType.Table)
                    {
                        tableContextMenu.Show(ResultsList.PointToScreen(e.Location));
                    }
                }
            }
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script = "SELECT TOP 1000 * FROM ";
            if (SelectedObject != null)
            {
                script += SelectedObject.Name;
                Parent.AddQueryForm(SelectedObject.Name, script, DataProvider, true);
            }
        }

        private void insertSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script;
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;
                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Add.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPAddGenerator sp = new SPAddGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Insert", sp.ToString(), DataProvider, false);
            }
        }

        private void updateSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script;
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;
                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Update.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPUpdateGenerator sp = new SPUpdateGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Update", sp.ToString(), DataProvider, false);
            }
        }

        private void deleteSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script;
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;
                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Delete.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPDeleteGenerator sp = new SPDeleteGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Delete", sp.ToString(), DataProvider, false);
            }
        }

        private void selectSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script;
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;
                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Get.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPGetGenerator sp = new SPGetGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Get", sp.ToString(), DataProvider, false);
            }
        }

        private void allOperationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script;
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;

                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Add.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPAddGenerator spInsert = new SPAddGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Insert", spInsert.ToString(), DataProvider, false);

                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Update.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPUpdateGenerator spUpdate = new SPUpdateGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Update", spUpdate.ToString(), DataProvider, false);

                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Delete.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPDeleteGenerator spDelete = new SPDeleteGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Delete", spDelete.ToString(), DataProvider, false);

                using (StreamReader rdr = new StreamReader(MainForm.DataStorageDir + "\\Templates\\SP_Get.sql"))
                {
                    script = rdr.ReadToEnd();
                }
                SPGetGenerator spSelect = new SPGetGenerator(script, t);
                Parent.AddQueryForm(SelectedObject.Name + "_Get", spSelect.ToString(), DataProvider, false);
            }
        }

        private void exToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;

                
                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.xlsx";
                svDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
                svDlg.Title = "Export Table to Excel File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + t.Name);
                    DataExporter dex = new DataExporter();
                    dex.Author = "EZ Sql";
                    dex.AutoCellAdjust = WidthAdjust.ByFirst10Rows;
                    dex.Company = "El Cid";
                    dex.ExportExcelStyle = ExcelStyle.Simple;
                    dex.ExportType = ExportTo.XLSX;
                    dex.ExportWithStyles = true;
                    dex.Subject = "Data on " + t.Schema + "." + t.Name;
                    dex.Title = "DB Table Export";
                    dex.UseAlternateRowStyles = true;
                    dex.WriteHeaders = true;

                    dex.ExportToFile(svDlg.FileName, buffer);

                    System.Diagnostics.Process.Start(svDlg.FileName);

                }
            }
        }

        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;


                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.csv";
                svDlg.Filter = "CSV files (*.csv)|*.csv";
                svDlg.Title = "Export Table to CSV File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + t.Name);
                    buffer.SaveToCsv(svDlg.FileName);
                    System.Diagnostics.Process.Start(svDlg.FileName);
                }
            }
        }

        private void txtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                Ez_SQL.DataBaseObjects.Table t = (Ez_SQL.DataBaseObjects.Table)SelectedObject;


                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.xlsx";
                svDlg.Filter = "Txt files (*.txt)|*.txt";
                svDlg.Title = "Export Table to Text File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + t.Name);
                    DataExporter dex = new DataExporter();
                    dex.ExportType = ExportTo.TXT;
                    dex.Separator = "|";
                    dex.WriteHeaders = true;

                    dex.ExportToFile(svDlg.FileName, buffer);

                    System.Diagnostics.Process.Start(svDlg.FileName);

                }
            }
        }

    }
}
