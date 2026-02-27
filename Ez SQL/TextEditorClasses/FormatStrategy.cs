using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Ez_SQL.Common_Code;

namespace Ez_SQL.TextEditorClasses
{
    /// <summary>Indicates whether a position in the SQL text is inside executable code, a comment, or a string literal.</summary>
    public enum CodeStartType { Code, Comment, String };

    /// <summary>
    /// A formatting strategy that extends <see cref="DefaultFormattingStrategy"/> with
    /// token-aware bracket matching for the SQL editor.
    /// Overrides <c>SearchBracketForward</c> and <c>SearchBracketBackward</c> to use the
    /// application's <see cref="TokenList"/> lexer, which correctly skips brackets that appear
    /// inside SQL string literals, line comments, or block comments.
    /// </summary>
    public class SqlBracketMatcher : DefaultFormattingStrategy
    {
        /// <summary>
        /// Searches forward from <paramref name="offset"/> for the matching closing bracket,
        /// skipping any brackets inside comments or string literals.
        /// </summary>
        /// <param name="document">The document to search.</param>
        /// <param name="offset">The character offset of the opening bracket.</param>
        /// <param name="openBracket">The opening bracket character.</param>
        /// <param name="closingBracket">The closing bracket character to find.</param>
        /// <returns>The offset of the matching closing bracket, or -1 if not found.</returns>
        public override int SearchBracketForward(IDocument document, int offset, char openBracket, char closingBracket)
        {
            int tokenIndex, bracketTrick = 1;
            Token CurrentToken;
            //get all the text
            string Script = document.GetText(0, document.TextLength);
            //tokenize text
            TokenList Tokens = Script.GetTokens();

            if (offset > 1)
            {//for some reason the offset received is from the word after the bracket, so to check if the bracket is in a comment or a string is necesary to take the offset - 1
                Token XXX = Tokens.GetTokenAtOffset(offset-1, out tokenIndex);
                if (XXX.Type == TokenType.LINECOMMENT || XXX.Type == TokenType.BLOCKCOMMENT || XXX.Type == TokenType.STRING)
                    return -1;
            }
            Token ttt = Tokens.GetTokenAtOffset(offset, out tokenIndex);
            if (tokenIndex >= 0)
            {//token found at offset
                for (int i = tokenIndex; i < Tokens.TokenCount; i++)
                {
                    CurrentToken = Tokens.GetToken(i);
                    if (CurrentToken.Type == TokenType.CLOSEBRACKET && CurrentToken.Text == closingBracket.ToString())
                    {
                        bracketTrick--;
                    }
                    else if (CurrentToken.Type == TokenType.OPENBRACKET && CurrentToken.Text == openBracket.ToString())
                    {
                        bracketTrick++;
                    }

                    if (bracketTrick == 0)
                    {
                        return Tokens.GetStartOf(CurrentToken);
                    }
                }
            }
            return -1;            
        }
        /// <summary>
        /// Searches backward from <paramref name="offset"/> for the matching opening bracket,
        /// skipping any brackets inside comments or string literals.
        /// </summary>
        /// <param name="document">The document to search.</param>
        /// <param name="offset">The character offset of the closing bracket.</param>
        /// <param name="openBracket">The opening bracket character to find.</param>
        /// <param name="closingBracket">The closing bracket character.</param>
        /// <returns>The offset of the matching opening bracket, or -1 if not found.</returns>
        public override int SearchBracketBackward(IDocument document, int offset, char openBracket, char closingBracket)
        {
            int tokenIndex, bracketTrick = -1;
            Token CurrentToken;
            //get all the text
            string Script = document.GetText(0, document.TextLength);
            //tokenize text
            TokenList Tokens = Script.GetTokens();
            if (offset > 1)
            {
                Token XXX = Tokens.GetTokenAtOffset(offset + 1, out tokenIndex);
                if (XXX.Type == TokenType.LINECOMMENT || XXX.Type == TokenType.BLOCKCOMMENT || XXX.Type == TokenType.STRING)
                    return -1;
            }
            Tokens.GetTokenAtOffset(offset, out tokenIndex);
            if (tokenIndex >= 0)
            {//token found at offset
                for (int i = tokenIndex; i >= 0; i--)
                {
                    CurrentToken = Tokens.GetToken(i);
                    if (CurrentToken.Type == TokenType.CLOSEBRACKET && CurrentToken.Text == closingBracket.ToString())
                    {
                        bracketTrick--;
                    }
                    else if (CurrentToken.Type == TokenType.OPENBRACKET && CurrentToken.Text == openBracket.ToString())
                    {
                        bracketTrick++;
                    }

                    if (bracketTrick == 0)
                    {
                        return Tokens.GetStartOf(CurrentToken);
                    }                    
                }
            }
            return -1;  
        }
    }
}
