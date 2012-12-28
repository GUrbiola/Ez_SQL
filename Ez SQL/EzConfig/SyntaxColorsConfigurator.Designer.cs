namespace Ez_SQL.EzConfig
{
    partial class SyntaxColorsConfigurator
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
            this.colorPickerCombobox1 = new ColorPicker.ColorPickerCombobox();
            this.SuspendLayout();
            // 
            // colorPickerCombobox1
            // 
            this.colorPickerCombobox1.Location = new System.Drawing.Point(37, 39);
            this.colorPickerCombobox1.Name = "colorPickerCombobox1";
            this.colorPickerCombobox1.SelectedItem = System.Drawing.Color.Wheat;
            this.colorPickerCombobox1.Size = new System.Drawing.Size(162, 34);
            this.colorPickerCombobox1.TabIndex = 0;
            this.colorPickerCombobox1.Text = "colorPickerCombobox1";
            // 
            // SyntaxColorsConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.colorPickerCombobox1);
            this.Name = "SyntaxColorsConfigurator";
            this.Text = "SyntaxColorsConfigurator";
            this.ResumeLayout(false);

        }

        #endregion

        private ColorPicker.ColorPickerCombobox colorPickerCombobox1;
    }
}