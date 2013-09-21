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
        private string CurWord;
        SearchAndReplace _findForm;
        private bool _lastSearchLoopedAround = false, _lastSearchWasBackward = false;
        TextLocation CurPos;
        int CurOffset;
        private int ToRefresh;

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

            #region Codigo para Inicializar los eventos del control de texto
            SharpText.ActiveTextAreaControl.TextArea.DoProcessDialogKey += SharpText_DoProcessDialogKey;
            SharpText.Document.DocumentChanged += SharpText_DocumentChanged;
            #endregion

            SharpText.ActiveTextAreaControl.TextArea.MouseClick += MouseClick;
        }

        void MouseClick(object sender, MouseEventArgs e)
        {
            CurWord = GetWordAtOffset();
        }
        bool SharpText_DoProcessDialogKey(Keys keyData)// Echo == true, then NoEcho == false
        {
            #region Procesado de Atajos, automaticamente deshabilitan el intellisense
            switch (keyData)
            {
                case Keys.Control | Keys.K:
                    //comentar seleccion
                    if (SharpText.Enabled)
                        BtnComment_Click(null, null);
                    return true;
                case Keys.Control | Keys.U:
                    //decomentar seleccion
                    if (SharpText.Enabled)
                        BtnUncomment_Click(null, null);
                    return true;
                case Keys.Control | Keys.F:
                    //abrir ventana de busqueda
                    BtnSearch_Click(null, null);
                    return true;
                case Keys.F3:
                    //buscar siguiente, silenciosamente
                    FindNext(true, false, String.Format("Cadena de Texto: {0}, no encontrada.", _findForm.LookFor));
                    return true;
                case Keys.Shift | Keys.F3:
                    //buscar anterior silenciosamente
                    FindNext(true, true, String.Format("Cadena de Texto: {0}, no encontrada.", _findForm.LookFor));
                    return true;
                case Keys.F2:
                    //poner bookmark
                    BtnBookmark_Click(null, null);
                    return true;
                case Keys.Shift | Keys.F2:
                    //ir al bookmark anterior
                    BtnPrevious_Click(null, null);
                    return true;
                case Keys.Control | Keys.F2:
                    //ir al siguiente bookmark
                    BtnNext_Click(null, null);
                    return true;
                case Keys.Control | Keys.Shift | Keys.F2:
                    //limpiar los bookmarks
                    BtnClearBookmarks_Click(null, null);
                    return true;
                case Keys.Control | Keys.G:
                    BtnSave_Click(null, null);
                    return true;
                case Keys.Control | Keys.L:
                    BtnLoad_Click(null, null);
                    return true;
            }
            #endregion

            return false;
        }
        private void SharpText_Load(object sender, EventArgs e)
        {
            //MainContainer.Panel2Collapsed = true;
            //if (!Conx.TestConnection())
            //{
            //    MessageBox.Show("No fue posible la conexion a la Base de Datos", "Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    BtnExecute.Enabled = false;
            //}
        }
        #region Codigo de los Botones de la Barra de Herramientas de esta ventana
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
            saver.Title = "Guardar Codigo de CSharp";
            saver.AddExtension = true;
            saver.AutoUpgradeEnabled = true;
            saver.Filter = "Archivo de CSharp|*.cs|Archivo de Texto|*.txt|Cualquier Archivo|*.*";
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
            opener.Title = "Cargar Codigo de CSharp";
            opener.Filter = "Archivo de CSharp|*.cs|Archivo de Texto|*.txt|Cualquier Archivo|*.*";
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
            //_findForm = new SearchAndReplace();
            //_findForm.ShowFor(this, SharpText, false);
        }
        public TextRange FindNext(bool viaF3, bool searchBackward, string messageIfNotFound)
        {
            if (string.IsNullOrEmpty(CurWord))
            {
                MessageBox.Show("Seleccione la palabra a buscar...");
                return null;
            }
            TextEditorSearcher _search = new TextEditorSearcher();
            _lastSearchWasBackward = searchBackward;
            _search.Document = SharpText.Document;
            _search.LookFor = CurWord;
            _search.MatchCase = false;
            _search.MatchWholeWordOnly = false;

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
                MsgLabel.Text = String.Format("Codigo Copiado - {0}", DateTime.Now.ToString("hh:mm:ss.ff"));
            }
        }
        #endregion
        #region Codigo para Refrescar la Expansion/Contraccion del texto
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
        #region Codigo de busqueda y seleccion de codigo y texto
        public int SelectionStart
        {
            get { return SharpText.ActiveTextAreaControl.Caret.Offset; }
        }
        private string GetWordAtOffset()
        {
            TextWord CW;
            CW = SharpText.Document.GetLineSegment(SharpText.ActiveTextAreaControl.Caret.Line).GetWord(SharpText.ActiveTextAreaControl.Caret.Column);
            if (CW != null && CW.Word.Trim().Length > 0)
            {
                CurPos = new TextLocation(CW.Offset, SharpText.ActiveTextAreaControl.Caret.Line);
                CurOffset = SharpText.Document.PositionToOffset(CurPos);
            }
            else
            {
                if (SharpText.ActiveTextAreaControl.Caret.Column > 0)
                    CW = SharpText.Document.GetLineSegment(SharpText.ActiveTextAreaControl.Caret.Line).GetWord(SharpText.ActiveTextAreaControl.Caret.Column - 1);
                if (CW != null && CW.Word.Trim().Length > 0)
                {
                    CurPos = new TextLocation(CW.Offset, SharpText.ActiveTextAreaControl.Caret.Line);
                    CurOffset = SharpText.Document.PositionToOffset(CurPos);
                }
                else
                {
                    CurPos.X = SharpText.ActiveTextAreaControl.Caret.Column;
                    CurPos.Y = SharpText.ActiveTextAreaControl.Caret.Line;
                    CurOffset = SharpText.Document.PositionToOffset(CurPos);
                }
            }
            return CW == null ? "" : CW.Word;
        }
        private string GetWordAtOffsetMinusOne()
        {
            TextWord CW;
            CW = SharpText.Document.GetLineSegment(SharpText.ActiveTextAreaControl.Caret.Line).GetWord(SharpText.ActiveTextAreaControl.Caret.Column - 1);
            if (CW != null && CW.Word.Trim().Length > 0)
            {
                CurPos = new TextLocation(CW.Offset, SharpText.ActiveTextAreaControl.Caret.Line);
                CurOffset = SharpText.Document.PositionToOffset(CurPos);
            }
            else
            {
                CurPos.X = SharpText.ActiveTextAreaControl.Caret.Column;
                CurPos.Y = SharpText.ActiveTextAreaControl.Caret.Line;
                CurOffset = SharpText.Document.PositionToOffset(CurPos);
            }
            return CW == null ? "" : CW.Word;
        }
        private string GetWordAtOffset(TextLocation Position)
        {
            TextWord CW;
            int offset;
            string line;
            try
            {
                line = SharpText.Document.GetLineSegment(Position.Line).ToString() ?? "";
                if (line.Length < Position.Column)
                    return "";
                CW = SharpText.Document.GetLineSegment(Position.Line).GetWord(Position.Column);
            }
            catch (Exception)
            {
                return "";
            }
            return CW == null ? "" : CW.Word;
        }
        private string GetWordAtOffsetMinus(int BackSpaces)
        {
            TextWord CW;

            if (SharpText.ActiveTextAreaControl.Caret.Column - BackSpaces >= 0)
                CW = SharpText.Document.GetLineSegment(SharpText.ActiveTextAreaControl.Caret.Line).GetWord(SharpText.ActiveTextAreaControl.Caret.Column - BackSpaces);
            else
                return "";

            if (CW != null && CW.Word.Trim().Length > 0)
            {
                ;
            }
            else
            {
                if (SharpText.ActiveTextAreaControl.Caret.Column > 0)
                    CW = SharpText.Document.GetLineSegment(SharpText.ActiveTextAreaControl.Caret.Line).GetWord(SharpText.ActiveTextAreaControl.Caret.Column - BackSpaces);
            }
            return CW == null ? "" : CW.Word;
        }
        private int PreviousIndexOf(string character)
        {
            for (int i = SelectionStart; i > 0; i--)
                if (SharpText.Text.Substring(i - 1, 1) == character)
                    return i;
            return 0;
        }
        #endregion

        private void SharpCodeForm_Load(object sender, EventArgs e)
        {

        }


    }
}
