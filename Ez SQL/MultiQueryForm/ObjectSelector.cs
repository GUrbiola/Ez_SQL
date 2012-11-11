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
        private List<string> Values;
        private List<string> Displays;
        private string _SelectedObject;
        public string SelectedObject { get { return _SelectedObject; } }
        private bool StartsWith;
        public ObjectSelector(string Title, string ObjectDescription, List<string> Values, bool StartsWith = false, int Decimals = 1)
        {
            InitializeComponent();
            Text = Title;
            LabDescription.Text = ObjectDescription;
            _SelectedObject = "";
            this.Values = Values;
            this.StartsWith = StartsWith;
            ItemList.Items.Clear();
            this.Values.Sort();
            foreach (string obj in Values)
            {
                ItemList.Items.Add(obj);
            }
            if (ItemList.Items.Count > 0)
                ItemList.SelectedIndex = 0;
            TxtFilter.WaitInterval = Decimals;
        }
        public ObjectSelector(string Title, string ObjectDescription, List<string> Values, List<string> Displays, bool StartsWith = false, int Decimals = 1)
        {
            InitializeComponent();
            Text = Title;
            LabDescription.Text = ObjectDescription;
            _SelectedObject = "";
            this.Values = Values;
            this.Displays = Displays;
            this.StartsWith = StartsWith;
            ItemList.Items.Clear();
            foreach (string obj in Displays)
            {
                ItemList.Items.Add(obj);
            }
            if (ItemList.Items.Count > 0)
                ItemList.SelectedIndex = 0;
            TxtFilter.WaitInterval = Decimals;
        }
        private void TxtFilter_TextWaitEnded(string Text, int Decimals)
        {
            FilterListBy(Text);
        }
        private void FilterListBy(string Text)
        {
            ItemList.Items.Clear();
            if (Displays != null && Displays.Count > 0)
            {
                foreach (string obj in Displays.Where(X => StartsWith ? X.StartsWith(Text, StringComparison.CurrentCultureIgnoreCase) : X.ToUpper().Contains(Text.ToUpper())))
                {
                    ItemList.Items.Add(obj);
                }
                if (ItemList.Items.Count > 0)
                {
                    ItemList.SelectedIndex = 0;
                }
            }
            else
            {
                foreach (string obj in Values.Where(X => StartsWith ? X.StartsWith(Text, StringComparison.CurrentCultureIgnoreCase) : X.ToUpper().Contains(Text.ToUpper())))
                {
                    ItemList.Items.Add(obj);
                }
                if (ItemList.Items.Count > 0)
                {
                    ItemList.SelectedIndex = 0;
                }
            }
        }
        private void TxtFilter_TextSecured(string Text)
        {
            if (ItemList.SelectedIndex >= 0)
            {
                if (Displays != null && Displays.Count > 0)
                {
                    string selected = ItemList.SelectedItem.ToString();
                    _SelectedObject = Values[Displays.IndexOf(selected)];
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    _SelectedObject = ItemList.SelectedItem.ToString();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
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
                if (Displays != null && Displays.Count > 0)
                {
                    string selected = ItemList.SelectedItem.ToString();
                    _SelectedObject = Values[Displays.IndexOf(selected)];
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    _SelectedObject = ItemList.SelectedItem.ToString();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
