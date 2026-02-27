using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ez_SQL
{
    /// <summary>
    /// Generic single-field text input dialog.
    /// Supports an optional forced-entry mode that prevents the user from confirming with an empty value.
    /// Title and label text are configurable at construction time or through properties.
    /// </summary>
	public partial class InputBox : Form
	{
        /// <summary>When <c>true</c>, the OK button is blocked if the input text is empty or whitespace.</summary>
		bool ForceEntry;
		public InputBox()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public InputBox(bool forceentry)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			ForceEntry = forceentry;
		}
		public InputBox(bool forceentry, string title)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			ForceEntry = forceentry;
			Title = title;
		}
		public InputBox(bool forceentry, string title, string label)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			ForceEntry = forceentry;
			Title = title;
			LabelText = label;
		}
	    /// <summary>Gets or sets the label text shown above the input field.</summary>
	public string LabelText
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text = value;
			}
		}
	    /// <summary>Gets or sets the dialog window title.</summary>
	public string Title
		{
			get
			{
				return Text;
			}
			set
			{
				Text = value;
			}
		}
	    /// <summary>Gets the text the user has typed into the input field.</summary>
	public string Input
		{
			get
			{
				return textBox1.Text;
			}
		}
        /// <summary>Programmatically pre-fills the input field with <paramref name="Txt"/>.</summary>
        /// <param name="Txt">The default text to display in the input field.</param>
        public void SetInput(string Txt)
        {
            textBox1.Text = Txt;
        }
		void BtnOKClick(object sender, EventArgs e)
		{
			if(ForceEntry)
			{
				if(Input.Trim().Length == 0)
				{
					MessageBox.Show("Datos Obligatorios, Intente de Nuevo", "Informacion Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Error);
					textBox1.Focus();
					return;
				}
			}
			DialogResult = DialogResult.OK;
		}
	}
}
