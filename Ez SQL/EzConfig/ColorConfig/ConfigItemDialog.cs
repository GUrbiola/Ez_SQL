using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.EzConfig.ColorConfig.Nodes;

namespace Ez_SQL.EzConfig.ColorConfig
{
    public partial class ConfigItemDialog : Form
    {
        public NodeType NodeType { get; set; }
        public string NodeName
        {
            get
            {
                if (txtName.Enabled && !String.IsNullOrEmpty(txtName.Text))
                    return txtName.Text;
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _IsEditable = true;
                    txtName.Enabled = true;
                    txtName.Text      = value;
                    labName.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    txtName.Enabled   = false;
                    txtName.Text      = "";
                    labName.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public bool? StopAtEOL
        {
            get
            {
                if (chkEOL.Enabled)
                    return chkEOL.Checked;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    chkEOL.Enabled = true;
                    chkEOL.Checked   = value ?? false;
                    labEOL.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    chkEOL.Enabled   = false;
                    chkEOL.Checked   = false;
                    labEOL.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public bool? Italic 
        {
            get
            {
                if (chkItalic.Enabled)
                    return chkItalic.Checked;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    chkItalic.Enabled = true;
                    chkItalic.Checked   = value ?? false;
                    labItalic.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    chkItalic.Enabled   = false;
                    chkItalic.Checked   = false;
                    labItalic.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public bool? Bold
        {
            get
            {
                if (chkBold.Enabled)
                    return chkBold.Checked;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    chkBold.Enabled = true;
                    chkBold.Checked   = value ?? false;
                    labBold.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    chkBold.Enabled   = false;
                    chkBold.Checked   = false;
                    labBold.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public bool? IgnoreCase
        {
            get
            {
                if (chkIgnoreCase.Enabled)
                    return chkIgnoreCase.Checked;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    chkIgnoreCase.Enabled = true;
                    chkIgnoreCase.Checked   = value ?? false;
                    labIgnoreCase.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    chkIgnoreCase.Enabled   = false;
                    chkIgnoreCase.Checked   = false;
                    labIgnoreCase.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public string Rule
        {
            get
            {
                if (txtRule.Enabled && !String.IsNullOrEmpty(txtRule.Text))
                    return txtRule.Text;
                return null;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _IsEditable = true;
                    txtRule.Enabled = true;
                    txtRule.Text      = value;
                    labRule.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    txtRule.Enabled   = false;
                    txtRule.Text      = "";
                    labRule.ForeColor = System.Drawing.Color.DarkGray;
                }
            }
        }
        public Color? Color
        {
            get
            {
                if (cpickColor.Enabled)
                    return cpickColor.SelectedItem;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    cpickColor.Enabled = true;
                    cpickColor.SelectedItem = value ?? System.Drawing.Color.DarkGray;
                    labColor.ForeColor      = System.Drawing.Color.Black;
                }
                else
                {
                    cpickColor.Enabled      = false;
                    cpickColor.SelectedItem = System.Drawing.Color.DarkGray;
                    labColor.ForeColor      = System.Drawing.Color.DarkGray;
                }
            }
        }
        public Color? BackGround
        {
            get
            {
                if (cpickBackground.Enabled)
                    return cpickBackground.SelectedItem;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsEditable = true;
                    cpickBackground.Enabled = true;
                    cpickBackground.SelectedItem = value ?? System.Drawing.Color.DarkGray;
                    labBackground.ForeColor      = System.Drawing.Color.Black;
                }
                else
                {
                    cpickBackground.Enabled      = false;
                    cpickBackground.SelectedItem = System.Drawing.Color.DarkGray;
                    labBackground.ForeColor      = System.Drawing.Color.DarkGray;
                }
            }
        }
        public List<string> Words
        {
            get
            {
                if (listKeywords.Enabled)
                {
                    List<string> back = new List<string>();
                    foreach (string item in listKeywords.Items)
                    {
                        back.Add(item);
                    }
                    return back;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    _IsEditable = true;
                    listKeywords.Enabled = true;
                    btnAddKeyword.Enabled            = true;
                    btnRemoveSelectedKeyword.Enabled = true;
                    txtAddKeyword.Enabled            = true;
                    labKeywords.ForeColor            = System.Drawing.Color.Black;
                    listKeywords.Items.Clear();
                    foreach (string str in value)
                    {
                        listKeywords.Items.Add(str);
                    }
                }
                else
                {
                    listKeywords.Enabled             = false;
                    btnAddKeyword.Enabled            = false;
                    btnRemoveSelectedKeyword.Enabled = false;
                    txtAddKeyword.Enabled            = false;
                    labKeywords.ForeColor            = System.Drawing.Color.DarkGray;
                }
            }
        }

        public ConfigItemDialog(SyntaxNode node)
        {
            InitializeComponent();
            this.NodeType   = node.NodeType;
            this.NodeName   = node.NodeName;
            this.StopAtEOL  = node.StopAtEOL;
            this.Italic     = node.Italic;
            this.Bold       = node.Bold;
            this.IgnoreCase = node.IgnoreCase;
            this.Rule       = node.Rule;
            this.Color      = node.Color;
            this.BackGround = node.BackGround;
            this.Words      = node.Words;
        }
        public void UpdateNodeData(ref SyntaxNode node)
        {
            if (node != null)
            {
                node.NodeType   = this.NodeType;
                node.NodeName   = this.NodeName;
                node.StopAtEOL  = this.StopAtEOL;
                node.Italic     = this.Italic;
                node.Bold       = this.Bold;
                node.IgnoreCase = this.IgnoreCase;
                node.Rule       = this.Rule;
                node.Color      = this.Color;
                node.BackGround = this.BackGround;
                node.Words      = this.Words;
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private bool _IsEditable = false;
        public bool IsEditable 
        {
            get
            {
                return _IsEditable;
            }
        }

        private void btnAddKeyword_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtAddKeyword.Text) && !String.IsNullOrWhiteSpace(txtAddKeyword.Text))
            {
                listKeywords.Items.Add(txtAddKeyword.Text.Trim());
                txtAddKeyword.Text = "";
            }
        }

        private void btnRemoveSelectedKeyword_Click(object sender, EventArgs e)
        {
            if (listKeywords.SelectedIndex >= 0)
            {
                listKeywords.Items.RemoveAt(listKeywords.SelectedIndex);
            }

        }
    }
}
