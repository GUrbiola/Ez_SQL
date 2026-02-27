using System;
using System.Collections;
using Ez_SQL.Custom_Controls.DifferenceEngine.Structure;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Engine
{
    /// <summary>
    /// Controls the quality/speed trade-off used by <see cref="DiffEngine.ProcessDiff"/> when
    /// searching for the longest common subsequence between source and destination lists.
    /// </summary>
    public enum DiffEngineLevel
    {
        /// <summary>
        /// Fastest but may miss optimal matches; jumps over already-matched spans in the
        /// destination pass, potentially skipping longer matches that start earlier.
        /// </summary>
        FastImperfect,
        /// <summary>
        /// A middle ground between <see cref="FastImperfect"/> and <see cref="SlowPerfect"/>;
        /// jumps over matched spans only when a new longer match is found.
        /// </summary>
        Medium,
        /// <summary>
        /// Exhaustive search that always finds the globally longest common subsequence;
        /// produces the highest-quality diff at the cost of more computation.
        /// </summary>
        SlowPerfect
    }
}
