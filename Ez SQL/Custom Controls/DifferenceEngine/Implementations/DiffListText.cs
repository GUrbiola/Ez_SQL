using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    /// <summary>
    /// An <see cref="IDiffList"/> implementation that splits a multi-line string (or file) into
    /// individual lines, each wrapped in a <see cref="TextLine"/> for hash-based comparison.
    /// Used by <see cref="Ez_SQL.Custom_Controls.SideToSideTextComparer"/> for line-level diffs
    /// of SQL scripts. Lines longer than 2048 characters trigger an exception when loaded from file.
    /// </summary>
    public class DiffListText : IDiffList
    {
        private const int MaxLineLength = 2048;
        private List<string> Lines;

        /// <summary>Initializes an empty list; populate via <see cref="LoadFromFile"/> or the string constructor.</summary>
        public DiffListText()
        {
            Lines = new List<string>();
        }

        /// <summary>
        /// Splits <paramref name="str"/> on <see cref="Environment.NewLine"/> and stores each line.
        /// </summary>
        /// <param name="str">The multi-line text to split into diffable lines.</param>
        public DiffListText(string str)
        {
            Lines = new List<string>();
            foreach (string line in str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                Lines.Add(line);
            }
        }

        /// <summary>
        /// Reads lines from a text file, enforcing a maximum line length of 2048 characters.
        /// Clears any previously loaded lines before reading.
        /// </summary>
        /// <param name="FileName">Full path to the text file to load.</param>
        /// <exception cref="InvalidOperationException">Thrown when a line exceeds <c>MaxLineLength</c> characters.</exception>
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

        /// <inheritdoc/>
        public int Count()
        {
            return Lines.Count;
        }

        /// <inheritdoc/>
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