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
using Ez_SQL.Common_Code;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.QueryLog
{
    /// <summary>
    /// A dockable panel that displays the query execution history for the current session.
    /// On load, it reads the XML query log from <c>DataStorageDir\QueriesLog\LogFile.xml</c>
    /// (via a safe copy to <c>Helper.xml</c>) and shows each entry in a grid.
    /// Selecting a row populates the detail area with connection info, execution metrics
    /// (start/end time, lapse, rows read/affected, grid count, success), the SQL script with
    /// syntax highlighting, and any errors that were raised during execution.
    /// </summary>
    public partial class HistoricForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        /// <summary>The application data-storage directory path, used to locate the log file.</summary>
        readonly string path = MainForm.DataStorageDir;

        /// <summary>The in-memory list of query log entries loaded from the XML log file.</summary>
        private List<QueryInfo> Qs;

        /// <summary>Reference to the main form, used for tab management commands.</summary>
        private MainForm Parent;

        /// <summary>
        /// Initializes a new instance of <see cref="HistoricForm"/>, reads the query log,
        /// configures the grid display, and sets up SQL syntax highlighting on the script viewer.
        /// </summary>
        /// <param name="Parent">The main form, used for tab management commands.</param>
        public HistoricForm(MainForm Parent)
        {
            InitializeComponent();

            this.Parent = Parent;
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
        /// <summary>Clears all detail fields in the UI, resetting them to empty values.</summary>
        private void CleanData()
        {
            //Connection data
            TxtGroup.Text = "";
            TxtName.Text = "";
            TxtServer.Text = "";
            TxtDb.Text = "";
            //Execution data
            TxtStart.Text = "";
            TxtEnd.Text = "";
            TxtLapse.Text = "";
            TxtReaded.Text = "";
            TxtAffected.Text = "";
            TxtTReturn.Text = "";
            TxtRightExec.Text = "";
            //Executed script
            ScriptText.Text = "";
            //No errors to show
            ErrorGrid.DataSource = null;
        }
        /// <summary>
        /// Populates the detail area with the data from the <see cref="QueryInfo"/> identified by <paramref name="QueryKey"/>.
        /// </summary>
        /// <param name="QueryKey">The unique key of the log entry to display.</param>
        private void LoadInfoFrom(int QueryKey)
        {
            QueryInfo Current = Qs.FindLast(X => X.Key == QueryKey);
            //Connection Data
            TxtGroup.Text = Current.Conx.Group;
            TxtName.Text = Current.Conx.Name;
            TxtServer.Text = Current.Conx.Server;
            TxtDb.Text = Current.Conx.Db;
            //Execution data
            TxtStart.Text = Current.Exec.StartTime.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            TxtEnd.Text = Current.Exec.EndTime.ToString("dd/MMM/yyyy HH:mm:ss.fff");
            TxtLapse.Text = Current.Exec.Lapse.ToString();
            TxtReaded.Text = Current.Exec.RecordsRead.ToString();
            TxtAffected.Text = Current.Exec.RecordsAffected.ToString();
            TxtTReturn.Text = Current.Exec.GridCount.ToString();
            TxtRightExec.Text = (Current.Exec.Correct == 1 ? "Yes" : "No");
            //Executed script
            ScriptText.Text = Current.Script;
            ScriptText.Refresh();
            //Load the errors that the script generated(if any)
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
        /// <summary>
        /// Reads the XML query log from disk by copying <c>LogFile.xml</c> to <c>Helper.xml</c>,
        /// appending a closing <c>&lt;/QueriesRoot&gt;</c> tag to make it well-formed, and
        /// parsing all <c>&lt;Query&gt;</c> elements into <see cref="QueryInfo"/> objects.
        /// The resulting list is sorted descending by key (most-recent first).
        /// </summary>
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

                CodeNode = Cq["Code"];//SQL Code/Script
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
        /// <summary>
        /// Parses an ANSI-formatted date string (<c>yyyyMMdd HH:mm:ss.fff</c>) into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="p">The date string in <c>yyyyMMdd HH:mm:ss.fff</c> format.</param>
        /// <returns>The parsed <see cref="DateTime"/> value.</returns>
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
        /// <summary>Binds the in-memory <see cref="Qs"/> list to the summary grid for display.</summary>
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
        /// <summary>
        /// Decodes XML-escaped character entities in the given string, reversing the encoding
        /// applied by <see cref="QueryRecord.ValidXmlText"/>.
        /// Handles <c>&amp;lt;</c>, <c>&amp;gt;</c>, <c>&amp;quot;</c>, <c>&amp;apos;</c>, and <c>&amp;amp;</c>.
        /// </summary>
        /// <param name="Text">The XML-escaped string to decode.</param>
        /// <returns>The decoded plain-text string.</returns>
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
    }
}
