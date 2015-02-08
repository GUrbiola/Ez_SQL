using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using Ez_SQL.Extensions;
using ICSharpCode.TextEditor;
using Ez_SQL.Custom_Controls.DifferenceEngine.Implementations;
using Ez_SQL.Custom_Controls.DifferenceEngine.Engine;
using System.Collections;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

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

            string DataStorageDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ez SQL";
            #region Code to load the Highlight rules(files in resources) and the folding strategy class
            try
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(DataStorageDir + "\\SintaxHighLight\\"));
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

            Txt1.ActiveTextAreaControl.VScrollBar.ValueChanged += new EventHandler(Txt1VerticalScrollChange);
            Txt2.ActiveTextAreaControl.VScrollBar.ValueChanged += new EventHandler(Txt2VerticalScrollChange);

            Txt1.ActiveTextAreaControl.HScrollBar.ValueChanged += new EventHandler(Txt1HorizontalScrollChanged);
            Txt2.ActiveTextAreaControl.HScrollBar.ValueChanged += new EventHandler(Txt2HorizontalScrollChanged);

            Txt1.ActiveTextAreaControl.TextArea.MouseClick += OnMouseClick;
            Txt2.ActiveTextAreaControl.TextArea.MouseClick += OnMouseClick;
            
        }

        private void Txt1HorizontalScrollChanged(object sender, EventArgs e)
        {
            Txt2.ActiveTextAreaControl.HScrollBar.Value = Txt1.ActiveTextAreaControl.HScrollBar.Value;
            Txt2.ActiveTextAreaControl.Refresh();
        }

        private void Txt2HorizontalScrollChanged(object sender, EventArgs e)
        {
            Txt1.ActiveTextAreaControl.HScrollBar.Value = Txt2.ActiveTextAreaControl.HScrollBar.Value;
            Txt1.ActiveTextAreaControl.Refresh();
        }
        private void OnMouseClick(object sender, EventArgs e)
        {
            TextArea ted = sender as TextArea;
            if (ted == null)
                return;
            
            int clickAtLine = ted.Caret.Line;

            Txt1.SelectLine(clickAtLine);
            Txt2.SelectLine(clickAtLine);

            string txt1 = Txt1.GetLineText(clickAtLine), txt2 = Txt2.GetLineText(clickAtLine);

            LineComparer.LoadTexts(txt1, txt2);
        }

        private void Txt1VerticalScrollChange(object sender, EventArgs e)
        {
            Txt2.ActiveTextAreaControl.VScrollBar.Value = Txt1.ActiveTextAreaControl.VScrollBar.Value;
            Txt2.ActiveTextAreaControl.Refresh();
        }

        private void Txt2VerticalScrollChange(object sender, EventArgs e)
        {
            Txt1.ActiveTextAreaControl.VScrollBar.Value = Txt2.ActiveTextAreaControl.VScrollBar.Value;
            Txt1.ActiveTextAreaControl.Refresh();
        }


        public void LoadTexts(string txt1, string txt2)
        {
            DiffListText t1, t2;

            List<Tuple<string, LineHighlight>> finalT1 = new List<Tuple<string, LineHighlight>>();
            List<Tuple<string, LineHighlight>> finalT2 = new List<Tuple<string, LineHighlight>>();

            t1 = new DiffListText(txt1);
            t2 = new DiffListText(txt2);

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
                            string h1 = (t1.GetByIndex(drs.SourceIndex + i) as TextLine).Line;
                            string h2 = "".PadLeft(h1.Length) + Environment.NewLine;
                            finalT1.Add(new Tuple<string,LineHighlight>(h1, LineHighlight.Remove));
                            finalT2.Add(new Tuple<string,LineHighlight>(h2, LineHighlight.Missing));
                        }
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            finalT1.Add(new Tuple<string, LineHighlight>((t1.GetByIndex(drs.SourceIndex + i) as TextLine).Line, LineHighlight.None));
                            finalT2.Add(new Tuple<string, LineHighlight>((t2.GetByIndex(drs.DestIndex + i) as TextLine).Line, LineHighlight.None));
                        }
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            string h1;
                            string h2 = (t2.GetByIndex(drs.DestIndex + i) as TextLine).Line;
                            h1 = "".PadLeft(h2.Length) + Environment.NewLine;
                            finalT1.Add(new Tuple<string, LineHighlight>(h1, LineHighlight.Missing));
                            finalT2.Add(new Tuple<string, LineHighlight>(h2, LineHighlight.Add));
                        }

                        break;
                    case DiffResultSpanStatus.Replace:
                        for (int i = 0; i < drs.Length; i++)
                        {
                            finalT1.Add(new Tuple<string, LineHighlight>((t1.GetByIndex(drs.SourceIndex + i) as TextLine).Line, LineHighlight.Update));
                            finalT2.Add(new Tuple<string, LineHighlight>((t2.GetByIndex(drs.DestIndex + i) as TextLine).Line, LineHighlight.Update));
                        }
                        break;
                }
            }

            LoadDiffResults(Txt1, finalT1);
            LoadDiffResults(Txt2, finalT2);
        }
        private void LoadDiffResults(TextEditorControl txtEditor, List<Tuple<string, LineHighlight>> diffResults)
        {
            StringBuilder buff = new StringBuilder();
            foreach (Tuple<string, LineHighlight> t in diffResults)
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
                        txtEditor.MarkLine(i, Color.Khaki);
                        break;
                    case LineHighlight.Missing:
                        txtEditor.MarkLine(i, Color.Gainsboro);
                        break;
                    case LineHighlight.None:
                    default:
                        break;
                }
            }
        }
    }
    public enum LineHighlight { Add, Remove, Update, None, Missing }
}
