using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Defines the syntax-highlighting style applied to numeric literals in the SQL editor.
    /// Serializes to a <c>&lt;Digits&gt;</c> element in the SyntaxDefinition XML.
    /// </summary>
    public class Digits
    {
        /// <summary>Gets or sets whether numeric literals are rendered in bold.</summary>
        public bool Bold { get; set; }

        /// <summary>Gets or sets whether numeric literals are rendered in italic.</summary>
        public bool Italic { get; set; }

        /// <summary>Gets or sets the foreground color used to render numeric literals.</summary>
        public Color Color { get; set; }

        public override string ToString()
        {
            return String.Format("<Digits name=\"Digits\" bold=\"{0}\" italic=\"{1}\" color=\"{2}\"/>", Bold.BoolAsString(), Italic.BoolAsString(), Color.ColorToString()).Indent(1);
        }
    }
}
