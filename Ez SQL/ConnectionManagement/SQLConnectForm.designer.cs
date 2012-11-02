namespace Ez_SQL
{
    partial class SQLConnectForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEx1 = new System.Windows.Forms.Panel();
            this.CBDs = new System.Windows.Forms.ComboBox();
            this.CAut = new System.Windows.Forms.ComboBox();
            this.CServers = new System.Windows.Forms.ComboBox();
            this.TPass = new System.Windows.Forms.TextBox();
            this.TUser = new System.Windows.Forms.TextBox();
            this.labelX5 = new System.Windows.Forms.Label();
            this.labelX4 = new System.Windows.Forms.Label();
            this.labelX3 = new System.Windows.Forms.Label();
            this.labelX2 = new System.Windows.Forms.Label();
            this.labelX1 = new System.Windows.Forms.Label();
            this.Conectar = new System.Windows.Forms.Button();
            this.Cancelar = new System.Windows.Forms.Button();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.CBDs);
            this.panelEx1.Controls.Add(this.CAut);
            this.panelEx1.Controls.Add(this.CServers);
            this.panelEx1.Controls.Add(this.TPass);
            this.panelEx1.Controls.Add(this.TUser);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(2, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(490, 197);
            this.panelEx1.TabIndex = 1;
            // 
            // CBDs
            // 
            this.CBDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBDs.FormattingEnabled = true;
            this.CBDs.Location = new System.Drawing.Point(110, 167);
            this.CBDs.Name = "CBDs";
            this.CBDs.Size = new System.Drawing.Size(377, 26);
            this.CBDs.TabIndex = 13;
            this.CBDs.DropDown += new System.EventHandler(this.CBDs_DropDown);
            // 
            // CAut
            // 
            this.CAut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CAut.FormattingEnabled = true;
            this.CAut.Items.AddRange(new object[] {
            "Windows authentication",
            "SQL server authentication"});
            this.CAut.Location = new System.Drawing.Point(110, 47);
            this.CAut.Name = "CAut";
            this.CAut.Size = new System.Drawing.Size(377, 26);
            this.CAut.TabIndex = 12;
            this.CAut.SelectedIndexChanged += new System.EventHandler(this.CAut_SelectedIndexChanged);
            // 
            // CServers
            // 
            this.CServers.FormattingEnabled = true;
            this.CServers.Location = new System.Drawing.Point(110, 7);
            this.CServers.Name = "CServers";
            this.CServers.Size = new System.Drawing.Size(377, 26);
            this.CServers.TabIndex = 11;
            this.CServers.DropDown += new System.EventHandler(this.CServers_DropDown);
            this.CServers.SelectedIndexChanged += new System.EventHandler(this.CServers_SelectedIndexChanged);
            // 
            // TPass
            // 
            this.TPass.Location = new System.Drawing.Point(110, 127);
            this.TPass.Name = "TPass";
            this.TPass.PasswordChar = '*';
            this.TPass.Size = new System.Drawing.Size(377, 24);
            this.TPass.TabIndex = 9;
            // 
            // TUser
            // 
            this.TUser.Location = new System.Drawing.Point(110, 87);
            this.TUser.Name = "TUser";
            this.TUser.Size = new System.Drawing.Size(377, 24);
            this.TUser.TabIndex = 8;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(37, 170);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(71, 18);
            this.labelX5.TabIndex = 5;
            this.labelX5.Text = "Database";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(33, 130);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 18);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "Password";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(68, 90);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(40, 18);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "User";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(8, 50);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(100, 18);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Authentication";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(13, 10);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(95, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Server Name";
            // 
            // Conectar
            // 
            this.Conectar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Conectar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Conectar.Location = new System.Drawing.Point(29, 203);
            this.Conectar.Name = "Conectar";
            this.Conectar.Size = new System.Drawing.Size(174, 32);
            this.Conectar.TabIndex = 2;
            this.Conectar.Text = "Connect";
            this.Conectar.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Cancelar
            // 
            this.Cancelar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Cancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Cancelar.Location = new System.Drawing.Point(287, 203);
            this.Cancelar.Name = "Cancelar";
            this.Cancelar.Size = new System.Drawing.Size(174, 32);
            this.Cancelar.TabIndex = 3;
            this.Cancelar.Text = "Cancel";
            // 
            // SQLConnectForm
            // 
            this.AcceptButton = this.Conectar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancelar;
            this.ClientSize = new System.Drawing.Size(494, 241);
            this.ControlBox = false;
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.Cancelar);
            this.Controls.Add(this.Conectar);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SQLConnectForm";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to Microsoft SQL Server";
            this.Shown += new System.EventHandler(this.SQLConnectForm_Shown);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEx1;
        private System.Windows.Forms.Label labelX5;
        private System.Windows.Forms.Label labelX4;
        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Label labelX2;
        private System.Windows.Forms.Label labelX1;
        private System.Windows.Forms.TextBox TPass;
        private System.Windows.Forms.TextBox TUser;
        //private DevComponents.Editors.ComboItem WAut;
        //private DevComponents.Editors.ComboItem SQLAut;
        private System.Windows.Forms.Button Conectar;
        private System.Windows.Forms.Button Cancelar;
        private System.Windows.Forms.ComboBox CBDs;
        private System.Windows.Forms.ComboBox CAut;
        private System.Windows.Forms.ComboBox CServers;


    }
}