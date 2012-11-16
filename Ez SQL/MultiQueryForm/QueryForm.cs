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
using ICSharpCode.TextEditor.Document;
using System.IO;
using ICSharpCode.TextEditor;
using Ez_SQL.QueryLog;
using System.Collections;
using Ez_SQL.Snippets;

namespace Ez_SQL.MultiQueryForm
{
    public delegate void ExecutionEvent();
    public partial class QueryForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        AutoCompleteWindow AutocompleteDialog;
        public bool IsIntellisenseOn
        {
            get
            {
                return ((AutocompleteDialog != null) && (!AutocompleteDialog.IsDisposed));
            }
        }
        Keys LastKeyPressed;

        private SqlConnector DataProvider;
        private QueryExecutor Executor;
        private MainForm Parent;

        private QueryRecord CurrentExecutionInfo;
        private string ConnectionGroup;
        private string ConnectionName;

        private string CurrentScript;

        private bool CancelAutoCompleteClosure;
        private string CurrentFilterString;
        private int AutoCompleteStartOffset;
        
        SearchAndReplace _findForm;
        private bool _lastSearchLoopedAround = false, _lastSearchWasBackward = false;

        public QueryForm(MainForm Parent, SqlConnector DataProvider, string Script = "")
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
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
                Query.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
                Query.Document.FormattingStrategy = new Ez_SQL.TextEditorClasses.SqlBracketMatcher();
                Query.Document.FoldingManager.FoldingStrategy = new Ez_SQL.TextEditorClasses.SqlFolder();
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
            ////just to catch @ input and # input
            //Query.ActiveTextAreaControl.TextArea.KeyEventHandler += new ICSharpCode.TextEditor.KeyEventHandler(TextArea_KeyEventHandler);
            #endregion

            //capture mouse click, to manage ctr + click
            Query.ActiveTextAreaControl.TextArea.MouseClick += MouseClicked;

