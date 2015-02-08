using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    internal class DiffStateList
    {
#if USE_HASH_TABLE
    		private Hashtable _table;
#else
        private DiffState[] _array;
#endif
        public DiffStateList(int destCount)
        {
#if USE_HASH_TABLE
    			_table = new Hashtable(Math.Max(9,destCount/10));
#else
            _array = new DiffState[destCount];
#endif
        }

        public DiffState GetByIndex(int index)
        {
#if USE_HASH_TABLE
    			DiffState retval = (DiffState)_table[index];
    			if (retval == null)
    			{
    				retval = new DiffState();
    				_table.Add(index,retval);
    			}
#else
            DiffState retval = _array[index];
            if (retval == null)
            {
                retval = new DiffState();
                _array[index] = retval;
            }
#endif
            return retval;
        }
    }
}
