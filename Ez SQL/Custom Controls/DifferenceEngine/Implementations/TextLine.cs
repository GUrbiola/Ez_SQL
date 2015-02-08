using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    public class TextLine : IComparable
    {
        public string Line;
        public int _hash;

        public TextLine(string str)
        {
            Line = str.Replace("\t", "    ");
            _hash = str.GetHashCode();
        }
        #region IComparable Members

        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((TextLine)obj)._hash);
        }

        #endregion
    }
}
