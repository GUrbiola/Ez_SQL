using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    internal class DiffState
    {
        private const int BAD_INDEX = -1;
        private int _startIndex;
        private int _length;

        public int StartIndex { get { return _startIndex; } }
        public int EndIndex { get { return ((_startIndex + _length) - 1); } }
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

        public DiffState()
        {
            SetToUnkown();
        }

        protected void SetToUnkown()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.Unknown;
        }

        public void SetMatch(int start, int length)
        {
            System.Diagnostics.Debug.Assert(length > 0, "Length must be greater than zero");
            System.Diagnostics.Debug.Assert(start >= 0, "Start must be greater than or equal to zero");
            _startIndex = start;
            _length = length;
        }

        public void SetNoMatch()
        {
            _startIndex = BAD_INDEX;
            _length = (int)DiffStatus.NoMatch;
        }


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
