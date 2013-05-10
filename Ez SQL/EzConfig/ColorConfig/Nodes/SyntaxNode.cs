using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ez_SQL.EzConfig.ColorConfig.Nodes
{
    public enum NodeType
    {
        Root,
        Environment,
        EnvironmentOption,
        Digits,
        RuleSets,
        RuleSet,
        Span,
        KeyWords,
        KeyWord
    }

    public class SyntaxNode :TreeNode
    {
        public NodeType NodeType { get; set; }
        public string NodeName { get; set; }
        public bool? StopAtEOL { get; set; }
        public bool? Italic { get; set; }
        public bool? Bold { get; set; }
        public bool? IgnoreCase { get; set; }
        public string Rule { get; set; }
        public Color? Color { get; set; }
        public Color? BackGround { get; set; }
        public List<string> Words { get; set; }

        public SyntaxNode(NodeType nodeType)
        {
            NodeType = nodeType;

            NodeName   = null;
            StopAtEOL  = null;
            Italic     = null;
            Bold       = null;
            IgnoreCase = null;
            Rule       = null;
            Color      = null;
            BackGround = null;
            Words      = null;

            switch (nodeType)
            {
                default:
                case NodeType.Root:
                    ImageIndex = 0;
                    break;
                case NodeType.Environment:
                    ImageIndex = 1;
                    break;
                case NodeType.RuleSets:
                    ImageIndex = 3;
                    break;
                case NodeType.EnvironmentOption:
                    ImageIndex = 4;
                    break;
                case NodeType.Digits:
                    ImageIndex = 2;
                    break;
                case NodeType.RuleSet:
                    ImageIndex = 4;
                    break;
                case NodeType.Span:
                    ImageIndex = 5;
                    break;
                case NodeType.KeyWords:
                    ImageIndex = 6;
                    break;
                case NodeType.KeyWord:
                    ImageIndex = 7;
                    break;
            }
        }
        
    }
}
