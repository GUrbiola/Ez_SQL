using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    /// <summary>
    /// An <see cref="IDiffList"/> implementation that exposes each <see cref="char"/> of a string
    /// as an individually diffable element. Used by <see cref="Ez_SQL.Custom_Controls.SideToSideLineComparer"/>
    /// to perform character-level comparison of a single line of text.
    /// </summary>
    public class DiffListString : IDiffList
    {
        private string chars;

        /// <summary>Initializes the list from the given string; each character becomes one diffable element.</summary>
        /// <param name="str">The string whose characters will be diffed.</param>
        public DiffListString(string str)
        {
            chars = str;
        }

        #region IDiffList Members
        /// <inheritdoc/>
        public int Count()
        {
            return String.IsNullOrEmpty(chars) ? 0 : chars.Length;
        }

        /// <inheritdoc/>
        public IComparable GetByIndex(int index)
        {
            return chars[index];
        }
        #endregion
    }
}
