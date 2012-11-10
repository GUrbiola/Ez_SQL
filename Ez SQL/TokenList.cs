using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL
{
    public class TokenList
    {
        public List<Token> List { get; set; }
        public List<int> StartOffsets { get; set; }
        public List<int> EndOffsets { get; set; }
        public List<int> TokenLengths { get; set; }
        public int TokenCount { get { return List.Count; } } 
       
        public TokenList()
        {
            List = new List<Token>();
            StartOffsets = new List<int>();
            EndOffsets = new List<int>();
            TokenLengths = new List<int>();
        }
        public void AddToken(Token Current)
        {
            if (Current == null || Current.IsTextEmpty)
                return;

            switch (Current.Type)
            {
                default:
                //if this token is a variable is going to be determined in this method, initially the token must be marked as word
                case TokenType.VARIABLE:
                //if this token is a temptable is going to be determined in this method, initially the token must be marked as word
                case TokenType.TEMPTABLE:
                //if this token is a reserved word is going to be determined in this method, initially the token must be marked as word
                case TokenType.RESERVED:
                //this types of token are already processed before this, so we must only add it to the list of tokens without any further check                
                case TokenType.OPENBRACKET:
                case TokenType.CLOSEBRACKET:
                case TokenType.LINECOMMENT:
                case TokenType.BLOCKCOMMENT:
                case TokenType.STRING:
                case TokenType.EMPTYSPACE:
                case TokenType.COMMA:
                    if(List.Count == 0)
                    {
                        StartOffsets.Add(0);
                    }
                    else
                    {
                        StartOffsets.Add(StartOffsets.Last() + TokenLengths.Last());
                    }
                    EndOffsets.Add(StartOffsets.Last() + Current.Text.Length - 1);
                    TokenLengths.Add(Current.Text.Length);
                    List.Add(Current);
                    break;
                case TokenType.WORD:
                    if (Current.Text.IsReserved())
                    {
                        Current.Type = TokenType.RESERVED;
                    }
                    else if (Current.Text.IsDataType())
                    {
                        Current.Type = TokenType.DATATYPE;
                    }
                    else if (Current.Text.StartsWith("#") && Current.Text.Length > 1)
                    {
                        Current.Type = TokenType.TEMPTABLE;
                    }
                    else if (Current.Text.StartsWith("@") && Current.Text.Length > 1)
                    {
                        Current.Type = TokenType.VARIABLE;
                    }
                    else if (Current.Text.Equals("begin", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Current.Type = TokenType.BLOCKSTART;
                    }
                    else if (Current.Text.Equals("end", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Current.Type = TokenType.BLOCKEND;
                    }


                    if(List.Count == 0)
                    {
                        StartOffsets.Add(0);
                    }
                    else
                    {
                        StartOffsets.Add(StartOffsets.Last() + TokenLengths.Last());
                    }
                    EndOffsets.Add(StartOffsets.Last() + Current.Text.Length - 1);
                    TokenLengths.Add(Current.Text.Length);
                    List.Add(Current);
                    break;
            }
        }
        public int GetStartOf(Token token)
        {
            int index = List.IndexOf(token);
            return GetStartOf(index);
        }
        public int GetStartOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return StartOffsets[tokenIndex];
            return -1;
        }
        public int GetEndOf(Token token)
        {
            int index = List.IndexOf(token);
            return GetEndOf(index);
        }
        public int GetEndOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return EndOffsets[tokenIndex];
            return -1;
        }
        public int GetLengthOf(Token token)
        {
            int index = List.IndexOf(token);
            return GetLengthOf(index);
        }
        public int GetLengthOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return TokenLengths[tokenIndex];
            return -1;
        }
        public void Clean()
        {
            List.Clear();
            StartOffsets.Clear();
            EndOffsets.Clear();
            TokenLengths.Clear();
        }
        public Token GetToken(int tokenIndex)
        {
            return tokenIndex < List.Count ? List[tokenIndex] : null;
        }
        public void RemoveTokenAt(int tokenIndex)
        {
            if (tokenIndex < List.Count)
            {
                List.RemoveAt(tokenIndex);
                StartOffsets.RemoveAt(tokenIndex);
                EndOffsets.RemoveAt(tokenIndex);
                TokenLengths.RemoveAt(tokenIndex);
                if (List.Count > 0)
                {
                    for (int ind = tokenIndex; ind < List.Count; ind++)
                    {
                        if (tokenIndex == 0 && ind == 0)
                        {
                            StartOffsets[0] = 0;
                            EndOffsets[0] = StartOffsets[0] + List[0].Text.Length - 1;
                            TokenLengths[0] = EndOffsets[0] - StartOffsets[0] + 1;
                        }
                        else
                        {
                            StartOffsets[ind] = StartOffsets[ind - 1] + TokenLengths[ind - 1];
                            EndOffsets[ind] = StartOffsets[ind] + List[ind].Text.Length - 1;
                            TokenLengths[ind] = EndOffsets[ind] - StartOffsets[ind] + 1;
                        }
                    }
                }
            }
        }
        public Token this[int index]
        {
            get { return GetToken(index); }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Token t in List)
            {
                sb.Append(t.Text);
            }
            return sb.ToString();
        }
        public Token GetTokenAtOffset(int offset, out int index)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (StartOffsets[i] <= offset && offset <= EndOffsets[i])
                {
                    index = i;
                    return GetToken(i);
                }
            }
            index = -1;
            return null;
        }
        internal List<Token> GetByType(TokenType tokenType)
        {
            return List.Where(X => X.Type == tokenType).ToList();
        }
        internal List<Token> GetCustomFolders(bool beginFold)
        {
            if (beginFold)
                return List.Where(X => X.Type == TokenType.LINECOMMENT && X.Text.StartsWith("--fold", StringComparison.CurrentCultureIgnoreCase)).ToList();
            return List.Where(X => X.Type == TokenType.LINECOMMENT && X.Text.StartsWith("--/fold", StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }
}
