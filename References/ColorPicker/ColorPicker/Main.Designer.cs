namespace ColorPicker
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.colorPickerCombobox2 = new ColorPicker.ColorPickerCombobox();
            this.colorPickerCombobox1 = new ColorPicker.ColorPickerCombobox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(145, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Color Picker Dialog";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // colorPickerCombobox2
            // 
            this.colorPickerCombobox2.Location = new System.Drawing.Point(145, 82);
            this.colorPickerCombobox2.Name = "colorPickerCombobox2";
            this.colorPickerCombobox2.SelectedItem = System.Drawing.Color.Wheat;
            this.colorPickerCombobox2.Size = new System.Drawing.Size(132, 23);
            this.colorPickerCombobox2.TabIndex = 2;
            this.colorPickerCombobox2.Text = "colorPickerCombobox2";
            // 
            // colorPickerCombobox1
            // 
            this.colorPickerCombobox1.Location = new System.Drawing.Point(145, 52);
            this.colorPickerCombobox1.Name = "colorPickerCombobox1";
            this.colorPickerCombobox1.SelectedItem = System.Drawing.Color.Wheat;
            this.colorPickerCombobox1.Size = new System.Drawing.Size(132, 23);
            this.colorPickerCombobox1.TabIndex = 1;
            this.colorPickerCombobox1.Text = "colorPickerCombobox1";
            this.colorPickerCombobox1.OnSelectedColor += new ColorPicker.SelectedColor(this.colorPickerCombobox1_OnSelectedColor);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 219);
            this.Controls.Add(this.colorPickerCombobox2);
            this.Controls.Add(this.colorPickerCombobox1);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button button1;
        private ColorPickerCombobox colorPickerCombobox1;
        private ColorPickerCombobox colorPickerCombobox2;
	}
}