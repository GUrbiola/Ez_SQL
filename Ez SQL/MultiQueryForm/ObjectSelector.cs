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
        private string DisplayField;
        private object _SelectedObject;
        public object SelectedObject { get { return _SelectedObject; } }
        public ObjectSelector(string Title, string ObjectDescription, List<ISqlChild> Objects, string DisplayField)
        {
            InitializeComponent();
            Text = Title;
            LabDescription.Text = ObjectDescription;
            foreach (ISqlChild child in Objects)
        	{
                CmbObjects.AutoCompleteCustomSource.Add(child.Name);
	        }
            
            CmbObjects.Items.AddRange(Objects.ToArray());
            CmbObjects.DisplayMember = DisplayField;
            _SelectedObject = null;
            this.DisplayField = DisplayField;
        }

        private void CmbObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbObjects.SelectedIndex >= 0 
                && CmbObjects.Text == CmbObjects.SelectedItem.GetType().GetProperty(DisplayField).ToString())
            {
                _SelectedObject = CmbObjects.SelectedItem;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
