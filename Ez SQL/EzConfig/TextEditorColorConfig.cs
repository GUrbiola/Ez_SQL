using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Ez_SQL.Common_Code;

namespace Ez_SQL.EzConfig
{
    public class TextEditorColorConfig
    {
        public Environment Environment { get; set; }
        public Digits Digits { get; set; }
        public List<RuleSet> RuleSets { get; set; }

        public TextEditorColorConfig(string filePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    DeserializeFromFile(filePath);
                }
                else
                {
                    DefaultInitialization();
                }
            }
            catch (Exception)
            {
                DefaultInitialization();
            }
        }

        private void DefaultInitialization()
        {
            Environment = new Environment
                {
                    BackgroundColor = "#FFFFFF".StringToColor(),
                    FontColor = "#00003E".StringToColor(),
                    LineNumberBackgroundColor = "#FFFFFF".StringToColor(),
                    LineNumberFontColor = "#008080".StringToColor(),
                    SelectionColor = "#ADD8E6".StringToColor()
                };

            Digits = new Digits {Bold = true, Italic = false, Color = "#FF8000".StringToColor()};
            RuleSet Rl1 = new RuleSet();
            Rl1.Rules = new List<ConfigRule>();
            Rl1.IsMainRuleSet = true;

            #region First ruleset, most of the highlighting rules in here

            //Name of the rule set
            Rl1.Name = "MainRuleSet";
            //Case insensitive 
            Rl1.IgnoreCase = true;

            //Delimiters
            Rl1.Delimiters = new Delimiter()
                {
                    DelimiterChars = @"=!&gt;&lt;+-/*%&amp;|^~.}{,;][?:()"
                };

            //1 Line Comments
            ConfigRule LineComment = new ConfigRule()
                {
                    Type = "Span",
                    Name = "LineComment",
                    Rule = "SpecialComments",
                    Bold = false,
                    Italic = true,
                    Color = "#006400".StringToColor(),
                    StopAtEOL = true
                };
            LineComment.SpecialSymbols = new Dictionary<string, string>();
            LineComment.SpecialSymbols.Add("Begin", "--");
            Rl1.Rules.Add(LineComment);

            //Block Comments
            ConfigRule BlockComment = new ConfigRule()
                {
                    Type = "Span",
                    Name = "BlockComment",
                    Bold = false,
                    Italic = true,
                    Color = "#006400".StringToColor(),
                    StopAtEOL = false
                };
            BlockComment.SpecialSymbols = new Dictionary<string, string>();
            BlockComment.SpecialSymbols.Add("Begin", "/*");
            BlockComment.SpecialSymbols.Add("End", "*/");
            Rl1.Rules.Add(BlockComment);

            //Strings
            ConfigRule Strings = new ConfigRule()
                {
                    Type = "Span",
                    Name = "String",
                    Bold = true,
                    Italic = false,
                    Color = "#808080".StringToColor(),
                    StopAtEOL = false
                };
            Strings.SpecialSymbols = new Dictionary<string, string>();
            Strings.SpecialSymbols.Add("Begin", "'");
            Strings.SpecialSymbols.Add("End", "'");
            Rl1.Rules.Add(Strings);

            //Join Keywords
            ConfigRule JoinKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "JoinKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#020262".StringToColor(),
                    StopAtEOL = false,
                    Words =
                        new List<string>()
                            {
                                "INNER",
                                "JOIN",
                                "LEFT",
                                "RIGHT",
                                "OUTER",
                                "APPLY",
                                "CROSS",
                                "UNION",
                                "ON",
                                "FULL",
                                "EXCEPT",
                                "MERGE"
                            }
                };
            Rl1.Rules.Add(JoinKeywords);

            //Alias Keywords
            ConfigRule AliasKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "AliasKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#BA8C1A".StringToColor(),
                    StopAtEOL = false,
                    Words = new List<string>() {"AS"}
                };
            Rl1.Rules.Add(AliasKeywords);

            //Comparison KeyWords
            ConfigRule ComparisonKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "ComparisonKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#008080".StringToColor(),
                    StopAtEOL = false,
                    Words = new List<string>() {"AND", "OR", "LIKE", "IN", "EXISTS"}
                };
            Rl1.Rules.Add(ComparisonKeyWords);

            //Destructive KeyWords
            ConfigRule DestructiveKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "DestructiveKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#FF0000".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"DROP", "DELETE", "TRUNCATE"}
                };
            Rl1.Rules.Add(DestructiveKeyWords);

            //Restrictive Keywords
            ConfigRule RestrictiveKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "RestrictiveKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#08D625".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"TOP", "DISTINCT", "LIMIT"}
                };
            Rl1.Rules.Add(RestrictiveKeywords);


            //Declarative Keywords
            ConfigRule DeclarativeKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "DeclarativeKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#08D625".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"DECLARE", "BEGIN", "END"}
                };
            Rl1.Rules.Add(DeclarativeKeywords);

            //Transaction KeyWords
            ConfigRule TransactionKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "TransactionKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#DA800A".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"COMMMIT", "ROLLBACK", "TRANSACTION", "TRAN"}
                };
            Rl1.Rules.Add(TransactionKeyWords);

            //Debug KeyWords
            ConfigRule DebugKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "DebugKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#FF9900".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"TRY", "CATCH", "RAISERROR"}
                };
            Rl1.Rules.Add(DebugKeyWords);

            //Cursor KeyWords
            ConfigRule CursorKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "CursorKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#00869E".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"FOR", "OPEN", "FETCH", "NEXT", "CLOSE", "DEALLOCATE"}
                };
            Rl1.Rules.Add(CursorKeyWords);

            //SQL KeyWords
            ConfigRule SQLKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SQLKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#000099".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>()
                            {
                                "NOT",
                                "SET",
                                "DESC",
                                "ASC",
                                "EXEC",
                                "WITH",
                                "EXECUTE",
                                "NULL",
                                "IS",
                                "VALUES",
                                "BY"
                            }
                };
            Rl1.Rules.Add(SQLKeyWords);

            //SQL Action KeyWords
            ConfigRule SQLActionKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SQLActionKeyWords",
                    Bold = true,
                    Italic = false,
                    Color = "#2E13CF".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>()
                            {
                                "INSERT",
                                "SELECT",
                                "UPDATE",
                                "FROM",
                                "WHERE",
                                "HAVING",
                                "GROUP",
                                "ORDER",
                                "CREATE",
                                "ALTER",
                                "ADD",
                                "INTO"
                            }
                };
            Rl1.Rules.Add(SQLActionKeyWords);

            //SQLTypes
            ConfigRule SQLTypes = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SQLTypes",
                    Bold = true,
                    Italic = false,
                    Color = "#00BFFF".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>()
                        {
                            "BIGINT",
                            "NUMERIC",
                            "BIT",
                            "INT",
                            "SMALLINT",
                            "TINYINT",
                            "SMALLMONEY",
                            "DECIMAL",
                            "FLOAT",
                            "REAL",
                            "DATE",
                            "DATETIME",
                            "DATETIME2",
                            "DATETIMEOFFSET",
                            "TIME",
                            "SMALLDATETIME",
                            "CHAR",
                            "VARCHAR",
                            "TEXT",
                            "NCHAR",
                            "NVARCHAR",
                            "NTEXT",
                            "BINARY",
                            "VARBINARY",
                            "IMAGE",
                            "XML",
                            "CURSOR",
                            "TIMESTAMP",
                            "UNIQUEIDENTIFIER",
                            "HIERARCHYID",
                            "SQL_VARIANT",
                            "TABLE",
                            "SYSNAME"
                        }
                };
            Rl1.Rules.Add(SQLTypes);

            //SQL Objects
            ConfigRule SqlObjects = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SqlObjects",
                    Bold = true,
                    Italic = true,
                    Color = "#8B0000".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>() {"TABLE", "PROC", "PROCEDURE", "FUNCTION", "VIEW", "TRIGGER", "INDEX", "DATABASE"}
                };
            Rl1.Rules.Add(SqlObjects);

            //Flow Control Keywords
            ConfigRule FlowControlKeyWords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "FlowControlKeyWords",
                    Bold = true,
                    Italic = true,
                    Color = "#4B0082".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>()
                            {
                                "IF",
                                "ELSE",
                                "CASE",
                                "THEN",
                                "WHEN",
                                "WHILE",
                                "WAITFOR",
                                "DELAY",
                                "RETURN",
                                "SWITCH",
                                "BREAK"
                            }
                };
            Rl1.Rules.Add(FlowControlKeyWords);

            //Special Keywords
            ConfigRule SpecialKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SpecialKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#339900".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"GO", "NOCOUNT", "OFF", "OPENDATASOURCE", "USE", "ISOLATION", "LEVEL"}
                };
            Rl1.Rules.Add(SpecialKeywords);

            //Special Construction Keywords
            ConfigRule SpecialConstructionKeywords = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SpecialConstructionKeywords",
                    Bold = true,
                    Italic = false,
                    Color = "#996666".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>()
                            {
                                "CHECK",
                                "NOCHECK",
                                "CONSTRAINT",
                                "FOREIGN",
                                "PRIMARY",
                                "KEY",
                                "REFERENCES",
                                "IDENTITY"
                            }
                };
            Rl1.Rules.Add(SpecialConstructionKeywords);

            //Transact SQL Flags
            ConfigRule TSqlFlags = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "TSqlFlags",
                    Bold = true,
                    Italic = false,
                    Color = "#FF9933".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>()
                        {
                            "XACT_ABORT",
                            "SERIALIZABLE",
                            "NUMERIC_ROUNDABORT",
                            "ANSI_PADDING",
                            "ANSI_WARNINGS",
                            "CONCAT_NULL_YIELDS_NULL",
                            "ARITHABORT",
                            "QUOTED_IDENTIFIER",
                            "ANSI_NULLS",
                            "COLLATION",
                            "UNCOMMITTED",
                            "READ"
                        }
                };
            Rl1.Rules.Add(TSqlFlags);

            //Punctuation
            ConfigRule Punctuation = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "Punctuation",
                    Bold = false,
                    Italic = false,
                    Color = "#2F4F4F".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"(", ")", "[", "]"}
                };
            Rl1.Rules.Add(Punctuation);

            //Comparison Operators
            ConfigRule ComparisonOperators = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "ComparisonOperators",
                    Bold = false,
                    Italic = false,
                    Color = "#2F4F4F".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>() {"&lt;", "&gt;", "="}
                };
            Rl1.Rules.Add(ComparisonOperators);

            //Transact SQL Functions
            ConfigRule TransactSQLFunctions = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "TransactSQLFunctions",
                    Bold = true,
                    Italic = false,
                    Color = "#8A2BE2".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>()
                        {
                            "PRINT",
                            "STUFF",
                            "SUBSTRING",
                            "UPPER",
                            "LOWER",
                            "REVERSE",
                            "REPLACE",
                            "LTRIM",
                            "RTRIM",
                            "LEN",
                            "CAST",
                            "CONVERT",
                            "ISNULL",
                            "DATEDIFF",
                            "DATEADD",
                            "GETDATE",
                            "GETUTCDATE",
                            "ROW_NUMBER",
                            "OVER"
                        }
                };
            Rl1.Rules.Add(TransactSQLFunctions);

            //Group by functions
            ConfigRule GroupByFunctions = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "GroupByFunctions",
                    Bold = true,
                    Italic = false,
                    Color = "#AE1737".StringToColor(),
                    StopAtEOL = true,
                    Words =
                        new List<string>()
                            {
                                "AVG",
                                "MIN",
                                "CHECKSUM_AGG",
                                "SUM",
                                "COUNT",
                                "STDEV",
                                "COUNT_BIG",
                                "STDEVP",
                                "GROUPING",
                                "VAR",
                                "GROUPING_ID",
                                "VARP",
                                "MAX"
                            }
                };
            Rl1.Rules.Add(GroupByFunctions);

            //SystemTables
            ConfigRule SystemTables = new ConfigRule()
                {
                    Type = "KeyWords",
                    Name = "SystemTables",
                    Bold = true,
                    Italic = false,
                    Color = "#008080".StringToColor(),
                    StopAtEOL = true,
                    Words = new List<string>()
                        {
                            "SYSALTFILES",
                            "SYSLOCKINFO",
                            "SYSCACHEOBJECTS",
                            "SYSLOGINS",
                            "SYSCHARSETS",
                            "SYSMESSAGES",
                            "SYSCONFIGURES",
                            "SYSOLEDBUSERS",
                            "SYSCURCONFIGS",
                            "SYSPERFINFO",
                            "SYSDATABASES",
                            "SYSPROCESSES",
                            "SYSDEVICES",
                            "SYSREMOTELOGINS",
                            "SYSLANGUAGES",
                            "SYSSERVERS",
                            "SYSCOLUMNS",
                            "SYSINDEXKEYS",
                            "SYSCOMMENTS",
                            "SYSMEMBERS",
                            "SYSCONSTRAINTS",
                            "SYSOBJECTS",
                            "SYSDEPENDS",
                            "SYSPERMISSIONS",
                            "SYSPROTECTS",
                            "SYSFILES",
                            "SYSREFERENCES",
                            "SYSFOREIGNKEYS",
                            "SYSTYPES",
                            "SYSFULLTEXTCATALOGS",
                            "SYSUSERS",
                            "SYSINDEXES",
                            "SYSALERTS",
                            "SYSJOBSTEPS",
                            "SYSCATEGORIES",
                            "SYSNOTIFICATIONS",
                            "SYSDOWNLOADLIST",
                            "SYSOPERATORS",
                            "SYSJOBHISTORY",
                            "SYSTARGETSERVERGROUPMEMBERS",
                            "SYSJOBS",
                            "SYSTARGETSERVERGROUPS",
                            "SYSJOBSCHEDULES",
                            "SYSTARGETSERVERS",
                            "SYSJOBSERVERS",
                            "SYSTASKIDS"
                        }
                };
            Rl1.Rules.Add(SystemTables);

            #endregion

            RuleSet Rl2 = new RuleSet();
            Rl2.Rules = new List<ConfigRule>();
            Rl2.IsMainRuleSet = false;

            #region Highlighting rules for user flags and the query naming

            //Name of the rule set
            Rl2.Name = "SpecialComments";
            //Case insensitive 
            Rl2.IgnoreCase = true;

            Rl2.Delimiters = new Delimiter() {DelimiterChars = @"=!&gt;&lt;+-/*%&amp;|^~.}{,;][?()"};

            Rl2.Rules.Add
                (
                    new ConfigRule()
                        {
                            Type = "KeyWords",
                            Name = "QueryName",
                            Bold = true,
                            Italic = false,
                            Color = "#191970".StringToColor(),
                            Words = new List<string>() {"NAME:"}
                        }
                );
            Rl2.Rules.Add
                (
                    new ConfigRule()
                        {
                            Type = "KeyWords",
                            Name = "UserFlags",
                            Bold = true,
                            Italic = false,
                            Color = "#FF0000".StringToColor(),
                            Words = new List<string>() {"HACK", "TODO", "FIXME"}
                        }
                );

            #endregion

            RuleSets = new List<RuleSet>();
            RuleSets.Add(Rl1);
            RuleSets.Add(Rl2);
        }

        public void DeserializeFromFile(string filePath)
        {
            XDocument settings = XDocument.Load(filePath);
            var xmlRoot = settings.Element("SyntaxDefinition");

            Environment                           = new Environment();
            var xmlEnvironment                    = xmlRoot.Element("Environment");
            Environment.BackgroundColor           = xmlEnvironment.Element("Default").Attribute("bgcolor").Value.StringToColor();
            Environment.FontColor                 = xmlEnvironment.Element("Default").Attribute("color").Value.StringToColor();
            Environment.LineNumberBackgroundColor = xmlEnvironment.Element("LineNumbers").Attribute("bgcolor").Value.StringToColor();
            Environment.LineNumberFontColor       = xmlEnvironment.Element("LineNumbers").Attribute("color").Value.StringToColor();
            Environment.SelectionColor            = xmlEnvironment.Element("Selection").Attribute("bgcolor").Value.StringToColor();

            Digits        = new Digits();
            var xmlDigits = xmlRoot.Element("Digits");
            Digits.Bold   = bool.Parse(xmlDigits.Attribute("bold").Value);
            Digits.Italic = bool.Parse(xmlDigits.Attribute("italic").Value);
            Digits.Color  = xmlDigits.Attribute("color").Value.StringToColor();

            RuleSets = new List<RuleSet>();
            foreach (var xmlRuleSet in xmlRoot.Element("RuleSets").Elements("RuleSet"))
            {
                RuleSet rs = new RuleSet();
                rs.Rules = new List<ConfigRule>();
                rs.Delimiters = new Delimiter() { DelimiterChars = @"=!&gt;&lt;+-/*%&amp;|^~.}{,;][?()" };
                if (xmlRuleSet.Attribute("name") != null)
                {
                    rs.Name = xmlRuleSet.Attribute("name").Value;
                    rs.IsMainRuleSet = false;
                }
                else
                {
                    rs.Name = "Main";
                    rs.IsMainRuleSet = true;
                }

                foreach (var xmlSpan in xmlRuleSet.Elements("Span"))
                {
                    ConfigRule cr                    = new ConfigRule();
                    cr.Type                          = "Span";
                    cr.Bold                          = bool.Parse(xmlSpan.Attribute("bold").Value);
                    cr.Color                         = xmlSpan.Attribute("color").Value.StringToColor();
                    cr.Italic                        = bool.Parse(xmlSpan.Attribute("italic").Value);
                    cr.StopAtEOL                     = bool.Parse(xmlSpan.Attribute("stopateol").Value);
                    cr.Name                          = xmlSpan.Attribute("name").Value;
                    cr.SpecialSymbols                = new Dictionary<string, string>();
                    cr.SpecialSymbols.Add("Begin", xmlSpan.Element("Begin").Value);
                    if (xmlSpan.Element("End") != null)
                        cr.SpecialSymbols.Add("End", xmlSpan.Element("End").Value);
                    if (xmlSpan.Attribute("rule") != null)
                        cr.Rule = xmlSpan.Attribute("rule").Value;
                    rs.Rules.Add(cr);
                }

                foreach (var xmlKeyWords in xmlRuleSet.Elements("KeyWords"))
                {
                    ConfigRule cr     = new ConfigRule();
                    cr.Type           = "KeyWords";
                    cr.Bold           = bool.Parse(xmlKeyWords.Attribute("bold").Value);
                    cr.Color          = xmlKeyWords.Attribute("color").Value.StringToColor();
                    cr.Italic         = bool.Parse(xmlKeyWords.Attribute("italic").Value);
                    cr.Name           = xmlKeyWords.Attribute("name").Value;
                    cr.SpecialSymbols = new Dictionary<string, string>();
                    cr.Words          = new List<string>();
                    
                    foreach (var xmlWord in xmlKeyWords.Elements("Key"))
                    {
                        cr.Words.Add(xmlWord.Attribute("word").Value);
                    }

                    rs.Rules.Add(cr);
                }
                RuleSets.Add(rs);
            }
        }
        public override string ToString()
        {
            return this.ToString(false);
        }
        public string ToString(bool IsPreview)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version = \"1.0\"?>");
            if(!IsPreview)
                sb.AppendLine("<SyntaxDefinition name = \"SQL\" extensions = \".sql\">");
            else
                sb.AppendLine("<SyntaxDefinition name = \"Preview\" extensions = \".sql\">");
            sb.AppendLine(Environment.ToString());
            sb.AppendLine("<Properties>".Indent(1));
            sb.AppendLine("<Property name=\"LineComment\" value=\"--\"/>".Indent(2));
            sb.AppendLine("</Properties>".Indent(1));
            sb.AppendLine(Digits.ToString());
            sb.AppendLine("<RuleSets>".Indent(1));
            foreach (RuleSet ruleSet in RuleSets)
            {
                sb.AppendLine(ruleSet.ToString());
            }
            sb.AppendLine("</RuleSets>".Indent(1));
            sb.AppendLine("</SyntaxDefinition>");
            return sb.ToString();
        }
    }
}
