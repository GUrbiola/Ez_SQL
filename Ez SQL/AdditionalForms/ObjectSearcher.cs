using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using Ez_SQL.Common_Code;
using Ez_SQL.MultiQueryForm;
using Ez_SQL.MultiQueryForm.Dialogs;
using View = System.Windows.Forms.View;

namespace Ez_SQL.AdditionalForms
{
    public partial class ObjectSearcher : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private ISqlObject SelectedObject;
        private SqlConnector DataProvider;
        private QueryExecutor Executor;
        private MainForm Parent;
        private List<ISqlObject> FilteredObjs;

        public ObjectSearcher(MainForm Parent, SqlConnector DataProvider)
        {
            InitializeComponent();
            this.DataProvider = DataProvider;
            this.Parent = Parent;
            CmbSearchType.SelectedIndex = 0;
            SelectedObject = null;

            Executor = new QueryExecutor();
            if (DataProvider != null && !String.IsNullOrEmpty(DataProvider.ConnectionString))
                Executor.Initialize(DataProvider.ConnectionString);

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
                        cRUDToolStripMenuItem.Visible = true;
                        tableContextMenu.Show(ResultsList.PointToScreen(e.Location));
                    }
                    else if (Obj.Kind == ObjectType.View)
                    {
                        cRUDToolStripMenuItem.Visible = false;
                        tableContextMenu.Show(ResultsList.PointToScreen(e.Location));
                    }
                    else if (Obj.Kind == ObjectType.Procedure)
                    {
                        spContextMenu.Show(ResultsList.PointToScreen(e.Location));
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
                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.xlsx";
                svDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
                svDlg.Title = "Export Table to Excel File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + SelectedObject.Name);
                    DataExporter dex = new DataExporter();
                    dex.Author = "EZ Sql";
                    dex.AutoCellAdjust = WidthAdjust.ByFirst10Rows;
                    dex.Company = "El Cid";
                    dex.ExportExcelStyle = ExcelStyle.Simple;
                    dex.ExportType = ExportTo.XLSX;
                    dex.ExportWithStyles = true;
                    dex.Subject = "Data on " + SelectedObject.Schema + "." + SelectedObject.Name;
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
                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.csv";
                svDlg.Filter = "CSV files (*.csv)|*.csv";
                svDlg.Title = "Export Table to CSV File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + SelectedObject.Name);
                    buffer.SaveToCsv(svDlg.FileName);
                    System.Diagnostics.Process.Start(svDlg.FileName);
                }
            }
        }

        private void txtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                SaveFileDialog svDlg = new SaveFileDialog();
                svDlg.AddExtension = true;
                svDlg.CheckFileExists = false;
                svDlg.CheckPathExists = true;
                svDlg.DefaultExt = "*.xlsx";
                svDlg.Filter = "Txt files (*.txt)|*.txt";
                svDlg.Title = "Export Table to Text File";

                if (svDlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable buffer = DataProvider.ExecuteTable("Select top 50000 * from " + SelectedObject.Name);
                    DataExporter dex = new DataExporter();
                    dex.ExportType = ExportTo.TXT;
                    dex.Separator = "|";
                    dex.WriteHeaders = true;

                    dex.ExportToFile(svDlg.FileName, buffer);

                    System.Diagnostics.Process.Start(svDlg.FileName);

                }
            }
        }

        private void generateCClassForTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                GenerateClass curTableGenerationDialog;
                curTableGenerationDialog = (SelectedObject as Table) == null ? new GenerateClass(SelectedObject as DataBaseObjects.View) : new GenerateClass(SelectedObject as Table);

                if (curTableGenerationDialog.ShowDialog() == DialogResult.OK)
                {
                    Parent.AddCSharpCodeForm(SelectedObject.Name, curTableGenerationDialog.GenerateCSharpCode);
                }
            }
        }

        private void generateMethodForStoreProcedureExecutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                string csharpCodeResult;
                Procedure proc = SelectedObject as Procedure;
                List<Field> returnColumns = GetReturnColumnsFromProcedure(proc);

                if (returnColumns != null && returnColumns.Count > 0)
                {//Query
                    csharpCodeResult = GetCSharpForQueryMethod(proc, returnColumns);
                    if (!String.IsNullOrEmpty(csharpCodeResult))
                        Parent.AddCSharpCodeForm(proc.Name, csharpCodeResult);
                }
                else
                {//NonQuery
                    csharpCodeResult = GetCSharpForNonQueryMethod(proc);
                    if (!String.IsNullOrEmpty(csharpCodeResult))
                        Parent.AddCSharpCodeForm(proc.Name, csharpCodeResult);
                }
            }
        }

        private List<Field> GetReturnColumnsFromProcedure(Procedure proc)
        {
            List<Field> Back = new List<Field>();

            if (proc == null || String.IsNullOrEmpty(proc.Schema) || String.IsNullOrEmpty(proc.Name))
            {
                return Back;
            }

            using (SqlCommand command = new SqlCommand(String.Format("{0}.{1}", proc.Schema, proc.Name), Executor.Connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    command.Connection.Open();
                    SqlCommandBuilder.DeriveParameters(command);
                    SqlDataAdapter Daa = new SqlDataAdapter();
                    Daa.SelectCommand = command;
                    DataSet ds = new DataSet("buff");
                    Daa.GetFillParameters();
                    Daa.FillSchema(ds, SchemaType.Source, "buff");
                    foreach (DataTable tab in ds.Tables)
                    {
                        foreach (DataColumn col in tab.Columns)
                        {
                            if (MainForm.SqlVsCSharp.ContainsKey(col.DataType.ToString()))
                            {
                                Field aux = new Field()
                                {
                                    Name = col.Caption,
                                    Type = col.DataType.ToString(),
                                    CSharpType = MainForm.SqlVsCSharp[col.DataType.ToString()]
                                };

                                if (!Back.Exists(x => x.Name.Equals(col.Caption, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    Back.Add(aux);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    command.Connection.Close();
                    MessageBox.Show(ex.Message, "Exception thrown when trying to pull the return data of the Stored Procedure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                command.Connection.Close();
            }
            return Back;
        }

        private string GetCSharpForNonQueryMethod(Procedure proc)
        {
            string buffer;
            StringBuilder method = new StringBuilder();
            //gets settings to create method
            NonQuerySp settingsDialog = new NonQuerySp();
            if (settingsDialog.ShowDialog() != DialogResult.OK)
                return "";
            GenerateNonQuerySpModelSettings settings = settingsDialog.CurrentSettings;

            //write the "region start"
            if (settings.InsideRegion)
                method.AppendLine(String.Format("#region Method to execute the SP: {0}.{1}", proc.Schema, proc.Name).Indent(2));

            #region write the first line of real code, the signature of the method
            bool parameterless = true;

            if (settings.ReturnSPR)
                buffer = String.Format("public SP_Result {0}(", proc.Name);
            else
                buffer = String.Format("public void {0}(", proc.Name);

            foreach (ISqlChild parameter in proc.Childs.Where(x => x.Kind == ChildType.Parameter))
            {
                parameterless = false;
                if (MainForm.SqlVsCSharp.ContainsKey(parameter.Type))
                    buffer += String.Format("{0} {1}, ", MainForm.SqlVsCSharp[parameter.Type], parameter.Name.Trim('@'));
                else
                    buffer += String.Format("??? {0}, ", parameter.Name.Trim('@'));
            }

            if (parameterless)
            {
                method.AppendLine(Extensions.Indent(buffer, 2) + ")");
            }
            else
            {
                method.AppendLine(Extensions.Indent(buffer.Substring(0, buffer.Length - 2), 2) + ")");
            }
            #endregion

            method.AppendLine("{".Indent(2)); //opening bracket of the whole method

            if (settings.LogStart) //check if was required log the start of the method
                method.AppendLine(String.Format("log.Debug(\"Start of Method that Executes the SP: {0}.{1}\");", proc.Schema, proc.Name).Indent(3));

            #region creation of transaction object
            if (settings.UseTransaction)
                method.AppendLine("SqlTransaction transaction = null;".Indent(3));
            #endregion

            if (settings.ReturnSPR)
            {
                method.AppendLine("SP_Response back = new SP_Response();".Indent(3));
                method.AppendLine();
            }

            #region creation of the command object
            method.AppendLine(String.Format("SqlCommand sqlCommand = new SqlCommand(\"{0}.{1}\", this.Connection);", proc.Schema, proc.Name).Indent(3));
            method.AppendLine("sqlCommand.CommandType = CommandType.StoredProcedure;".Indent(3));
            method.AppendLine();
            #endregion

            #region assignation of the parameters
            foreach (ISqlChild parameter in proc.Childs.Where(x => x.Kind == ChildType.Parameter))
            {
                if (MainForm.SqlVsCSharpDb.ContainsKey(parameter.Type))
                    method.AppendLine(String.Format("sqlCommand.Parameters.Add(new SqlParameter(\"{0}\", {1}, {2}));", parameter.Name, MainForm.SqlVsCSharpDb[parameter.Type], parameter.Precision).Indent(3));
                else
                    method.AppendLine(String.Format("sqlCommand.Parameters.Add(new SqlParameter(\"{0}\", {1}, {2}));", parameter.Name, "??", parameter.Precision).Indent(3));
                method.AppendLine(String.Format("sqlCommand.Parameters[\"{0}\"].Value = {1};", parameter.Name, parameter.Name.Trim('@')).Indent(3));
            }
            #endregion

            method.AppendLine("try".Indent(3));
            method.AppendLine("{".Indent(3)); //start of try

            //If the time elapsed must be measured
            if (settings.MeasureTimeElapsed)
                method.AppendLine("this.Executing = true;".Indent(4));

            //Open the connection to the DB
            method.AppendLine("sqlCommand.Connection.Open();".Indent(4));

            //Create the transaction and assign it to the command
            if (settings.UseTransaction)
            {
                method.AppendLine(String.Format("transaction = this.Connection.BeginTransaction(\"Trans_{0}\");", proc.Name).Indent(4));
                method.AppendLine("sqlCommand.Transaction = transaction;".Indent(4));
            }

            method.AppendLine(settings.SaveRowsAffectedCount ? "RowsAffected = sqlCommand.ExecuteNonQuery();".Indent(4) : "sqlCommand.ExecuteNonQuery();".Indent(4));

            if (settings.ReturnSPR)
            {
                method.AppendLine("back.Result = 1;".Indent(4));
                method.AppendLine("back.Message = \"Success\";".Indent(4));
            }

            if (settings.UseTransaction)
                method.AppendLine("transaction.Commit();".Indent(4));

            method.AppendLine("LastMessage = \"OK\";".Indent(4));
            method.AppendLine("}".Indent(3)); //end of try

            #region SQL Exception Handling
            method.AppendLine("catch (SqlException sqlex)".Indent(3));
            method.AppendLine("{".Indent(3)); //start of Sql Exception Catch
            method.AppendLine("LastMessage = sqlex.Errors[0].Message;".Indent(4));
            method.AppendLine("LastSqlException = sqlex;".Indent(4));

            if (settings.LogException)
                method.AppendLine(String.Format("log.Error(\"SqlException on Execution of SP: {0}.{1}\", sqlex.GetBaseException());", proc.Schema, proc.Name).Indent(4));

            if (settings.UseTransaction)
            {
                method.AppendLine("if(transaction != null)".Indent(4));
                method.AppendLine("transaction.Rollback();".Indent(5));
            }

            if (settings.ReturnSPR)
            {
                method.AppendLine("back.Result = -1;".Indent(4));
                method.AppendLine("back.Message = sqlex.Errors[0].Message;".Indent(4));
            }
            else
            {
                method.AppendLine("throw;".Indent(4));
            }

            method.AppendLine("}".Indent(3)); //end of Sql Exception Catch
            #endregion

            #region Generic Exception Handling
            method.AppendLine("catch (Exception ex)".Indent(3));
            method.AppendLine("{".Indent(3)); //start of Exception Catch
            method.AppendLine("LastMessage = ex.Message;".Indent(4));
            method.AppendLine("LastException = ex;".Indent(4));

            if (settings.LogException)
                method.AppendLine(String.Format("log.Error(\"Exception on Execution of SP: {0}.{1}\", ex);", proc.Schema, proc.Name).Indent(4));

            if (settings.UseTransaction)
            {
                method.AppendLine("if(transaction != null)".Indent(4));
                method.AppendLine("transaction.Rollback();".Indent(5));
            }


            if (settings.ReturnSPR)
            {
                method.AppendLine("back.Result = -2;".Indent(4));
                method.AppendLine("back.Message = ex.Message;".Indent(4));
            }
            else
            {
                method.AppendLine("throw;".Indent(4));
            }
            method.AppendLine("}".Indent(3)); //end of Exception Catch
            #endregion

            #region block "finally", of the try-catch
            method.AppendLine("finally".Indent(3));
            method.AppendLine("{".Indent(3));
            method.AppendLine("if (sqlCommand.Connection.State != ConnectionState.Closed)".Indent(4));
            method.AppendLine("sqlCommand.Connection.Close();".Indent(5));
            method.AppendLine("sqlCommand.Dispose();".Indent(4));

            //If the time elapsed must be measured
            if (settings.MeasureTimeElapsed)
                method.AppendLine("this.Executing = false;".Indent(4));

            if (settings.LogEnd) //check if was required log the start of the method
                method.AppendLine(String.Format("log.Debug(\"End of Method that Executes the SP: {0}.{1}\");", proc.Schema, proc.Name).Indent(4));

            method.AppendLine("}".Indent(3));
            #endregion

            method.AppendLine("}".Indent(2)); //closing bracket of the whole method

            if (settings.InsideRegion)
                method.AppendLine("#endregion".Indent(2)); //closing the region

            return method.ToString();
        }

        private string GetCSharpForQueryMethod(Procedure proc, List<Field> returnColumns)
        {
            string buffer;
            StringBuilder method = new StringBuilder();
            //gets settings to create method
            QuerySp settingsDialog = new QuerySp();
            if (settingsDialog.ShowDialog() != DialogResult.OK)
                return "";
            GenerateQuerySpModelSettings settings = settingsDialog.CurrentSettings;

            //write the "region start"
            if (settings.InsideRegion)
                method.AppendLine(String.Format("#region Method to execute the SP: {0}.{1}", proc.Schema, proc.Name).Indent(2));

            #region write the first line of real code, the signature of the method
            bool parameterless = true;
            buffer = "public ";
            if (settings.IsSPR)
            {
                buffer += "SPR_Collection<";
            }
            else if (settings.IsList)
            {
                buffer += "List<";
            }
            buffer += settings.ReturnName;
            if (settings.IsList || settings.IsSPR)
                buffer += ">";

            buffer += String.Format(" {0}(", proc.Name);
            foreach (ISqlChild parameter in proc.Childs.Where(x => x.Kind == ChildType.Parameter))
            {
                parameterless = false;
                if (MainForm.SqlVsCSharp.ContainsKey(parameter.Type))
                    buffer += String.Format("{0} {1}, ", MainForm.SqlVsCSharp[parameter.Type], parameter.Name.Trim('@'));
                else
                    buffer += String.Format("??? {0}, ", parameter.Name.Trim('@'));
            }

            if (parameterless)
            {
                method.AppendLine(Extensions.Indent(buffer, 2) + ")");
            }
            else
            {
                method.AppendLine(Extensions.Indent(buffer.Substring(0, buffer.Length - 2), 2) + ")");
            }

            #endregion

            method.AppendLine("{".Indent(2)); //opening bracket of the whole method

            if (settings.LogStart) //check if was required log the start of the method
                method.AppendLine(String.Format("log.Debug(\"Start of Method that Executes the SP: {0}.{1}\");", proc.Schema, proc.Name).Indent(3));

            //declaration of the object/primitive that will be returned
            if (settings.IsList)
            {
                if (settings.IsObject)
                    method.AppendLine(String.Format("List<{0}> back = new List<{0}>();", settings.ReturnName).Indent(3));
                else
                    method.AppendLine(String.Format("List<{0}> back = List<{0}>();", settings.ReturnName).Indent(3));
            }
            else if (settings.IsSPR)
            {
                if (settings.IsObject)
                    method.AppendLine(String.Format("SPR_Collection<{0}> back = new SPR_Collection<{0}>();", settings.ReturnName).Indent(3));
                else
                    method.AppendLine(String.Format("SPR_Collection<{0}> back = SPR_Collection<{0}>();", settings.ReturnName).Indent(3));
            }
            else
            {
                if (settings.IsObject)
                    method.AppendLine(String.Format("{0} back = new {0}();", settings.ReturnName).Indent(3));
                else
                    method.AppendLine(String.Format("{0} back = {1};", settings.ReturnName, PrimitiveInitialization(settings.ReturnName)).Indent(3));
            }



            #region creation of transaction object
            if (settings.UseTransaction)
                method.AppendLine("SqlTransaction transaction = null;".Indent(3));
            #endregion
            method.AppendLine();

            #region creation of the command object
            method.AppendLine(String.Format("SqlCommand sqlCommand = new SqlCommand(\"{0}.{1}\", this.Connection);", proc.Schema, proc.Name).Indent(3));
            method.AppendLine("sqlCommand.CommandType = CommandType.StoredProcedure;".Indent(3));
            method.AppendLine();
            #endregion

            #region assignation of the parameters
            foreach (ISqlChild parameter in proc.Childs.Where(x => x.Kind == ChildType.Parameter))
            {
                if (MainForm.SqlVsCSharpDb.ContainsKey(parameter.Type))
                    method.AppendLine(String.Format("sqlCommand.Parameters.Add(new SqlParameter(\"{0}\", {1}, {2}));", parameter.Name, MainForm.SqlVsCSharpDb[parameter.Type], parameter.Precision).Indent(3));
                else
                    method.AppendLine(String.Format("sqlCommand.Parameters.Add(new SqlParameter(\"{0}\", {1}, {2}));", parameter.Name, "??", parameter.Precision).Indent(3));
                method.AppendLine(String.Format("sqlCommand.Parameters[\"{0}\"].Value = {1};", parameter.Name, parameter.Name.Trim('@')).Indent(3));
            }
            #endregion

            method.AppendLine("try".Indent(3));
            method.AppendLine("{".Indent(3)); //start of try

            //If the time elapsed must be measured
            if (settings.MeasureTimeElapsed)
                method.AppendLine("this.Executing = true;".Indent(4));

            //Open the connection to the DB
            method.AppendLine("sqlCommand.Connection.Open();".Indent(4));

            //Create the transaction and assign it to the command
            if (settings.UseTransaction)
            {
                method.AppendLine(String.Format("transaction = this.Connection.BeginTransaction(\"Trans_{0}\");", proc.Name).Indent(4));
                method.AppendLine("sqlCommand.Transaction = transaction;".Indent(4));
            }

            if (settings.IsList)
            {
                #region Code to Handle Lists
                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead = 0;".Indent(4));

                method.AppendLine("using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())".Indent(4));
                method.AppendLine("{".Indent(4)); //start of using statement
                method.AppendLine("while (sqlDataReader.Read())".Indent(5));
                method.AppendLine("{".Indent(5));

                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead++;".Indent(6));

                if (settings.IsObject)
                {
                    #region Map results to an object and add it to the list
                    method.AppendLine(String.Format("{0} obj = new {0}();", settings.ReturnName).Indent(6));
                    foreach (Field f in returnColumns)
                    {
                        if (f.CSharpType.Equals("string"))
                        {
                            method.AppendLine(String.Format("obj.{0} = sqlDataReader[\"{0}\"].ToString();", f.Name).Indent(6));
                        }
                        else if (f.CSharpType.Equals("bool"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToBoolean(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("obj.{0} = false;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("int"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToInt32(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("decimal"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDecimal(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("double") || f.CSharpType.Equals("float"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDouble(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("DateTime"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDateTime(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("obj.{0} = new DateTime(1900, 1, 1);", f.Name).Indent(7));
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Map first column of the results to a primitive and add it to the list

                    method.AppendLine(
                        String.Format("{0} obj = {1};", settings.ReturnName, PrimitiveInitialization(settings.ReturnName))
                              .Indent(6));
                    Field primitiveField = returnColumns[0];

                    if (primitiveField.CSharpType.Equals("string"))
                    {
                        method.AppendLine("obj = sqlDataReader[0].ToString();".Indent(6));
                    }
                    else if (primitiveField.CSharpType.Equals("bool"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("obj = Convert.ToBoolean(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("obj = false;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("int"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("obj = Convert.ToInt32(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("obj = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("decimal"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("obj = Convert.ToDecimal(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("obj = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("double") || primitiveField.CSharpType.Equals("float"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("obj = Convert.ToDouble(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("obj = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("DateTime"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("obj = Convert.ToDateTime(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("obj = new DateTime(1900, 1, 1);".Indent(7));
                    }

                    #endregion
                }
                method.AppendLine();
                method.AppendLine("back.Add(obj);".Indent(6));
                method.AppendLine("}".Indent(5));

                method.AppendLine("LastMessage = \"OK\";".Indent(5));
                #endregion
            }
            else if (settings.IsSPR)
            {
                #region Code to Handle SPR_Collection
                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead = 0;".Indent(4));

                method.AppendLine("using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())".Indent(4));
                method.AppendLine("{".Indent(4)); //start of using statement

                method.AppendLine("bool hasColumn = false;".Indent(5));
                method.AppendLine("for (int i = 0; i < sqlDataReader.FieldCount; i++)".Indent(5));
                method.AppendLine("{".Indent(5));
                method.AppendLine("if (sqlDataReader.GetName(i).Equals(\"Message\", StringComparison.InvariantCultureIgnoreCase))".Indent(6));
                method.AppendLine("hasColumn = true;".Indent(6));
                method.AppendLine("}".Indent(5));

                method.AppendLine("if(hasColumn)".Indent(5));
                method.AppendLine("{".Indent(5));
                method.AppendLine("if (!String.IsNullOrEmpty(sqlDataReader[\"Result\"].ToString()))".Indent(6));
                method.AppendLine("back.Result = Convert.ToInt32(sqlDataReader[\"Result\"].ToString());".Indent(7));
                method.AppendLine("else".Indent(6));
                method.AppendLine("back.Result = 0;".Indent(7));
                method.AppendLine("back.Message = sqlDataReader[\"Message\"].ToString();".Indent(6));
                method.AppendLine("}".Indent(5));


                method.AppendLine("else".Indent(5));
                method.AppendLine("{".Indent(5));
                method.AppendLine("while (sqlDataReader.Read())".Indent(6));
                method.AppendLine("{".Indent(6));

                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead++;".Indent(7));

                if (settings.IsObject)
                {
                    #region Map results to an object and add it to the list
                    method.AppendLine(String.Format("{0} obj = new {0}();", settings.ReturnName).Indent(7));
                    foreach (Field f in returnColumns)
                    {
                        if (f.Name.Equals("Result", StringComparison.OrdinalIgnoreCase) || f.Name.Equals("Message", StringComparison.OrdinalIgnoreCase))
                            continue;

                        if (f.CSharpType.Equals("string"))
                        {
                            method.AppendLine(String.Format("obj.{0} = sqlDataReader[\"{0}\"].ToString();", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("bool"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(7));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToBoolean(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(7));
                            method.AppendLine("else".Indent(7));
                            method.AppendLine(String.Format("obj.{0} = false;", f.Name).Indent(8));
                        }
                        else if (f.CSharpType.Equals("int"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(7));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToInt32(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(8));
                            method.AppendLine("else".Indent(7));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(8));
                        }
                        else if (f.CSharpType.Equals("decimal"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(7));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDecimal(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(8));
                            method.AppendLine("else".Indent(7));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(8));
                        }
                        else if (f.CSharpType.Equals("double") || f.CSharpType.Equals("float"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(7));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDouble(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(8));
                            method.AppendLine("else".Indent(7));
                            method.AppendLine(String.Format("obj.{0} = 0;", f.Name).Indent(8));
                        }
                        else if (f.CSharpType.Equals("DateTime"))
                        {
                            method.AppendLine(String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(7));
                            method.AppendLine(String.Format("obj.{0} = Convert.ToDateTime(sqlDataReader[\"{0}\"].ToString());", f.Name).Indent(8));
                            method.AppendLine("else".Indent(7));
                            method.AppendLine(String.Format("obj.{0} = new DateTime(1900, 1, 1);", f.Name).Indent(8));
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Map first column of the results to a primitive and add it to the list

                    method.AppendLine(String.Format("{0} obj = {1};", settings.ReturnName, PrimitiveInitialization(settings.ReturnName)).Indent(7));
                    Field primitiveField = returnColumns[0];

                    if (primitiveField.CSharpType.Equals("string"))
                    {
                        method.AppendLine("obj = sqlDataReader[0].ToString();".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("bool"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(7));
                        method.AppendLine("obj = Convert.ToBoolean(sqlDataReader[0].ToString());".Indent(8));
                        method.AppendLine("else".Indent(7));
                        method.AppendLine("obj = false;".Indent(8));
                    }
                    else if (primitiveField.CSharpType.Equals("int"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(7));
                        method.AppendLine("obj = Convert.ToInt32(sqlDataReader[0].ToString());".Indent(8));
                        method.AppendLine("else".Indent(7));
                        method.AppendLine("obj = 0;".Indent(8));
                    }
                    else if (primitiveField.CSharpType.Equals("decimal"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(7));
                        method.AppendLine("obj = Convert.ToDecimal(sqlDataReader[0].ToString());".Indent(8));
                        method.AppendLine("else".Indent(7));
                        method.AppendLine("obj = 0;".Indent(8));
                    }
                    else if (primitiveField.CSharpType.Equals("double") || primitiveField.CSharpType.Equals("float"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(7));
                        method.AppendLine("obj = Convert.ToDouble(sqlDataReader[0].ToString());".Indent(8));
                        method.AppendLine("else".Indent(7));
                        method.AppendLine("obj = 0;".Indent(8));
                    }
                    else if (primitiveField.CSharpType.Equals("DateTime"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(7));
                        method.AppendLine("obj = Convert.ToDateTime(sqlDataReader[0].ToString());".Indent(8));
                        method.AppendLine("else".Indent(7));
                        method.AppendLine("obj = new DateTime(1900, 1, 1);".Indent(8));
                    }

                    #endregion
                }
                method.AppendLine();
                method.AppendLine("back.Add(obj);".Indent(7));
                method.AppendLine("}".Indent(6));

                method.AppendLine("LastMessage = \"OK\";".Indent(6));
                method.AppendLine("back.Result = 1;".Indent(6));
                method.AppendLine("back.Message = \"Success\";".Indent(6));
                method.AppendLine("}".Indent(5));
                #endregion
            }
            else
            {
                #region Code to handle single object/primitive
                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead = 0;".Indent(4));

                method.AppendLine("using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult))".Indent(4));
                method.AppendLine("{".Indent(4)); //start of using statement
                method.AppendLine("if (sqlDataReader.Read())".Indent(5));
                method.AppendLine("{".Indent(5));

                if (settings.SaveRowsReadCount)
                    method.AppendLine("RowsRead = 1;".Indent(6));

                if (settings.IsObject)
                {
                    #region Map results to an object
                    foreach (Field f in returnColumns)
                    {
                        if (f.CSharpType.Equals("string"))
                        {
                            method.AppendLine(String.Format("back.{0} = sqlDataReader[\"{0}\"].ToString();", f.Name).Indent(6));
                        }
                        else if (f.CSharpType.Equals("bool"))
                        {
                            method.AppendLine(
                                String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(
                                String.Format("back.{0} = Convert.ToBoolean(sqlDataReader[\"{0}\"].ToString());", f.Name)
                                      .Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("back.{0} = false;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("int"))
                        {
                            method.AppendLine(
                                String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(
                                String.Format("back.{0} = Convert.ToInt32(sqlDataReader[\"{0}\"].ToString());", f.Name)
                                      .Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("back.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("decimal"))
                        {
                            method.AppendLine(
                                String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(
                                String.Format("back.{0} = Convert.ToDecimal(sqlDataReader[\"{0}\"].ToString());", f.Name)
                                      .Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("back.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("double") || f.CSharpType.Equals("float"))
                        {
                            method.AppendLine(
                                String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(
                                String.Format("back.{0} = Convert.ToDouble(sqlDataReader[\"{0}\"].ToString());", f.Name)
                                      .Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("back.{0} = 0;", f.Name).Indent(7));
                        }
                        else if (f.CSharpType.Equals("DateTime"))
                        {
                            method.AppendLine(
                                String.Format("if(!String.IsNullOrEmpty(sqlDataReader[\"{0}\"].ToString()))", f.Name).Indent(6));
                            method.AppendLine(
                                String.Format("back.{0} = Convert.ToDateTime(sqlDataReader[\"{0}\"].ToString());", f.Name)
                                      .Indent(7));
                            method.AppendLine("else".Indent(6));
                            method.AppendLine(String.Format("back.{0} = new DateTime(1900, 1, 1);", f.Name).Indent(7));
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Map first column of the results to an object and add it to the list
                    //method.AppendLine(String.Format("{0} obj = {1};", settings.ReturnName, PrimitiveInitialization(settings.ReturnName)).Indent(6));
                    Field primitiveField = returnColumns[0];

                    if (primitiveField.CSharpType.Equals("string"))
                    {
                        method.AppendLine("back = sqlDataReader[0].ToString();".Indent(6));
                    }
                    else if (primitiveField.CSharpType.Equals("bool"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("back = Convert.ToBoolean(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("back = false;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("int"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("back = Convert.ToInt32(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("back = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("decimal"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("back = Convert.ToDecimal(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("back = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("double") || primitiveField.CSharpType.Equals("float"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("back = Convert.ToDouble(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("back = 0;".Indent(7));
                    }
                    else if (primitiveField.CSharpType.Equals("DateTime"))
                    {
                        method.AppendLine("if(!String.IsNullOrEmpty(sqlDataReader[0].ToString()))".Indent(6));
                        method.AppendLine("back = Convert.ToDateTime(sqlDataReader[0].ToString());".Indent(7));
                        method.AppendLine("else".Indent(6));
                        method.AppendLine("back = new DateTime(1900, 1, 1);".Indent(7));
                    }
                    #endregion
                }
                method.AppendLine("LastMessage = \"OK\";".Indent(6));

                method.AppendLine("}".Indent(5));
                #endregion
            }

            method.AppendLine("}".Indent(4)); //end of using statement
            if (settings.UseTransaction)
                method.AppendLine("transaction.Commit();".Indent(4));
            method.AppendLine("}".Indent(3)); //end of try

            #region SQL Exception Handling
            method.AppendLine("catch (SqlException sqlex)".Indent(3));
            method.AppendLine("{".Indent(3)); //start of Sql Exception Catch
            method.AppendLine("LastMessage = sqlex.Errors[0].Message;".Indent(4));
            method.AppendLine("LastSqlException = sqlex;".Indent(4));

            if (settings.LogException)
                method.AppendLine(String.Format("log.Error(\"SqlException on Execution of SP: {0}.{1}\", sqlex.GetBaseException());", proc.Schema, proc.Name).Indent(4));

            if (settings.UseTransaction)
            {
                method.AppendLine("if(transaction != null)".Indent(4));
                method.AppendLine("transaction.Rollback();".Indent(5));
            }

            if (settings.IsSPR)
            {
                method.AppendLine("back.Result = -1;".Indent(4));
                method.AppendLine("back.Message = sqlex.Errors[0].Message;".Indent(4));
            }
            else
            {
                method.AppendLine("throw;".Indent(4));
            }

            method.AppendLine("}".Indent(3)); //end of Sql Exception Catch
            #endregion

            #region Generic Exception Handling
            method.AppendLine("catch (Exception ex)".Indent(3));
            method.AppendLine("{".Indent(3)); //start of Exception Catch
            method.AppendLine("LastMessage = ex.Message;".Indent(4));
            method.AppendLine("LastException = ex;".Indent(4));

            if (settings.LogException)
                method.AppendLine(String.Format("log.Error(\"Exception on Execution of SP: {0}.{1}\", ex);", proc.Schema, proc.Name).Indent(4));

            if (settings.UseTransaction)
            {
                method.AppendLine("if(transaction != null)".Indent(4));
                method.AppendLine("transaction.Rollback();".Indent(5));
            }

            if (settings.IsSPR)
            {
                method.AppendLine("back.Result = -1;".Indent(4));
                method.AppendLine("back.Message = ex.Message;".Indent(4));
            }
            else
            {
                method.AppendLine("throw;".Indent(4));
            }

            method.AppendLine("}".Indent(3)); //end of Exception Catch
            #endregion

            #region block "finally", of the try-catch
            method.AppendLine("finally".Indent(3));
            method.AppendLine("{".Indent(3));
            method.AppendLine("if (sqlCommand.Connection.State != ConnectionState.Closed)".Indent(4));
            method.AppendLine("sqlCommand.Connection.Close();".Indent(5));
            method.AppendLine("sqlCommand.Dispose();".Indent(4));

            //If the time elapsed must be measured
            if (settings.MeasureTimeElapsed)
                method.AppendLine("this.Executing = false;".Indent(4));

            if (settings.LogEnd) //check if was required log the start of the method
                method.AppendLine(String.Format("log.Debug(\"End of Method that Executes the SP: {0}.{1}\");", proc.Schema, proc.Name).Indent(4));

            method.AppendLine("}".Indent(3));
            #endregion

            method.AppendLine("return back;".Indent(3)); //closing bracket of the whole method
            method.AppendLine("}".Indent(2)); //closing bracket of the whole method

            if (settings.InsideRegion)
                method.AppendLine("#endregion".Indent(2)); //closing the region

            return method.ToString();
        }

        private string PrimitiveInitialization(string primitive)
        {
            if (primitive.Equals("bool"))
                return "false";

            if (primitive.Equals("int"))
                return "0";

            if (primitive.Equals("string"))
                return "\"\"";

            if (primitive.Equals("float"))
                return "0.0";

            if (primitive.Equals("double"))
                return "0.0";

            if (primitive.Equals("DateTime"))
                return "new DateTime(1900,1,1)";

            return "\"\"";
        }

    }
}
