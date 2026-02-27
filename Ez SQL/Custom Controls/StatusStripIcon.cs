using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// A <see cref="ToolStripControlHost"/> that embeds a <see cref="PictureBox"/> as an item
    /// in a <see cref="MenuStrip"/>, <see cref="ContextMenuStrip"/>, or <see cref="StatusStrip"/>.
    /// Allows an image/icon to be placed directly inside a tool strip without a separate label.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class StatusStripIcon : ToolStripControlHost
    {
        /// <summary>The embedded <see cref="PictureBox"/> control that renders the icon image.</summary>
        private PictureBox Pict;

        public StatusStripIcon() : base(new PictureBox())
        {
            this.Pict = this.Control as PictureBox;
        }

        // Add properties, events etc. you want to expose...
    }
}
