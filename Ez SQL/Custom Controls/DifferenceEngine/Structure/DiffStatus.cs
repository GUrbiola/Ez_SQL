using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// Encodes the match status of a single destination element during the LCS search inside
    /// <see cref="DiffState"/>. Negative sentinel values are stored directly in
    /// <c>DiffState._length</c> to avoid a separate status field.
    /// </summary>
    internal enum DiffStatus
    {
        /// <summary>A matching source span was found; <c>_length</c> holds its length (&gt; 0).</summary>
        Matched = 1,
        /// <summary>No matching source span exists for this destination element.</summary>
        NoMatch = -1,
        /// <summary>The match has not yet been evaluated.</summary>
        Unknown = -2
    }
}
