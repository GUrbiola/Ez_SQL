using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    /// <summary>
    /// Represents a single highlighting rule within a <see cref="RuleSet"/>.
    /// A rule is either a <c>Span</c> (delimited region such as a comment or string literal)
    /// or a <c>KeyWords</c> group (a list of exact token matches).
    /// Serializes to the corresponding ICSharpCode SyntaxDefinition XML element via <see cref="ToString"/>.
    /// </summary>
    public class ConfigRule
    {
        /// <summary>Gets or sets the rule type: <c>"Span"</c> or <c>"KeyWords"</c>.</summary>
        public string Type { get; set; }

        /// <summary>Gets or sets the display name of this rule (used as the XML <c>name</c> attribute).</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets whether tokens matched by this rule are rendered in bold.</summary>
        public bool Bold { get; set; }

        /// <summary>Gets or sets whether tokens matched by this rule are rendered in italic.</summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets whether a span rule stops at end-of-line (e.g., <c>true</c> for single-line comments).
        /// Not used for <c>KeyWords</c> rules.
        /// </summary>
        public bool StopAtEOL { get; set; }

        /// <summary>
        /// Gets or sets the name of a nested <see cref="RuleSet"/> to apply inside this span.
        /// Only relevant for <c>Span</c> rules.
        /// </summary>
        public string Rule { get; set; }

        /// <summary>Gets or sets the foreground color used for tokens matched by this rule.</summary>
        public Color Color { get; set; }

        /// <summary>Gets or sets the keyword list for <c>KeyWords</c>-type rules.</summary>
        public List<string> Words { get; set; }

        /// <summary>
        /// Gets or sets the begin/end delimiters for <c>Span</c>-type rules
        /// (keys: <c>"Begin"</c> and optionally <c>"End"</c>).
        /// </summary>
        public Dictionary<string, string> SpecialSymbols;
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(Rule))
            {
                if (Type.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                {
                    sb.AppendLine
                        (
                            String.Format
                                (
                                    "<{0} name=\"{1}\" bold=\"{2}\" italic=\"{3}\" color=\"{4}\" stopateol =\"{5}\" >",
                                    Type,
                                    Name,
                                    Bold.BoolAsString(),
                                    Italic.BoolAsString(),
                                    Color.ColorToString(),
                                    StopAtEOL.BoolAsString()
                                ).Indent(3)
                        );
                }
                else
                {
                    sb.AppendLine
                        (
                            String.Format
                                (
                                    "<{0} name=\"{1}\" bold=\"{2}\" italic=\"{3}\" color=\"{4}\" >",
                                    Type,
                                    Name,
                                    Bold.BoolAsString(),
                                    Italic.BoolAsString(),
                                    Color.ColorToString()
                                ).Indent(3)
                        );                    
                }
            }
            else
            {
                if (Type.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                {
                    sb.AppendLine
                        (
                            String.Format
                            (
                                "<{0} name=\"{1}\" rule=\"{2}\" bold=\"{3}\" italic=\"{4}\" color=\"{5}\" stopateol=\"{6}\" >",
                                Type,
                                Name,
                                Rule,
                                Bold.BoolAsString(),
                                Italic.BoolAsString(),
                                Color.ColorToString(),
                                StopAtEOL.BoolAsString()
                            ).Indent(3)
                        );
                }
                else
                {
                    sb.AppendLine
                        (
                            String.Format
                            (
                                "<{0} name=\"{1}\" rule=\"{2}\" bold=\"{3}\" italic=\"{4}\" color=\"{5}\" >",
                                Type,
                                Name,
                                Rule,
                                Bold.BoolAsString(),
                                Italic.BoolAsString(),
                                Color.ColorToString()
                            ).Indent(3)
                        );
                }
            }

            if (SpecialSymbols == null || SpecialSymbols.Count == 0)
            {
                foreach (string word in Words)
                {
                    sb.AppendLine(String.Format("<Key word=\"{0}\" />", word.AsValidXML()).Indent(4));
                }
            }
            else
            {
                if (Type.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (KeyValuePair<string, string> specialSymbol in SpecialSymbols)
                    {
                        sb.AppendLine(String.Format("<{0}>{1}</{0}>", specialSymbol.Key, specialSymbol.Value.AsValidXML()).Indent(4));
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> specialSymbol in SpecialSymbols)
                    {
                        sb.AppendLine(String.Format("<{0}>{1}</{0}>", specialSymbol.Key, specialSymbol.Value.AsValidXML()).Indent(4));
                    }
                }
            }
            sb.AppendLine(String.Format("</{0}>", Type).Indent(3));

            return sb.ToString();

        }
    }
}
