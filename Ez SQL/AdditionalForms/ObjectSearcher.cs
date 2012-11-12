using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using Ez_SQL.Extensions;

namespace Ez_SQL.AdditionalForms
{
    public partial class ObjectSearcher : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private SqlConnector DataProvider;
        private MainForm Parent;
        private List<ISqlObject> FilteredObjs; 
        public ObjectSearcher(MainForm Parent, SqlConnector DataProvider)
        {
            InitializeComponent();
            this.DataProvider = DataProvider;
            this.Parent = Parent;
            CmbSearchType.SelectedIndex = 0;
        }

        private void TxtFilter_TextWaitEnded(string Text, int Decimals)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
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
    }
}
