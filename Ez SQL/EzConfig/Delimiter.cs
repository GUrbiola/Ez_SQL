using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL.EzConfig
{
    public class Delimiter
    {
        public string DelimiterChars { get; set; }
        public override string ToString()
        {
            return String.Format("<Delimiters>{0}</Delimiters>", DelimiterChars).Indent(3);
        }
    }
}
