using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// Tracks the best known source match for a single destination element during the LCS
    /// search in <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine"/>.
    /// The <c>_length</c> field doubles as a status sentinel: positive values represent a
    /// matched run length, <c>-1</c> means <see cref="DiffStatus.NoMatch"/>, and <c>-2</c>
    /// means <see cref="DiffStatus.Unknown"/> (not yet evaluated).
    /// </summary>
    internal class DiffState
    {
        private const int BAD_INDEX = -1;
        private int _startIndex;
        private int _length;

        /// <summary>Gets the zero-based start index in the source list for the best match found so far.</summary>
        public int StartIndex { get { return _startIndex; } }
        /// <summary>Gets the inclusive end index of the match in the source list.</summary>
        public int EndIndex { get { return ((_startIndex + _length) - 1); } }
        /// <summary>Gets the length of the best matching run (at least 1 when <see cref="Status"/> is <see cref="DiffStatus.Matched"/>).</summary>
        public int Length
        {
            get
            {
                int len;
                if (_length > 0)
                {
                    len = _length;
                }
                else
                {
                    if (_length == 0)
                    {
                        len = 1;
                    }
                    else
                    {
                        len = 0;
                    }
                }
                return len;
            }
        }
        /// <summary>Gets the current match status derived from the internal <c>_length</c> sentinel value.</summary>
        public DiffStatus Status
        {
            get
            {
                DiffStatus stat;
                if (_length > 0)
                {
                    stat = DiffStatus.Matched;
                }
                else
                {
                    switch (_length)
                    {
                        case -1:
                            stat = DiffStatus.NoMatch;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(_length == -2, "Invalid status: _length < -2");
                            stat = DiffStatus.Unknown;
                            break;
                    }
                }
                return stat;
            }
        }

        /// <summary>Initializes the state to <see cref="DiffStatus.Unknown"/>.</summary>
        public DiffState()
        {
            SetToUnkown();
        }

        protected void SetToUnkown()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.Unknown;
        }

        /// <summary>Records a confirmed match starting at <paramref name="start"/> in the source list, covering <paramref name="length"/> elements.</summary>
        /// <param name="start">Zero-based start index in the source list.</param>
        /// <param name="length">Number of consecutively matching elements (must be &gt; 0).</param>
        public void SetMatch(int start, int length)
        {
            System.Diagnostics.Debug.Assert(length > 0, "Length must be greater than zero");
            System.Diagnostics.Debug.Assert(start >= 0, "Start must be greater than or equal to zero");
            _startIndex = start;
            _length = length;
        }

        /// <summary>Marks this destination element as having no matching source element.</summary>
        public void SetNoMatch()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.NoMatch;
        }


        /// <summary>
        /// Returns <c>true</c> if the cached match is still valid within the new source window
        /// [<paramref name="newStart"/>, <paramref name="newEnd"/>] and fits within
        /// <paramref name="maxPossibleDestLength"/> remaining destination elements.
        /// If the cached result is no longer valid it is reset to <see cref="DiffStatus.Unknown"/>.
        /// </summary>
        public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
        {
            if (_length > 0) //have unlocked match
            {
                if ((maxPossibleDestLength < _length) ||
                    ((_startIndex < newStart) || (EndIndex > newEnd)))
                {
                    SetToUnkown();
                }
            }
            return (_length != (int)DiffStatus.Unknown);
        }
    }
}
