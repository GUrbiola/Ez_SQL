using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SideToSideComparer
{
    public partial class SideToSideTextComparer : UserControl
    {
        public string Text1Name
        {
            get { return LabTxt1.Text; }
            set { LabTxt1.Text = value; }
        }
        public string Text2Name
        {
            get { return LabTxt2.Text; }
            set { LabTxt2.Text = value; }
        }

        public SideToSideTextComparer()
        {
            InitializeComponent();
            Txt1.ActiveTextAreaControl
        }

        public void Initialize()
        {
            
        }

    }
}
