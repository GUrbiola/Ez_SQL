using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL.EzConfig
{
    public class ConfigRule
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool StopAtEOL { get; set; }
        public string Rule { get; set; }
        public Color Color { get; set; }
        public List<string> Words { get; set; }
        public Dictionary<string, string> SpecialSymbols;
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(Rule))
            {
                sb.AppendLine
                    (
                        String.Format
                        (
                            "<{0} name =\"{1}\" bold=\"{2}\" italic=\"{3}\" color =\"{4}\" stopateol =\"{5}\">",
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
                            "<{0} name =\"{1}\" rule=\"{2}\" bold=\"{3}\" italic=\"{4}\" color =\"{5}\" stopateol =\"{6}\">",
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
            if (SpecialSymbols == null || SpecialSymbols.Count == 0)
            {
                foreach (string word in Words)
                {
                    sb.AppendLine(String.Format("<Key word=\"{0}\" />", word).Indent(4));
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> specialSymbol in SpecialSymbols)
                {
                    sb.AppendLine(String.Format("<{0}>{1}</{0}>", specialSymbol.Key, specialSymbol.Value).Indent(4));
                }
            }
            sb.AppendLine(String.Format("</{0}>", Type).Indent(3));

            return sb.ToString();
        }
    }
}
