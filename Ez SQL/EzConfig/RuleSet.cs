using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Represents a named group of syntax-highlighting rules for the ICSharpCode text editor,
    /// corresponding to a <c>&lt;RuleSet&gt;</c> element in a SyntaxDefinition XML file.
    /// The main rule set (IsMainRuleSet = true) has no name attribute in XML;
    /// sub-rule-sets are referenced by name from span rules in other rule sets.
    /// </summary>
    public class RuleSet
    {
        /// <summary>Gets or sets the name of this rule set (used as the XML <c>name</c> attribute for sub-rule-sets).</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets whether keyword matching is case-insensitive for this rule set.</summary>
        public bool IgnoreCase { get; set; }

        /// <summary>Gets or sets the delimiter character set that separates tokens in this rule set.</summary>
        public Delimiter Delimiters { get; set; }

        /// <summary>Gets or sets the ordered list of span and keyword rules in this rule set.</summary>
        public List<ConfigRule> Rules { get; set; }

        /// <summary>Gets or sets whether this is the primary (unnamed) rule set in the syntax definition.</summary>
        public bool IsMainRuleSet { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (IsMainRuleSet)
            {
                sb.AppendLine("<RuleSet ignorecase=\"true\">".Indent(2));
            }
            else
            {
                sb.AppendLine(String.Format("<RuleSet name=\"{0}\" ignorecase=\"true\">", Name).Indent(2));
            }

            sb.AppendLine(Delimiters.ToString());
            foreach (ConfigRule cr in Rules)
            {
                sb.AppendLine(cr.ToString());
            }
            sb.AppendLine("</RuleSet>".Indent(2));
            return sb.ToString();
        }
    }
}
