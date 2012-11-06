using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Ez_SQL
{
    public static class Extensions
    {
        #region Extensions for TextEditor
        public static void SelectLine(this TextEditorControl TxtEditor, int LineNumber)
        {
            LineSegment Line;
            ICSharpCode.TextEditor.TextLocation Start, End;

            if (LineNumber >= TxtEditor.Document.TotalNumberOfLines)
            {
                TxtEditor.ActiveTextAreaControl.SelectionManager.ClearSelection();
                return;
            }

            if (LineNumber >= 0)
            {
                Line = TxtEditor.Document.GetLineSegment(LineNumber);
                Start = new ICSharpCode.TextEditor.TextLocation(0, Line.LineNumber);
                End = new ICSharpCode.TextEditor.TextLocation(Line.Length, Line.LineNumber);
                TxtEditor.ActiveTextAreaControl.SelectionManager.SetSelection(Start, End);
                TxtEditor.ActiveTextAreaControl.ScrollTo(LineNumber);
            }
            else
            {
                TxtEditor.ActiveTextAreaControl.SelectionManager.ClearSelection();
            }
        }
        public static int CurrentOffset(this TextEditorControl TxtEditor)
        {
            return TxtEditor.Document.PositionToOffset(TxtEditor.ActiveTextAreaControl.Caret.Position);
        }
        public static void InsertString(this TextEditorControl TxtEditor, string InsStr, int Position = -1)
        {
            if (String.IsNullOrEmpty(InsStr))
                return;
            if(Position == -1)
                Position = TxtEditor.CurrentOffset();
            if (TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected)
            {
                TxtEditor.ActiveTextAreaControl.TextArea.Caret.Position = TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection[0].StartPosition;
                TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.RemoveSelectedText();
            }

            TxtEditor.Document.Insert(Position, InsStr);
            TxtEditor.ActiveTextAreaControl.Caret.Column += InsStr.Length;
        }
        #endregion

        #region Extensions for string
        public static string Left(this string s, int count)
        {
            return s.Substring(0, count);
        }
        public static string Right(this string s, int count)
        {
            return s.Substring(s.Length - count, count);
        }
        public static string Mid(this string s, int index, int count)
        {
            return s.Substring(index, count);
        }
        public static int ToInteger(this string s)
        {
            int integerValue = 0;
            int.TryParse(s, out integerValue);
            return integerValue;
        }
        public static bool IsInteger(this string s)
        {
            Regex regularExpression = new Regex("^-[0-9]+$|^[0-9]+$");
            return regularExpression.Match(s).Success;
        }
        public static string AppendChar(this string Str, char c)
        {
            if (String.IsNullOrEmpty(Str))
                return c.ToString();
            return Str.Insert(Str.Length, c.ToString());
        }
        public static Token GetLastToken(this string Text)
        {
            Token Back = new Token(TokenType.EMPTYSPACE, "");
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            for (int index = Text.Length - 1; index >= 0; index--)
            {
                char CurChar = Text[index];
                if(String.IsNullOrEmpty(Back.Text))
                {
                    if (IsWhiteSpace(CurChar))
                    {
                        Back.Type = TokenType.EMPTYSPACE;
                        Back.Text = Back.Text.Insert(0, CurChar.ToString());
                    }
                    else if(CurChar == ',')
                    {
                        Back.Type = TokenType.COMMA;
                        Back.Text = Back.Text.Insert(0, CurChar.ToString());
                        return Back;
                    }
                    else
                    {
                        Back.Type = TokenType.WORD;
                        Back.Text = Back.Text.Insert(0, CurChar.ToString());
                    }

                }
                else
                {
                    switch (Back.Type)
                    {
                        case TokenType.EMPTYSPACE:
                            if (IsWhiteSpace(CurChar))
                            {
                                Back.Text = Back.Text.Insert(0, CurChar.ToString());
                            }
                            else
                            {
                                return Back;
                            }
                        break;
                        case TokenType.WORD:
                            if (IsWhiteSpace(CurChar) || CurChar == ',')
                            {
                                return Back;
                            }
                            else
                            {
                                Back.Text = Back.Text.Insert(0, CurChar.ToString());
                            }
                        break;
                    }
                }
            }

            return Back;
        }
        public static Token GetFirstToken(this string Text)
        {
            Token Back = new Token(TokenType.EMPTYSPACE, "");
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            for (int index = 0; index < Text.Length; index++)
            {
                char CurChar = Text[index];
                if (String.IsNullOrEmpty(Back.Text))
                {
                    if (IsWhiteSpace(CurChar))
                    {
                        Back.Type = TokenType.EMPTYSPACE;
                        Back.Text = Back.Text.AppendChar(CurChar);
                    }
                    else if (CurChar == ',')
                    {
                        Back.Type = TokenType.COMMA;
                        Back.Text = Back.Text.AppendChar(CurChar);
                        return Back;
                    }
                    else
                    {
                        Back.Type = TokenType.WORD;
                        Back.Text = Back.Text.AppendChar(CurChar);
                    }
                }
                else
                {
                    switch (Back.Type)
                    {
                        case TokenType.EMPTYSPACE:
                            if (IsWhiteSpace(CurChar))
                            {
                                Back.Text = Back.Text.AppendChar(CurChar);
                            }
                            else
                            {
                                return Back;
                            }
                            break;
                        case TokenType.WORD:
                            if (IsWhiteSpace(CurChar) || CurChar == ',')
                            {
                                return Back;
                            }
                            else
                            {
                                Back.Text = Back.Text.AppendChar(CurChar);
                            }
                            break;
                    }
                }
            }

            return Back;
        }
        public static List<Token> GetTokens(this string Text)
        {
            List<Token> Back = new List<Token>();
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            Token Current = null;

            for (int index = 0; index < StringLength; index++)
            {
                char CurChar = Text[index];

                if (IsWhiteSpace(CurChar))
                {
                    if (Current != null)
                    {
                        if (Current.Type != TokenType.EMPTYSPACE)
                        {
                            AddToken(Back, Current);
                            Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                        }
                        else
                        {
                            Current.Text = Current.Text.AppendChar(CurChar);
                        }
                    }
                    else
                    {
                        Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                    }
                }
                else
                {
                    if (Current != null && Current.Type == TokenType.EMPTYSPACE)
                    {
                        AddToken(Back, Current);
                        Current = null;
                    }

                    if (CurChar == ',')
                    {
                        if (Current == null)
                        {
                            Current = new Token(TokenType.COMMA, ",");
                            AddToken(Back, Current);
                            Current = null;
                        }
                        else
                        {
                            AddToken(Back, Current);
                            Current = new Token(TokenType.COMMA, ",");
                            AddToken(Back, Current);
                            Current = null;
                        }
                    }
                    else
                    {
                        if (Current == null)
                            Current = new Token(TokenType.WORD, "");
                        Current.Text = Current.Text.AppendChar(CurChar);
                    }
                }
            }

            if (Current != null)
                AddToken(Back, Current);

            return Back;
        }
        public static bool IsReserved(this string Word)
        {
            return ReservedWords.Contains(Word);
        }
        #endregion

        #region Extensions for IEnumerable
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        #endregion

        #region Auxiliar functions/items
        #region Uppercased reserved words, to tokenize strings
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
        #endregion
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

            if (TokenList == null)
                TokenList = new List<Token>();

            if (IsWhiteSpace(Current.Text))
            {
                Current.Type = TokenType.EMPTYSPACE;
            }
            else if (Current.Text == ",")
            {
                Current.Type = TokenType.COMMA;
            }
            else if (ReservedWords.Contains(Current.Text.ToUpper()))
            {
                Current.Type = TokenType.RESERVED;
            }
            else if (Current.Text.StartsWith("@"))
            {
                Current.Type = TokenType.VARIABLE;
            }
            else if (Current.Text.StartsWith("#"))
            {
                Current.Type = TokenType.TEMPTABLE;
            }
            else
            {
                Current.Type = TokenType.WORD;
            }
            
            TokenList.Add(Current);
            
            //if (Current == null || Current.IsEmpty)
            //    return;

            //if (IsWhiteSpace(Current.Text))
            //{
            //    Current.Type = TokenType.EMPTYSPACE;
            //    Current.Text = " ";
            //}
            //else if (Current.Type != TokenType.COMMA)
            //{

            //    Current.Type = TokenType.WORD;
            //}

            //if (TokenList.Count == 0)
            //{
            //    TokenList.Add(Current);
            //}
            //else
            //{
            //    if (Current.Type != TokenType.EMPTYSPACE)
            //    {
            //        TokenList.Add(Current);
            //    }
            //    else
            //    {
            //        if (TokenList.Last().Type != TokenType.EMPTYSPACE)
            //            TokenList.Add(Current);
            //    }
            //}

        }
        #endregion
    }

}
