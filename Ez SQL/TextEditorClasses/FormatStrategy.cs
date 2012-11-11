using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Ez_SQL.Extensions;

namespace Ez_SQL.TextEditorClasses
{
    public enum CodeStartType { Code, Comment, String };

    public class SqlBracketMatcher : DefaultFormattingStrategy
    {
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
