using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// Defines the contract for a sequence of comparable items that can be fed into
    /// <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine"/>.
    /// Implementations wrap different data sources (characters, lines, tokens, files)
    /// so that the engine can diff any ordered collection of <see cref="IComparable"/> elements.
    /// </summary>
    public interface IDiffList
    {
        /// <summary>Returns the total number of elements in the list.</summary>
        int Count();
        /// <summary>Returns the element at the specified zero-based <paramref name="index"/>.</summary>
        /// <param name="index">Zero-based position of the element to retrieve.</param>
        IComparable GetByIndex(int index);
    }
}
