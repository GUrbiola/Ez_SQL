using System;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// Provides data for the <see cref="DatagridViewCheckBoxHeaderCell.OnCheckBoxClicked"/> event,
    /// carrying the new checked state of the header checkbox.
    /// </summary>
    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        bool _bChecked;
        /// <summary>
        /// Initializes a new instance with the column index and the new checked state.
        /// </summary>
        /// <param name="columnIndex">The zero-based index of the column whose header checkbox changed.</param>
        /// <param name="bChecked"><c>true</c> if the header checkbox was checked; <c>false</c> if unchecked.</param>
        public DataGridViewCheckBoxHeaderCellEventArgs(int columnIndex, bool bChecked)
        {
            _bChecked = bChecked;
        }
        /// <summary>Gets the new checked state of the header checkbox.</summary>
        public bool Checked
        {
            get { return _bChecked; }
        }
    }
}