using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    public class DiffListText : IDiffList
    {
        private const int MaxLineLength = 2048;
        private List<string> Lines;

        public DiffListText()
        {
            Lines = new List<string>();
        }

        public DiffListText(string str)
        {
            Lines = new List<string>();
            foreach (string line in str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))    
            {
                Lines.Add(line);
            }
        }

        public void LoadFromFile(string FileName)
        {
            Lines = new List<string>();
            using (StreamReader sr = new StreamReader(FileName))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > MaxLineLength)
                    {
                        throw new InvalidOperationException(
                            string.Format("File contains a line greater than {0} characters.",
                            MaxLineLength.ToString()));
                    }
                    Lines.Add(line);
                }
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
                if(index == Lines.Count -1)
                    return new TextLine(Lines[index]);
                else
                    return new TextLine(Lines[index] + Environment.NewLine);
            }
            return new TextLine("");
        }
    }
}