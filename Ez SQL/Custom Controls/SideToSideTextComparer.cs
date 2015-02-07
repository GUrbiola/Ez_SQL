using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.Custom_Controls
{
    public partial class SideToSideTextComparer : UserControl
    {
        public string Text1Label
        {
            get { return LabTxt1.Text; }
            set { LabTxt1.Text = value; }
        }
        public string Text2Label
        {
            get { return LabTxt2.Text; }
            set { LabTxt2.Text = value; }
        }

        public SideToSideTextComparer()
        {
            InitializeComponent();

            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(MainForm.DataStorageDir + "\\SintaxHighLight\\"));
                Txt1.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
                Txt2.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            Txt1.IsReadOnly = true;
            Txt2.IsReadOnly = true;


        }

        public LoadTexts(string txt1, string txt2)
        {

        }
    }
}
