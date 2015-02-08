using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Ez_SQL
{
    public partial class SideToSideTester : Form
    {
        private bool flag;
        public SideToSideTester()
        {
            InitializeComponent();
            flag = false;
        }

        private void SideToSideTester_Load(object sender, EventArgs e)
        {
            string txt1, txt2;
            using (StreamReader rdr = new StreamReader("c:\\Employee ReviewDB.sql"))
            {
                txt1 = rdr.ReadToEnd();
            }
            using (StreamReader rdr = new StreamReader("c:\\Employee ReviewDBVarPay.sql"))
            {
                txt2 = rdr.ReadToEnd();
            }

            stsComparer.LoadTexts(txt1, txt2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt1, txt2;
            if (flag)
            {
                using (StreamReader rdr = new StreamReader("c:\\Employee ReviewDB.sql"))
                {
                    txt1 = rdr.ReadToEnd();
                    stsComparer.Text1Label = "Employee ReviewDB.sql";
                }
                using (StreamReader rdr = new StreamReader("c:\\Employee ReviewDBVarPay.sql"))
                {
                    txt2 = rdr.ReadToEnd();
                    stsComparer.Text2Label = "Employee ReviewDBVarPay.sql";
                }
            }
            else
            {
                using (StreamReader rdr = new StreamReader("c:\\Most of the scripts.sql"))
                {
                    txt1 = rdr.ReadToEnd();
                    stsComparer.Text1Label = "Most of the scripts.sql";
                }
                using (StreamReader rdr = new StreamReader("c:\\Most of the scripts 2.sql"))
                {
                    txt2 = rdr.ReadToEnd();
                    stsComparer.Text2Label = "Most of the scripts 2.sql";
                }
            }
            flag = !flag;

            stsComparer.LoadTexts(txt1, txt2);
        }
    }
}
