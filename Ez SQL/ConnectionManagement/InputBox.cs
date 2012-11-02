using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ez_SQL
{
	/// <summary>
	/// Description of InputBox.
	/// </summary>
	public partial class InputBox : Form
	{
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
		public string Input
		{
			get
			{
				return textBox1.Text;
			}
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
