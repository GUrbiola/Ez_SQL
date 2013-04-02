using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL.EzConfig
{
    public class Digits
    {
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public Color Color { get; set; }

        public override string ToString()
        {
            return String.Format("<Digits name=\"Digits\" bold=\"{0}\" italic=\"{1}\" color=\"{2}\"/>", Bold.BoolAsString(), Italic.BoolAsString(), Color.ColorToString()).Indent(1);
        }
    }
}
