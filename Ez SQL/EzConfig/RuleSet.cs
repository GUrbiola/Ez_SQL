using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL.EzConfig
{
    public class RuleSet
    {
        public string Name { get; set; }
        public bool IgnoreCase { get; set; }
        public Delimiter Delimiters { get; set; }
        public List<ConfigRule> Rules { get; set; }
        public bool IsMainRuleSet { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (IsMainRuleSet)
            {
                sb.AppendLine(String.Format("<RuleSet ignorecase=\"{0}\">", IgnoreCase.BoolAsString()).Indent(2));
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
