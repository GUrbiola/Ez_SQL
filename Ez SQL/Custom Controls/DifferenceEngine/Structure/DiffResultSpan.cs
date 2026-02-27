//#define USE_HASH_TABLE

using System;
using System.Collections;

namespace Ez_SQL.Custom_Controls.DifferenceEngine.Structure
{
    /// <summary>
    /// Represents one contiguous span in the diff output produced by
    /// <see cref="Ez_SQL.Custom_Controls.DifferenceEngine.Engine.DiffEngine.DiffReport"/>.
    /// Each span carries a <see cref="DiffResultSpanStatus"/> indicating whether the items
    /// were unchanged, replaced, deleted from source, or added to destination, along with
    /// the start indices into each list and the number of elements covered.
    /// Instances are created via the static factory methods and are sorted by
    /// <see cref="DestIndex"/> for ordered enumeration.
    /// </summary>
    public class DiffResultSpan : IComparable
	{
		private const int BAD_INDEX = -1;
		private int _destIndex;
		private int _sourceIndex;
		private int _length;
		private DiffResultSpanStatus _status;

        /// <summary>Gets the zero-based start index in the destination list, or -1 for <see cref="DiffResultSpanStatus.DeleteSource"/> spans.</summary>
		public int DestIndex {get{return _destIndex;}}
        /// <summary>Gets the zero-based start index in the source list, or -1 for <see cref="DiffResultSpanStatus.AddDestination"/> spans.</summary>
		public int SourceIndex {get{return _sourceIndex;}}
        /// <summary>Gets the number of consecutive elements covered by this span.</summary>
		public int Length {get{return _length;}}
        /// <summary>Gets the classification of this span (NoChange, Replace, DeleteSource, or AddDestination).</summary>
		public DiffResultSpanStatus Status {get{return _status;}}

		protected DiffResultSpan(
			DiffResultSpanStatus status,
			int destIndex,
			int sourceIndex,
			int length)
		{
			_status = status;
			_destIndex = destIndex;
			_sourceIndex = sourceIndex;
			_length = length;
		}

        /// <summary>Creates a span indicating that <paramref name="length"/> consecutive elements are identical in both lists.</summary>
		public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.NoChange,destIndex,sourceIndex,length);
		}

        /// <summary>Creates a span indicating that <paramref name="length"/> source elements were replaced by destination elements.</summary>
		public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.Replace,destIndex,sourceIndex,length);
		}

        /// <summary>Creates a span indicating that <paramref name="length"/> source elements were deleted with no corresponding destination items.</summary>
		public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.DeleteSource,BAD_INDEX,sourceIndex,length);
		}

        /// <summary>Creates a span indicating that <paramref name="length"/> destination elements were added with no corresponding source items.</summary>
		public static DiffResultSpan CreateAddDestination(int destIndex, int length)
		{
			return new DiffResultSpan(DiffResultSpanStatus.AddDestination,destIndex,BAD_INDEX,length);
		}

        /// <summary>Extends this span by <paramref name="i"/> additional elements (used to merge adjacent NoChange spans).</summary>
		public void AddLength(int i)
		{
			_length += i;
		}

        /// <inheritdoc/>
		public override string ToString()
		{
			return string.Format("{0} (Dest: {1},Source: {2}) {3}",
				_status.ToString(),
				_destIndex.ToString(),
				_sourceIndex.ToString(),
				_length.ToString());
		}
		#region IComparable Members

        /// <inheritdoc/>
		public int CompareTo(object obj)
		{
			return _destIndex.CompareTo(((DiffResultSpan)obj)._destIndex);
		}

		#endregion
	}
}