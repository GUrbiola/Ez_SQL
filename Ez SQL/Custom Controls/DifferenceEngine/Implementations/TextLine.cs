using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    /// <summary>
    /// An <see cref="IComparable"/> wrapper around a single line of text used as the diffable
    /// element in <see cref="DiffListText"/> and <see cref="DiffListTextFile"/>.
    /// Tab characters are normalised to four spaces on construction; equality comparisons
    /// use a pre-computed hash code for fast LCS matching inside
    /// <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine"/>.
    /// </summary>
    public class TextLine : IComparable
    {
        /// <summary>The line content with tabs expanded to four spaces.</summary>
        public string Line;
        /// <summary>Pre-computed hash of the original string used for fast comparison.</summary>
        public int _hash;

        /// <summary>
        /// Initializes the wrapper, expanding tabs to four spaces in <see cref="Line"/>
        /// and computing the hash from the original <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The raw line text to wrap.</param>
        public TextLine(string str)
        {
            Line = str.Replace("\t", "    ");
            _hash = str.GetHashCode();
        }
        #region IComparable Members

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((TextLine)obj)._hash);
        }

        #endregion
    }
}
