using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// Delegate for the <see cref="AnimatedWaitTextBox.TextWaitEnded"/> event,
    /// raised when the debounce timer elapses or the user presses Enter.
    /// </summary>
    /// <param name="Text">The current text in the input field.</param>
    /// <param name="Decimals">The number of timer ticks that elapsed since the last keystroke.</param>
    public delegate void OnTextWaitEnded(string Text, int Decimals);

    /// <summary>
    /// Delegate for the <see cref="AnimatedWaitTextBox.TextSecured"/> event,
    /// raised when the user presses Enter after the debounce timer has already fired.
    /// </summary>
    /// <param name="Text">The current text in the input field.</param>
    public delegate void OnTextSecured(string Text);

    /// <summary>
    /// A text box that delays raising its change notification by a configurable number of timer ticks
    /// (<see cref="WaitInterval"/>), providing a debounce effect suitable for incremental search.
    /// While the user types, an animated image cycles through frames in the <c>IList</c> image list.
    /// When the timer completes or the user presses Enter, the <see cref="TextWaitEnded"/> event fires.
    /// If the user presses Enter after the timer has already fired, <see cref="TextSecured"/> fires instead.
    /// </summary>
    public partial class AnimatedWaitTextBox : UserControl
    {
        /// <summary>Tracks the number of timer ticks since the last text change.</summary>
        int CurPos;

        /// <summary>Tracks the current animation frame index in the image list.</summary>
        int CurImage;

        private int _WaitInterval;

        /// <summary>Raised when the debounce period elapses or the user presses Enter.</summary>
        public event OnTextWaitEnded TextWaitEnded;

        /// <summary>Raised when the user presses Enter after the debounce timer has already completed.</summary>
        public event OnTextSecured TextSecured;

        /// <summary>Raised for each key press that is not the Enter key.</summary>
        public event KeyPressEventHandler KeyPressed;

        /// <summary>Raised for each key-down event in the internal text box.</summary>
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
        /// <summary>
        /// Gets or sets the number of timer ticks to wait after the last keystroke before
        /// raising <see cref="TextWaitEnded"/>. Higher values increase the debounce delay.
        /// </summary>
        public int WaitInterval
        {
            get { return _WaitInterval; }
            set { _WaitInterval = value; }
        }
        private Image _defaultImage;

        /// <summary>
        /// Gets or sets the static image displayed when no animation is running.
        /// Setting this property also immediately applies the image to the picture box.
        /// </summary>
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
