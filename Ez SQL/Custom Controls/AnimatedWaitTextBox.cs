using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ez_SQL.Custom_Controls
{
    public delegate void OnTextWaitEnded(string Text, int Decimals);
    public delegate void OnTextSecured(string Text);
    public partial class AnimatedWaitTextBox : UserControl
    {
        int CurPos, CurImage;
        private int _WaitInterval;
        public event OnTextWaitEnded TextWaitEnded;
        public event OnTextSecured TextSecured;
        public event KeyPressEventHandler KeyPressed;
        public event KeyEventHandler KeyDowned;
        public AnimatedWaitTextBox()
        {
            InitializeComponent();
            CurPos = CurImage = 0;
        }
        private void TOText_FontChanged(object sender, EventArgs e)
        {
            Edit.Font = Font;
            this.Height = Edit.Height;
        }
        public int WaitInterval
        {
            get { return _WaitInterval; }
            set { _WaitInterval = value; }
        }
        private Image _defaultImage;
        public Image DefaultImage
        {
           get
            {
                return _defaultImage;
            }
           set
            {
                _defaultImage = value;
                Img.Image = value;
            }
        }
        private void Edit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (Step.Enabled)
                {
                    Step.Enabled = false;
                }
                else
                {
                    if (TextSecured != null)
                        TextSecured(Edit.Text);
                }
                if (TextWaitEnded != null)
                    TextWaitEnded(Edit.Text, CurPos);
                CurImage = 0;
                Img.Image = _defaultImage;
            }
            else
            {
                if (KeyPressed != null)
                    KeyPressed(sender, e);
            }
        }
        private void Step_Tick(object sender, EventArgs e)
        {
            CurPos++;
            CurImage++;
            Img.Image = IList.Images[CurImage % IList.Images.Count];
            if (CurPos >= WaitInterval)
            {
                CurImage = 0;
                Step.Enabled = false;
                if (TextWaitEnded != null)
                    TextWaitEnded(Edit.Text, CurPos);
                Img.Image = _defaultImage;
            }
        }
        public override string Text
        {
            get
            {
                return Edit.Text;
            }
            set
            {
                Edit.Text = value;
            }
        }
        private void Edit_TextChanged(object sender, EventArgs e)
        {
            if (!Step.Enabled)
                Step.Enabled = true;
            CurPos = 0;
            Img.Image = IList.Images[CurImage % IList.Images.Count];
        }

        private void Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDowned != null)
                KeyDowned(sender, e);
        }

    }
}
