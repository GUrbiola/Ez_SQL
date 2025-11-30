using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL
{
    public enum TokenType { EMPTYSPACE, RESERVED, COMMA, WORD, VARIABLE, TEMPTABLE, OPENBRACKET, CLOSEBRACKET, LINECOMMENT, BLOCKCOMMENT, STRING, OPERATOR, COMPARATOR, DATATYPE, BLOCKSTART, BLOCKEND, BEGINTRANSACTION, SEMMICOLON, TABLE, VIEW };
    public class Token : IComparable
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
