using Ez_SQL.Common_Code;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    public class DiffListToken : IDiffList
    {
        private List<TokenList> Lines;

        public DiffListToken(string script)
        {
            Lines = new List<TokenList>();
            foreach (string line in script.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Lines.Add(line.GetTokens());
            }
        }

        public int Count()
        {
            return Lines.Count;
        }

        public IComparable GetByIndex(int index)
        {
            if (index >= 0 && index < Lines.Count)
            {
                return Lines[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range in DiffListToken.GetByIndex");
            }
        }
    }

    public class DiffListLineToken : IDiffList
    {
        private TokenList tokens;

        public DiffListLineToken(string script)
        {
            tokens = script.GetTokens();
        }

        public int Count()
        {
            return tokens.TokenCount;
        }

        public IComparable GetByIndex(int index)
        {
            if (index >= 0 && index < tokens.TokenCount)
            {
                return tokens.GetToken(index);
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range in DiffListToken.GetByIndex");
            }
        }
    }
}
