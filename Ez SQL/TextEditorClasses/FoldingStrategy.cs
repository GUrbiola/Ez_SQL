using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Ez_SQL.Extensions;


namespace Ez_SQL.TextEditorClasses
{
    /// <summary>
    /// The class to generate the foldings, it implements ICSharpCode.TextEditor.Document.IFoldingStrategy
    /// </summary>
    public class SqlFoldingStrategy : IFoldingStrategy
    {
        /// <summary>
        /// Generates the foldings for our document.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <param name="fileName">The filename of the document.</param>
        /// <param name="parseInformation">Extra parse information, not used in this sample.</param>
        /// <returns>A list of FoldMarkers.</returns>
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> list = new List<FoldMarker>();
            List<Point> Starters = new List<Point>();
            int linea, columna;
            List<string> StartFoldTokens = new List<string>();
            List<string> EndFoldTokens = new List<string>();
            string buffer, after_line;

            StartFoldTokens.Add("begin");
            EndFoldTokens.Add("end");
            StartFoldTokens.Add("--fold");
            EndFoldTokens.Add("--/fold");
            StartFoldTokens.Add("/*");
            EndFoldTokens.Add("*/");

            for (int i = 0; i < StartFoldTokens.Count; i++)
            {
                //search for the start token
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        Starters.Add(new Point(linea, columna));
                        columna += StartFoldTokens[i].Length;
                        columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    }
                }

