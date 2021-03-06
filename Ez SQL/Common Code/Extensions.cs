﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Ez_SQL.Common_Code
{
    public enum SortDirection { Ascending, Decending };
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
        public static void InsertString(this TextEditorControl TxtEditor, string InsStr, int Position = -1, bool DoRefreshAfter = true)
        {
            int SelectionLength = 0;
            if (String.IsNullOrEmpty(InsStr))
                return;


            if (Position == -1)
            {
                if (TxtEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                {
                    SelectionLength = TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length;
                    Position = TxtEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
                    TxtEditor.ActiveTextAreaControl.TextArea.Caret.Position = TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionCollection[0].StartPosition;
                    TxtEditor.ActiveTextAreaControl.TextArea.SelectionManager.RemoveSelectedText();
                }
                else
                {
                    Position = TxtEditor.CurrentOffset();
                }
            }
            TxtEditor.Document.Insert(Position, InsStr);
            TxtEditor.ActiveTextAreaControl.Caret.Column += InsStr.Length;

            if (DoRefreshAfter)
                TxtEditor.Refresh();
        }
        public static void SetSelectionByOffset(this TextEditorControl TxtEditor, int StartOffset, int EndOffset)
        {
            TxtEditor.ActiveTextAreaControl.SelectionManager.ClearSelection();
            TxtEditor.ActiveTextAreaControl.SelectionManager.SetSelection(TxtEditor.Document.OffsetToPosition(StartOffset), TxtEditor.Document.OffsetToPosition(EndOffset));
        }
        public static void SelectText(this TextEditorControl TxtEditor, int offset, int length)
        {
            TextLocation p1 = TxtEditor.Document.OffsetToPosition(offset);
            TextLocation p2 = TxtEditor.Document.OffsetToPosition(offset + length);
            TxtEditor.ActiveTextAreaControl.SelectionManager.SetSelection(p1, p2);
            TxtEditor.ActiveTextAreaControl.ScrollTo(p1.Line, p1.Column);
            // Also move the caret to the end of the selection, because when the user 
            // presses F3, the caret is where we start searching next time.
            TxtEditor.ActiveTextAreaControl.Caret.Position = TxtEditor.Document.OffsetToPosition(offset + length);
        }
        public static int CurrentLineNumber(this TextEditorControl TxtEditor)
        {
            return TxtEditor.Document.GetLineSegmentForOffset(TxtEditor.CurrentOffset()).LineNumber;
        }
        public static string GetLineText(this TextEditorControl TxtEditor, int LineNumber)
        {
            LineSegment line;
            ICSharpCode.TextEditor.TextLocation Start;
            string lineText = "";

            if (LineNumber <= TxtEditor.Document.TotalNumberOfLines && LineNumber >= 0)
            {
                line = TxtEditor.Document.GetLineSegment(LineNumber);
                Start = new ICSharpCode.TextEditor.TextLocation(0, line.LineNumber);
                lineText = TxtEditor.Document.GetText(line.Offset, line.Length);
            }

            return lineText;
        }
        public static void MarkLine(this TextEditorControl TxtEditor, int LineNumber, Color MarkColor, TextMarkerType MarkType = TextMarkerType.SolidBlock)
        {
            LineSegment line = TxtEditor.Document.GetLineSegment(LineNumber);
            TextMarker marker = new TextMarker(line.Offset, line.Length, MarkType, MarkColor);
            TxtEditor.Document.MarkerStrategy.AddMarker(marker);
        }
        public static void SetMarker(this TextEditorControl TxtEditor, int Offset, int Length, Color MarkColor, TextMarkerType MarkType = TextMarkerType.SolidBlock)
        {
            TextMarker marker = new TextMarker(Offset, Length, MarkType, MarkColor);
            TxtEditor.Document.MarkerStrategy.AddMarker(marker);
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
            return Text.GetTokens().List.LastOrDefault() ?? new Token(TokenType.EMPTYSPACE, "");
        }
        public static Token GetFirstToken(this string Text)
        {
            TokenList Back = new TokenList();
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            Token Current = null;

            for (int index = 0; index < StringLength; index++)
            {
                if (Back.TokenCount > 1)
                    break;

                char CurChar = Text[index];
                char[] wordTokenBreaker;
                if (Current != null)
                {
                    switch (Current.Type)
                    {
                        ////can not be a comma, because a comma is a 1 char token
                        //case TokenType.COMMA:
                        //    break;
                        ////can not be aa oppenbracket, because a oppenbracket is a 1 char token
                        //case TokenType.OPENBRACKET:
                        //    break;
                        ////can not be a closebracket, because a closebracket is a 1 char token
                        //case TokenType.CLOSEBRACKET:
                        //    break;
                        //can not be any of the next either, because this is decided when the token is added to the list
                        //case TokenType.RESERVED:
                        //case TokenType.VARIABLE:
                        //case TokenType.TEMPTABLE:
                        //    break;
                        case TokenType.EMPTYSPACE:
                            #region Code to process an empty space token + something
                            if (CurChar.IsWhiteSpace())
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            else
                            {
                                Back.AddToken(Current);
                                Current = null;
                                if (CurChar.IsComma())
                                {
                                    Back.AddToken(new Token(TokenType.COMMA, ","));
                                }
                                else if (CurChar.IsStringOperator())
                                {
                                    Current = new Token(TokenType.STRING, "'");
                                }
                                else if (CurChar.IsOpenBracket())
                                {
                                    Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsCloseBracket())
                                {
                                    Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsOperator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    //must check if this chars means the start of comment
                                    if (CurChar == '-' && nextc == '-')
                                    {
                                        Current = new Token(TokenType.LINECOMMENT, "--");
                                        index++;
                                    }
                                    else if (CurChar == '/' && nextc == '*')
                                    {
                                        Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                                    }
                                }
                                else if (CurChar.IsComparator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    if (nextc.IsComparator())
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                                    }
                                }
                                else
                                {
                                    Current = new Token(TokenType.WORD, CurChar.ToString());
                                }
                            }
                            #endregion
                            break;
                        case TokenType.WORD:
                            wordTokenBreaker = new char[] { ' ', '\t', '\r', '\n', '\'', ',', '(', '[', '{', '}', ']', ')', '-', '*', '+', '/', '>', '<', '=' };
                            #region Code to process a word token + something
                            if (wordTokenBreaker.Contains(CurChar))
                            {
                                Back.AddToken(Current);
                                Current = null;
                                if (CurChar.IsWhiteSpace())
                                {
                                    Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                                }
                                else if (CurChar.IsStringOperator())
                                {
                                    Current = new Token(TokenType.STRING, "'");
                                }
                                else if (CurChar.IsComma())
                                {
                                    Current = new Token(TokenType.STRING, ",");
                                }
                                else if (CurChar.IsOpenBracket())
                                {
                                    Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsCloseBracket())
                                {
                                    Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsOperator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    //must check if this chars means the start of comment
                                    if (CurChar == '-' && nextc == '-')
                                    {
                                        Current = new Token(TokenType.LINECOMMENT, "--");
                                        index++;
                                    }
                                    else if (CurChar == '/' && nextc == '*')
                                    {
                                        Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                                    }
                                }
                                else if (CurChar.IsComparator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    if (nextc.IsComparator())
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                                    }
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.LINECOMMENT:
                            #region Code to process a char inside a linecomment, a token breaker are "\n" and "\r\n"
                            if (CurChar == '\r')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '\n')
                                {
                                    Current.Text += "\r\n";
                                    Back.AddToken(Current);
                                    Current = null;
                                }
                                else
                                {
                                    Current.Text += "\r";
                                }
                            }
                            else if (CurChar == '\n')
                            {
                                Current.Text += "\n";
                                Back.AddToken(Current);
                                Current = null;
                            }
                            else
                            {
                                Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.BLOCKCOMMENT:
                            #region Code to process a char inside a block comment, the only token breaker is "*/"
                            if (CurChar == '*')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '/')
                                {
                                    Current.Text += "*/";
                                    Back.AddToken(Current);
                                    Current = null;
                                    index++;
                                }
                                else
                                {
                                    Current.Text = Current.Text.AppendChar(CurChar);
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.STRING:
                            #region Code to process a char inside a string, only token breaker is "'", but not "''"
                            if (CurChar == '\'')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '\'')
                                {//double '', so is an escaped ', it is a string still then
                                    Current.Text += "''";
                                    index++;
                                }
                                else
                                {
                                    Current.Text += "'";
                                    Back.AddToken(Current);
                                    Current = null;
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;

                    }
                }
                else if (Current == null)
                {
                    #region no previous token, must check if create a new one, or let the current stay null and add a new instance of token
                    if (IsWhiteSpace(CurChar))
                    {
                        Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                    }
                    else if (CurChar.IsComma())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.COMMA, CurChar.ToString()));
                    }
                    else if (CurChar.IsStringOperator())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.STRING, CurChar.ToString()));
                    }
                    else if (CurChar.IsOpenBracket())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                    }
                    else if (CurChar.IsCloseBracket())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                    }
                    else if (CurChar.IsOperator())
                    {
                        char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                        //must check if this chars means the start of comment
                        if (CurChar == '-' && nextc == '-')
                        {
                            Current = new Token(TokenType.LINECOMMENT, "--");
                            index++;
                        }
                        else if (CurChar == '/' && nextc == '*')
                        {
                            Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                            index++;
                        }
                        else
                        {
                            Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                        }
                    }
                    else if (CurChar.IsComparator())
                    {
                        char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                        if (nextc.IsComparator())
                        {
                            Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                            index++;
                        }
                        else
                        {
                            Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                        }
                    }
                    else
                    {
                        Current = new Token(TokenType.WORD, CurChar.ToString());
                    }
                    #endregion
                }
            }
            if (Current != null)
                Back.AddToken(Current);

            return Back.TokenCount > 0 ? Back[0]:new Token(TokenType.EMPTYSPACE, "");
        }
        public static TokenList GetTokens(this string Text)
        {
            TokenList Back = new TokenList();
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            Token Current = null;

            for (int index = 0; index < StringLength; index++)
            {
                char CurChar = Text[index];
                char[] wordTokenBreaker;
                if (Current != null)
                {
                    switch (Current.Type)
                    {
                        ////can not be a comma, because a comma is a 1 char token
                        //case TokenType.COMMA:
                        //    break;
                        ////can not be aa oppenbracket, because a oppenbracket is a 1 char token
                        //case TokenType.OPENBRACKET:
                        //    break;
                        ////can not be a closebracket, because a closebracket is a 1 char token
                        //case TokenType.CLOSEBRACKET:
                        //    break;
                        //can not be any of the next either, because this is decided when the token is added to the list
                        //case TokenType.RESERVED:
                        //case TokenType.VARIABLE:
                        //case TokenType.TEMPTABLE:
                        //    break;
                        case TokenType.EMPTYSPACE:
                            #region Code to process an empty space token + something
                            if (CurChar.IsWhiteSpace())
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            else
                            {
                                Back.AddToken(Current);
                                Current = null;
                                if (CurChar.IsComma())
                                {
                                    Back.AddToken(new Token(TokenType.COMMA, ","));
                                }
                                else if (CurChar.IsStringOperator())
                                {
                                    Current = new Token(TokenType.STRING, "'");
                                }
                                else if (CurChar.IsOpenBracket())
                                {
                                    Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsCloseBracket())
                                {
                                    Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsOperator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    //must check if this chars means the start of comment
                                    if (CurChar == '-' && nextc == '-')
                                    {
                                        Current = new Token(TokenType.LINECOMMENT, "--");
                                        index++;
                                    }
                                    else if (CurChar == '/' && nextc == '*')
                                    {
                                        Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                                    }
                                }
                                else if (CurChar.IsComparator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    if (nextc.IsComparator())
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                                    }
                                }
                                else
                                {
                                    Current = new Token(TokenType.WORD, CurChar.ToString());
                                }
                            }
                            #endregion
                            break;
                        case TokenType.WORD:
                            wordTokenBreaker = new char[] { ' ', '\t', '\r', '\n', '\'', ',', '(', '[', '{', '}', ']', ')', '-', '*', '+', '/', '>', '<', '=', ';' };
                            #region Code to process a word token + something
                            if (wordTokenBreaker.Contains(CurChar))
                            {
                                Back.AddToken(Current);
                                Current = null;
                                if (CurChar.IsWhiteSpace())
                                {
                                    Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                                }
                                else if (CurChar.IsStringOperator())
                                {
                                    Current = new Token(TokenType.STRING, "'");
                                }
                                else if (CurChar.IsComma())
                                {
                                    Back.AddToken(new Token(TokenType.COMMA, ","));
                                }
                                else if (CurChar.IsSemmiColon())
                                {
                                    Back.AddToken(new Token(TokenType.SEMMICOLON, ";"));
                                }
                                else if (CurChar.IsOpenBracket())
                                {
                                    Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsCloseBracket())
                                {
                                    Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                                }
                                else if (CurChar.IsOperator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    //must check if this chars means the start of comment
                                    if (CurChar == '-' && nextc == '-')
                                    {
                                        Current = new Token(TokenType.LINECOMMENT, "--");
                                        index++;
                                    }
                                    else if (CurChar == '/' && nextc == '*')
                                    {
                                        Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                                    }
                                }
                                else if (CurChar.IsComparator())
                                {
                                    char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                    if (nextc.IsComparator())
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                                        index++;
                                    }
                                    else
                                    {
                                        Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                                    }
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.LINECOMMENT:
                            #region Code to process a char inside a linecomment, a token breaker are "\n" and "\r\n"
                            if (CurChar == '\r')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '\n')
                                {
                                    Current.Text += "\r\n";
                                    Back.AddToken(Current);
                                    index++;
                                    Current = null;
                                }
                                else
                                {
                                    Current.Text += "\r";
                                }
                            }
                            else if (CurChar == '\n')
                            {
                                Current.Text += "\n";
                                Back.AddToken(Current);
                                Current = null;
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.BLOCKCOMMENT:
                            #region Code to process a char inside a block comment, the only token breaker is "*/"
                            if (CurChar == '*')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '/')
                                {
                                    Current.Text += "*/";
                                    Back.AddToken(Current);
                                    Current = null;
                                    index++;
                                }
                                else
                                {
                                    Current.Text = Current.Text.AppendChar(CurChar);
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;
                        case TokenType.STRING:
                            #region Code to process a char inside a string, only token breaker is "'", but not "''"
                            if (CurChar == '\'')
                            {
                                char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                                if (nextc == '\'')
                                {//double '', so is an escaped ', it is a string still then
                                    Current.Text += "''";
                                    index++;
                                }
                                else
                                {
                                    Current.Text += "'";
                                    Back.AddToken(Current);
                                    Current = null;
                                }
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            #endregion
                            break;

                    }
                }
                else if (Current == null)
                {
                    #region no previous token, must check if create a new one, or let the current stay null and add a new instance of token
                    if (IsWhiteSpace(CurChar))
                    {
                        Current = new Token(TokenType.EMPTYSPACE, CurChar.ToString());
                    }
                    else if (CurChar.IsComma())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.COMMA, CurChar.ToString()));
                    }
                    else if (CurChar.IsStringOperator())
                    {
                        Current = new Token(TokenType.STRING, CurChar.ToString());
                    }
                    else if (CurChar.IsOpenBracket())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.OPENBRACKET, CurChar.ToString()));
                    }
                    else if (CurChar.IsCloseBracket())
                    {//1 char token, so Current stays null
                        Back.AddToken(new Token(TokenType.CLOSEBRACKET, CurChar.ToString()));
                    }
                    else if (CurChar.IsOperator())
                    {
                        char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                        //must check if this chars means the start of comment
                        if (CurChar == '-' && nextc == '-')
                        {
                            Current = new Token(TokenType.LINECOMMENT, "--");
                            index++;
                        }
                        else if (CurChar == '/' && nextc == '*')
                        {
                            Current = new Token(TokenType.BLOCKCOMMENT, "/*");
                            index++;
                        }
                        else
                        {
                            Back.AddToken(new Token(TokenType.OPERATOR, CurChar.ToString()));
                        }
                    }
                    else if (CurChar.IsComparator())
                    {
                        char nextc = Text.Length > index + 1 ? Text[index + 1] : ' ';
                        if (nextc.IsComparator())
                        {
                            Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString() + nextc.ToString()));
                            index++;
                        }
                        else
                        {
                            Back.AddToken(new Token(TokenType.COMPARATOR, CurChar.ToString()));
                        }
                    }
                    else
                    {
                        Current = new Token(TokenType.WORD, CurChar.ToString());
                    }
                    #endregion
                }
            }
            if (Current != null)
                Back.AddToken(Current);
            
            List<int> ToReplace = new List<int>();
            for (int i = 0; i < Back.TokenCount - 2; i++)
            {
                if (
                    Back[i].Type == TokenType.BLOCKSTART &&
                    Back[i + 1].Type == TokenType.EMPTYSPACE &&
                    Back[i + 2].Type == TokenType.RESERVED &&
                    (Back[i + 2].Text.Equals("tran", StringComparison.CurrentCultureIgnoreCase) ||
                     Back[i + 2].Text.Equals("transaction", StringComparison.CurrentCultureIgnoreCase)))
                {
                    ToReplace.Add(i);
                }
            }

            if (ToReplace.Count > 0)
            {
                int found = 0;
                foreach (int i in ToReplace)
                {
                    int buffIndex = i - (found * 2);

                    Token buff = new Token(TokenType.BEGINTRANSACTION, String.Format("{0}{1}{2}", Back[buffIndex].Text, Back[buffIndex + 1].Text, Back[buffIndex + 2].Text));
                    Back.RemoveTokenAt(buffIndex);
                    Back.RemoveTokenAt(buffIndex);
                    Back.RemoveTokenAt(buffIndex);
                    Back.AddTokenAt(buffIndex, buff);

                    found++;
                }
            }

            return Back;
        }
        public static string GetUpperCasedLetters(this string Word, int MaxLength = -1)
        {
            string back;
            back = String.Concat(Word.Select(X => char.IsUpper(X) ? X.ToString() : ""));
            if (MaxLength == -1)
                return back;
            return back.Substring(0, Math.Min(MaxLength, back.Length));
        }
        public static string GetLowerCasedLetters(this string Word, int MaxLength = -1)
        {
            string back;
            back = String.Concat(Word.Select(X => char.IsLower(X) ? X.ToString() : ""));
            if (MaxLength == -1)
                return back;
            return back.Substring(0, Math.Min(MaxLength, back.Length));
        }
        public static string GetAsSentence(this string Word)
        {
            if (String.IsNullOrEmpty(Word))
                return Word;
            return Word.Substring(0, 1).ToUpper() + (Word.Length > 1 ? Word.Substring(1).ToLower() : "");
        }
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
        public static string Indent(this string source, int indent = 1, int useSpaces = 0)
        {
            string back = String.IsNullOrEmpty(source) ? "" : source;
            string spaces = "";

            if (useSpaces > 0)
            {
                for (int i = 0; i < useSpaces; i++)
                {
                    spaces += " ";
                }
            }
            else
            {
                spaces = "\t";
            }

            for (int i = 0; i < indent; i++)
            {
                back = spaces + back;
            }
            return back;
        }
        public static string AsValidXML(this string source)
        {
            string back = source;
            back = back.Replace("<", "&lt;");
            back = back.Replace("&", "&amp;");
            back = back.Replace(">", "&gt;");
            back = back.Replace("\"", "&quot;");
            back = back.Replace("'", "&apos;");
            return back;
        }
        public static string FirstLetterLowerCase(this string source)
        {
            return source.Substring(0, 1).ToLower() + source.Substring(1);
        }
        public static bool IsEmpty(this string str)
        {
            return String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
        }
        public static string EnsureCSVField(this string str)
        {
            if (str.Contains("\""))
                str = "\"" + str.Replace("\"", "\"\"") + "\"";

            if (str.Contains(","))
                str = str.EnsureQuotedString();

            return str;
        }
        public static string EnsureQuotedString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return "\"\"";
            if (!str.StartsWith("\""))
                str = "\"" + str;
            if (!str.EndsWith("\""))
                str = str + "\"";
            return str;
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
        public static IEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> sorter, SortDirection direction = SortDirection.Ascending)
        {
            if (direction == SortDirection.Decending)
                source.OrderByDescending(sorter);
            return source.OrderBy(sorter);
        }
        #endregion

        #region Miscellaneous extensions
        public static int InRange(this int x, int lo, int hi)
        {
            Debug.Assert(lo <= hi);
            return x < lo ? lo : (x > hi ? hi : x);
        }
        public static bool IsInRange(this int x, int lo, int hi)
        {
            return x >= lo && x <= hi;
        }
        public static Color HalfMix(this Color one, Color two)
        {
            return Color.FromArgb(
                (one.A + two.A) >> 1,
                (one.R + two.R) >> 1,
                (one.G + two.G) >> 1,
                (one.B + two.B) >> 1);
        }
        public static Color StringToColor(this string HtmlColor)
        {
            return ColorTranslator.FromHtml(HtmlColor);
        }
        public static string ColorToString(this Color HtmlColor)
        {
            Color Aux = Color.FromArgb(HtmlColor.ToArgb());
            return ColorTranslator.ToHtml(Aux);
       }
        public static string BoolAsString(this bool boolean)
        {
            return boolean ? "true" : "false";
        }
        public static bool IsInitialPoint(this Point p)
        {
            return p.X == 0 && p.Y == 0;
        }
        public static void SetToInitialPoint(this Point p)
        {
            p.X = 0;
            p.Y = 0;
        }
        #endregion

        #region Auxiliar functions/items
        #region Uppercased reserved words and Sql DataTypes, for tokenization of strings
        public static HashSet<string> ReservedWords = new HashSet<string>{"@@FETCH_STATUS", "@@IDENTITY","ADD","ALL","ALTER","AND","ANY ","AS","ASC","AUTHORIZATION","AVG","BACKUP",
                                                      "BETWEEN","BREAK","BROWSE","BULK","BY","CASCADE","CASE","CATCH", "CHECK","CHECKPOINT","CLOSE",
                                                      "CLUSTERED","COALESCE","COLLATE","COLUMN","COMMIT","COMPUTE","CONSTRAINT","CONTAINS","CONTAINSTABLE",
                                                      "CONTINUE","CONVERT","COUNT","CREATE","CROSS","CURRENT","CURRENT_DATE","CURRENT_TIME","CURRENT_TIMESTAMP",
                                                      "CURRENT_USER","CURSOR","DATABASE","DATABASEPASSWORD","DATEADD","DATEDIFF","DATENAME","DATEPART",
                                                      "DBCC","DEALLOCATE","DECLARE","DEFAULT","DELAY","DELETE","DENY","DESC","DISK","DISTINCT","DISTRIBUTED",
                                                      "DOUBLE","DROP","DUMP","ELSE","ENCRYPTION","ERRLVL","ESCAPE","EXCEPT","EXEC","EXECUTE",
                                                      "EXISTS","EXIT","EXPRESSION","FETCH","FILE","FILLFACTOR","FOR","FOREIGN","FREETEXT","FREETEXTTABLE",
                                                      "FROM","FULL","FUNCTION","GOTO","GRANT","GROUP","HAVING","HOLDLOCK","IDENTITY","IDENTITY_INSERT",
                                                      "IDENTITYCOL","IF","IN","INDEX","INNER","INSERT","INTERSECT","INTO","IS","JOIN","KEY",
                                                      "KILL","LEFT","LIKE","LINENO","LOAD","MAX","MIN","NATIONAL","NOCHECK","NONCLUSTERED",
                                                      "NOT","NULL","NULLIF","OF","OFF","OFFSETS","ON","OPEN","OPENDATASOURCE","OPENQUERY",
                                                      "OPENROWSET","OPENXML","OPTION","OR","ORDER","OUTER","OVER","PERCENT","PLAN","PRECISION",
                                                      "PRIMARY","PRINT","PROC","PROCEDURE","PUBLIC","RAISERROR","READ","READTEXT","RECONFIGURE","REFERENCES",
                                                      "REPLICATION","RESTORE","RESTRICT","RETURN","REVOKE","RIGHT","ROLLBACK","ROWCOUNT","ROWGUIDCOL",
                                                      "RULE","SAVE","SCHEMA","SELECT","SESSION_USER","SET","SETUSER","SHUTDOWN","SOME","STATISTICS",
                                                      "SUM","SYSTEM_USER","TABLE","TEXTSIZE","THEN","TO","TOP","TRAN","TRANSACTION","TRIGGER","TRUNCATE", "TRY",
                                                      "TSEQUAL","UNION","UNIQUE","UPDATE","UPDATETEXT","USE","USER","VALUES","VARYING","VIEW","WAITFOR",
                                                      "WHEN","WHERE","WHILE","WITH","WRITETEXT"};
        public static HashSet<string> DataTypes = new HashSet<string>{
                                                      "BIGINT", "NUMERIC","BIT","INT","SMALLINT","TINYINT","SMALLMONEY","MONEY","DECIMAL", //exact numeric type
                                                      "FLOAT","REAL",//aproximate numeric type
                                                      "DATE","DATETIME","DATETIME2","DATETIMEOFFSET","TIME","SMALLDATETIME",//date type
                                                      "CHAR","VARCHAR","TEXT",//char type
                                                      "NCHAR","NVARCHAR","NTEXT",//unicode char types
                                                      "BINARY","VARBINARY","IMAGE",//binary types
                                                      "XML","CURSOR","TIMESTAMP","UNIQUEIDENTIFIER","HIERARCHYID","SQL_VARIANT","TABLE"//other data types
                                                      };
        #endregion
        private static bool IsWhiteSpace(this char c)
        {
            return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
        }
        private static bool IsComma(this char c)
        {
            return c == ',';
        }
        private static bool IsSemmiColon(this char c)
        {
            return c == ';';
        }
        private static bool IsOpenBracket(this char c)
        {
            return (c == '(' || c == '[' || c == '{');
        }
        private static bool IsCloseBracket(this char c)
        {
            return (c == ')' || c == ']' || c == '}');
        }
        private static bool IsOperator(this char c)
        {
            return (c == '+' || c == '/' || c == '-' || c == '*');
        }
        private static bool IsComparator(this char c)
        {
            return (c == '>' || c == '<' || c == '=');
        }
        private static bool IsStringOperator(this char c)
        {
            return c == '\'';
        }
        public static bool IsReserved(this string Word)
        {
            return ReservedWords.Contains(Word.ToUpper());
        }
        public static bool IsDataType(this string Word)
        {
            return DataTypes.Contains(Word.ToUpper());
        }
        #endregion

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
        public static void SaveToCsv(this DataTable dt, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            string headerLine = "";
            foreach (string cn in columnNames)
                headerLine += headerLine.IsEmpty() ? cn.EnsureCSVField() : "," + cn.EnsureCSVField();
            sb.AppendLine(headerLine);

            foreach (DataRow row in dt.Rows)
            {
                string line = "";
                int index = 0;
                foreach (string cell in row.ItemArray.Select(x => x.ToString()))
                {
                    if (index == 0)
                        line += cell.EnsureCSVField();
                    else
                        line += "," + cell.EnsureCSVField();
                    index++;
                }
                sb.AppendLine(line);
            }

            File.WriteAllText(fileName, sb.ToString());
        }
        public static void SaveToXlsx(this DataTable dt, string fileName, bool withStyles = true)
        {
            DataExporter dex = new DataExporter();
            dex.ExportExcelStyle = ExcelStyle.Simple;
            dex.ExportType = ExportTo.XLSX;
            dex.ExportWithStyles = withStyles;
            dex.UseAlternateRowStyles = withStyles;
            dex.WriteHeaders = true;
            dex.UseDefaultSheetNames = true;

            dex.ExportToFile(fileName, dt);
        }
        public static DataTable ConvertToDataTable<T>(this IList<T> list, string tableName = "")
        {
            DataTable table = CreateTable<T>(tableName);
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        public static DataTable ConvertToDataTable<T>(this ICollection<T> list, string tableName = "")
        {
            DataTable table = CreateTable<T>(tableName);
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        public static DataTable CreateTable<T>(string tableName = "")
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(String.IsNullOrEmpty(tableName) ? entityType.Name : tableName);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }

        public static void SerializeToXmlFile(this object obj, string fileName, string root = "Data", int ver = 1)
        {
            XmlDocument xDoc = XmlObjectSerializer.Serialize(obj, ver, root);
            xDoc.Save(fileName);
        }
        public static string SerializeToXmlString(this object obj, string root = "Data", int ver = 1)
        {
            TextWriter tw = new StringWriter();
            XmlDocument xDoc = XmlObjectSerializer.Serialize(obj, ver, root);
            xDoc.Save(tw);
            return tw.ToString();
        }
        public static object DeserializeFromXmlFile(this string fileName, int ver = 1, XmlObjectDeserializer.ITypeConverter typeConverter = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return XmlObjectDeserializer.Deserialize(doc.OuterXml, ver, typeConverter);
        }
        public static object DeserializeFromXmlString(this string xmlString, int ver = 1, XmlObjectDeserializer.ITypeConverter typeConverter = null)
        {
            return XmlObjectDeserializer.Deserialize(xmlString, ver, typeConverter);
        }
        public static bool IsStringOnList(this List<string> list, string str, bool caseSensitive)
        {
            if (caseSensitive)
            {
                return list.Contains(str);
            }
            else
            {
                return list.FindIndex(x => x.Equals(str, StringComparison.OrdinalIgnoreCase)) != -1;
            }
        }

    }

}
