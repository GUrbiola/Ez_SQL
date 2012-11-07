using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.DataBaseObjects;

namespace Ez_SQL.MultiQueryForm
{
    public partial class ObjectSelector : Form
    {
        private List<string> Objects;
        private string _SelectedObject;
        public string SelectedObject { get { return _SelectedObject; } }
        private bool StartsWith;
        public ObjectSelector(string Title, string ObjectDescription, List<string> Objects, bool StartsWith = false)
        {
            InitializeComponent();
            Text = Title;
            LabDescription.Text = ObjectDescription;
            _SelectedObject = "";
            this.Objects = Objects;
            this.StartsWith = StartsWith;
            ItemList.Items.Clear();
            foreach (string obj in Objects)
            {
                ItemList.Items.Add(obj);
            }
            if (ItemList.Items.Count > 0)
                ItemList.SelectedIndex = 0;
        }
        private void TxtFilter_TextWaitEnded(string Text, int Decimals)
        {
            FilterListBy(Text);
        }
        private void FilterListBy(string Text)
        {
            ItemList.Items.Clear();
            foreach (string obj in Objects.Where(X => StartsWith ? X.StartsWith(Text, StringComparison.CurrentCultureIgnoreCase) : X.ToUpper().Contains(Text.ToUpper())))
            {
                ItemList.Items.Add(obj);
            }
            if (ItemList.Items.Count > 0)
            {
                ItemList.SelectedIndex = 0;
            }
        }
        private void TxtFilter_TextSecured(string Text)
        {
            if (ItemList.SelectedIndex >= 0)
            {
                _SelectedObject = ItemList.SelectedItem.ToString();
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
            else if (e.KeyCode == Keys.Escape)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                _SelectedObject = "";
            }
        }
        private void MoveSelection(int Movement)
        {
            int current = ItemList.SelectedIndex;
            if (ItemList.Items.Count > (current + Movement) && (current + Movement) >= 0)
            {
                ItemList.SelectedIndex = (current + Movement);
            }
        }
        private void ItemList_DoubleClick(object sender, EventArgs e)
        {
            if (ItemList.SelectedIndex >= 0)
            {
                _SelectedObject = ItemList.SelectedItem.ToString();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
