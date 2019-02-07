using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using Ez_SQL.Custom_Controls.DifferenceEngine.Implementations;
using Ez_SQL.Custom_Controls.DifferenceEngine.Engine;
using System.Collections;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;
using ICSharpCode.TextEditor;
using Ez_SQL.Common_Code;

namespace Ez_SQL.Custom_Controls
{
    public partial class SideToSideLineComparer : UserControl
    {
        public SideToSideLineComparer()
        {
            InitializeComponent();

            Line1.Text = "";
            Line2.Text = "";

            Line1.ActiveTextAreaControl.VScrollBar.Hide();
            Line2.ActiveTextAreaControl.VScrollBar.Hide();

            Line1.IsReadOnly = true;
            Line2.IsReadOnly = true;

            string DataStorageDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ez SQL";
            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(DataStorageDir + "\\SintaxHighLight\\"));
                Line1.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
                Line2.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            Line1.ActiveTextAreaControl.HScrollBar.ValueChanged += new EventHandler(Line1HorizontalScrollChanged);
            Line2.ActiveTextAreaControl.HScrollBar.ValueChanged += new EventHandler(Line2HorizontalScrollChanged);
        }
        
        private void Line1HorizontalScrollChanged(object sender, EventArgs e)
        {
            Line2.ActiveTextAreaControl.HScrollBar.Value = Line1.ActiveTextAreaControl.HScrollBar.Value;
            Line2.ActiveTextAreaControl.Refresh();
        }

        private void Line2HorizontalScrollChanged(object sender, EventArgs e)
        {
            Line1.ActiveTextAreaControl.HScrollBar.Value = Line2.ActiveTextAreaControl.HScrollBar.Value;
            Line1.ActiveTextAreaControl.Refresh();
        }

        public void LoadTexts(string txt1, string txt2)
        {
            Line1.Text = "";
            Line2.Text = "";

            DiffListString t1, t2;

            List<Tuple<char, LineHighlight>> finalT1 = new List<Tuple<char, LineHighlight>>();
            List<Tuple<char, LineHighlight>> finalT2 = new List<Tuple<char, LineHighlight>>();

            t1 = new DiffListString(txt1);
            t2 = new DiffListString(txt2);

            DiffEngine Engine = new DiffEngine();
            Engine.ProcessDiff(t1, t2, DiffEngineLevel.SlowPerfect);
            ArrayList rep = Engine.DiffReport();

            foreach (DiffResultSpan drs in rep)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            string h1 = t1.GetByIndex(drs.SourceIndex + i).ToString();
                            string h2 = " ";
                            finalT1.Add(new Tuple<char, LineHighlight>(h1[0], LineHighlight.Remove));
                            finalT2.Add(new Tuple<char, LineHighlight>(h2[0], LineHighlight.Missing));
                        }
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            string h1 = t1.GetByIndex(drs.SourceIndex + i).ToString();
                            string h2 = t2.GetByIndex(drs.DestIndex + i).ToString();

                            finalT1.Add(new Tuple<char, LineHighlight>(h1[0], LineHighlight.None));
                            finalT2.Add(new Tuple<char, LineHighlight>(h2[0], LineHighlight.None));
                        }
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            string h1 = " ";
                            string h2 = t2.GetByIndex(drs.DestIndex + i).ToString();
                            finalT1.Add(new Tuple<char, LineHighlight>(h1[0], LineHighlight.Missing));
                            finalT2.Add(new Tuple<char, LineHighlight>(h2[0], LineHighlight.Add));
                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            string h1 = t1.GetByIndex(drs.SourceIndex + i).ToString();
                            string h2 = t2.GetByIndex(drs.DestIndex + i).ToString();
                            finalT1.Add(new Tuple<char, LineHighlight>(h1[0], LineHighlight.Update));
                            finalT2.Add(new Tuple<char, LineHighlight>(h2[0], LineHighlight.Update));
                        }
                        break;
                }
            }

            LoadDiffResults(Line1, finalT1);
            LoadDiffResults(Line2, finalT2);

            Line1.Refresh();
            Line2.Refresh();

        }

        private void LoadDiffResults(TextEditorControl txtEditor, List<Tuple<char, LineHighlight>> diffResults)
        {
            StringBuilder buff = new StringBuilder();
            foreach (Tuple<char, LineHighlight> t in diffResults)
            {
                buff.Append(t.Item1);
            }
            txtEditor.Text = buff.ToString();


            for (int i = 0; i < diffResults.Count; i++)
            {
                switch (diffResults[i].Item2)
                {
                    case LineHighlight.Add:
                    case LineHighlight.Remove:
                    case LineHighlight.Update:
                        txtEditor.SetMarker(i, 1, Color.Khaki);
                        break;
                    case LineHighlight.Missing:
                        txtEditor.SetMarker(i, 1, Color.Gainsboro);
                        break;
                    case LineHighlight.None:
                    default:
                        break;
                }
            }
        }

        public void Clean()
        {
            Line1.Text = "";
            Line2.Text = "";

            Line1.Refresh();
            Line2.Refresh();
        }
    }
}
