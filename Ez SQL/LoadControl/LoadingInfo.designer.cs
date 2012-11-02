namespace Ez_SQL.LoadControl
{
    partial class LoadingInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                graphicsPath.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.LDB = new System.Windows.Forms.Label();
            this.ReadingIcon = new System.Windows.Forms.PictureBox();
            this.ProgressShower = new System.Windows.Forms.ProgressBar();
            this.LAction = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ReadingIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(110, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Extrayendo Informacion";
            // 
            // LDB
            // 
            this.LDB.BackColor = System.Drawing.Color.Transparent;
            this.LDB.Location = new System.Drawing.Point(5, 38);
            this.LDB.Name = "LDB";
            this.LDB.Size = new System.Drawing.Size(321, 15);
            this.LDB.TabIndex = 1;
            this.LDB.Text = "Base de Datos";
            // 
            // ReadingIcon
            // 
            this.ReadingIcon.BackColor = System.Drawing.Color.Transparent;
            this.ReadingIcon.Image = global::Ez_SQL.Properties.Resources.Book_32;
            this.ReadingIcon.Location = new System.Drawing.Point(72, 3);
            this.ReadingIcon.Name = "ReadingIcon";
            this.ReadingIcon.Size = new System.Drawing.Size(32, 32);
            this.ReadingIcon.TabIndex = 2;
            this.ReadingIcon.TabStop = false;
            // 
            // ProgressShower
            // 
            this.ProgressShower.Location = new System.Drawing.Point(8, 56);
            this.ProgressShower.Name = "ProgressShower";
            this.ProgressShower.Size = new System.Drawing.Size(318, 23);
            this.ProgressShower.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressShower.TabIndex = 3;
            // 
            // LAction
            // 
            this.LAction.BackColor = System.Drawing.Color.Transparent;
            this.LAction.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LAction.Location = new System.Drawing.Point(5, 82);
            this.LAction.Name = "LAction";
            this.LAction.Size = new System.Drawing.Size(321, 21);
            this.LAction.TabIndex = 4;
            this.LAction.Text = "Accion Realizandose";
            // 
            // LoadingInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.LAction);
            this.Controls.Add(this.ProgressShower);
            this.Controls.Add(this.ReadingIcon);
            this.Controls.Add(this.LDB);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Name = "LoadingInfo";
            this.Size = new System.Drawing.Size(337, 103);
            ((System.ComponentModel.ISupportInitialize)(this.ReadingIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LDB;
        private System.Windows.Forms.PictureBox ReadingIcon;
        private System.Windows.Forms.ProgressBar ProgressShower;
        private System.Windows.Forms.Label LAction;
    }
}
