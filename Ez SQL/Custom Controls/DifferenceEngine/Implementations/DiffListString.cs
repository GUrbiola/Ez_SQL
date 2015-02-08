using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    public class DiffListString : IDiffList
    {
        private string chars;

        public DiffListString(string str)
        {
            chars = str;
        }

        #region IDiffList Members
        public int Count()
        {
            return String.IsNullOrEmpty(chars) ? 0 : chars.Length;
        }

        public IComparable GetByIndex(int index)
        {
            return chars[index];
        }
        #endregion
    }
}
