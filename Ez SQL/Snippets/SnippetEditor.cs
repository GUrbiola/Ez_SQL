using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.Snippets
{
    public partial class SnippetEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public SnippetEditor()
        {
            InitializeComponent();

            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.ExecDir + "\\SintaxHighLight\\"));
                ScriptText.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
