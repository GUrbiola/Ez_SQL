using Ez_SQL.Common_Code;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    /// <summary>
    /// An <see cref="IDiffList"/> implementation that tokenizes each line of a multi-line SQL
    /// script using <see cref="Ez_SQL.Common_Code.TokenList"/> and treats each line's
    /// <see cref="Ez_SQL.Common_Code.TokenList"/> as a single diffable element.
    /// Used by <see cref="Ez_SQL.Custom_Controls.SideToSideTextComparer.LoadTokenizers"/> for
    /// token-aware line-level diffs where lines with minor whitespace differences
    /// compare as equal if their tokens match.
    /// </summary>
    public class DiffListToken : IDiffList
    {
        private List<TokenList> Lines;

        /// <summary>
        /// Splits <paramref name="script"/> into lines and tokenizes each one.
        /// </summary>
        /// <param name="script">The multi-line SQL script to tokenize and diff.</param>
        public DiffListToken(string script)
        {
            Lines = new List<TokenList>();
            foreach (string line in script.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Lines.Add(line.GetTokens());
            }
        }

        /// <inheritdoc/>
        public int Count()
        {
            return Lines.Count;
        }

        /// <inheritdoc/>
        public IComparable GetByIndex(int index)
        {
            if (index >= 0 && index < Lines.Count)
            {
                return Lines[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range in DiffListToken.GetByIndex");
            }
        }
    }

    /// <summary>
    /// An <see cref="IDiffList"/> implementation that tokenizes a single line of SQL text and
    /// exposes each individual <see cref="Ez_SQL.Common_Code.Token"/> as a diffable element.
    /// Used by <see cref="Ez_SQL.Custom_Controls.SideToSideLineComparer.LoadTokenizers"/> for
    /// token-level intra-line comparison, grouping SQL keywords, identifiers, and operators
    /// into atomic diff units rather than individual characters.
    /// </summary>
    public class DiffListLineToken : IDiffList
    {
        private TokenList tokens;

        /// <summary>Tokenizes <paramref name="script"/> into individual SQL tokens for single-line diffing.</summary>
        /// <param name="script">The single-line SQL text to tokenize.</param>
        public DiffListLineToken(string script)
        {
            tokens = script.GetTokens();
        }

        /// <inheritdoc/>
        public int Count()
        {
            return tokens.TokenCount;
        }

        /// <inheritdoc/>
        public IComparable GetByIndex(int index)
        {
            if (index >= 0 && index < tokens.TokenCount)
            {
                return tokens.GetToken(index);
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range in DiffListToken.GetByIndex");
            }
        }
    }
}
