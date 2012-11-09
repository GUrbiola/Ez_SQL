using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez_SQL.Extensions;

namespace Ez_SQL
{
    public class SQLTokenizer
    {
        public static HashSet<string> ReservedWords = new HashSet<string>{"@@IDENTITY","ADD","ALL","ALTER","AND","ANY ","AS","ASC","AUTHORIZATION","AVG ","BACKUP",
                                                      "BEGIN","BETWEEN","BREAK","BROWSE","BULK","BY","CASCADE","CASE","CHECK","CHECKPOINT","CLOSE",
                                                      "CLUSTERED","COALESCE","COLLATE","COLUMN","COMMIT","COMPUTE","CONSTRAINT","CONTAINS","CONTAINSTABLE",
                                                      "CONTINUE","CONVERT","COUNT","CREATE","CROSS","CURRENT","CURRENT_DATE","CURRENT_TIME","CURRENT_TIMESTAMP",
                                                      "CURRENT_USER","CURSOR","DATABASE","DATABASEPASSWORD","DATEADD","DATEDIFF","DATENAME","DATEPART",
                                                      "DBCC","DEALLOCATE","DECLARE","DEFAULT","DELAY","DELETE","DENY","DESC","DISK","DISTINCT","DISTRIBUTED",
                                                      "DOUBLE","DROP","DUMP","ELSE","ENCRYPTION","END","ERRLVL","ESCAPE","EXCEPT","EXEC","EXECUTE",
                                                      "EXISTS","EXIT","EXPRESSION","FETCH","FILE","FILLFACTOR","FOR","FOREIGN","FREETEXT","FREETEXTTABLE",
                                                      "FROM","FULL","FUNCTION","GOTO","GRANT","GROUP","HAVING","HOLDLOCK","IDENTITY","IDENTITY_INSERT",
                                                      "IDENTITYCOL","IF","IN","INDEX","INNER","INSERT","INTERSECT","INTO","IS","JOIN","KEY",
                                                      "KILL","LEFT","LIKE","LINENO","LOAD","MAX","MIN","NATIONAL","NOCHECK","NONCLUSTERED",
                                                      "NOT","NULL","NULLIF","OF","OFF","OFFSETS","ON","OPEN","OPENDATASOURCE","OPENQUERY",
                                                      "OPENROWSET","OPENXML","OPTION","OR","ORDER","OUTER","OVER","PERCENT","PLAN","PRECISION",
                                                      "PRIMARY","PRINT","PROC","PROCEDURE","PUBLIC","RAISERROR","READ","READTEXT","RECONFIGURE","REFERENCES",
                                                      "REPLICATION","RESTORE","RESTRICT","RETURN","REVOKE","RIGHT","ROLLBACK","ROWCOUNT","ROWGUIDCOL",
                                                      "RULE","SAVE","SCHEMA","SELECT","SESSION_USER","SET","SETUSER","SHUTDOWN","SOME","STATISTICS",
                                                      "SUM","SYSTEM_USER","TABLE","TEXTSIZE","THEN","TO","TOP","TRAN","TRANSACTION","TRIGGER","TRUNCATE",
                                                      "TSEQUAL","UNION","UNIQUE","UPDATE","UPDATETEXT","USE","USER","VALUES","VARYING","VIEW","WAITFOR",
                                                      "WHEN","WHERE","WHILE","WITH","WRITETEXT"};
        internal static List<Token> GetTokens(string Text)
        {
            //char[] EMPTYTOKENS = { ' ', '\t', '\r', '\n' };
            List<Token> Back = new List<Token>();
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            Token Current = null;

            for (int index = 0; index < StringLength; index++)
            {
                char CurChar = Text[index];

                if (IsWhiteSpace(CurChar))
                {
                    if (Current == null)
                    {//no previous token, so create a new token
                        Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                    }
                    else if (Current.Type == TokenType.EMPTYSPACE)
                    {//previous token is the same, an empty space, so append to text
                        Current.Text = Current.Text.AppendChar(CurChar);
                    }
                    else
                    {//previous token is different, add and then create token for current char
                        AddToken(Back, Current);
                        Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                    }
                }
                else if (IsComma(CurChar))
                {
                    if (Current == null)
                    {//no previous token, in this just add the new token and continue
                        AddToken(Back, new Token(TokenType.COMMA, ","));
                    }
                    else
                    {//if last token not empty, add now, because this chars means its end, also add the new token found
                        AddToken(Back, Current);
                        AddToken(Back, new Token(TokenType.COMMA, ","));
                        Current = null;
                    }
                }
                else if (IsOpenBracket(CurChar))
                {
                    if (Current == null)
                    {//no previous token, in this just add the new token and continue
                        AddToken(Back, new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                    }
                    else
                    {//if last token not empty, add now, because this chars means its end, also add the new token found
                        AddToken(Back, Current);
                        AddToken(Back, new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                        Current = null;
                    }
                }
                else if (IsCloseBracket(CurChar))
                {
                    if (Current == null)
                    {//no previous token, in this just add the new token and continue
                        AddToken(Back, new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                    }
                    else
                    {//if last token not empty, add now, because this chars means its end, also add the new token found
                        AddToken(Back, Current);
                        AddToken(Back, new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                        Current = null;
                    }
                }
                else //token was not processed before, must be a word part
                {
                    if (Current == null)
                    {
                        Current = new Token(TokenType.WORD, CurChar.ToString());
                    }
                    else
                    {
                        Current.Text = Current.Text.AppendChar(CurChar);
                    }
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
        private static bool IsComma(char c)
        {
            return c == ',';
        }
        private static bool IsOpenBracket(char c)
        {
            if (c == '(' || c == '[' || c == '{')
            {
                return true;
            }
            return false;
        }
        private static bool IsCloseBracket(char c)
        {
            if (c == ')' || c == ']' || c == '}')
            {
                return true;
            }
            return false;
        }
        private static void AddToken(List<Token> TokenList, Token Current)
        {
            if (TokenList == null)
                TokenList = new List<Token>();
            if (Current == null || Current.IsTextEmpty)
                return;

            switch (Current.Type)
            {
                default:
                case TokenType.EMPTYSPACE:
                case TokenType.CLOSEBRACKET:
                case TokenType.OPENBRACKET:
                case TokenType.COMMA:
                    TokenList.Add(Current);
                    break;
                case TokenType.WORD:
                    if (Current.Text.StartsWith("@") && Current.Text.Length > 0)
                    {
                        Current.Type = TokenType.VARIABLE;
                    }
                    else if (Current.Text.StartsWith("#") && Current.Text.Length > 0)
                    {
                        Current.Type = TokenType.TEMPTABLE;
                    }
                    else if (ReservedWords.Contains(Current.Text.ToUpper()))
                    {
                        Current.Type = TokenType.RESERVED;
                    }
                    TokenList.Add(Current);
                    break;
            }
        }
    }
}
