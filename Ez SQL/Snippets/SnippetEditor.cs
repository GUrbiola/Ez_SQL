using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.Common_Code;
using ICSharpCode.TextEditor.Document;
using System.IO;

namespace Ez_SQL.Snippets
{
    /// <summary>
    /// A dockable panel that provides a full CRUD interface for managing SQL code snippets.
    /// Snippets are stored as <c>.snp</c> XML files in <c>DataStorageDir\Snippets\</c>.
    /// The panel loads and displays all saved snippets in a combo box; selecting one populates
    /// the name, shortcut, description, and SQL script editor fields (with SQL syntax highlighting).
    /// The user can create new snippets, edit and save existing ones, or delete them.
    /// </summary>
    public partial class SnippetEditor : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        /// <summary>The in-memory list of all loaded snippets, including the transient "New Snippet" entry.</summary>
        List<Snippet> Snippets;

        /// <summary>A placeholder snippet pre-populated in the combo box for creating a new entry.</summary>
        Snippet NewSnippet = new Snippet() { Name = "New Snippet", Description = "", Script = "", ShortCut = "" };

        /// <summary>Reference to the main form, used for tab management commands.</summary>
        private MainForm Parent;

        /// <summary>
        /// Gets the currently selected <see cref="Snippet"/> from the combo box,
        /// or <c>null</c> if nothing is selected.
        /// </summary>
        public Snippet SelectedSnippet
        {
            get
            {
                if (CmbSnippet.SelectedIndex >= 0)
                    return Snippets[CmbSnippet.SelectedIndex];
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SnippetEditor"/> and sets up SQL syntax highlighting.
        /// </summary>
        /// <param name="Parent">The main form, used for tab management commands.</param>
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
