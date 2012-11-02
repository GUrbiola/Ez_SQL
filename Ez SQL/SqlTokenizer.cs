using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL
{
    public class SQLTokenizer
    {
        internal static List<Token> GetTokens(string Q)
        {
            //char[] EMPTYTOKENS = { ' ', '\t', '\r', '\n' };
            List<Token> Back = new List<Token>();
            int StringLength = String.IsNullOrEmpty(Q) ? 0 : Q.Length;
            bool TokenEnded = false;
            Token Current = null;

            for (int index = 0; index < StringLength; index++)
            {
                TokenEnded = false;
                char CurChar = Q[index];

                if (IsWhiteSpace(CurChar))
                {
                    if (Current != null)
                        AddToken(Back, Current);
                    Current = new Token(TokenType.EMPTYSPACE, "\n");
                    TokenEnded = true;
                }
                else
                {
                    if (CurChar == ',')
                    {
                        if (Current == null)
                        {
                            Current = new Token(TokenType.COMMA, ",");
                            TokenEnded = true;
                        }
                        else
                        {
                            if (Current != null)
                                AddToken(Back, Current);
                            Current = new Token(TokenType.COMMA, ",");
                            TokenEnded = true;
                        }
                    }
                    else
                    {
                        if (Current == null)
                            Current = new Token();
                        Current.Text = Current.Text.AppendChar(CurChar);
                    }
                }

                if (TokenEnded && Current != null)
                {
                    AddToken(Back, Current);
                    Current = null;
                    TokenEnded = false;
                }
            }

            if (Current != null)
                AddToken(Back, Current);

            return Back;
        }

        private static bool IsWhiteSpace(char c)
        {
            char[] EMPTYTOKENS = { ' ', '\t', '\r', '\n' };
            for (int i = 0; i < EMPTYTOKENS.Length; i++)
            {
                if (c == EMPTYTOKENS[i])
                    return true;
            }
            return false;
        }
        private static bool IsWhiteSpace(string str)
        {
            char[] EMPTYTOKENS = { ' ', '\t', '\r', '\n' };
            if (String.IsNullOrEmpty(str))
                return false;
            str = str.Trim(EMPTYTOKENS);
            return str.Length == 0;
        }
        private static void AddToken(List<Token> TokenList, Token Current)
        {
            if (Current == null || Current.IsEmpty)
                return;

            if (IsWhiteSpace(Current.Text))
            {
                Current.Type = TokenType.EMPTYSPACE;
                Current.Text = " ";
            }
            else if (Current.Type != TokenType.COMMA)
            {
                Current.Type = TokenType.WORD;
            }

            if (TokenList.Count == 0)
            {
                TokenList.Add(Current);
            }
            else
            {
                if (Current.Type != TokenType.EMPTYSPACE)
                {
                    TokenList.Add(Current);
                }
                else
                {
                    if (TokenList.Last().Type != TokenType.EMPTYSPACE)
                        TokenList.Add(Current);
                }
            }

        }
    }
}
