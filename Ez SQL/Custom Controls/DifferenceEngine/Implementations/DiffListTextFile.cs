using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Implementations
{
    /// <summary>
    /// An <see cref="IDiffList"/> implementation that reads a text file line by line at construction
    /// time, wrapping each line in a <see cref="TextLine"/> for hash-based comparison.
    /// Intended for file-to-file diff scenarios; lines are limited to 1024 characters.
    /// </summary>
    public class DiffListTextFile : IDiffList
    {
        private const int MaxLineLength = 1024;
        private ArrayList _lines;

        /// <summary>
        /// Reads all lines from <paramref name="fileName"/> into the list.
        /// </summary>
        /// <param name="fileName">Full path to the text file to load.</param>
        /// <exception cref="InvalidOperationException">Thrown when a line exceeds 1024 characters.</exception>
        public DiffListTextFile(string fileName)
        {
            _lines = new ArrayList();
            using (StreamReader sr = new StreamReader(fileName))
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
                    _lines.Add(new TextLine(line));
                }
            }
        }
        #region IDiffList Members
        /// <inheritdoc/>
        public int Count()
        {
            return _lines.Count;
        }

        /// <inheritdoc/>
        public IComparable GetByIndex(int index)
        {
            return (TextLine)_lines[index];
        }
        #endregion

    }
}