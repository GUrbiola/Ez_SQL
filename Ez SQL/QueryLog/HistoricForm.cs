using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.IO;
using Ez_SQL.Extensions;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.QueryLog
{
    public partial class HistoricForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        readonly string path = MainForm.DataStorageDir;
        private List<QueryInfo> Qs;
 
        public HistoricForm()
        {
            InitializeComponent();
            ReadQueryLog();
            SGrid.Grid.RowTemplate.Height = 35;
            SGrid.Grid.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            SGrid.Grid.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            SGrid.Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            SGrid.Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SGrid.Grid.SelectionChanged += Grid_SelectionChanged;
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
                ScriptText.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void Grid_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView G = sender as DataGridView;

            if (G.CurrentRow != null)
            {
                LoadInfoFrom(Convert.ToInt32(G.CurrentRow.Cells["Key"].Value));
            }
            else
            {
                CleanData();
            }
        }
        private void CleanData()
        {
            //datos de la conexion
            TxtGroup.Text = "";
            TxtName.Text = "";
            TxtServer.Text = "";
            TxtDb.Text = "";
            //datos de la ejecucion
            TxtStart.Text = "";
            TxtEnd.Text = "";
            TxtLapse.Text = "";
            TxtReaded.Text = "";
            TxtAffected.Text = "";
            TxtTReturn.Text = "";
            TxtRightExec.Text = "";
            //Script de SQL Ejecutado
            ScriptText.Text = "";
            //Por ultimo carga de los errores si es que los hubo
            ErrorGrid.DataSource = null;
        }
        private void LoadInfoFrom(int QueryKey)
        {
            QueryInfo Current = Qs.FindLast(X => X.Key == QueryKey);
            //datos de la conexion
            TxtGroup.Text = Current.Conx.Group;
            TxtName.Text = Current.Conx.Name;
            TxtServer.Text = Current.Conx.Server;
            TxtDb.Text = Current.Conx.Db;
            //datos de la ejecucion
            TxtStart.Text = Current.Exec.StartTime.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            TxtEnd.Text = Current.Exec.EndTime.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            TxtLapse.Text = Current.Exec.Lapse.ToString();
            TxtReaded.Text = Current.Exec.RecordsRead.ToString();
            TxtAffected.Text = Current.Exec.RecordsAffected.ToString();
            TxtTReturn.Text = Current.Exec.GridCount.ToString();
            TxtRightExec.Text = (Current.Exec.Correct == 1 ? "Yes" : "No");
            //Script de SQL Ejecutado
            ScriptText.Text = Current.Script;
            ScriptText.Refresh();
            //Por ultimo carga de los errores si es que los hubo
            ErrorGrid.DataSource = null;
            if (Current.Mistakes != null && Current.Mistakes.Count > 0)
            {
                DataTable Ers = new DataTable("ErrorsTable");
                Ers.Columns.Add(new DataColumn("Line", typeof(int)));
                Ers.Columns.Add(new DataColumn("Message", typeof(string)));
                foreach (ErrorInfo Er in Current.Mistakes)
                {
                    DataRow Nr = Ers.NewRow();
                    Nr["Line"] = Er.Line;
                    Nr["Message"] = Er.Message;
                    Ers.Rows.Add(Nr);
                }
                ErrorGrid.DataSource = Ers;
            }
        }
        private void ReadQueryLog()
        {
            if (Qs == null)
                Qs = new List<QueryInfo>();
            else
                Qs.Clear();

            File.Copy(String.Format("{0}\\QueriesLog\\LogFile.xml", path), String.Format("{0}\\QueriesLog\\Helper.xml", path), true);
            using (StreamWriter ST = new StreamWriter(String.Format("{0}\\QueriesLog\\Helper.xml", path), true))
            {
                ST.WriteLine("</QueriesRoot>");
                ST.Close();
            }

            XmlDocument xDoc = new XmlDocument();
            XmlNodeList Queries;
            xDoc.Load(String.Format("{0}\\QueriesLog\\Helper.xml", path));
            Queries = xDoc.GetElementsByTagName("Query");
            int Key = 0;
            foreach (XmlNode Cq in Queries)
	        {
                XmlElement ConNode, ExecNode, CodeNode;
                XmlNodeList ErrorsNode;
                QueryInfo Q = new QueryInfo();
                Key++;

                Q.Key = Key;
                ConNode = Cq["Connection"];//Connection Info
                Q.Conx.Group = ConNode.Attributes["Group"].Value;
                Q.Conx.Name = ConNode.Attributes["Name"].Value;
                Q.Conx.Server = ConNode.Attributes["Server"].Value;
                Q.Conx.Db = ConNode.Attributes["Db"].Value;

                ExecNode = Cq["Execution"];//Execution Info
                Q.Exec.StartTime = AnsiToDate(ExecNode.Attributes["StartTime"].Value);
                Q.Exec.EndTime = AnsiToDate(ExecNode.Attributes["EndTime"].Value);
                Q.Exec.Lapse = int.Parse(ExecNode.Attributes["Lapse"].Value);
                Q.Exec.Correct = int.Parse(ExecNode.Attributes["Correct"].Value);
                Q.Exec.RecordsAffected = int.Parse(ExecNode.Attributes["RecordsAffected"].Value);
                Q.Exec.RecordsRead = int.Parse(ExecNode.Attributes["RecordsRead"].Value);
                Q.Exec.GridCount = int.Parse(ExecNode.Attributes["GridCount"].Value);

                CodeNode = Cq["Code"];//Codigo
                Q.Script = TextFromXml(CodeNode.InnerText.Trim('"'));

                ErrorsNode = ((XmlElement)Cq).GetElementsByTagName("Error");

                foreach (XmlNode Er in ErrorsNode)
                {
                    Q.Mistakes.Add(new ErrorInfo(int.Parse(Er.Attributes["Line"].Value), TextFromXml(Er.Attributes["Message"].Value)));
                }
                Qs.Add(Q);
            }
            Qs.Sort((Q1, Q2) => Q2.Key - Q1.Key);
        }
        private DateTime AnsiToDate(string p)
        {
            int year, month, day, hour, minute, second, ms;

            year = int.Parse(p.Substring(0, 4));
            month = int.Parse(p.Substring(4, 2));
            day = int.Parse(p.Substring(6, 2));
            hour = int.Parse(p.Substring(9, 2));
            minute = int.Parse(p.Substring(12, 2));
            second = int.Parse(p.Substring(15, 2));
            ms = int.Parse(p.Substring(18));
            
            return new DateTime(year, month, day, hour, minute, second, ms);
        }
        private void HistoricForm_Shown(object sender, EventArgs e)
        {
            ShowInfo();
        }
        private void ShowInfo()
        {
            using (DataTable aux = new DataTable("LogQuery"))
            {
                aux.Columns.Add(new DataColumn("Key", typeof(int)));
                aux.Columns.Add(new DataColumn("Script", typeof(string)));
                aux.Columns.Add(new DataColumn("Connection", typeof(string)));
                aux.Columns.Add(new DataColumn("Server", typeof(string)));
                aux.Columns.Add(new DataColumn("Db", typeof(string)));
                aux.Columns.Add(new DataColumn("Execution", typeof(DateTime)));
                aux.Columns.Add(new DataColumn("Lapse", typeof(int)));
                aux.Columns.Add(new DataColumn("Correct", typeof(int)));
                foreach (QueryInfo Q in Qs)
                {
                    DataRow Rc = aux.NewRow();
                    Rc["Key"] = Q.Key;
                    Rc["Connection"] = Q.Conx.Name;
                    Rc["Server"] = Q.Conx.Server;
                    Rc["Db"] = Q.Conx.Db;
                    Rc["Execution"] = Q.Exec.StartTime; //.ToString("yyyy - MMM - dd hh:mm:ss.fff");
                    Rc["Script"] = TextFromXml(Q.Script);
                    Rc["Lapse"] = Q.Exec.Lapse;
                    Rc["Correct"] = Q.Exec.Correct;
                    aux.Rows.Add(Rc);
                }
                SGrid.DataSource = aux;
            }
        }
        public string TextFromXml(string Text)
        {
            StringBuilder sb = new StringBuilder(Text);
            sb = sb.Replace("&lt;", "<");
            sb = sb.Replace("&gt;", ">");
            sb = sb.Replace("&quot;", "\"");
            sb = sb.Replace("&apos;", "'");
            sb = sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        private void ErrorGrid_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView G = sender as DataGridView;

            if (G.CurrentRow != null)
            {
                ScriptText.SelectLine(Convert.ToInt32(G.CurrentRow.Cells["Line"].Value) - 1);
            }
            else
            {
                ScriptText.SelectLine(- 1);
            }
        }
    }
}
