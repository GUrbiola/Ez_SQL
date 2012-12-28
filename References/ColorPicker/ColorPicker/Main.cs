using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ColorPicker
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ColorPickerDialog dlg = new ColorPickerDialog();
			dlg.ShowDialog(this);
		}

        private void colorPickerCombobox1_OnSelectedColor(Color color)
        {
            MessageBox.Show(String.Format("Color: '{0}', Selected!", colorPickerCombobox1.HtmlSelectedItem));
        }
	}
}