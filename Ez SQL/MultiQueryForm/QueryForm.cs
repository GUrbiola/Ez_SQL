using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using ICSharpCode.TextEditor.Document;
using System.IO;
using ICSharpCode.TextEditor;
using Ez_SQL.QueryLog;
using System.Collections;

namespace Ez_SQL.MultiQueryForm
{
    public delegate void ExecutionEvent();
    public partial class QueryForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        Keys KeyBefore;
        AutoCompleteWindow AutocompleteDialog;
        public bool IsIntellisenseOn
        {
            get
            {
                return ((AutocompleteDialog != null) && (!AutocompleteDialog.IsDisposed));
            }
        }

        private SQLConnector DataProvider;
        private QueryExecutor Executor;
        private MainForm Parent;

        private QueryRecord CurrentExecutionInfo;
        private string ConnectionGroup;
        private string ConnectionName;

        private string CurrentScript;

        private bool CancelAutoCompleteClosure;
        private string CurrentFilterString;
        private int AutoCompleteStartOffset;

        public QueryForm(MainForm Parent, SQLConnector DataProvider, string Script = "")
        {
            InitializeComponent();
            this.Parent = Parent;
            this.DataProvider = DataProvider;
            Query.Text = Script;
            CurrentScript = Script;

            Executor = new QueryExecutor();
            if (DataProvider != null && !String.IsNullOrEmpty(DataProvider.ConnectionString))
                Executor.Initialize(DataProvider.ConnectionString);
            Executor.FinishExec += new ProcessingQuery(Conx_FinishExec);
            Executor.StartExec += new ProcessingQuery(Conx_StartExec);
            
            ServerTxt.Text = DataProvider.Server;
            BDTxt.Text = DataProvider.DataBase;
            ConnectionGroup = Parent.ConxGroup;
            ConnectionName = Parent.ConxName;

            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                if (!Directory.Exists(MainForm.ExecDir + "\\SintaxHighLight"))
                {
                    Directory.CreateDirectory(MainForm.ExecDir + "\\SintaxHighLight\\");
                }
                if (!File.Exists(String.Format("{0}\\SintaxHighLight\\SQL.xshd", MainForm.ExecDir)))
                {
                    using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\SQL.xshd", MainForm.ExecDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        Writer.Write(Properties.Resources.SQL, 0, Properties.Resources.SQL.Length);
                        Writer.Close();
                    }
                }
                if (!File.Exists(String.Format("{0}\\SintaxHighLight\\CSharp.xshd", MainForm.ExecDir)))
                {
                    using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\CSharp.xshd", MainForm.ExecDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        Writer.Write(Properties.Resources.SQL, 0, Properties.Resources.SQL.Length);
                        Writer.Close();
                    }
                }
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.ExecDir + "\\SintaxHighLight\\"));
                Query.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
                Query.Document.FoldingManager.FoldingStrategy = new Ez_SQL.TextEditorClasses.SQLFoldingStrategy();
                Query.Document.FoldingManager.UpdateFoldings(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            #region Code to assign the method that will handle the key press event and the method to refresh the folding
            //Method that handles autocomplete and key shortcuts
            Query.ActiveTextAreaControl.TextArea.DoProcessDialogKey += Query_DoProcessDialogKey;
            //Methos that refresh the folding
            Query.Document.DocumentChanged += Query_DocumentChanged;
            //just to catch @ input and # input
            Query.ActiveTextAreaControl.TextArea.KeyEventHandler += new ICSharpCode.TextEditor.KeyEventHandler(TextArea_KeyEventHandler);
            #endregion

            //capture mouse click, to manage ctr + click
            Query.ActiveTextAreaControl.TextArea.MouseClick += MouseClicked;

            this.AutoScroll = false;
        }

        bool TextArea_KeyEventHandler(char ch)
        {
            if (ch == '@')
            {
                var helper = DataProvider.DbObjects.Where(XX => XX.Kind == ObjectType.Procedure && XX.Childs.Count > 0).FirstOrDefault();
                ObjectSelector X = new ObjectSelector(
                    "Object selector test",
                    "This emulates a selection of tables or something",
                    helper.Childs,
                    "Text");
                if (X.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Query.InsertString(((ISqlChild)X.SelectedObject).Text);
                    return true;
                }
                
            }
            return false;
        }


        #region Code for the actions in the toolstrip of the query form
        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (Query.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                CurrentScript = Query.ActiveTextAreaControl.SelectionManager.SelectedText;
            else
                CurrentScript = Query.Text;
            Executor.AsyncExecuteDataSet(CurrentScript);
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            Executor.CancelExecute();
        }
        private void BtnExtremeStop_Click(object sender, EventArgs e)
        {
            Executor.ExtremeStop();
        }
        private void BtnComment_Click(object sender, EventArgs e)
        {
            int startLine, endLine, i;
            string comment = "--";
            if (Query.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {//if there is a selection, comment each line within the selection range
                foreach (ISelection selection in Query.ActiveTextAreaControl.SelectionManager.SelectionCollection)
                {
                    startLine = selection.StartPosition.Y;
                    endLine = selection.EndPosition.Y;

                    for (i = endLine; i >= startLine; --i)
                    {
                        LineSegment line = Query.Document.GetLineSegment(i);
                        if (selection != null && i == endLine && line.Offset == selection.Offset + selection.Length)
                        {
                            --endLine;
                            continue;
                        }

                        string lineText = Query.Document.GetText(line.Offset, line.Length);
                        Query.Document.Insert(line.Offset, comment);
                    }
                }
            }
            else
            {//If there is no selection comment the current line
                startLine = Query.ActiveTextAreaControl.TextArea.Caret.Line;
                endLine = Query.ActiveTextAreaControl.TextArea.Caret.Line;

                for (i = endLine; i >= startLine; --i)
                {
                    LineSegment line = Query.Document.GetLineSegment(i);
                    if (line.ToString().Trim().Length == 0)
                    {
                        --endLine;
                        continue;
                    }

                    string lineText = Query.Document.GetText(line.Offset, line.Length);
                    Query.Document.Insert(line.Offset, comment);
                }
            }

        }
        private void BtnUncomment_Click(object sender, EventArgs e)
        {
            int startLine, endLine, i;
            string comment = "--";

            if (Query.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {//if there is a selection, uncomment each line within the selection range
                foreach (ISelection selection in Query.ActiveTextAreaControl.SelectionManager.SelectionCollection)
                {
                    startLine = selection.StartPosition.Y;
                    endLine = selection.EndPosition.Y;

                    for (i = endLine; i >= startLine; --i)
                    {
                        LineSegment line = Query.Document.GetLineSegment(i);
                        if (selection != null && i == endLine && line.Offset == selection.Offset + selection.Length)
                        {
                            --endLine;
                            continue;
                        }

                        string lineText = Query.Document.GetText(line.Offset, line.Length);
                        if (lineText.Trim().StartsWith(comment))
                            Query.Document.Remove(line.Offset + lineText.IndexOf(comment), comment.Length);
                    }
                }
            }
            else
            {//If there is no selection uncomment the current line
                startLine = Query.ActiveTextAreaControl.TextArea.Caret.Line;
                endLine = Query.ActiveTextAreaControl.TextArea.Caret.Line;

                for (i = endLine; i >= startLine; --i)
                {
                    LineSegment line = Query.Document.GetLineSegment(i);
                    if (line.ToString().Trim().Length == 0)
                    {
                        --endLine;
                        continue;
                    }

                    string lineText = Query.Document.GetText(line.Offset, line.Length);
                    if (lineText.Trim().StartsWith(comment))
                        Query.Document.Remove(line.Offset + lineText.IndexOf(comment), comment.Length);

                }
            }
        }
        private void BtnBookmark_Click(object sender, EventArgs e)
        {
            DoEditAction(Query, new ICSharpCode.TextEditor.Actions.ToggleBookmark());
        }
        private void DoEditAction(ICSharpCode.TextEditor.TextEditorControl editor, ICSharpCode.TextEditor.Actions.IEditAction action)
        {
            if (editor != null && action != null)
            {
                var area = editor.ActiveTextAreaControl.TextArea;
                editor.BeginUpdate();
                try
                {
                    lock (editor.Document)
                    {
                        action.Execute(area);
                        if (area.SelectionManager.HasSomethingSelected && area.AutoClearSelection /*&& caretchanged*/)
                        {
                            if (area.Document.TextEditorProperties.DocumentSelectionMode == DocumentSelectionMode.Normal)
                            {
                                area.SelectionManager.ClearSelection();
                            }
                        }
                    }
                }
                finally
                {
                    editor.EndUpdate();
                    area.Caret.UpdateCaretPosition();
                }
            }
        }
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            DoEditAction(Query, new ICSharpCode.TextEditor.Actions.GotoPrevBookmark(bookmark => true));
        }
        private void BtnNext_Click(object sender, EventArgs e)
        {
            DoEditAction(Query, new ICSharpCode.TextEditor.Actions.GotoNextBookmark(bookmark => true));
        }
        private void BtnClearBookmarks_Click(object sender, EventArgs e)
        {
            DoEditAction(Query, new ICSharpCode.TextEditor.Actions.ClearAllBookmarks(bookmark => true));
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saver = new SaveFileDialog();
            saver.Title = "Save SQL script";
            saver.AddExtension = true;
            saver.AutoUpgradeEnabled = true;
            saver.Filter = "Text file|*.txt|SQL file|*.sql|Any file|*.*";
            if (saver.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter Wr = new StreamWriter(saver.FileName))
                {
                    Wr.Write(Query.Text);
                    Wr.Close();
                }
            }
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.Title = "Load script from file";
            opener.Filter = "Text file|*.txt|SQL file|*.sql|Any file|*.*";
            if (opener.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader Rdr = new StreamReader(opener.FileName))
                {
                    string line;
                    while ((line = Rdr.ReadLine()) != null)
                        sb.AppendLine(line);
                    Rdr.Close();
                }
                Query.Text = sb.ToString();
                Query.Refresh();
            }

        }
        private void BtnShowHideResults_Click(object sender, EventArgs e)
        {
            MainContainer.Panel2Collapsed = !MainContainer.Panel2Collapsed;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //_findForm = new SearchAndReplace();
            //_findForm.ShowFor(this, Query, false);
        }
        //public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
        //{
        //    if (string.IsNullOrEmpty(CurWord))
        //    {
        //        MessageBox.Show("Seleccione la palabra a buscar...");
        //        return null;
        //    }
        //    TextEditorSearcher _search = new TextEditorSearcher();
        //    _lastSearchWasBackward = searchBackward;
        //    _search.Document = Query.Document;
        //    _search.LookFor = CurWord;
        //    _search.MatchCase = false;
        //    _search.MatchWholeWordOnly = false;

        //    var caret = Query.ActiveTextAreaControl.Caret;
        //    if (viaF3 && _search.HasScanRegion && !caret.Offset.IsInRange(_search.BeginOffset, _search.EndOffset))
        //    {
        //        // user moved outside of the originally selected region
        //        _search.ClearScanRegion();
        //    }

        //    int startFrom = caret.Offset - (searchBackward ? 1 : 0);
        //    TextRange range = _search.FindNext(startFrom, searchBackward, out _lastSearchLoopedAround);
        //    if (range != null)
        //        SelectResult(range);
        //    else if (messageIfNotFound != null)
        //        MessageBox.Show(messageIfNotFound);
        //    return range;
        //}
        //private void SelectResult(TextRange range)
        //{
        //    TextLocation p1 = Query.Document.OffsetToPosition(range.Offset);
        //    TextLocation p2 = Query.Document.OffsetToPosition(range.Offset + range.Length);
        //    Query.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
        //    Query.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
        //    Query.ActiveTextAreaControl.Caret.Position = Query.Document.OffsetToPosition(range.Offset + range.Length);
        //}
        private void BtnXportAll_Click(object sender, EventArgs e)
        {
            //if (TabHolder != null && TabHolder.TabPages != null && TabHolder.TabPages.Count > 0)
            //{
            //    List<TabPage> XportPages = new List<TabPage>();
            //    List<DataGridView> Grids = new List<DataGridView>();
            //    foreach (TabPage tab in TabHolder.TabPages)
            //    {
            //        if (tab.Text.StartsWith("Resultado", StringComparison.CurrentCultureIgnoreCase))
            //            XportPages.Add(tab);
            //    }

            //    if (XportPages.Count == 0)
            //    {
            //        MessageBox.Show("No hay resultados para exportar");
            //        return;
            //    }

            //    SaveFileDialog xport = new SaveFileDialog();
            //    xport.AddExtension = true;
            //    xport.FileName = "";
            //    xport.Filter = "Archivo de Excel|*.xls";
            //    xport.FilterIndex = 0;
            //    xport.OverwritePrompt = true;
            //    xport.Title = "Ruta y Nombre de Archivo a Exportar Informacion";
            //    xport.SupportMultiDottedExtensions = true;
            //    if (xport.ShowDialog() == DialogResult.OK)
            //    {
            //        foreach (TabPage DataTab in XportPages)
            //        {
            //            HDesarrollo.Controles.Grid.SmartGrid Aux;
            //            if (DataTab.Controls[0] is HDesarrollo.Controles.Grid.SmartGrid)
            //                Aux = DataTab.Controls[0] as HDesarrollo.Controles.Grid.SmartGrid;
            //            else
            //                continue;
            //            Grids.Add(Aux.Grid);
            //        }
            //        Exporter xp = new Exporter(Grids, xport.FileName);
            //        xp.ShowDialog();
            //    }
            //}
        }
        private void BtnXportCSharp_Click(object sender, EventArgs e)
        {
            //int name = 1;
            //if (TabHolder != null && TabHolder.TabPages != null && TabHolder.TabPages.Count > 0)
            //{
            //    if (Conx.Results == null || Conx.Results.Tables.Count == 0)
            //    {
            //        MessageBox.Show("No hay resultados para exportar");
            //        return;
            //    }
            //    FormCName cn = new FormCName();
            //    foreach (DataTable tab in Conx.Results.Tables)
            //    {
            //        if (tab.Columns.Count == 0 || (tab.Columns.Count == 1 && tab.Columns[0].ColumnName.Equals("rowsaffected", StringComparison.CurrentCultureIgnoreCase)))
            //            continue;
            //        cn.SetName(name);
            //        name++;
            //        if (cn.ShowDialog() == DialogResult.OK)
            //        {
            //            StringBuilder X = GenerateTable(cn.ClassName, tab);
            //            Dad.AgregaCSharpCodeForm(X.ToString());
            //        }
            //    }
            //}
        }
        #endregion
        #region Functions triggered at the start/ending of a query execution
        private void DoubleClickedResultItem(object sender, MouseEventArgs e)
        {
            ListView Lvw;
            ListViewItem Item;

            if (sender is ListView)
            {
                Lvw = sender as ListView;
                Item = Lvw.GetItemAt(e.X, e.Y);
                if (Item != null)
                {
                    int Line = 0;
                    try
                    {
                        Line = int.Parse(Item.Tag.ToString());
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    if (Line == 0)
                        return;
                    Query.SelectLine(Line - 1);
                }
            }
        }
        void Conx_StartExec(string SQLScript, DateTime Time)
        {
            CurrentExecutionInfo = new QueryRecord(ConnectionGroup, ConnectionName, Executor.Server, Executor.DataBase, SQLScript);
            BeginInvoke(new ExecutionEvent(ExecutionStarted));
        }
        void Conx_FinishExec(string Query, DateTime Hora)
        {
            BeginInvoke(new ExecutionEvent(ExecutionEnded));
        }
        void ExecutionStarted()
        {
            BtnExecute.Enabled = false;
            BtnStop.Enabled = true;
            BtnExtremeStop.Enabled = true;
            Query.Enabled = false;
        }
        void ExecutionEnded()
        {
            bool error = false;
            int AffectedRecords = 0, ReadedRecords = 0, ResultsTables = 0, intaux;
            List<string> ResultQueue = new List<string>();
            List<TabPage> Pages = new List<TabPage>();

            if (CurrentExecutionInfo.HasErrors())//evitar dobles terminos??!!
                return;

            TabHolder.TabPages.Clear();
            TabHolder.SuspendLayout();

            #region First we must add the results tab
            if (Executor.Results != null && Executor.Results.Tables != null && Executor.Results.Tables.Count > 0)
            {
                int index = 0;
                foreach (DataTable Table in Executor.Results.Tables)
                {
                    if (Table.TableName.StartsWith("NonQuery", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ResultQueue.Add(String.Format("Affected Records: {0}", Table.Rows[0][0].ToString()));
                        if (int.TryParse(Table.Rows[0][0].ToString(), out intaux))
                            AffectedRecords += intaux;
                    }
                    else
                    {
                        TabPage TableTab = new TabPage();
                        index++;
                        HDesarrollo.Controles.Grid.SmartGrid Grid = new HDesarrollo.Controles.Grid.SmartGrid();
                        Grid.FilterBoxPosition = GridViewExtensions.FilterPosition.Off;
                        Grid.Dock = DockStyle.Fill;
                        TableTab.SuspendLayout();

                        TableTab.Controls.Add(Grid);
                        TableTab.ImageIndex = 0;
                        TableTab.Location = new System.Drawing.Point(4, 29);
                        TableTab.Name = "TableTab_" + index.ToString();
                        TableTab.Padding = new System.Windows.Forms.Padding(3);
                        TableTab.Size = new System.Drawing.Size(900, 150);
                        TableTab.TabIndex = 0;
                        TableTab.Text = "Resultado " + index.ToString();
                        TableTab.UseVisualStyleBackColor = true;
                        TableTab.ResumeLayout(false);

                        Grid.DataSource = Table;

                        if (Table != null)
                            ReadedRecords += Table.Rows.Count;
                        ResultsTables++;

                        Pages.Add(TableTab);
                    }
                }
            }
            #endregion
            #region then we add the data tabs
            TabPage ResultsTab = new TabPage();
            ListView MessageList = new ListView();
            Label LabExecutionTime = new Label();

            ResultsTab.SuspendLayout();
            ColumnHeader MessageColumn1 = new ColumnHeader();
            MessageColumn1.Text = "";
            MessageColumn1.Width = 750;
            MessageList.Columns.AddRange(new ColumnHeader[] { MessageColumn1 });
            MessageList.Dock = System.Windows.Forms.DockStyle.Fill;
            MessageList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            MessageList.LargeImageList = TabIcons;
            MessageList.Location = new System.Drawing.Point(3, 18);
            MessageList.Name = "MessageList";
            MessageList.Size = new System.Drawing.Size(894, 129);
            MessageList.SmallImageList = TabIcons;
            MessageList.TabIndex = 0;
            MessageList.UseCompatibleStateImageBehavior = false;
            MessageList.View = System.Windows.Forms.View.Details;
            MessageList.MouseDoubleClick += DoubleClickedResultItem;

            if (Executor.Error)
            {
                error = true;
                if (Executor.SqlEx != null)
                {//Sql exception
                    int i, n = Executor.SqlEx.Errors.Count;
                    for (i = 0; i < n; i++)
                    {
                        ListViewItem SqlExc = new ListViewItem(Executor.SqlEx.Errors[i].Message, 2);
                        SqlExc.Name = "SqlEx_" + i;
                        SqlExc.Tag = Executor.SqlEx.Errors[i].LineNumber;
                        MessageList.Items.Add(SqlExc);
                        CurrentExecutionInfo.AddError(Executor.SqlEx.Errors[i].LineNumber, Executor.SqlEx.Errors[i].Message);
                    }
                }
                else if (Executor.NrEx != null)
                {//regular exception
                    ListViewItem Exc = new ListViewItem(Executor.NrEx.Message, 2);
                    Exc.Name = "Exc";
                    MessageList.Items.Add(Exc);
                    CurrentExecutionInfo.AddError(-1, Executor.NrEx.Message);
                }
                else
                {//Exception from de executor??
                    ListViewItem Error = new ListViewItem(Executor.LastMessage, 2);
                    Error.Name = "Error";
                    MessageList.Items.Add(Error);
                    CurrentExecutionInfo.AddError(-1, Executor.LastMessage);
                }
            }
            else
            {
                ListViewItem RightExec = new ListViewItem("Execution ended", 1);
                RightExec.Name = "RightExec";
                MessageList.Items.Add(RightExec);
            }

            foreach (string msg in Executor.Messages)
            {
                ListViewItem Message = new ListViewItem(msg, 3);
                MessageList.Items.Add(Message);
            }

            foreach (string Res in ResultQueue)
            {
                ListViewItem Affecteds = new ListViewItem(Res, 1);
                MessageList.Items.Add(Affecteds);
            }


            LabExecutionTime.Dock = System.Windows.Forms.DockStyle.Top;
            LabExecutionTime.Location = new System.Drawing.Point(3, 3);
            LabExecutionTime.Name = "LabExecutionTime";
            LabExecutionTime.Size = new System.Drawing.Size(894, 15);
            LabExecutionTime.TabIndex = 1;
            LabExecutionTime.Text = Executor.ExecutionLapse.ToString();

            ResultsTab.Controls.Add(MessageList);
            ResultsTab.Controls.Add(LabExecutionTime);
            ResultsTab.ImageIndex = 1;
            ResultsTab.Location = new System.Drawing.Point(4, 29);
            ResultsTab.Name = "ResultsTab";
            ResultsTab.Padding = new System.Windows.Forms.Padding(3);
            ResultsTab.Size = new System.Drawing.Size(900, 150);
            ResultsTab.TabIndex = 1;
            ResultsTab.Text = "ResultsTab";
            ResultsTab.UseVisualStyleBackColor = true;
            ResultsTab.ResumeLayout();

            Pages.Add(ResultsTab);
            #endregion

            TabHolder.TabPages.AddRange(Pages.ToArray());
            TabHolder.ResumeLayout(false);

            if (MainContainer.Panel2Collapsed)
                MainContainer.Panel2Collapsed = false;

            if (CurrentExecutionInfo != null)
            {
                CurrentExecutionInfo.RecordsAffected = AffectedRecords;
                CurrentExecutionInfo.RecordsRead = ReadedRecords;
                CurrentExecutionInfo.EndTime = DateTime.Now;
                CurrentExecutionInfo.Lapse = (int)CurrentExecutionInfo.EndTime.Subtract(CurrentExecutionInfo.StartTime).TotalMilliseconds;
                CurrentExecutionInfo.GridCount = ResultsTables;
                CurrentExecutionInfo.Correct = (error ? 0 : 1);
                Globals.SaveToQueryLog(CurrentExecutionInfo);
            }

            BtnExecute.Enabled = true;
            BtnStop.Enabled = false;
            BtnExtremeStop.Enabled = false;
            Query.Enabled = true;

            Query.Focus();
        }
        #endregion
        #region Code to refresh the folding, it will execute a second when the "last change" has been made a second ago
        private int ToRefresh=5;
        void Query_DocumentChanged(object sender, DocumentEventArgs e)
        {
            ToRefresh = 5;
            if (!FoldingRefresher.Enabled)
                FoldingRefresher.Enabled = true;
        }
        private void FoldingRefresher_Tick(object sender, EventArgs e)
        {
            ToRefresh--;
            if (ToRefresh <= 0)
            {
                Query.Document.FoldingManager.UpdateFoldings(null, null);
                FoldingRefresher.Enabled = false;
                ToRefresh = 5;
            }
        }
        #endregion
        bool Query_DoProcessDialogKey(Keys keyData)//Process hot keys
        {
            int CurPos;
            string TxtBef, TxtAft, CurrentWord;
            Token CurrentToken;

            // Echo == true, then NoEcho == false
            #region Key shortcut processing
            switch (keyData)
            {
                case Keys.F5://Execute query
                    if (BtnExecute.Enabled)
                        BtnExecute_Click(null, null);
                    return true;
                case Keys.Shift | Keys.F5://Stop execution
                    if (BtnStop.Enabled)
                        BtnStop_Click(null, null);
                    return true;
                case Keys.Control | Keys.F5://Forced stop execution
                    if (BtnExtremeStop.Enabled)
                        BtnExtremeStop_Click(null, null);
                    return true;
                case Keys.Control | Keys.K://Comment selection
                    if (Query.Enabled)
                        BtnComment_Click(null, null);
                    return true;
                case Keys.Control | Keys.U://Uncomment selection
                    if (Query.Enabled)
                        BtnUncomment_Click(null, null);
                    return true;
                case Keys.Control | Keys.F://Open search dialog
                    BtnSearch_Click(null, null);
                    return true;
                case Keys.F3://Search next (forward)
                    //FindNext(true, false, String.Format("Cadena de Texto: {0}, no encontrada.", _findForm.LookFor));
                    return true;
                case Keys.Shift | Keys.F3://Search next (Backward)
                    //FindNext(true, true, String.Format("Cadena de Texto: {0}, no encontrada.", _findForm.LookFor));
                    return true;
                case Keys.F2://Toggle bookmark
                    BtnBookmark_Click(null, null);
                    return true;
                case Keys.Shift | Keys.F2://Go to previous bookmark
                    BtnPrevious_Click(null, null);
                    return true;
                case Keys.Control | Keys.F2://Go to next bookmark
                    BtnNext_Click(null, null);
                    return true;
                case Keys.Control | Keys.Shift | Keys.F2://Clear bookmarks
                    BtnClearBookmarks_Click(null, null);
                    return true;
                case Keys.Control | Keys.S://Save to file
                    BtnSave_Click(null, null);
                    return true;
                case Keys.Control | Keys.L://Load file
                    BtnLoad_Click(null, null);
                    return true;
                case Keys.Control | Keys.W://Hide/show results tab
                    BtnShowHideResults_Click(null, null);
                    return true;
                case Keys.Control | Keys.E://Export results tab to excel
                    BtnXportAll_Click(null, null);
                    return true;
                case Keys.Control | Keys.Shift | Keys.C://Create C# class from results tab(1 class for each grid)
                    BtnXportCSharp_Click(null, null);
                    return true;
                case Keys.F12:
                    Token Last, Next, SelToken;
                    CurPos = Query.CurrentOffset();
                    TxtBef = Query.Document.GetText(0, CurPos);
                    TxtAft = Query.Document.GetText(CurPos, Query.Text.Length - CurPos);
                    Last = TxtBef.GetLastToken();
                    Next = TxtAft.GetFirstToken();

                    if (Next.Type == TokenType.EMPTYSPACE || Next.Type == TokenType.COMMA || Next.Text.StartsWith("("))
                    {
                        SelToken = Last;
                    }
                    else
                    {
                        SelToken = new Token(TokenType.WORD, Last.Text + Next.Text);
                    }

                    ISqlObject Obj = DataProvider.IsSqlObject(SelToken.Text);
                    if (Obj != null)
                    {
                        Parent.AddQueryForm(Obj.Name, Obj.Script, DataProvider);
                    }
                    return true;
            }
            #endregion

            CurPos = Query.CurrentOffset();
            TxtBef = Query.Document.GetText(0, CurPos);
            TxtAft = Query.Document.GetText(CurPos, Query.Text.Length - CurPos);


           #region Codigo Anterior para Intellisense, codigo propio no de ICSharp.TextEditor
            if (IsIntellisenseOn)
            {
                #region Codigo para el Manejo de Teclas si el "IntelliSense" esta Activado
                
                switch (keyData)
                {
                    case Keys.Back:
                        AutocompleteDialog.Close();
                        if (CurrentFilterString != null && CurrentFilterString.Length > 0)
                        {
                            CurrentFilterString = CurrentFilterString.Remove(CurrentFilterString.Length - 1);
                            CancelAutoCompleteClosure = true;
                            ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        }
                        return true;
                    case Keys.Escape:
                        AutocompleteDialog.Close();
                        break;
                    case Keys.OemMinus:
                        CurrentFilterString += "-";
                        ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return false;
                    case Keys.OemMinus | Keys.Shift:
                        CurrentFilterString += "_";
                        ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return false;
                    case (Keys)65601:
                    case Keys.A:
                    case (Keys)65602:
                    case Keys.B:
                    case (Keys)65603:
                    case Keys.C:
                    case (Keys)65604:
                    case Keys.D:
                    case (Keys)65605:
                    case Keys.E:
                    case (Keys)65606:
                    case Keys.F:
                    case (Keys)65607:
                    case Keys.G:
                    case (Keys)65608:
                    case Keys.H:
                    case (Keys)65609:
                    case Keys.I:
                    case (Keys)65610:
                    case Keys.J:
                    case (Keys)65611:
                    case Keys.K:
                    case (Keys)65612:
                    case Keys.L:
                    case (Keys)65613:
                    case Keys.M:
                    case (Keys)65614:
                    case Keys.N:
                    case (Keys)65615:
                    case Keys.O:
                    case (Keys)65616:
                    case Keys.P:
                    case (Keys)65617:
                    case Keys.Q:
                    case (Keys)65618:
                    case Keys.R:
                    case (Keys)65619:
                    case Keys.S:
                    case (Keys)65620:
                    case Keys.T:
                    case (Keys)65621:
                    case Keys.U:
                    case (Keys)65622:
                    case Keys.V:
                    case (Keys)65623:
                    case Keys.W:
                    case (Keys)65624:
                    case Keys.X:
                    case (Keys)65625:
                    case Keys.Y:
                    case (Keys)65626:
                    case Keys.Z:
                    case Keys.NumPad0:
                    case Keys.NumPad1:
                    case Keys.NumPad2:
                    case Keys.NumPad3:
                    case Keys.NumPad4:
                    case Keys.NumPad5:
                    case Keys.NumPad6:
                    case Keys.NumPad7:
                    case Keys.NumPad8:
                    case Keys.NumPad9:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.Back | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.Back | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                        CurrentFilterString += ((char)keyData).ToString();
                        ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return false;
                    case Keys.Delete:
                    case Keys.Left:
                    case Keys.Right:
                        return true;
                    default:
                        if (IsIntellisenseOn)
                            return true;
                        else
                            return false;
                }
                
                #endregion
            }
            else
            {
                #region Switch para saber que se va a mostrar cuando se quiera hacer intellisense
                
                switch (keyData)
                {
                    case Keys.Control | Keys.OemPeriod://ctrl + . , Show autocomplete
                    case Keys.Space | Keys.Control://ctrl + space, Show autocomplete
                        CurrentToken = TxtBef.GetLastToken();
                        if (CurrentToken.Type == TokenType.EMPTYSPACE || CurrentToken.Type == TokenType.COMMA)
                        {
                            CurrentWord = "";
                        }
                        else
                        {
                            CurrentWord = CurrentToken.Text;
                        }
                        CurrentFilterString = CurrentWord;
                        AutoCompleteStartOffset = CurPos - CurrentWord.Length;
                        ShowIntellisense(CurrentWord, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return true;
                    case Keys.Space:
                        //string WordBehind = GetWordAtOffset();
                        //int auxoffset2 = Query.Document.PositionToOffset(Query.ActiveTextAreaControl.Caret.Position);
                        //if (WordBehind.Equals("FROM", StringComparison.CurrentCultureIgnoreCase) || WordBehind.Equals("JOIN", StringComparison.CurrentCultureIgnoreCase))
                        //{//si es un FROM o un ON sacar la lista de tablas y vistas
                        //    Query.Document.Insert(auxoffset2, " ");
                        //    Query.ActiveTextAreaControl.Caret.Column += 1;
                        //    auxoffset2++;
                        //    CurOffset = auxoffset2;

                        //    CurrentFilter = FilteringTypeValues.Fields;
                        //    FilterString = "";
                        //    FireAt = CurOffset;
                        //    ShowIntellisense();
                        //    return true;
                        //}
                        //else if (WordBehind.Equals("DELETE", StringComparison.CurrentCultureIgnoreCase) || WordBehind.Equals("UPDATE", StringComparison.CurrentCultureIgnoreCase) || WordBehind.Equals("INTO", StringComparison.CurrentCultureIgnoreCase))
                        //{//si es un DELETE, un UPDATE o un INTO sacar la lista de tablas
                        //    Query.Document.Insert(auxoffset2, " ");
                        //    Query.ActiveTextAreaControl.Caret.Column += 1;
                        //    auxoffset2++;
                        //    CurOffset = auxoffset2;

                        //    CurrentFilter = FilteringTypeValues.Tables;
                        //    FilterString = "";
                        //    FireAt = CurOffset;
                        //    ShowIntellisense();
                        //    return true;
                        //}
                        //else if (WordBehind.Equals("EXEC", StringComparison.CurrentCultureIgnoreCase) || WordBehind.Equals("EXECUTE", StringComparison.CurrentCultureIgnoreCase))
                        //{//si es un EXEC o un EXECUTE la lista de sps
                        //    Query.Document.Insert(auxoffset2, " ");
                        //    Query.ActiveTextAreaControl.Caret.Column += 1;
                        //    auxoffset2++;
                        //    CurOffset = auxoffset2;

                        //    CurrentFilter = FilteringTypeValues.Sps;
                        //    FilterString = "";
                        //    FireAt = CurOffset;
                        //    ShowIntellisense();
                        //    return true;
                        //}
                        return false;
                    case Keys.OemPeriod:
                    case Keys.Decimal:
                        Query.InsertString(".");
                        CurPos = Query.CurrentOffset();
                        TxtBef = Query.Document.GetText(0, CurPos);
                        TxtAft = Query.Document.GetText(CurPos, Query.Text.Length - CurPos);
                        CurrentToken = TxtBef.GetLastToken();
                        if (CurrentToken.Type == TokenType.EMPTYSPACE || CurrentToken.Type == TokenType.COMMA)
                        {
                            CurrentWord = "";
                        }
                        else
                        {
                            CurrentWord = CurrentToken.Text;
                        }
                        CurrentFilterString = CurrentWord;
                        AutoCompleteStartOffset = CurPos - CurrentWord.Length;
                        ShowIntellisense(CurrentWord, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);


                        #region Validar que la posicion del punto amerite un autocompletado de campos
                        ////primero averiguar si hay tengo frente al cursor
                        //int auxoffset = Query.Document.PositionToOffset(Query.ActiveTextAreaControl.Caret.Position);
                        //Query.Document.Insert(auxoffset, ".");
                        //Query.ActiveTextAreaControl.Caret.Column += 1;
                        //auxoffset++;
                        //bool pass = false;
                        //int pos = Query.ActiveTextAreaControl.Caret.Column;
                        //string line = Query.Document.GetText(Query.Document.GetLineSegment(Query.ActiveTextAreaControl.Caret.Line));

                        //if (line.Length > pos)
                        //{//hay texto enfrente
                        //    //averiguar si el inmediato es un espacio en blanco
                        //    if (line.Substring(pos, 1).Trim() == "")
                        //    {//no hay texto enfrente ahora si llamar el autocompletado
                        //        pass = true;
                        //    }
                        //}
                        //else if (line.Length == pos)
                        //{//no hay texto enfrente, esta en la ultima posicion
                        //    pass = true;
                        //}
                        #endregion
                        #region Si la posicion del punto es valida ahora revisar que la palabra antes de este sea una tabla, vista o un alias
                        //if (pass)
                        //{
                        //    //ahora obtener la palabra detras del punto
                        //    CurWord = GetWordAtOffsetMinus(2);
                        //    CurOffset = auxoffset;
                        //    CurChilds = null;
                        //    if (!String.IsNullOrEmpty(CurWord))
                        //    {
                        //        SQLServer_Object Obj = DataProvider.GetByName(CurWord);
                        //        if (Obj != null)//es el nombre de una tabla
                        //        {
                        //            CurChilds = Obj.Childs;
                        //            CurrentFilter = FilteringTypeValues.Childs;
                        //            FilterString = "";
                        //            FireAt = CurOffset;
                        //            ShowIntellisense();
                        //        }
                        //        else
                        //        {//revisar si es un alias
                        //            #region Obtener la Lista de alias declarados en el script
                        //            if (Aliases == null)
                        //                Aliases = new Hashtable();
                        //            else
                        //                Aliases.Clear();

                        //            string CleanText = Query.Text.Remove(auxoffset - 1, 1);
                        //            int index, total;
                        //            CleanText = CleanText.Replace("\r\n", " ");
                        //            CleanText = CleanText.Replace("\t", " ");
                        //            while (CleanText.Contains("  "))
                        //                CleanText = CleanText.Replace("  ", " ");
                        //            string[] Words = CleanText.Split(new char[] { ' ' });
                        //            total = Words.Length;
                        //            for (index = 0; index < total; index++)
                        //            {
                        //                if (Words[index].Equals("Join", StringComparison.CurrentCultureIgnoreCase) || Words[index].Equals("From", StringComparison.CurrentCultureIgnoreCase) || Words[index].Equals(",", StringComparison.CurrentCultureIgnoreCase))
                        //                {
                        //                    if (index + 2 < total)
                        //                    {
                        //                        if (!ReservedWords.Contains(Words[index + 2].ToUpper()))
                        //                        {
                        //                            if (!Aliases.ContainsKey(Words[index + 2].ToUpper()))
                        //                                Aliases.Add(Words[index + 2].ToUpper(), Words[index + 1].ToUpper());
                        //                            index += 2;
                        //                            continue;
                        //                        }
                        //                        else if (Words[index + 2].Equals("AS", StringComparison.CurrentCultureIgnoreCase) && index + 3 < total)
                        //                        {
                        //                            string auxalias, auxtable;
                        //                            auxtable = Words[index + 1];
                        //                            auxalias = Words[index + 3];

                        //                            if (!Aliases.ContainsKey(auxalias.ToUpper()))
                        //                                Aliases.Add(auxalias.ToUpper(), auxtable.ToUpper());
                        //                            index += 3;
                        //                            continue;
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            #endregion

                        //            if (Aliases.ContainsKey(CurWord.ToUpper()))
                        //            {
                        //                SQLServer_Object AliasObj = DataProvider.GetByName(Aliases[CurWord.ToUpper()].ToString());
                        //                if (AliasObj != null)
                        //                {
                        //                    CurChilds = AliasObj.Childs;
                        //                    CurrentFilter = FilteringTypeValues.Childs;
                        //                    FilterString = "";
                        //                    FireAt = CurOffset;
                        //                    ShowIntellisense();
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        return true;
                    case Keys.T | Keys.Control://ctrl + t , mostrar tablas + vistas
                        
                    
                        //CurrentFilter = FilteringTypeValues.Fields;
                        //CurWord = GetWordAtOffsetMinusOne();
                        //FilterString = CurWord;
                        //FireAt = CurOffset;
                        //ShowIntellisense();
                        break;
                    case Keys.P | Keys.Control://ctrl + p , mostrar procedimientos almacenados
                        //CurrentFilter = FilteringTypeValues.Sps;
                        //CurWord = GetWordAtOffsetMinusOne();
                        //FilterString = CurWord;
                        //FireAt = CurOffset;
                        //ShowIntellisense();
                        break;
                    case Keys.D | Keys.Control://ctrl + d , mostrar variables 
                        //CurrentFilter = FilteringTypeValues.Variables;
                        //CurWord = GetWordAtOffsetMinusOne();
                        //FilterString = CurWord;
                        //FireAt = CurOffset;
                        //AddVariables();
                        //ShowIntellisense();
                        return true;
                    //case Keys.Alt | Keys.NumPad4://aroba, @
                    //    if (KeyBefore == (Keys.Alt | Keys.NumPad6))
                    //    {
                    //        Text = "@ FTW!!!!!!";
                    //        return false;
                    //    }
                    //    break;
                    ////case Keys.RButton | Keys.ShiftKey | Keys.Space | Keys.Control | Keys.Alt:
                    //    Query.Document.Insert(CurOffset, "@");
                    //    CurrentFilter = FilteringTypeValues.Variables;
                    //    CurWord = GetWordAtOffsetMinusOne();
                    //    FilterString = CurWord;
                    //    FireAt = CurOffset;
                    //    AddVariables();
                    //    ShowIntellisense();
                    //    return true;
                    case Keys.S | Keys.Control://ctrl + s , mostrar snippets
                        MessageBox.Show("Ctr + S");
                        break;
                    default:
                        KeyBefore = keyData;
                        //this.Text = keyData.ToString();
                        break;
                }
                
                #endregion
            }
            #endregion
            
            return false;
        }

        private List<ISqlObject> GetAliasesAndAuxiliarTables(string FullScript)
        {
            List<ISqlObject> Back = new List<ISqlObject>();
            List<Token> Tokens = FullScript.GetTokens();
            List<int> TokensToDelete = new List<int>();

            #region First i have to process the full script, remove the "()" from the table valued functions
            for (int i = 0; i < Tokens.Count; i++)
            {
                if(Tokens[i].Type == TokenType.WORD && DataProvider.IsTableValuedFunction(Tokens[i].Text) != null)
                {
                    //find the first opening parenthesis
                    while(i < Tokens.Count && !Tokens[i].Text.StartsWith("("))
                        i++;
                    //then from there till the first closing parenthesis mark every token to be removed
                    for (int j = i; j < Tokens.Count; j++)
			        {
                        TokensToDelete.Add(j);
                        if (Tokens[j].Text.EndsWith(")"))
                        {
                            break;
                        }

			        }
                }
            }
            //remove the tokens marked
            for (int i = TokensToDelete.Count - 1; i >= 0 ; i--)
			{
                Tokens.RemoveAt(TokensToDelete[i]);
			}
            //if there was any token removed at all
            if(TokensToDelete.Count > 0 && Tokens.Count > TokensToDelete[0])
            {//if the token right before the deletion and the next after the deletion are both empty spaces or from the same type
                for (int i = Tokens.Count - 1; i >= 1; i--)
                {
                    if(Tokens[i].Type == Tokens[i-1].Type)
                        Tokens.RemoveAt(i);
                }
            }
            #endregion
            
            for (int i = 0; i < Tokens.Count; i++)
            {
                Token CurToken = Tokens[i];
                #region check for aliases, type: TableTypeObject Alias
                if ((i + 2) < Tokens.Count)
                {
                    Token ProposedAlias = Tokens[i + 2];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        ISqlObject Aliased = DataProvider.IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)//if it is a table, view or a table value function
                        {
                            Back.Add
                            (
                                new Alias()
                                {
                                    AliasedObject = Aliased.Name,
                                    Childs = Aliased.Childs,
                                    Comment = Aliased.Comment,
                                    Id = 0,
                                    Name = ProposedAlias.Text,
                                    Schema = Aliased.Schema,
                                    Text = ProposedAlias.Text
                                }
                            );
                        }
                    }
                }
                #endregion
                #region check for aliases, type: TableTypeObject AS Alias
                if ((i + 4) < Tokens.Count)
                {
                    Token ProposedAlias = Tokens[i + 4];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && Tokens[i + 2].Text.Equals("AS", StringComparison.CurrentCultureIgnoreCase) && Tokens[i + 3].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        ISqlObject Aliased = DataProvider.IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)
                        {
                            Back.Add
                            (
                                new Alias()
                                {
                                    AliasedObject = Aliased.Name,
                                    Childs = Aliased.Childs,
                                    Comment = Aliased.Comment,
                                    Id = 0,
                                    Name = ProposedAlias.Text,
                                    Schema = Aliased.Schema,
                                    Text = ProposedAlias.Text
                                }
                            );
                        }
                    }
                }
                #endregion
            }
            return Back;
        }
        void ShowIntellisense(string FilterString, List<ISqlObject> ComplementaryObjects, FilteringType Filter = FilteringType.Any)
        {
            string Sc, Ta, Fi;
            bool QualifierBehind = false;
            int AutoCompleteLength = 0;
            List<string> Data;

            if (IsIntellisenseOn)
            {
                AutocompleteDialog.Close();
            }
            Data = FilterString.Split('.').ToList();

            CompletionDataProvider completionDataProvider;
            switch (Data.Count)
            {
                case 1://filter string 1 word, so is an schema or a dbobject or a temp object(alias, #table or @table)
                    completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, null, Data[0], null);
                    AutoCompleteLength = Data[0].Length;
                    break;
                case 2://filter string 2 word, must be an schema, object or an object, child
                    if(DataProvider.DbObjects.Any(X => X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)))
                        completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, Data[0], Data[1], null);
                    else
                        completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, null, Data[0], Data[1]);
                    AutoCompleteLength = Data[1].Length;
                    QualifierBehind = true;
                    break;
                case 3://filter string 3 word, must be an schema, object, child
                    completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, Data[0], Data[1], Data[2]);
                    AutoCompleteLength = Data[2].Length;
                    break;
                default://no filter string, so show all
                case 0://no filter string, so show all
                    completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, null, "", null);
                    AutoCompleteLength = 0;
                    break;
            }
            completionDataProvider.FilteringOption = Filter;
            completionDataProvider.ComplementaryObjects = ComplementaryObjects;
            //CompletionDataProvider completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, FilterString);
                //SQLSCCProvider(CurChilds, Vars, DataProvider, PopIList, FilterString, CurrentFilter, FireAt);
            AutocompleteDialog = AutoCompleteWindow.ShowCompletionWindow(
                         this,
                         Query,
                         "sql",
                         completionDataProvider,
                         ' ',
                         AutoCompleteStartOffset + (FilterString.Length - AutoCompleteLength),//AutoCompleteStartOffset,
                         AutoCompleteStartOffset + FilterString.Length,//FilterString.Length
                         QualifierBehind
                         );

            if (AutocompleteDialog != null)
            {
                AutocompleteDialog.Closing += ISense_Closing;
                AutocompleteDialog.Closed += CodeCompletionWindowClosed;
            }
        }
        void CodeCompletionWindowClosed(object sender, EventArgs e)
        {
            AutocompleteDialog.Closed -= CodeCompletionWindowClosed;
            AutocompleteDialog.Closing -= ISense_Closing;
            AutocompleteDialog.Dispose();
            AutocompleteDialog = null;
        }
        void ISense_Closing(object sender, CancelEventArgs e)
        {
            if (CancelAutoCompleteClosure)
            {
                CancelAutoCompleteClosure = false;
                e.Cancel = true;
            }
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            MainContainer.Panel2Collapsed = true;
            if (!Executor.TestConnection())
            {
                MessageBox.Show("Couldn't connect to the database", "Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BtnExecute.Enabled = false;
            }
        }
        void MouseClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    Token Last, Next, SelToken;
                    int CurPos = Query.CurrentOffset();
                    string TxtBef = Query.Document.GetText(0, CurPos);
                    string TxtAft = Query.Document.GetText(CurPos, Query.Text.Length - CurPos);
                    Last = TxtBef.GetLastToken();
                    Next = TxtAft.GetFirstToken();

                    if (Next.Type == TokenType.EMPTYSPACE || Next.Type == TokenType.COMMA || Next.Text.StartsWith("("))
                    {
                        SelToken = Last;
                    }
                    else
                    {
                        SelToken = new Token(TokenType.WORD, Last.Text + Next.Text);
                    }

                    ISqlObject Obj = DataProvider.IsSqlObject(SelToken.Text);
                    if (Obj != null)
                    {
                        Parent.AddQueryForm(Obj.Name, Obj.Script, DataProvider);
                    }
                }
            }
        }

     }
}
