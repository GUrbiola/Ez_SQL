using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ez_SQL.MultiQueryForm
{
    public partial class ChildSelector : Form
    {
        private List<string> Objects;
        private List<string> _SelectedChilds;
        public List<string> SelectedChilds { get { return _SelectedChilds; } }
        private bool StartsWith;
        public ChildSelector(string Title, string ObjectDescription, List<string> Objects, bool StartsWith = false, int Decimals = 1)
        {
            InitializeComponent();
            Text = Title;
            LabDescription.Text = ObjectDescription;
            _SelectedChilds = new List<string>();
            this.Objects = Objects;
            this.StartsWith = StartsWith;
            this.Objects.Sort();
            ChkList.Items.Clear();
            foreach (string obj in Objects)
            {
                ChkList.Items.Add(obj, true);
            }
            ChkAll.Checked = true;
        }
        private void TxtFilter_TextWaitEnded(string Text, int Decimals)
        {
            FilterListBy(Text);
        }
        private void FilterListBy(string Text)
        {
            ChkList.Items.Clear();
            foreach (string obj in Objects.Where(X => StartsWith ? X.StartsWith(Text, StringComparison.CurrentCultureIgnoreCase) : X.ToUpper().Contains(Text.ToUpper())))
            {
                ChkList.Items.Add(obj, true);
            }
            ChkAll.Checked = true;
        }
        private void TxtFilter_TextSecured(string Text)
        {
            if (ChkList.CheckedIndices.Count > 0)
            {
                _SelectedChilds.Clear();
                foreach (int childIndex in ChkList.CheckedIndices)
                {
                    _SelectedChilds.Add(ChkList.GetItemText(ChkList.Items[childIndex]));
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
        private void TxtFilter_KeyDowned(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                MoveSelection(1);
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.PageUp)
            {
                MoveSelection(-1);
            }
            else if(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                ToggleCheck();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                _SelectedChilds = new List<string>();
            }
        }
        private void ToggleCheck()
        {
            foreach (int itemIndex in ChkList.SelectedIndices)
            {
                ChkList.SetItemChecked(itemIndex, ChkList.GetItemCheckState(itemIndex) != CheckState.Checked);
            }
        }
        private void MoveSelection(int Movement)
        {
            int current = ChkList.SelectedIndex;
            if (ChkList.Items.Count > (current + Movement) && (current + Movement) >= 0)
            {
                ChkList.SelectedIndex = (current + Movement);
            }
        }
        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkList.Items.Count; i++)
            {
                ChkList.SetItemChecked(i, ChkAll.Checked);
            }
        }
    }
}
