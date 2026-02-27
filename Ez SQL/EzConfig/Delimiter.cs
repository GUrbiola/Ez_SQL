using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Defines the set of characters treated as token delimiters in a <see cref="RuleSet"/>.
    /// Serializes to a <c>&lt;Delimiters&gt;</c> XML element for use by the ICSharpCode text editor.
    /// </summary>
    public class Delimiter
    {
        /// <summary>Gets or sets the string of individual delimiter characters (e.g., <c>"=!+-/*.,;"</c>).</summary>
        public string DelimiterChars { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.Format("<Delimiters>{0}</Delimiters>", DelimiterChars).Indent(3);
        }
    }
}
