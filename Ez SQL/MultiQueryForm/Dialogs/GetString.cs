using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ez_SQL.Common_Code;

namespace Ez_SQL.MultiQueryForm.Dialogs
{
    public partial class GetString : Form
    {
        public bool AllowEmpty { get; set; }
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
        public string Message
        {
            get { return dialogText.Text; }
            set { this.dialogText.Text = value; }
        }

        public Icon DialogIcon 
        {             
            get { return this.Icon; }
            set { this.Icon = value; }
        }

        public string Value
        {
            get { return txtValue.Text; }
        }

        public GetString()
        {
            InitializeComponent();
        }

        private void PickFromList_Shown(object sender, EventArgs e)
        {
            txtValue.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (AllowEmpty)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                if(!txtValue.Text.IsEmpty())
                    DialogResult = DialogResult.OK;
            }

        }
    }
}