            this.AutoScroll = false;
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
            if (_findForm == null)
                _findForm = new SearchAndReplace();
            _findForm.ShowFor(this, Query, false);
        }
        public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
        {
            if (_findForm == null)
                _findForm = new SearchAndReplace();


            //if (String.IsNullOrEmpty(TxtSearch.Text) == null || selToken.IsTextEmpty)
            //{
            //    MessageBox.Show("Word to find not defined.");
            //    return null;
            //}

            TextEditorSearcher _search = new TextEditorSearcher();
            _lastSearchWasBackward = searchBackward;
            _search.Document = Query.Document;
            _search.LookFor = _findForm.LookFor;
            _search.MatchCase = _findForm.MatchCase;
            _search.MatchWholeWordOnly = _findForm.MatchWholeWordOnly;

            var caret = Query.ActiveTextAreaControl.Caret;
            if (viaF3 && _search.HasScanRegion && !caret.Offset.IsInRange(_search.BeginOffset, _search.EndOffset))
            {
                // user moved outside of the originally selected region
                _search.ClearScanRegion();
            }

            int startFrom = caret.Offset - (searchBackward ? 1 : 0);
            TextRange range = _search.FindNext(startFrom, searchBackward, out _lastSearchLoopedAround);
            if (range != null)
                SelectResult(range);
            else if (messageIfNotFound != null)
                MessageBox.Show(messageIfNotFound);
            return range;
        }
        private void SelectResult(TextRange range)
        {
            TextLocation p1 = Query.Document.OffsetToPosition(range.Offset);
            TextLocation p2 = Query.Document.OffsetToPosition(range.Offset + range.Length);
            Query.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
            Query.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
            Query.ActiveTextAreaControl.Caret.Position = Query.Document.OffsetToPosition(range.Offset + range.Length);
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
                {//Exception from the executor??
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
        #region Code to refresh the folding, it will execute a second when the "last change" has been made 2 seconds ago
        private int ToRefresh = 10;
        void Query_DocumentChanged(object sender, DocumentEventArgs e)
        {
            ToRefresh = 10;
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
                ToRefresh = 10;
            }
        }
        #endregion
        bool Query_DoProcessDialogKey(Keys keyData)//Process hot keys
        {
            bool NoEcho = true, Echo = false;
            int CurPos;
            string TxtBef, TxtAft, CurrentWord;
            Token CurrentToken, LastToken;

            // Echo == true, then NoEcho == false
            #region Key shortcut processing
            switch (keyData)
            {
                case Keys.F5://Execute query
                    if (BtnExecute.Enabled)
                        BtnExecute_Click(null, null);
                    return NoEcho;
                case Keys.Shift | Keys.F5://Stop execution
                    if (BtnStop.Enabled)
                        BtnStop_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.F5://Forced stop execution
                    if (BtnExtremeStop.Enabled)
                        BtnExtremeStop_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.C://Comment selection / Collapse outlining
                    if (LastKeyPressed == (Keys.Control | Keys.K))
                    {//comment selection, or current line
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (Query.Enabled)
                            BtnComment_Click(null, null);
                        return NoEcho;
                    }
                    else if (LastKeyPressed == (Keys.Control | Keys.O))
                    {//collapse outlining
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (Query.Enabled)
                            collapseToolStripMenuItem_Click(null, null);
                        return NoEcho;
                    }
                    return Echo;
                case Keys.Control | Keys.U://Uncomment selection / to lower case the selection
                    if (LastKeyPressed == (Keys.Control | Keys.K))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (Query.Enabled)
                            BtnUncomment_Click(null, null);
                        return NoEcho;
                    }
                    else
                    {//to lower case the selection
                        toLowerCaseToolStripMenuItem1_Click(null, null);
                    }
                    return NoEcho;
                case Keys.Control | Keys.Shift | Keys.U://to upper case selection
                    toUpperCseToolStripMenuItem_Click(null, null);
                    break;
                case Keys.Control | Keys.F://Open search dialog
                    BtnSearch_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.F3:
                    string lookFor;
                    if (Query.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected)
                    {
                        lookFor = Query.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                    }
                    else
                    {
                        Token selToken;
                        TokenList tokens = Query.Text.GetTokens();
                        selToken = tokens.GetTokenAtOffset(Query.CurrentOffset());
                        lookFor = selToken.Text;
                    }

                    if (_findForm == null)
                        _findForm = new SearchAndReplace();
                    if (!String.IsNullOrEmpty(lookFor) && lookFor.Trim(' ', '\t', '\r', '\n').Length > 0)
                    {
                        _findForm.SetSearchString(lookFor);
                        FindNext(true, false, String.Format("Specified text: {0}, was not found.", _findForm.LookFor));
                        return NoEcho;
                    }
                    break;
                case Keys.F3://Search next (forward)
                    if (_findForm != null && !String.IsNullOrEmpty(_findForm.LookFor) && _findForm.LookFor.Trim(' ', '\t', '\r', '\n').Length > 0)
                    {
                        FindNext(true, false, String.Format("Specified text: {0}, was not found.", _findForm.LookFor));
                    }
                    return NoEcho;
                case Keys.Shift | Keys.F3://Search next (Backward)
                    if (_findForm != null && !String.IsNullOrEmpty(_findForm.LookFor) && _findForm.LookFor.Trim(' ', '\t', '\r', '\n').Length > 0)
                    {
                        FindNext(true, true, String.Format("Specified text: {0}, was not found.", _findForm.LookFor));
                    }
                    return NoEcho;
                case Keys.F2://Toggle bookmark
                    BtnBookmark_Click(null, null);
                    return NoEcho;
                case Keys.Shift | Keys.F2://Go to previous bookmark
                    BtnPrevious_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.F2://Go to next bookmark
                    BtnNext_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.Shift | Keys.F2://Clear bookmarks
                    BtnClearBookmarks_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.G://Save to file
                    BtnSave_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.L://Load file
                    BtnLoad_Click(null, null);
                    return NoEcho;
                case Keys.Control | Keys.W://Hide/show results tab
                    BtnShowHideResults_Click(null, null);
                    return NoEcho;
                case Keys.F12://go to definition
                    Token selToken2;
                    TokenList tokens2 = Query.Text.GetTokens();
                    selToken2 = tokens2.GetTokenAtOffset(Query.CurrentOffset());

                    if (selToken2 != null && selToken2.Type == TokenType.WORD)
                    {
                        ISqlObject Obj = DataProvider.IsSqlObject(selToken2.Text);
                        if (Obj != null)
                        {
                            Parent.AddQueryForm(Obj.Name, Obj.Script, DataProvider);
                        }
                    }
                    return NoEcho;
                case Keys.Control | Keys.E://Expand outlining
                    if (LastKeyPressed == (Keys.Control | Keys.O))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (Query.Enabled)
                            expandToolStripMenuItem_Click(null, null);
                        return NoEcho;                        
                    }
                    return NoEcho;
                case Keys.Control | Keys.T://Toggle outlining
                    if (LastKeyPressed == (Keys.Control | Keys.O))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (Query.Enabled)
                            toggleToolStripMenuItem_Click(null, null);
                        return NoEcho;
                    }
                    return NoEcho;
            }
            #endregion

            CurPos = Query.CurrentOffset();
            TxtBef = Query.Document.GetText(0, CurPos);
            TxtAft = Query.Document.GetText(CurPos, Query.Text.Length - CurPos);


            #region Autocomplete/intellisense code
            if (IsIntellisenseOn)
            {
                #region Code to handle the key pressed if the "intellisense" is active
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
                        return NoEcho;
                    case Keys.Escape:
                        AutocompleteDialog.Close();
                        break;
                    case Keys.OemMinus:
                        CurrentFilterString += "-";
                        ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return Echo;
                    case Keys.OemMinus | Keys.Shift:
                        CurrentFilterString += "_";
                        ShowIntellisense(CurrentFilterString, GetAliasesAndAuxiliarTables(Query.Text), FilteringType.Smart);
                        return Echo;
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
                        return Echo;
                    case Keys.Delete:
                    case Keys.Left:
                    case Keys.Right:
                        return NoEcho;
                    default:
                        if (IsIntellisenseOn)
                            return Echo;
                        else
                            return NoEcho;
                }

                #endregion
            }
            else
            {
                LastKeyPressed = keyData;
                #region Code to handle the key pressed if the intellisense is inactive
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
                        return NoEcho;
                    case Keys.Space://if the text before is an @ or an #, show posible vars or temp tables
                        LastToken = TxtBef.GetLastToken();
                        if (LastToken.Text == "@")
                        {
                            TokenList Tokens = Query.Text.GetTokens();
                            List<string> Helper = new List<string>();
                            foreach (Token t in Tokens.List.Where(X => X.Type == TokenType.VARIABLE))
                            {
                                string buff = t.Text.Contains(".") ? t.Text.Split('.')[0] : t.Text;
                                if (!Helper.Contains(buff, StringComparer.CurrentCultureIgnoreCase))
                                    Helper.Add(buff);
                            }
                            if (Helper.Count > 0)
                            {
                                ObjectSelector Os = new ObjectSelector("Variables detected on script", "Select variable", Helper);
                                if (Os.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    Query.InsertString(Os.SelectedObject.Length > 0 && Os.SelectedObject.StartsWith("@") ? Os.SelectedObject.Substring(1) : Os.SelectedObject);
                                    return NoEcho;
                                }
                                else
                                {
                                    return Echo;
                                }
                            }
                            return Echo;
                        }
                        else if (LastToken.Text == "#")
                        {
                            TokenList Tokens = Query.Text.GetTokens();
                            List<string> Helper = new List<string>();
                            foreach (Token t in Tokens.List.Where(X => X.Type == TokenType.TEMPTABLE))
                            {
                                string buff = t.Text.Contains(".") ? t.Text.Split('.')[0] : t.Text;
                                if (!Helper.Contains(buff, StringComparer.CurrentCultureIgnoreCase))
                                    Helper.Add(buff);
                            }
                            if (Helper.Count > 0)
                            {
                                ObjectSelector Os = new ObjectSelector("Variables temp tables on script", "Select table", Helper);
                                if (Os.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    Query.InsertString(Os.SelectedObject.Length > 0 && Os.SelectedObject.StartsWith("#") ? Os.SelectedObject.Substring(1) : Os.SelectedObject);
                                    return NoEcho;
                                }
                                else
                                {
                                    return Echo;
                                }
                            }
                            return Echo;
                        }
                        break;
                    case Keys.OemPeriod:
                    case Keys.Decimal://show Sql Object childs... if any
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
                        return NoEcho;
                    case Keys.S | Keys.Control://ctrl + s , show list of snippets
                        List<string> SnippetsNames = new List<string>();
                        List<string> SnippetsValues = new List<string>();

                        foreach (Snippet sn in Parent.GetSnippetList())
                        {
                            SnippetsNames.Add(sn.Name);
                            SnippetsValues.Add(sn.ShortCut);
                        }

                        ObjectSelector SnSelector = new ObjectSelector("Snippets detected", "Select the snippet, to insert his shortcut", SnippetsValues, SnippetsNames, false, 1);
                        if (SnSelector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Query.InsertString(SnSelector.SelectedObject);
                            return NoEcho;
                        }
                        else
                        {
                            return Echo;
                        }
                        
                        break;
                    case Keys.Tab://check if it is a request for a snippet, a variable or a temp table
                        LastToken = TxtBef.GetLastToken();
                        if (LastToken.Text == "@")
                        {
                            TokenList Tokens = Query.Text.GetTokens();
                            List<string> Helper = new List<string>();
                            foreach (Token t in Tokens.List.Where(X => X.Type == TokenType.VARIABLE))
                            {
                                string buff = t.Text.Contains(".") ? t.Text.Split('.')[0] : t.Text;
                                if (!Helper.Contains(buff, StringComparer.CurrentCultureIgnoreCase))
                                    Helper.Add(buff);
                            }
                            if (Helper.Count > 0)
                            {
                                ObjectSelector Os = new ObjectSelector("Variables detected on script", "Select variable", Helper);
                                if (Os.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    Query.InsertString(Os.SelectedObject.Length > 0 && Os.SelectedObject.StartsWith("@") ? Os.SelectedObject.Substring(1) : Os.SelectedObject);
                                    return NoEcho;
                                }
                                else
                                {
                                    return Echo;
                                }
                            }
                            return Echo;
                        }
                        else if (LastToken.Text == "#")
                        {
                            TokenList Tokens = Query.Text.GetTokens();
                            List<string> Helper = new List<string>();
                            foreach (Token t in Tokens.List.Where(X => X.Type == TokenType.TEMPTABLE))
                            {
                                string buff = t.Text.Contains(".") ? t.Text.Split('.')[0] : t.Text;
                                if (!Helper.Contains(buff, StringComparer.CurrentCultureIgnoreCase))
                                    Helper.Add(buff);
                            }
                            if (Helper.Count > 0)
                            {
                                ObjectSelector Os = new ObjectSelector("Variables temp tables on script", "Select table", Helper);
                                if (Os.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    Query.InsertString(Os.SelectedObject.Length > 0 && Os.SelectedObject.StartsWith("#") ? Os.SelectedObject.Substring(1) : Os.SelectedObject);
                                    return NoEcho;
                                }
                                else
                                {
                                    return Echo;
                                }
                            }
                            return Echo;
                        }
                        else if (LastToken.Type == TokenType.WORD)
                        {//check for snippet
                            string SnippetScript = Parent.IsInsertSnippet(LastToken.Text);
                            if (!String.IsNullOrEmpty(SnippetScript))
                            {
                                int Offset = Query.CurrentOffset();
                                Query.SetSelectionByOffset(Offset - LastToken.Text.Length, Offset);
                                InsertSnippet(SnippetScript);
                                return NoEcho;
                            }
                            return Echo;
                        }
                        break;
                    default:
                        //this.Text = keyData.ToString();
                        return Echo;
                }

                #endregion
            }
            #endregion

            return Echo;
        }

        #region Snippet inserting and processing methods
        private void InsertSnippet(string SnippetScript)
        {
            string ProcessedSnippet = SnippetScript;
            List<string> Objs;
            List<SnippetInnerObject> InnerObjects = new List<SnippetInnerObject>();
            if (ProcessedSnippet.IndexOf("$OBJ:") > 0)
            {
                Objs = DataProvider.DbObjects.Where(X => X.Kind != ObjectType.Schema && X.Kind != ObjectType.Alias).Select(X => X.Schema + "." + X.Name).ToList();
                while (ProcessedSnippet.IndexOf("$OBJ:") > 0)
                {
                    try
                    {
                        int start = ProcessedSnippet.IndexOf("$OBJ:");
                        int length = (ProcessedSnippet.IndexOf('$', start + 1) - start) + 1;
                        if (length <= 0 || ProcessedSnippet.IndexOf('$', start + 1) == -1)
                        {
                            MessageBox.Show("Error while processing the snippet, check sintax(not found ending $ for object)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        string Id = ProcessedSnippet.Substring(start, length);
                        Id = Id.Trim('$').Split(':')[1];
                        if (String.IsNullOrEmpty(Id) || Id.Trim().Length == 0)
                        {
                            MessageBox.Show("Error while processing the snippet, check sintax(not found object id for object)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        SnippetInnerObject Obj;
                        ObjectSelector Os = new ObjectSelector("Objects on the current database connection", "Select a database object", Objs, false, 5);
                        if (Os.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Obj = new SnippetInnerObject()
                            {
                                Id = Id,
                                Object = DataProvider.DbObjects.Find
                                (X => String.Format("{0}.{1}", X.Schema, X.Name).Equals(Os.SelectedObject)
                                    && X.Kind != ObjectType.Schema
                                    && X.Kind != ObjectType.Alias
                                )
                            };
                            switch (Obj.Object.Kind)
                            {
                                default:
                                case ObjectType.Table:
                                case ObjectType.View:
                                case ObjectType.TableFunction:
                                    if (Obj.Id.StartsWith("na", StringComparison.CurrentCultureIgnoreCase))
                                    {//this object must be managed without an alias, so we use <schema>.<object name> as an alias
                                        Obj.Alias = String.Format("{0}.{1}", Obj.Object.Schema, Obj.Object.Name);
                                    }
                                    else
                                    {
                                        Obj.Alias = Obj.Object.Name.GetUpperCasedLetters(2).GetAsSentence();//camel cased objects FTW!!!

                                        if (String.IsNullOrEmpty(Obj.Alias))
                                            Obj.Alias = Obj.Object.Name.Substring(0, 2);//if not camel cased object, get the first 2 letters in the name

                                        if (InnerObjects.Any(X => X.Alias.Equals(Obj.Alias)))//making sure this alias is unique
                                            Obj.Alias += InnerObjects.Count + 1;
                                    }
                                    break;
                                case ObjectType.Procedure:
                                case ObjectType.ScalarFunction:
                                    Obj.Alias = "";
                                    break;
                            }

                            if (String.IsNullOrEmpty(Obj.Alias) || Obj.Alias.Contains("."))
                            {
                                ProcessedSnippet = ProcessedSnippet.Replace(Obj.Name, String.Format("{0}.{1}", Obj.Object.Schema, Obj.Object.Name));
                            }
                            else
                            {
                                ProcessedSnippet = ProcessedSnippet.Replace(Obj.Name, String.Format("{0}.{1} AS {2}", Obj.Object.Schema, Obj.Object.Name, Obj.Alias));
                            }
                            InnerObjects.Add(Obj);
                            //check for all fields
                            ProcessedSnippet = ProcessSnippetChilds(Obj, ProcessedSnippet, "Fields", true);
                            //check for field selector
                            ProcessedSnippet = ProcessSnippetChilds(Obj, ProcessedSnippet, "Fields", false);
                            //check for all Params
                            ProcessedSnippet = ProcessSnippetChilds(Obj, ProcessedSnippet, "Parameters", true);
                            //check for param selector
                            ProcessedSnippet = ProcessSnippetChilds(Obj, ProcessedSnippet, "Parameters", false);
                        }
                        else
                        {//cancelled the snippet processing
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error while processing the snippet, check sintax", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                }
            }

            Query.InsertString(ProcessedSnippet);
        }
        private string ProcessSnippetChilds(SnippetInnerObject Obj, string ProcessedSnippet, string Type, bool All)
        {
            string sep, buff = "", searchString, fname, bkup = ProcessedSnippet;
            int ocstart, oclen;
            ChildType CurType;
            List<string> PosibleChilds = null;
            ChildSelector Cs = null;

            if (Type.Equals("Fields", StringComparison.CurrentCultureIgnoreCase))
            {//fields
                CurType = ChildType.Field;
                if (All)
                {
                    searchString = Obj.AllFieldsText;
                }
                else
                {
                    searchString = Obj.FieldsText;
                }
            }
            else
            {//parameters
                CurType = ChildType.Parameter;
                if (All)
                {
                    searchString = Obj.AllParamsText;
                }
                else
                {
                    searchString = Obj.ParamsText;
                }
            }

            while (ProcessedSnippet.IndexOf(searchString) > 0)
            {
                ocstart = ProcessedSnippet.IndexOf(searchString);
                oclen = (ProcessedSnippet.IndexOf('$', ocstart + 1) - ocstart) + 1;
                if (oclen <= 0 || ProcessedSnippet.IndexOf('$', ocstart + 1) == -1)
                {
                    MessageBox.Show("Error while processing the snippet, check sintax(not found ending $ for object " + Type + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return "";
                }
                fname = ProcessedSnippet.Substring(ocstart, oclen).Trim('$');
                sep = fname.Split(':')[2];
                if (String.IsNullOrEmpty(sep) || sep.Trim().Length == 0)
                {
                    MessageBox.Show("Error while processing the snippet, check sintax(not found object separator for object " + Type + ")", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return "";
                }
                sep = sep.Replace("\\n", Environment.NewLine).Replace("\\t", "\t");
                if (All)
                {
                    foreach (ISqlChild child in Obj.Object.Childs.Where(X => X.Kind == CurType))
                    {
                        if (String.IsNullOrEmpty(Obj.Alias))
                        {//object without an alias, must be a procedure or a scalar function
                            if (String.IsNullOrEmpty(buff))
                                buff += String.Format("{0}", child.Name);
                            else
                                buff += String.Format("{0}{1}", sep, child.Name);
                        }
                        else
                        {//object with an alias, must be a table, a view, or a table function
                            if (String.IsNullOrEmpty(buff))
                                buff += String.Format("{0}.{1}", Obj.Alias, child.Name);
                            else
                                buff += String.Format("{0}{1}.{2}", sep, Obj.Alias, child.Name);
                        }
                    }
                    ProcessedSnippet = ProcessedSnippet.Replace(String.Format("${0}$", fname), buff);
                    buff = "";
                }
                else
                {
                    if (PosibleChilds == null || PosibleChilds.Count == 0)
                    {
                        PosibleChilds = Obj.Object.Childs.Where(X => X.Kind == CurType).Select(X => X.Name).ToList();
                        Cs = new ChildSelector("Select field to use in the snippet", String.Format("Select field of DB Object: {0}.{1}", Obj.Object.Schema, Obj.Object.Name), PosibleChilds, false, 1);
                        if (Cs.ShowDialog() == DialogResult.OK)
                        {
                            foreach (string child in Cs.SelectedChilds)
                            {
                                if (String.IsNullOrEmpty(Obj.Alias))
                                {//object without an alias, must be a procedure or a scalar function
                                    if (String.IsNullOrEmpty(buff))
                                        buff += String.Format("{0}", child);
                                    else
                                        buff += String.Format("{0}{1}", sep, child);
                                }
                                else
                                {//object with an alias, must be a table, a view, or a table function
                                    if (String.IsNullOrEmpty(buff))
                                        buff += String.Format("{0}.{1}", Obj.Alias, child);
                                    else
                                        buff += String.Format("{0}{1}.{2}", sep, Obj.Alias, child);
                                }
                            }
                        }
                        else
                        {//cancelled the snippet processing
                            return bkup;
                        }
                    }
                    else
                    {
                        foreach (string child in Cs.SelectedChilds)
                        {
                            if (String.IsNullOrEmpty(Obj.Alias))
                            {//object without an alias, must be a procedure or a scalar function
                                if (String.IsNullOrEmpty(buff))
                                    buff += String.Format("{0}", child);
                                else
                                    buff += String.Format("{0}{1}", sep, child);
                            }
                            else
                            {//object with an alias, must be a table, a view, or a table function
                                if (String.IsNullOrEmpty(buff))
                                    buff += String.Format("{0}.{1}", Obj.Alias, child);
                                else
                                    buff += String.Format("{0}{1}.{2}", sep, Obj.Alias, child);
                            }
                        }
                    }
                    ProcessedSnippet = ProcessedSnippet.Replace(String.Format("${0}$", fname), buff);
                    buff = "";
                }
            }
            return ProcessedSnippet;
        }
        #endregion

        private List<ISqlObject> GetAliasesAndAuxiliarTables(string FullScript)
        {
            List<ISqlObject> Back = new List<ISqlObject>();
            TokenList Tokens = FullScript.GetTokens();
            List<int> TokensToDelete = new List<int>();

            #region First i have to process the full script, remove the "()" from the table valued functions
            for (int i = 0; i < Tokens.TokenCount; i++)
            {
                if (Tokens[i].Type == TokenType.WORD && DataProvider.IsTableValuedFunction(Tokens[i].Text) != null)
                {
                    //find the first opening parenthesis
                    while (i < Tokens.TokenCount && !Tokens[i].Text.StartsWith("("))
                        i++;
                    //then from there till the first closing parenthesis mark every token to be removed
                    for (int j = i; j < Tokens.TokenCount; j++)
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
            for (int i = TokensToDelete.Count - 1; i >= 0; i--)
            {
                Tokens.RemoveTokenAt(TokensToDelete[i]);
            }
            //if there was any token removed at all
            if (TokensToDelete.Count > 0 && Tokens.TokenCount > TokensToDelete[0])
            {//if the token right before the deletion and the next after the deletion are both empty spaces or from the same type
                for (int i = Tokens.TokenCount - 1; i >= 1; i--)
                {
                    if (Tokens[i].Type == Tokens[i - 1].Type)
                        Tokens.RemoveTokenAt(i);
                }
            }
            #endregion

            for (int i = 0; i < Tokens.TokenCount; i++)
            {
                Token CurToken = Tokens.GetToken(i);
                #region check for aliases, type: TableTypeObject Alias
                if ((i + 2) < Tokens.TokenCount)
                {
                    Token ProposedAlias = Tokens[i + 2];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        ISqlObject Aliased = DataProvider.IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)//if it is a table, view or a table value function
                        {
                            if (Back.Any(X => X.Name.Equals(ProposedAlias.Text)))
                            {
                                Back.Remove(Back.First(X => X.Name.Equals(ProposedAlias.Text)));
                            }

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
                if ((i + 4) < Tokens.TokenCount)
                {
                    Token ProposedAlias = Tokens[i + 4];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && Tokens[i + 2].Text.Equals("AS", StringComparison.CurrentCultureIgnoreCase) && Tokens[i + 3].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        ISqlObject Aliased = DataProvider.IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)
                        {
                            if (Back.Any(X => X.Name.Equals(ProposedAlias.Text)))
                            {
                                Back.Remove(Back.First(X => X.Name.Equals(ProposedAlias.Text)));
                            }

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
                    if (DataProvider.DbObjects.Any(X => X.Schema.Equals(Data[0], StringComparison.CurrentCultureIgnoreCase)))
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
            if (e.Button == MouseButtons.Left)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    Token selToken;
                    TokenList tokens = Query.Text.GetTokens();
                    selToken = tokens.GetTokenAtOffset(Query.CurrentOffset());

                    if (selToken != null && selToken.Type == TokenType.WORD)
                    {
                        ISqlObject Obj = DataProvider.IsSqlObject(selToken.Text);
                        if (Obj != null)
                        {
                            Parent.AddQueryForm(Obj.Name, Obj.Script, DataProvider);
                        }
                    }
                }
            }
        }

        #region Options of the contextual of the query editor
        private void goToDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script = Query.Text;
            int tokenIndex;
            TokenList tokens = script.GetTokens();
            Token t = tokens.GetTokenAtOffset(Query.CurrentOffset(), out tokenIndex);
            
            if (t == null)
                return;

            ISqlObject Obj = DataProvider.IsSqlObject(t.Text);
            if (Obj != null)
            {
                Parent.AddQueryForm(Obj.Name, Obj.Script, DataProvider);
            }
        }
        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in Query.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = true;
            }
            Query.Document.FoldingManager.UpdateFoldings(null, null);
            Query.Refresh();
        }
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in Query.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = false;
            }
            Query.Document.FoldingManager.UpdateFoldings(null, null);
            Query.Refresh();
        }
        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in Query.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = !fm.IsFolded;
            }
            Query.Document.FoldingManager.UpdateFoldings(null, null);
            Query.Refresh();
        }
        //all the reserved words to upper case
        private void toUpperCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script = Query.Text;
            TokenList tokens = script.GetTokens();
            foreach (Token t in tokens.List)
            {
                if (t.Type == TokenType.RESERVED || t.Type == TokenType.DATATYPE || t.Type == TokenType.BLOCKSTART || t.Type == TokenType.BLOCKEND)
                {
                    t.Text = t.Text.ToUpper();
                }
            }
            Query.Text = tokens.ToString();
            Query.Refresh();
        }
        //all the reserved words to lower case
        private void toLowerCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string script = Query.Text;
            TokenList tokens = script.GetTokens();
            foreach (Token t in tokens.List)
            {
                if (t.Type == TokenType.RESERVED || t.Type == TokenType.DATATYPE || t.Type == TokenType.BLOCKSTART || t.Type == TokenType.BLOCKEND)
                {
                    t.Text = t.Text.ToLower();
                }
            }
            Query.Text = tokens.ToString();
            Query.Refresh();
        }
        //upper case selection
        private void toUpperCseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Query.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                ISelection s = Query.ActiveTextAreaControl.SelectionManager.SelectionCollection[0];
                Query.Document.Replace(s.Offset, s.Length, s.SelectedText.ToUpper());
                Query.Refresh();
            }
            else
            {
                TokenList tokens = Query.Text.GetTokens();
                Token t = tokens.GetTokenAtOffset(Query.CurrentOffset());
                if(t != null && t.Text.Length > 0)
                    Query.Document.Replace(tokens.GetStartOf(t), tokens.GetLengthOf(t), t.Text.ToUpper());
            }
        }
        //lower case selection
        private void toLowerCaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Query.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                ISelection s = Query.ActiveTextAreaControl.SelectionManager.SelectionCollection[0];
                Query.Document.Replace(s.Offset, s.Length, s.SelectedText.ToLower());
                Query.Refresh();
            }
            else
            {
                TokenList tokens = Query.Text.GetTokens();
                Token t = tokens.GetTokenAtOffset(Query.CurrentOffset());
                if (t != null && t.Text.Length > 0)
                    Query.Document.Replace(tokens.GetStartOf(t), tokens.GetLengthOf(t), t.Text.ToLower());

            }
        }
        #endregion
    }
}
