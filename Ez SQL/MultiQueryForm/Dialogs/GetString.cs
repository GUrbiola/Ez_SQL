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
    /// <summary>
    /// A reusable modal dialog that prompts the user to enter a single text value.
    /// The dialog can optionally enforce a non-empty input via <see cref="AllowEmpty"/>.
    /// Clicking OK only closes the dialog (with <see cref="DialogResult.OK"/>) when the
    /// input is non-empty, unless <see cref="AllowEmpty"/> is set to <c>true</c>.
    /// </summary>
    public partial class GetString : Form
    {
        /// <summary>
        /// Gets or sets whether the OK button accepts an empty input.
        /// When <c>false</c> (default), clicking OK while the text box is empty has no effect.
        /// </summary>
        public bool AllowEmpty { get; set; }

        /// <summary>Gets or sets the form's title bar caption.</summary>
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        /// <summary>Gets or sets the descriptive message label displayed above the input field.</summary>
        public string Message
        {
            get { return dialogText.Text; }
            set { this.dialogText.Text = value; }
        }

        /// <summary>Gets or sets the icon displayed in the form's title bar and taskbar button.</summary>
        public Icon DialogIcon
        {
            get { return this.Icon; }
            set { this.Icon = value; }
        }

        /// <summary>Gets the text the user typed into the input field.</summary>
        public string Value
        {
            get { return txtValue.Text; }
        }

        /// <summary>Initializes a new instance of <see cref="GetString"/> and sets up the designer components.</summary>
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
