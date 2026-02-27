using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Stores the global color settings for the SQL text editor's visual environment,
    /// including background, font, line-number area, and text-selection colors.
    /// Serializes to the <c>&lt;Environment&gt;</c> block in a SyntaxDefinition XML file.
    /// </summary>
    public class Environment
    {
        /// <summary>The editor background color (Default bgcolor).</summary>
        public Color BackgroundColor;

        /// <summary>The default text foreground color (Default color).</summary>
        public Color FontColor;

        /// <summary>The background color of the line-number gutter.</summary>
        public Color LineNumberBackgroundColor;

        /// <summary>The foreground color used to render line numbers.</summary>
        public Color LineNumberFontColor;

        /// <summary>The background color of the selected text region.</summary>
        public Color SelectionColor;


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<Environment>".Indent(1));

            sb.AppendLine(String.Format("<Default color=\"{0}\" bgcolor=\"{1}\"/>", FontColor.ColorToString(), BackgroundColor.ColorToString()).Indent(2));
            sb.AppendLine("<VRuler color = \"#0000FF\"/>".Indent(2));
            sb.AppendLine(String.Format("<Selection bgcolor=\"{0}\"/>", SelectionColor.ColorToString()).Indent(2));
            sb.AppendLine(String.Format("<LineNumbers color=\"{0}\" bgcolor=\"{1}\"/>", LineNumberFontColor.ColorToString(), LineNumberBackgroundColor.ColorToString()).Indent(2));
            sb.AppendLine("<InvalidLines color = \"#FF0000\"/>".Indent(2));
            sb.AppendLine("<EOLMarkers color = \"#FFFFFF\"/>".Indent(2));
            sb.AppendLine("<SpaceMarkers color = \"#E0E0E5\"/>".Indent(2));
            sb.AppendLine("<TabMarkers color = \"#E0E0E5\"/>".Indent(2));
            sb.AppendLine("<CaretMarker color = \"#FFFF00\"/>".Indent(2));
            sb.AppendLine("<FoldLine color = \"#808080\" bgcolor=\"#000000\"/>".Indent(2));
            sb.AppendLine(String.Format("<FoldMarker color = \"#808080\" bgcolor=\"{0}\"/>", LineNumberBackgroundColor.ColorToString()).Indent(2));

            sb.AppendLine("</Environment>".Indent(1));

            return sb.ToString();
        }
    }
}
