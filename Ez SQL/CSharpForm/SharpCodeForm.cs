using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Ez_SQL.MultiQuery;
using System.Drawing.Drawing2D;
using Ez_SQL.Extensions;
using Ez_SQL.MultiQueryForm;
using ICSharpCode.TextEditor.Document;
using System.IO;
using ICSharpCode.TextEditor;

namespace Ez_SQL.CSharp
{
    public partial class SharpCodeForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        SearchAndReplace _findForm;
        private bool _lastSearchLoopedAround = false, _lastSearchWasBackward = false;
        private int ToRefresh;
        Keys LastKeyPressed;

        public SharpCodeForm(string Script)
        {
            InitializeComponent();

            SharpText.Text = Script;

            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
                SharpText.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("C#");
                SharpText.Document.FormattingStrategy = new Ez_SQL.TextEditorClasses.SqlBracketMatcher();
                SharpText.Document.FoldingManager.FoldingStrategy = new Ez_SQL.TextEditorClasses.CSharpFoldingStrategy();
                SharpText.Document.FoldingManager.UpdateFoldings(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            #region Code to assign the method that will handle the key press event and the method to refresh the folding
            SharpText.ActiveTextAreaControl.TextArea.DoProcessDialogKey += SharpText_DoProcessDialogKey;
            SharpText.Document.DocumentChanged += SharpText_DocumentChanged;
            #endregion

        }

        bool SharpText_DoProcessDialogKey(Keys keyData)//Process hot keys
        {
            bool NoEcho = true, Echo = false;

            // Echo == true, then NoEcho == false
            #region Key shortcut processing
            switch (keyData)
            {
                case Keys.Control | Keys.C://Comment selection / Collapse outlining
                    if (LastKeyPressed == (Keys.Control | Keys.K))
                    {//comment selection, or current line
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (SharpText.Enabled)
                            BtnComment_Click(null, null);
                        return NoEcho;
                    }
                    else if (LastKeyPressed == (Keys.Control | Keys.O))
                    {//collapse outlining
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (SharpText.Enabled)
                            collapseToolStripMenuItem_Click(null, null);
                        return NoEcho;
                    }
                    return Echo;
                case Keys.Control | Keys.U://Uncomment selection / to lower case the selection
                    if (LastKeyPressed == (Keys.Control | Keys.K))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (SharpText.Enabled)
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
                    if (SharpText.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected)
                    {
                        lookFor = SharpText.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                    }
                    else
                    {
                        Token selToken;
                        TokenList tokens = SharpText.Text.GetTokens();
                        selToken = tokens.GetTokenAtOffset(SharpText.CurrentOffset());
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
                case Keys.Control | Keys.E://Expand outlining
                    if (LastKeyPressed == (Keys.Control | Keys.O))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (SharpText.Enabled)
                            expandToolStripMenuItem_Click(null, null);
                        return NoEcho;
                    }
                    return NoEcho;
                case Keys.Control | Keys.T://Toggle outlining
                    if (LastKeyPressed == (Keys.Control | Keys.O))
                    {
                        LastKeyPressed = Keys.Space;//clean the last key pressed aux var, to avoid wrong behavior
                        if (SharpText.Enabled)
                            toggleToolStripMenuItem_Click(null, null);
                        return NoEcho;
                    }
                    return NoEcho;
            }
            #endregion
            LastKeyPressed = keyData;
            return Echo;
        }
        private void BtnComment_Click(object sender, EventArgs e)
        {
            int startLine, endLine, i;
            string comment = "//";
            if (SharpText.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                foreach (ISelection selection in SharpText.ActiveTextAreaControl.SelectionManager.SelectionCollection)
                {
                    startLine = selection.StartPosition.Y;
                    endLine = selection.EndPosition.Y;

                    for (i = endLine; i >= startLine; --i)
                    {
                        LineSegment line = SharpText.Document.GetLineSegment(i);
                        if (selection != null && i == endLine && line.Offset == selection.Offset + selection.Length)
                        {
                            --endLine;
                            continue;
                        }

                        string lineText = SharpText.Document.GetText(line.Offset, line.Length);
                        SharpText.Document.Insert(line.Offset, comment);
                    }
                }
            }
            else
            {
                startLine = SharpText.ActiveTextAreaControl.TextArea.Caret.Line;
                endLine = SharpText.ActiveTextAreaControl.TextArea.Caret.Line;

                for (i = endLine; i >= startLine; --i)
                {
                    LineSegment line = SharpText.Document.GetLineSegment(i);
                    if (line.ToString().Trim().Length == 0)
                    {
                        --endLine;
                        continue;
                    }

                    string lineText = SharpText.Document.GetText(line.Offset, line.Length);
                    SharpText.Document.Insert(line.Offset, comment);
                }
            }

        }
        private void BtnUncomment_Click(object sender, EventArgs e)
        {
            int startLine, endLine, i;
            string comment = "//";

            if (SharpText.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                foreach (ISelection selection in SharpText.ActiveTextAreaControl.SelectionManager.SelectionCollection)
                {
                    startLine = selection.StartPosition.Y;
                    endLine = selection.EndPosition.Y;

                    for (i = endLine; i >= startLine; --i)
                    {
                        LineSegment line = SharpText.Document.GetLineSegment(i);
                        if (selection != null && i == endLine && line.Offset == selection.Offset + selection.Length)
                        {
                            --endLine;
                            continue;
                        }

                        string lineText = SharpText.Document.GetText(line.Offset, line.Length);
                        if (lineText.Trim().StartsWith(comment))
                            SharpText.Document.Remove(line.Offset + lineText.IndexOf(comment), comment.Length);
                    }
                }
            }
            else
            {
                startLine = SharpText.ActiveTextAreaControl.TextArea.Caret.Line;
                endLine = SharpText.ActiveTextAreaControl.TextArea.Caret.Line;

                for (i = endLine; i >= startLine; --i)
                {
                    LineSegment line = SharpText.Document.GetLineSegment(i);
                    if (line.ToString().Trim().Length == 0)
                    {
                        --endLine;
                        continue;
                    }

                    string lineText = SharpText.Document.GetText(line.Offset, line.Length);
                    if (lineText.Trim().StartsWith(comment))
                        SharpText.Document.Remove(line.Offset + lineText.IndexOf(comment), comment.Length);

                }
            }
        }
        private void BtnBookmark_Click(object sender, EventArgs e)
        {
            DoEditAction(SharpText, new ICSharpCode.TextEditor.Actions.ToggleBookmark());
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
            DoEditAction(SharpText, new ICSharpCode.TextEditor.Actions.GotoPrevBookmark(bookmark => true));
        }
        private void BtnNext_Click(object sender, EventArgs e)
        {
            DoEditAction(SharpText, new ICSharpCode.TextEditor.Actions.GotoNextBookmark(bookmark => true));
        }
        private void BtnClearBookmarks_Click(object sender, EventArgs e)
        {
            DoEditAction(SharpText, new ICSharpCode.TextEditor.Actions.ClearAllBookmarks(bookmark => true));
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saver = new SaveFileDialog();
            saver.Title = "Save to File";
            saver.AddExtension = true;
            saver.AutoUpgradeEnabled = true;
            saver.Filter = "C# File|*.cs|Text File|*.txt|Any File|*.*";
            if (saver.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter Wr = new StreamWriter(saver.FileName))
                {
                    Wr.Write(SharpText.Text);
                    Wr.Close();
                }
                MessageBox.Show("Archivo Guardado Correctamente");
            }
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.Title = "Load cs file";
            opener.Filter = "C# File|*.cs|Text File|*.txt|Any File|*.*";
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
                SharpText.Text = sb.ToString();
                SharpText.Refresh();
            }

        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (_findForm == null)
                _findForm = new SearchAndReplace();
            _findForm.ShowFor(this, SharpText, false);
        }
        public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
        {
            if (_findForm == null)
                _findForm = new SearchAndReplace();


            TextEditorSearcher _search = new TextEditorSearcher();
            _lastSearchWasBackward = searchBackward;
            _search.Document = SharpText.Document;
            _search.LookFor = _findForm.LookFor;
            _search.MatchCase = _findForm.MatchCase;
            _search.MatchWholeWordOnly = _findForm.MatchWholeWordOnly;

            var caret = SharpText.ActiveTextAreaControl.Caret;
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
            TextLocation p1 = SharpText.Document.OffsetToPosition(range.Offset);
            TextLocation p2 = SharpText.Document.OffsetToPosition(range.Offset + range.Length);
            SharpText.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
            SharpText.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
            SharpText.ActiveTextAreaControl.Caret.Position = SharpText.Document.OffsetToPosition(range.Offset + range.Length);
        }
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SharpText.Text))
            {
                Clipboard.Clear();
                Clipboard.SetText(SharpText.Text);
                MsgLabel.Text = String.Format("Code Copied - {0}", DateTime.Now.ToString("hh:mm:ss.ff"));
            }
        }
        
        #region Code to refresh the expand/collapse status of the text
        void SharpText_DocumentChanged(object sender, DocumentEventArgs e)
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
                SharpText.Document.FoldingManager.UpdateFoldings(null, null);
                FoldingRefresher.Enabled = false;
                ToRefresh = 5;
            }
        }
        #endregion

        private void SharpCodeForm_Load(object sender, EventArgs e)
        {

        }
        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in SharpText.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = true;
            }
            SharpText.Document.FoldingManager.UpdateFoldings(null, null);
            SharpText.Refresh();
        }
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in SharpText.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = false;
            }
            SharpText.Document.FoldingManager.UpdateFoldings(null, null);
            SharpText.Refresh();
        }
        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var fm in SharpText.Document.FoldingManager.FoldMarker)
            {
                fm.IsFolded = !fm.IsFolded;
            }
            SharpText.Document.FoldingManager.UpdateFoldings(null, null);
            SharpText.Refresh();
        }
        //upper case selection
        private void toUpperCseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharpText.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                ISelection s = SharpText.ActiveTextAreaControl.SelectionManager.SelectionCollection[0];
                SharpText.Document.Replace(s.Offset, s.Length, s.SelectedText.ToUpper());
                SharpText.Refresh();
            }
            else
            {
                TokenList tokens = SharpText.Text.GetTokens();
                Token t = tokens.GetTokenAtOffset(SharpText.CurrentOffset());
                if (t != null && t.Text.Length > 0)
                    SharpText.Document.Replace(tokens.GetStartOf(t), tokens.GetLengthOf(t), t.Text.ToUpper());
            }
        }
        //lower case selection
        private void toLowerCaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (SharpText.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                ISelection s = SharpText.ActiveTextAreaControl.SelectionManager.SelectionCollection[0];
                SharpText.Document.Replace(s.Offset, s.Length, s.SelectedText.ToLower());
                SharpText.Refresh();
            }
            else
            {
                TokenList tokens = SharpText.Text.GetTokens();
                Token t = tokens.GetTokenAtOffset(SharpText.CurrentOffset());
                if (t != null && t.Text.Length > 0)
                    SharpText.Document.Replace(tokens.GetStartOf(t), tokens.GetLengthOf(t), t.Text.ToLower());

            }
        }


    }
}
