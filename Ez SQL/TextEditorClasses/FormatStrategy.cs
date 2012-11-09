using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace Ez_SQL.TextEditorClasses
{
    public enum CodeStartType { Code, Comment, String };

    public class SqlFormattingStrategy : DefaultFormattingStrategy
    {
        #region SearchBracket helper functions
        static int ScanLineStart(IDocument document, int offset)
        {
            for (int i = offset - 1; i > 0; --i)
            {
                if (document.GetCharAt(i) == '\n')
                    return i + 1;
            }
            return 0;
        }

        /// <summary>
        /// Gets the type of code at offset.<br/>
        /// 0 = Code,<br/>
        /// 1 = Comment,<br/>
        /// 2 = String<br/>
        /// Block comments and multiline strings are not supported.
        /// </summary>
        static CodeStartType GetStartType(IDocument document, int linestart, int offset)
        {
            bool inString = false;
            bool verbatim = false;
            for (int i = linestart; i < offset; i++)
            {
                switch (document.GetCharAt(i))
                {
                    case '-':
                        if (!inString && i + 1 < document.TextLength)
                        {
                            if (document.GetCharAt(i + 1) == '-')
                            {
                                return CodeStartType.Comment;
                            }
                        }
                        break;
                    case '\'':
                        if (inString && verbatim)
                        {
                            if (i + 1 < document.TextLength && document.GetCharAt(i + 1) == '\'')
                            {
                                ++i; // skip escaped quote
                                inString = false; // let the string go on
                            }
                            else
                            {
                                verbatim = false;
                            }
                        }
                        else if (!inString && i > 0 && document.GetCharAt(i - 1) == 'N')
                        {
                            verbatim = true;
                        }
                        inString = !inString;
                        break;
                }
            }
            return (inString || inChar) ? CodeStartType.String : CodeStartType.Code;
        }
        #endregion
        
        public override int SearchBracketForward(IDocument document, int offset, char openBracket, char closingBracket)
        {
            bool inString = false;
            bool inChar = false;
            bool verbatim = false;

            bool lineComment = false;
            bool blockComment = false;

            if (offset < 0) return -1;

            // first try "quick find" - find the matching bracket if there is no string/comment in the way
            int quickResult = base.SearchBracketForward(document, offset, openBracket, closingBracket);
            if (quickResult >= 0) return quickResult;

            // we need to parse the line from the beginning, so get the line start position
            int linestart = ScanLineStart(document, offset);

            // we need to know where offset is - in a string/comment or in normal code?
            // ignore cases where offset is in a block comment
            int starttype = GetStartType(document, linestart, offset);
            if (starttype != 0) return -1; // start position is in a comment/string

            int brackets = 1;

            while (offset < document.TextLength)
            {
                char ch = document.GetCharAt(offset);
                switch (ch)
                {
                    case '\r':
                    case '\n':
                        lineComment = false;
                        inChar = false;
                        if (!verbatim) inString = false;
                        break;
                    case '/':
                        if (blockComment)
                        {
                            Debug.Assert(offset > 0);
                            if (document.GetCharAt(offset - 1) == '*')
                            {
                                blockComment = false;
                            }
                        }
                        if (!inString && !inChar && offset + 1 < document.TextLength)
                        {
                            if (!blockComment && document.GetCharAt(offset + 1) == '/')
                            {
                                lineComment = true;
                            }
                            if (!lineComment && document.GetCharAt(offset + 1) == '*')
                            {
                                blockComment = true;
                            }
                        }
                        break;
                    case '"':
                        if (!(inChar || lineComment || blockComment))
                        {
                            if (inString && verbatim)
                            {
                                if (offset + 1 < document.TextLength && document.GetCharAt(offset + 1) == '"')
                                {
                                    ++offset; // skip escaped quote
                                    inString = false; // let the string go
                                }
                                else
                                {
                                    verbatim = false;
                                }
                            }
                            else if (!inString && offset > 0 && document.GetCharAt(offset - 1) == '@')
                            {
                                verbatim = true;
                            }
                            inString = !inString;
                        }
                        break;
                    case '\'':
                        if (!(inString || lineComment || blockComment))
                        {
                            inChar = !inChar;
                        }
                        break;
                    case '\\':
                        if ((inString && !verbatim) || inChar)
                            ++offset; // skip next character
                        break;
                    default:
                        if (ch == openBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                ++brackets;
                            }
                        }
                        else if (ch == closingBracket)
                        {
                            if (!(inString || inChar || lineComment || blockComment))
                            {
                                --brackets;
                                if (brackets == 0)
                                {
                                    return offset;
                                }
                            }
                        }
                        break;
                }
                ++offset;
            }
            return -1;
            
        }
        public override int SearchBracketBackward(IDocument document, int offset, char openBracket, char closingBracket)
        {
            return base.SearchBracketBackward(document, offset, openBracket, closingBracket);
        }
    }
}
