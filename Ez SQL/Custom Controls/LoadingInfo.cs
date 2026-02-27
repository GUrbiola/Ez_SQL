using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// A lightweight floating overlay control that displays a database name and a current action label
    /// inside a rounded-rectangle border. Used to show loading-progress feedback (e.g., "Loading tables…")
    /// while the <see cref="Ez_SQL.DataBaseObjects.SqlConnector"/> populates its object catalog.
    /// The rounded shape is achieved by setting the control's <c>Region</c> to a <see cref="GraphicsPath"/>
    /// created at construction time.
    /// </summary>
    public partial class LoadingInfo : UserControl
    {
        /// <summary>Initializes the control and clips its bounds to a rounded rectangle.</summary>
        public LoadingInfo()
        {
            InitializeComponent();
            Region = new Region(graphicsPath = CreateRoundRectangle(Width - 1, Height - 1, 6));
        }
        private static GraphicsPath CreateRoundRectangle(int w, int h, int r)
        {
            int d = r << 1;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, d, d), 180, 90);
            path.AddLine(r, 0, w - r, 0);
            path.AddArc(new Rectangle(w - d, 0, d, d), 270, 90);
            path.AddLine(w + 1, r, w + 1, h - r);
            path.AddArc(new Rectangle(w - d, h - d, d, d), 0, 90);
            path.AddLine(w - r, h + 1, r, h + 1);
            path.AddArc(new Rectangle(0, h - d, d, d), 90, 90);
            path.AddLine(0, h - r, 0, r);
            path.CloseFigure();
            return path;
        }
        private GraphicsPath graphicsPath;
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(-1, -1);
            using (Pen p = new Pen(SystemColors.WindowFrame, 2))
            {
                e.Graphics.DrawPath(p, graphicsPath);
            }
            e.Graphics.ResetTransform();
        }
        /// <summary>
        /// Updates the displayed database name and action label.
        /// </summary>
        /// <param name="DB">The database name to display.</param>
        /// <param name="Action">A short description of the current loading action (e.g., "Loading tables…").</param>
        public void SetInfo(string DB, string Action)
        {
            LDB.Text = DB;
            LAction.Text = Action;
        }
    }
}
