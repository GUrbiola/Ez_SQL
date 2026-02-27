using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// A lazy-initialised indexed store of <see cref="DiffState"/> objects, one per destination
    /// element, used by <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine"/>
    /// to cache per-element LCS match results during the diff computation.
    /// Compiles with either a pre-allocated array (default) or a <see cref="Hashtable"/>
    /// (enable <c>#define USE_HASH_TABLE</c> for sparse destination lists).
    /// </summary>
    internal class DiffStateList
    {
#if USE_HASH_TABLE
    		private Hashtable _table;
#else
        private DiffState[] _array;
#endif
        /// <summary>Allocates storage for <paramref name="destCount"/> destination elements.</summary>
        /// <param name="destCount">Total number of elements in the destination list.</param>
        public DiffStateList(int destCount)
        {
#if USE_HASH_TABLE
    			_table = new Hashtable(Math.Max(9,destCount/10));
#else
            _array = new DiffState[destCount];
#endif
        }

        /// <summary>
        /// Returns the <see cref="DiffState"/> for the destination element at <paramref name="index"/>,
        /// creating and storing a new default instance on first access.
        /// </summary>
        /// <param name="index">Zero-based destination element index.</param>
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
