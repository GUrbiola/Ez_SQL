using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;
using Ez_SQL.Common_Code;
using Ez_SQL.EzConfig.ColorConfig;
using Ez_SQL.EzConfig.ColorConfig.Nodes;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.EzConfig
{
    public partial class SyntaxColorsConfigurator : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private TextEditorColorConfig colorConfig;
        private bool _preventExpand = false;
        private DateTime _lastMouseDown = DateTime.Now;
        private MainForm Parent;

        public SyntaxColorsConfigurator(MainForm Parent)
        {
            InitializeComponent();
            this.Parent = Parent;
            txtEditorPreview.Document.ReadOnly = true;
        }
        private void SyntaxColorsConfigurator_Load(object sender, EventArgs e)
        {
            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
                txtEditorPreview.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
                txtEditorPreview.Document.FormattingStrategy = new Ez_SQL.TextEditorClasses.SqlBracketMatcher();
                txtEditorPreview.Document.FoldingManager.FoldingStrategy = new Ez_SQL.TextEditorClasses.SqlFolder();
                txtEditorPreview.Document.FoldingManager.UpdateFoldings(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            colorConfig = new TextEditorColorConfig(MainForm.DataStorageDir + "\\SintaxHighLight\\SQL.xshd");
            LoadTree();
            cmbStyles.SelectedIndex = 0;
        }
        private void LoadTree()
        {
            syntaxTreeView.Nodes.Clear();

            //create the root node
            SyntaxNode root = new SyntaxNode(NodeType.Root);
            root.Text       = "Configuration Root";

            //create the environment node
            SyntaxNode environment = new SyntaxNode(NodeType.Environment);
            environment.Text       = "Environment";

            //create the environment options
            SyntaxNode Default = new SyntaxNode(NodeType.EnvironmentOption);
            Default.Text       = "Font and Background";
            Default.Color      = colorConfig.Environment.FontColor;
            Default.BackGround = colorConfig.Environment.BackgroundColor;
            environment.Nodes.Add(Default);
            
            SyntaxNode LineNumbers = new SyntaxNode(NodeType.EnvironmentOption);
            LineNumbers.Text       = "Line Numbers";
            LineNumbers.Color      = colorConfig.Environment.LineNumberFontColor;
            LineNumbers.BackGround = colorConfig.Environment.LineNumberBackgroundColor;
            environment.Nodes.Add(LineNumbers);
            
            SyntaxNode Selection = new SyntaxNode(NodeType.EnvironmentOption);
            Selection.Text       = "Selection";
            Selection.Color      = null;
            Selection.BackGround = colorConfig.Environment.SelectionColor;
            environment.Nodes.Add(Selection);
            
            environment.Expand();
            root.Nodes.Add(environment);

            //create the digits node
            SyntaxNode Digits = new SyntaxNode(NodeType.Digits);
            Digits.Text       = "Digits";
            Digits.Bold       = colorConfig.Digits.Bold;
            Digits.Italic     = colorConfig.Digits.Italic;
            Digits.Color      = colorConfig.Digits.Color;
            root.Nodes.Add(Digits);

            //create the rule sets
            SyntaxNode RuleSets      = new SyntaxNode(NodeType.RuleSets);
            RuleSets.Text            = "Rule Sets";

            foreach (RuleSet rs in colorConfig.RuleSets)
            {
                SyntaxNode ruleSetNode = new SyntaxNode(NodeType.RuleSet);
                ruleSetNode.Text       = rs.Name;
                ruleSetNode.IgnoreCase = true;
                if (!rs.IsMainRuleSet)
                {
                    ruleSetNode.NodeName = rs.Name;
                }
                else
                {
                    ruleSetNode.NodeName = null;
                }

                foreach (ConfigRule cr in rs.Rules.Where(x => x.Type.Equals("Span", StringComparison.CurrentCultureIgnoreCase)))
                {
                    SyntaxNode span  = new SyntaxNode(NodeType.Span);
                    span.Text        = cr.Name;
                    span.ToolTipText = "Span Item";
                    span.Rule        = String.IsNullOrEmpty(cr.Rule) ? null : cr.Rule;
                    span.NodeName    = null;
                    span.Bold        = cr.Bold;
                    span.Italic      = cr.Italic;
                    span.Color       = cr.Color;
                    span.StopAtEOL   = cr.StopAtEOL;
                    ruleSetNode.Nodes.Add(span);
                }

                foreach (ConfigRule cr in rs.Rules.Where(x => x.Type.Equals("KeyWords", StringComparison.CurrentCultureIgnoreCase)))
                {
                    SyntaxNode kwords  = new SyntaxNode(NodeType.KeyWords);
                    kwords.Text        = cr.Name;
                    kwords.ToolTipText = "Key Words Item";
                    kwords.NodeName    = null;
                    kwords.Bold        = cr.Bold;
                    kwords.Italic      = cr.Italic;
                    kwords.Color       = cr.Color;
                    kwords.Words       = new List<string>();
                    foreach (string word in cr.Words)
                    {
                        kwords.Words.Add(word);
                        SyntaxNode kword = new SyntaxNode(NodeType.KeyWord);
                        kword.Text = word;
                        kwords.Nodes.Add(kword);
                    }
                    ruleSetNode.Nodes.Add(kwords);
                }
                RuleSets.Nodes.Add(ruleSetNode);
            }
            RuleSets.Expand();
            root.Nodes.Add(RuleSets);
            root.Expand();

            syntaxTreeView.Nodes.Add(root);
        }
        private void syntaxTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel       = _preventExpand;
            _preventExpand = false;
        }
        private void syntaxTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel       = _preventExpand;
            _preventExpand = false;
        }
        private void syntaxTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            int delta      = (int)DateTime.Now.Subtract(_lastMouseDown).TotalMilliseconds;
            _preventExpand = (delta < SystemInformation.DoubleClickTime);
            _lastMouseDown = DateTime.Now;
        }
        private void syntaxTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SyntaxNode selected = syntaxTreeView.SelectedNode as SyntaxNode;
            if (selected != null)
            {
                ConfigItemDialog dialog = new ConfigItemDialog(selected);
                if (dialog.IsEditable)
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        dialog.UpdateNodeData(ref selected);

                        switch (selected.NodeType)
                        {
                            case NodeType.Root:
                            case NodeType.Environment:
                            case NodeType.RuleSets:
                            case NodeType.KeyWord://Nonthing needs to be refresehd since this options dont allow changes
                                break;
                            case NodeType.EnvironmentOption:
                                switch (selected.Text.ToLower())
                                {
                                    case "font and background":
                                        colorConfig.Environment.FontColor       = selected.Color ?? System.Drawing.Color.Black;
                                        colorConfig.Environment.BackgroundColor = selected.BackGround ?? System.Drawing.Color.White;
                                        break;
                                    case "line numbers":
                                        colorConfig.Environment.LineNumberFontColor       = selected.Color ?? System.Drawing.Color.Black;
                                        colorConfig.Environment.LineNumberBackgroundColor = selected.BackGround ?? System.Drawing.Color.White;
                                        break;
                                    case "selection":
                                        colorConfig.Environment.SelectionColor = selected.BackGround ?? System.Drawing.Color.Black;
                                        break;
                                }
                                break;
                            case NodeType.Digits:
                                colorConfig.Digits.Bold   = selected.Bold ?? false;
                                colorConfig.Digits.Italic = selected.Italic ?? false;
                                colorConfig.Digits.Color  = selected.Color ?? System.Drawing.Color.Black;
                                break;
                            case NodeType.RuleSet:
                                if (String.IsNullOrEmpty(selected.NodeName))
                                {//main rule
                                    colorConfig.RuleSets[0].IgnoreCase = selected.IgnoreCase ?? false;
                                }
                                else
                                {
                                    colorConfig.RuleSets[1].IgnoreCase = selected.IgnoreCase ?? false;
                                    colorConfig.RuleSets[1].Name = selected.Name;
                                }
                                break;
                            case NodeType.Span:
                                if (colorConfig.RuleSets[0].Rules.Any(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("Span", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    ConfigRule ptr = colorConfig.RuleSets[0].Rules.FirstOrDefault(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("Span", StringComparison.CurrentCultureIgnoreCase));
                                    ptr.Rule       = selected.Rule;
                                    ptr.Bold       = selected.Bold ?? false;
                                    ptr.Italic     = selected.Italic ?? false;
                                    ptr.Color      = selected.Color ?? System.Drawing.Color.Black;
                                    ptr.StopAtEOL  = selected.StopAtEOL ?? false;
                                }
                                else if (colorConfig.RuleSets[1].Rules.Any(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("Span", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    ConfigRule ptr = colorConfig.RuleSets[1].Rules.FirstOrDefault(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("Span", StringComparison.CurrentCultureIgnoreCase));
                                    ptr.Rule       = selected.Rule;
                                    ptr.Bold       = selected.Bold ?? false;
                                    ptr.Italic     = selected.Italic ?? false;
                                    ptr.Color      = selected.Color ?? System.Drawing.Color.Black;
                                    ptr.StopAtEOL  = selected.StopAtEOL ?? false;
                                }
                                break;
                            case NodeType.KeyWords:
                                if (colorConfig.RuleSets[0].Rules.Any(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("KeyWords", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    ConfigRule ptr = colorConfig.RuleSets[0].Rules.FirstOrDefault(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("KeyWords", StringComparison.CurrentCultureIgnoreCase));
                                    ptr.Bold       = selected.Bold ?? false;
                                    ptr.Italic     = selected.Italic ?? false;
                                    ptr.Color      = selected.Color ?? System.Drawing.Color.Black;
                                    ptr.Words      = selected.Words;
                                }
                                else if (colorConfig.RuleSets[1].Rules.Any(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("KeyWords", StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    ConfigRule ptr = colorConfig.RuleSets[1].Rules.FirstOrDefault(x => x.Name.Equals(selected.Text, StringComparison.CurrentCultureIgnoreCase) && x.Type.Equals("KeyWords", StringComparison.CurrentCultureIgnoreCase));
                                    ptr.Bold       = selected.Bold ?? false;
                                    ptr.Italic     = selected.Italic ?? false;
                                    ptr.Color      = selected.Color ?? System.Drawing.Color.Black;
                                    ptr.Words      = selected.Words;
                                }
                                break;
                        }
                        btnRefreshPreview_Click(null, null);
                    }
                }

            }
        }
        private void btnRefreshPreview_Click(object sender, EventArgs e)
        {
            StreamWriter helper = new StreamWriter(MainForm.DataStorageDir + "\\SintaxHighLight\\Preview.xshd");
            helper.Write(colorConfig.ToString(true));
            helper.Flush();
            helper.Close();

            HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
            txtEditorPreview.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("Preview");
            txtEditorPreview.Document.FormattingStrategy = new Ez_SQL.TextEditorClasses.SqlBracketMatcher();
            txtEditorPreview.Document.FoldingManager.FoldingStrategy = new Ez_SQL.TextEditorClasses.SqlFolder();
            txtEditorPreview.Document.FoldingManager.UpdateFoldings(null, null);
            txtEditorPreview.Refresh();
        }
        private void btnRestoreToDefault_Click(object sender, EventArgs e)
        {
            if (File.Exists(MainForm.DataStorageDir + "\\SintaxHighLight\\Preview.xshd"))
                File.Delete(MainForm.DataStorageDir + "\\SintaxHighLight\\Preview.xshd");
            colorConfig = new TextEditorColorConfig("");
            LoadTree();
            btnRefreshPreview_Click(null, null);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            StreamWriter helper = new StreamWriter(MainForm.DataStorageDir + "\\SintaxHighLight\\SQL.xshd");
            helper.Write(colorConfig.ToString());
            helper.Flush();
            helper.Close();
        }

        private void cmbStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStyles.SelectedIndex > 0)
            {
                //Current - 0
                //Default - 1
                //Son of Obsidian - 2
                //Selenitic - 3
                //Old Timer(Turbo C++) - 4

                switch (cmbStyles.SelectedIndex)
                {
                    case 1:
                        colorConfig = new TextEditorColorConfig("");
                        LoadTree();
                        btnRefreshPreview_Click(null, null);
                        break;
                    case 2:
                        using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\Preview.xshd", MainForm.DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            Writer.Write(Properties.Resources.Almost_Son_of_Obsidian, 0, Properties.Resources.Almost_Son_of_Obsidian.Length);
                            Writer.Close();
                        }
                        break;
                    case 3:
                        using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\Preview.xshd", MainForm.DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            Writer.Write(Properties.Resources.Almost_Selenitic, 0, Properties.Resources.Almost_Selenitic.Length);
                            Writer.Close();
                        }
                        break;
                    case 4:
                        using (FileStream Writer = new FileStream(String.Format("{0}\\SintaxHighLight\\Preview.xshd", MainForm.DataStorageDir), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            Writer.Write(Properties.Resources.TurboC, 0, Properties.Resources.TurboC.Length);
                            Writer.Close();
                        }
                        break;

                }
                colorConfig = new TextEditorColorConfig(MainForm.DataStorageDir + "\\SintaxHighLight\\Preview.xshd");
                LoadTree();
                btnRefreshPreview_Click(null, null);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnector conx = new SqlConnector("Data Source=.\\SQLSERVER;Initial Catalog=CompensationDB;Integrated Security=True;Persist Security Info=True");
            TokenList tl = txtEditorPreview.Text.GetTokens();
            tl.ParseTokens(conx);
            MessageBox.Show("Tokenizing completed", "Parsing SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
