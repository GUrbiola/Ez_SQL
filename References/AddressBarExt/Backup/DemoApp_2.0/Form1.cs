#region Using Statements

#region .NET Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

#region Custom Namespaces

using AddressBarExt.Controls;
#endregion

#endregion

namespace AddressBarExt
{
    /// <summary>
    /// Basic form that demos the AddressBarExt control.
    /// 
    /// This code is very messy, jsut made for demonstration purposes :)
    /// 
    /// -James Strain
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //initialize the bar
            this.AdBar.InitializeRoot(new FileSystemNode());
        }

        private void AdBar_SelectionChange(object sender, NodeChangedArgs nca)
        {
            this.rtb_path.Text = (String)nca.OUniqueID;
        }

        private void btn_c_Click(object sender, EventArgs e)
        {
            this.AdBar.InitializeRoot(new FileSystemNode("C:\\",null));
            this.rtb_path.Text = "C:\\";
        }

        private void btn_d_Click(object sender, EventArgs e)
        {
            //get the root node
            IAddressNode root = this.AdBar.RootNode;

            //search the child nodes, non-recursivly
            this.AdBar.CurrentNode = root.GetChild(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), false);
        }

        private void AdBar_NodeDoubleClick(object sender, NodeChangedArgs nca)
        {
            this.rtb_path.Text = (String)nca.OUniqueID;


            System.Diagnostics.ProcessStartInfo exploreTest = new System.Diagnostics.ProcessStartInfo();
            exploreTest.FileName = "explorer.exe";
            exploreTest.Arguments = (String)nca.OUniqueID;
            System.Diagnostics.Process.Start(exploreTest);

        }

        private void btn_fake_Click(object sender, EventArgs e)
        {
            this.AdBar.CurrentNode = new FileSystemNode("D:\\", null);
        }

        private void AdBar_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("AIDS!");
        }
    }
}