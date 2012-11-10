using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL
{
    public enum TokenType { EMPTYSPACE, RESERVED, COMMA, WORD, VARIABLE, TEMPTABLE, OPENBRACKET, CLOSEBRACKET, LINECOMMENT, BLOCKCOMMENT, STRING, OPERATOR, COMPARATOR, DATATYPE, BLOCKSTART, BLOCKEND };
    public class Token
    {
        public TokenType Type { get; set; }
        public string Text { get; set; }
        public Token() { }
        public Token(TokenType Type, string Text)
        {
            this.Type = Type;
            this.Text = Text;
        }
        public bool IsTextEmpty { get { return String.IsNullOrEmpty(Text); } }
    }
}
