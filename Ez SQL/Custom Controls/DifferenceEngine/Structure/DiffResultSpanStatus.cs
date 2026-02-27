using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// Classifies the relationship between a contiguous span of source items and destination items
    /// returned by <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine.DiffReport"/>.
    /// </summary>
    public enum DiffResultSpanStatus
    {
        /// <summary>Source and destination items at this span are identical — no change.</summary>
        NoChange,
        /// <summary>Source items at this span were changed (replaced) by different destination items.</summary>
        Replace,
        /// <summary>Source items at this span were deleted and have no corresponding destination items.</summary>
        DeleteSource,
        /// <summary>Destination items at this span were added and have no corresponding source items.</summary>
        AddDestination
    }
}
