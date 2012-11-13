using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;


namespace Ez_SQL
{
    public partial class SQLConnectForm : Form
    {
        SqlConnectionStringBuilder StrBuilder;
        public string SelectedDB { get { return StrBuilder.InitialCatalog; } }
        public SQLConnectForm()
        {
            InitializeComponent();
            StrBuilder = new SqlConnectionStringBuilder();
        }
        private bool Controles_Activos
        {
            get
            {
                return TUser.Enabled;
            }
            set
            {
                if (value)
                {
                    TUser.Enabled = true;
                    TPass.Enabled = true;
                }
                else
                {
                    TUser.Enabled = false;
                    TUser.Text = "";
                    TPass.Enabled = false;
                    TPass.Text = "";
                }
            }
        }
        private void CAut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CAut.SelectedIndex == 0)
                Controles_Activos = false;
            else
                Controles_Activos = true;
        }
        private void CServers_DropDown(object sender, EventArgs e)
        {
            if (CServers.Items.Count > 0)
                return;

            DataTable Aux;
            Aux = SqlDataSourceEnumerator.Instance.GetDataSources();
            for (int i = 0; i < Aux.Rows.Count; i++)
            {
                string txt = Aux.Rows[i][0] + "\\" + Aux.Rows[i][1].ToString();
                CServers.Items.Add(txt);
                CServers.Refresh();
            }
        }
        private void CServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            CBDs.Items.Clear();
            CBDs.Text = "";
        }
        private void SQLConnectForm_Shown(object sender, EventArgs e)
        {
            if (StrBuilder == null || StrBuilder.ConnectionString == null || StrBuilder.ConnectionString == "")
            {
                CAut.SelectedIndex = 0;
            }
        }
        private void CBDs_DropDown(object sender, EventArgs e)
        {
            if (CBDs.Items.Count > 0)
                return;
            //create connection string to connect to master database
            string Constr = "";
            if (CAut.SelectedIndex == 0)
            {
                //windows authentication
                Constr = Constr + "Integrated Security=SSPI;";
                Constr = Constr + "Persist Security Info=False;";
                Constr = Constr + "Initial Catalog=master;";
                Constr = Constr + "Data Source=" + CServers.Text;
            }
            else
            {
                Constr = Constr + "Password=" + TPass.Text + ";";
                Constr = Constr + "Persist Security Info=True;";
                Constr = Constr + "User ID=" + TUser.Text + ";";
                Constr = Constr + "Initial Catalog=master;";
                Constr = Constr + "Data Source=" + CServers.Text;
            }
            
            //create query to get database names
            string comando = "select name from sysdatabases order by name";
            
            System.Data.SqlClient.SqlConnection Conexion = null;
            Conexion = new System.Data.SqlClient.SqlConnection();
            Conexion.ConnectionString = Constr;
            //trying to connect
            try
            {
                Conexion.Open();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error when trying to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            //Connected, time to execute query.
            System.Data.SqlClient.SqlCommand SqlCommand1;
            SqlCommand1 = new System.Data.SqlClient.SqlCommand();
            SqlCommand1.CommandType = System.Data.CommandType.Text;
            SqlCommand1.Connection = Conexion;
            SqlCommand1.CommandText = comando;
            try
            {
                System.Data.SqlClient.SqlDataReader dr;
                dr = SqlCommand1.ExecuteReader();
                while (dr.Read())
                {
                    CBDs.Items.Add(dr["name"].ToString());
                }
            }
            catch (System.Exception ex)
            {
                //if something dfailed, close connection and show exception
                Conexion.Close();
                MessageBox.Show(ex.Message, "Error when retrieving database names", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Conexion.Close();
        }
        public string ConnectionString
        {
            get
            {
                if (CServers.Text == "")
                {
                    MessageBox.Show("Select/introduce a server name", "Server Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }
                if (CBDs.Text == "")
                {
                    MessageBox.Show("Select a database", "Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "";
                }

                StrBuilder = new SqlConnectionStringBuilder();
                StrBuilder.DataSource = CServers.Text;
                if (CAut.SelectedIndex == 0)
                {//Autentificacion de Windows
                    StrBuilder.IntegratedSecurity = true;
                }
                else
                {
                    StrBuilder.IntegratedSecurity = false;
                    StrBuilder.UserID = TUser.Text;
                    StrBuilder.Password = TPass.Text;
                }

                StrBuilder.InitialCatalog = CBDs.Text;
                StrBuilder.PersistSecurityInfo = true;

                return StrBuilder.ConnectionString;
            }
            set
            {
                try
                {
                    StrBuilder = new SqlConnectionStringBuilder(value);
                    CServers.Text = StrBuilder.DataSource;
                    if (StrBuilder.IntegratedSecurity)
                    {//Autentificacion de Windows
                        CAut.SelectedIndex = 0;
                    }
                    else
                    {//Autentificacion de SQL Server
                        CAut.SelectedIndex = 1;
                        TUser.Text = StrBuilder.UserID;
                        TPass.Text = StrBuilder.Password;
                    }
                    CBDs.Text = StrBuilder.InitialCatalog;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            SqlConnection Con;
            if (ConnectionString != "")
                Con = new SqlConnection(ConnectionString);
            else
                return;

            try
            {
                Con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Con.Close();
            DialogResult = DialogResult.OK;
        }

    }
}