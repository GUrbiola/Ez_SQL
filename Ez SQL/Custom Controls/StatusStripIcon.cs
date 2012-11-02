using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Ez_SQL.Custom_Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class StatusStripIcon : ToolStripControlHost
    {
        private PictureBox Pict;

        public StatusStripIcon() : base(new PictureBox())
        {
            this.Pict = this.Control as PictureBox;
        }

        // Add properties, events etc. you want to expose...
    }
}
