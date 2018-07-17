using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.Extensions;
using ICSharpCode.TextEditor.Document;
using System.IO;

namespace Ez_SQL.Snippets
{
    public partial class SnippetEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        List<Snippet> Snippets;
        Snippet NewSnippet = new Snippet() { Name = "New Snippet", Description = "", Script = "", ShortCut = "" };
        private MainForm Parent;

        public Snippet SelectedSnippet
        {
            get
            {
                if (CmbSnippet.SelectedIndex >= 0)
                    return Snippets[CmbSnippet.SelectedIndex];
                return null;
            }
        }
        public SnippetEditor(MainForm Parent)
        {
            InitializeComponent();

            this.Parent = Parent;
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
        private void SnippetEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                LoadSnippets();
            }
        }
        private void LoadSnippets()
        {
            if (Snippets == null)
                Snippets = new List<Snippet>();
            else
                Snippets.Clear();
            Snippets.Add(NewSnippet);

            TxtName.Text = "";
            TxtDescription.Text = "";
            TxtShortcut.Text = "";
            ScriptText.Text = "";
            ScriptText.Refresh();

            if(Directory.Exists(String.Format("{0}\\Snippets", MainForm.DataStorageDir)))
            {
                string[] Files = Directory.GetFiles(String.Format("{0}\\Snippets", MainForm.DataStorageDir), "*.snp");
                foreach (string f in Files)
                {
                    try
                    {
                        Snippets.Add((Snippet)f.DeserializeFromXmlFile());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show
                        (
                            String.Format("Reading {0} file, raised exception: {1}", f.Substring(f.LastIndexOf('\\') + 1), ex.Message),
                            "Error while reading a snippet file", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error
                        );
                    }
                }
                Snippets.Sort((X, Y) => String.Compare(X.Name, Y.Name));
                CmbSnippet.Items.Clear();

                foreach (Snippet sn in Snippets)
                {
                    CmbSnippet.Items.Add(sn.Name);
                }
            }
        }
        private void CmbSnippet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedSnippet != null)
            {
                TxtName.Text = SelectedSnippet.Name;
                TxtDescription.Text = SelectedSnippet.Description;
                TxtShortcut.Text = SelectedSnippet.ShortCut;
                ScriptText.Text = SelectedSnippet.Script;
                ScriptText.Refresh();
            }
            else 
            {
                TxtName.Text = "";
                TxtDescription.Text = "";
                TxtShortcut.Text = "";
                ScriptText.Text = "";
                ScriptText.Refresh();
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TxtName.Text))
            {
                MessageBox.Show("Name can not be empty");
                return;
            }
            if (String.IsNullOrEmpty(TxtShortcut.Text))
            {
                MessageBox.Show("Shortcut can not be empty");
                return;
            }
            if (String.IsNullOrEmpty(ScriptText.Text))
            {
                MessageBox.Show("Script can not be empty");
                return;
            }

            GenerateSnippet().SerializeToXmlFile(String.Format("{0}\\Snippets\\{1}.snp", MainForm.DataStorageDir, TxtName.Text));
            LoadSnippets();

        }
        private Snippet GenerateSnippet()
        {
            Snippet Back = new Snippet();
            Back.Name = TxtName.Text;
            Back.Script = ScriptText.Text;
            Back.ShortCut = TxtShortcut.Text;
            Back.Description = TxtDescription.Text;
            return Back;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedSnippet != null)
            {
                if (MessageBox.Show("Do you want to delete the current snippet(" + SelectedSnippet.Name + ")?", "Delete snippet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    File.Delete(String.Format("{0}\\Snippets\\{1}.snp", MainForm.DataStorageDir, SelectedSnippet.Name));
                    LoadSnippets();
                }
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
