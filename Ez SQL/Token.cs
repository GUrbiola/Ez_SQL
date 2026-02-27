using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL
{
    /// <summary>
    /// Classifies the syntactic role of a token produced by the SQL lexer.
    /// Used by <see cref="TokenList"/> and <see cref="TextEditorClasses.FoldingStrategy"/> to parse query structure.
    /// </summary>
    public enum TokenType
    {
        EMPTYSPACE, RESERVED, COMMA, WORD, VARIABLE, TEMPTABLE,
        OPENBRACKET, CLOSEBRACKET, LINECOMMENT, BLOCKCOMMENT, STRING,
        OPERATOR, COMPARATOR, DATATYPE, BLOCKSTART, BLOCKEND,
        BEGINTRANSACTION, SEMMICOLON, TABLE, VIEW
    };

    /// <summary>
    /// Represents a single lexical token parsed from SQL text.
    /// Stores the token's text content and its classified <see cref="TokenType"/>.
    /// Implements <see cref="IComparable"/> so token lists can be sorted and compared.
    /// </summary>
    public class Token : IComparable
    {
        /// <summary>Gets or sets the syntactic category of this token.</summary>
        public TokenType Type { get; set; }

        /// <summary>Gets or sets the raw SQL text of this token.</summary>
        public string Text { get; set; }

        /// <summary>Initializes an empty token.</summary>
        public Token() { }

        /// <summary>Initializes a token with the specified type and text.</summary>
        /// <param name="Type">The syntactic category.</param>
        /// <param name="Text">The raw SQL text.</param>
        public Token(TokenType Type, string Text)
        {
            this.Type = Type;
            this.Text = Text;
        }
        /// <summary>Gets a value indicating whether <see cref="Text"/> is null or empty.</summary>
        public bool IsTextEmpty { get { return String.IsNullOrEmpty(Text); } }
        public override string ToString() { return Text; }

        public int CompareTo(Object obj)
        {
            Token token = obj as Token;

            if (token == null) 
                return 1;

            if(this.Type == TokenType.EMPTYSPACE && token.Type == TokenType.EMPTYSPACE)
                return 0;

            int typeComparison = Type.CompareTo(token.Type);
            if (typeComparison != 0) 
                return typeComparison;

            return Text.ToUpper().CompareTo(token.Text.ToUpper());
        }
    }
}