                //luego buscar el cierre de begin, ie end
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        //ahora a partir de aqui tengo que buscar el starter mas grande que este mas
                        //atras de este ender que encontre
                        for (int rev = Starters.Count - 1; rev >= 0; rev--)
                        {
                            if (Starters[rev].X > linea || (Starters[rev].X == linea && Starters[rev].Y > columna))
                                continue;
                            else
                            {
                                after_line = document.GetText(document.GetLineSegment(Starters[rev].X)).Substring(Starters[rev].Y);
                                if (after_line.Length > StartFoldTokens[i].Length)
                                    after_line = after_line.Substring(StartFoldTokens[i].Length).Trim();
                                else
                                    after_line = "...";
                                list.Add(new FoldMarker(document, Starters[rev].X, Starters[rev].Y + StartFoldTokens[i].Length, linea, columna, FoldType.Region, after_line, false));
                                Starters.RemoveAt(rev);
                                break;
                            }
                        }
                        columna += EndFoldTokens[i].Length;
                        columna = buffer.IndexOf(EndFoldTokens[i], columna);
                        if (Starters.Count == 0)
                            break;
                    }
                }

                Starters.Clear();
            }

            return list;
        }
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation, string StartFoldToken, string EndFoldToken)
        {
            List<string> AuxIni = new List<string>();
            List<string> AuxFin = new List<string>();
            AuxIni.Add(StartFoldToken);
            AuxFin.Add(EndFoldToken);
            return GenerateFoldMarkers(document, fileName, parseInformation, AuxIni, AuxFin);
        }
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation, List<string> StartFoldTokens, List<string> EndFoldTokens)
        {
            List<FoldMarker> list = new List<FoldMarker>();
            List<Point> Starters = new List<Point>();
            int linea, columna, FoldingCouples;
            string buffer, after_line;

            if (StartFoldTokens == null || EndFoldTokens == null || StartFoldTokens.Count == 0 || EndFoldTokens.Count == 0)
                return list;
            FoldingCouples = Math.Min(StartFoldTokens.Count, EndFoldTokens.Count);
            for (int i = 0; i < FoldingCouples; i++)
            {
                //primero buscar inicio, el clave de inicio de codefolding, 
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        Starters.Add(new Point(linea, columna));
                        columna += StartFoldTokens[i].Length;
                        columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    }
                }

                //luego buscar el cierre del codefolding
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        //ahora a partir de aqui tengo que buscar el starter mas avanzado que este mas
                        //atras de este ender que encontre
                        for (int rev = Starters.Count - 1; rev >= 0; rev--)
                        {
                            if (Starters[rev].X > linea || (Starters[rev].X == linea && Starters[rev].Y > columna))
                                continue;
                            else
                            {
                                after_line = document.GetText(document.GetLineSegment(Starters[rev].X)).Substring(Starters[rev].Y);
                                if (after_line.Length > StartFoldTokens[i].Length)
                                    after_line = after_line.Substring(StartFoldTokens[i].Length).Trim();
                                else
                                    after_line = "...";
                                list.Add(new FoldMarker(document, Starters[rev].X, Starters[rev].Y + StartFoldTokens[i].Length, linea, columna, FoldType.Region, after_line, false));
                                Starters.RemoveAt(rev);
                                break;
                            }
                        }
                        columna += EndFoldTokens[i].Length;
                        columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    }
                }

                Starters.Clear();
            }

            return list;
        }
    }
    /// <summary>
    /// The class to generate the foldings, it implements ICSharpCode.TextEditor.Document.IFoldingStrategy
    /// </summary>
    public class CSharpFoldingStrategy : IFoldingStrategy
    {
        /// <summary>
        /// Generates the foldings for our document.
        /// </summary>
        /// <param name="document">The current document.</param>
        /// <param name="fileName">The filename of the document.</param>
        /// <param name="parseInformation">Extra parse information, not used in this sample.</param>
        /// <returns>A list of FoldMarkers.</returns>
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> list = new List<FoldMarker>();
            List<Point> Starters = new List<Point>();
            int linea, columna;
            List<string> StartFoldTokens = new List<string>();
            List<string> EndFoldTokens = new List<string>();
            string buffer, after_line;

            StartFoldTokens.Add("#region");
            EndFoldTokens.Add("#endregion");
            StartFoldTokens.Add("/*");
            EndFoldTokens.Add("*/");

            for (int i = 0; i < StartFoldTokens.Count; i++)
            {
                //primero buscar el folding de begin
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        Starters.Add(new Point(linea, columna));
                        columna += StartFoldTokens[i].Length;
                        columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    }
                }

                //luego buscar el cierre de begin, ie end
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        //ahora a partir de aqui tengo que buscar el starter mas grande que este mas
                        //atras de este ender que encontre
                        for (int rev = Starters.Count - 1; rev >= 0; rev--)
                        {
                            if (Starters[rev].X > linea || (Starters[rev].X == linea && Starters[rev].Y > columna))
                                continue;
                            else
                            {
                                after_line = document.GetText(document.GetLineSegment(Starters[rev].X)).Substring(Starters[rev].Y);
                                if (after_line.Length > StartFoldTokens[i].Length)
                                    after_line = after_line.Substring(StartFoldTokens[i].Length).Trim();
                                else
                                    after_line = "...";
                                list.Add(new FoldMarker(document, Starters[rev].X, Starters[rev].Y + StartFoldTokens[i].Length, linea, columna, FoldType.Region, after_line, false));
                                Starters.RemoveAt(rev);
                                break;
                            }
                        }
                        columna += EndFoldTokens[i].Length;
                        columna = buffer.IndexOf(EndFoldTokens[i], columna);
                        if (Starters.Count == 0)
                            break;
                    }
                }

                Starters.Clear();
            }

            return list;
        }
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation, string StartFoldToken, string EndFoldToken)
        {
            List<string> AuxIni = new List<string>();
            List<string> AuxFin = new List<string>();
            AuxIni.Add(StartFoldToken);
            AuxFin.Add(EndFoldToken);
            return GenerateFoldMarkers(document, fileName, parseInformation, AuxIni, AuxFin);
        }
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation, List<string> StartFoldTokens, List<string> EndFoldTokens)
        {
            List<FoldMarker> list = new List<FoldMarker>();
            List<Point> Starters = new List<Point>();
            int linea, columna, FoldingCouples;
            string buffer, after_line;

            if (StartFoldTokens == null || EndFoldTokens == null || StartFoldTokens.Count == 0 || EndFoldTokens.Count == 0)
                return list;
            FoldingCouples = Math.Min(StartFoldTokens.Count, EndFoldTokens.Count);
            for (int i = 0; i < FoldingCouples; i++)
            {
                //primero buscar inicio, el clave de inicio de codefolding, 
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        Starters.Add(new Point(linea, columna));
                        columna += StartFoldTokens[i].Length;
                        columna = buffer.IndexOf(StartFoldTokens[i], columna);
                    }
                }

                //luego buscar el cierre del codefolding
                for (linea = 0; linea < document.TotalNumberOfLines; linea++)
                {
                    buffer = document.GetText(document.GetLineSegment(linea)).ToLower();
                    columna = 0;
                    columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    while (columna >= 0)
                    {
                        //ahora a partir de aqui tengo que buscar el starter mas avanzado que este mas
                        //atras de este ender que encontre
                        for (int rev = Starters.Count - 1; rev >= 0; rev--)
                        {
                            if (Starters[rev].X > linea || (Starters[rev].X == linea && Starters[rev].Y > columna))
                                continue;
                            else
                            {
                                after_line = document.GetText(document.GetLineSegment(Starters[rev].X)).Substring(Starters[rev].Y);
                                if (after_line.Length > StartFoldTokens[i].Length)
                                    after_line = after_line.Substring(StartFoldTokens[i].Length).Trim();
                                else
                                    after_line = "...";
                                list.Add(new FoldMarker(document, Starters[rev].X, Starters[rev].Y + StartFoldTokens[i].Length, linea, columna, FoldType.Region, after_line, false));
                                Starters.RemoveAt(rev);
                                break;
                            }
                        }
                        columna += EndFoldTokens[i].Length;
                        columna = buffer.IndexOf(EndFoldTokens[i], columna);
                    }
                }

                Starters.Clear();
            }

            return list;
        }
    }

    public class SqlFolder : IFoldingStrategy
    {
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> Back = new List<FoldMarker>();
            //get all the text
            string Script = document.GetText(0, document.TextLength);
            //tokenize text
            TokenList Tokens = Script.GetTokens();

            #region Calculate folding for BEGIN <----> END
            List<Token> FoldStarter = Tokens.GetByType(TokenType.BLOCKSTART);
            List<Token> FoldEnder = Tokens.GetByType(TokenType.BLOCKEND);

            for (int i = 0; i < FoldEnder.Count; i++)
            {
                for (int j = FoldStarter.Count - 1; j >= 0; j--)
                {
                    if (Tokens.GetStartOf(FoldStarter[j]) < Tokens.GetStartOf(FoldEnder[i]))
                    {
                        Token Starter, Ender;
                        Ender = FoldEnder[i];
                        Starter = FoldStarter[j];
                        TextLocation Start, End, afterFoldHelper;

                        Start = document.OffsetToPosition(Tokens.GetStartOf(Starter) + "begin".Length);
                        afterFoldHelper = document.OffsetToPosition(Tokens.GetStartOf(Starter));
                        End = document.OffsetToPosition(Tokens.GetStartOf(Ender));

                        string afterStarter = document.GetText(document.GetLineSegment(afterFoldHelper.Line)).Substring(afterFoldHelper.Column).Trim(' ', '\t', '\n', '\r');
                        afterStarter = afterStarter.Length > 5 ? afterStarter.Substring(5) : "";
                        if (String.IsNullOrEmpty(afterStarter) || afterStarter.Trim().Length == 0)
                            afterStarter = "...";


                        Back.Add(new FoldMarker(document, Start.Line, Start.Column, End.Line, End.Column, FoldType.Region, afterStarter, false));

                        FoldStarter.RemoveAt(j);
                        break;
                    }
                }
            }
            #endregion

            #region Calculate folding for block comments
            List<Token> BlockCommentFolds = Tokens.GetByType(TokenType.BLOCKCOMMENT);

            for (int i = 0; i < BlockCommentFolds.Count; i++)
            {
                Token BlockComment = BlockCommentFolds[i];
                TextLocation Start, End;

                Start = document.OffsetToPosition(Tokens.GetStartOf(BlockComment) + 2);
                End = document.OffsetToPosition(Tokens.GetEndOf(BlockComment) - 1);

                Back.Add(new FoldMarker(document, Start.Line, Start.Column, End.Line, End.Column, FoldType.Region, "***", false));
            }
            #endregion

            #region Calculate folding for custom folders --fold <----> --/fold
            FoldStarter = Tokens.GetCustomFolders(true);
            FoldEnder = Tokens.GetCustomFolders(false);


            for (int i = 0; i < FoldEnder.Count; i++)
            {
                for (int j = FoldStarter.Count - 1; j >= 0; j--)
                {
                    if (Tokens.GetStartOf(FoldStarter[j]) < Tokens.GetStartOf(FoldEnder[i]))
                    {
                        Token Starter, Ender;
                        Ender = FoldEnder[i];
                        Starter = FoldStarter[j];
                        TextLocation Start, End, afterFoldHelper;

                        Start = document.OffsetToPosition(Tokens.GetStartOf(Starter) + "--fold".Length);
                        afterFoldHelper = document.OffsetToPosition(Tokens.GetStartOf(Starter));
                        End = document.OffsetToPosition(Tokens.GetStartOf(Ender));

                        string afterStarter = document.GetText(document.GetLineSegment(afterFoldHelper.Line)).Substring(afterFoldHelper.Column).Trim(' ', '\t', '\n', '\r');
                        afterStarter = afterStarter.Length > 6 ? afterStarter.Substring(6) : "";
                        if (String.IsNullOrEmpty(afterStarter) || afterStarter.Trim().Length == 0)
                            afterStarter = "...";

                        Back.Add(new FoldMarker(document, Start.Line, Start.Column, End.Line, End.Column, FoldType.Region, afterStarter, false));

                        FoldStarter.RemoveAt(j);
                        break;
                    }
                }
            }
            #endregion


            return Back;
        }
    }

}

