using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// Delegate for the <see cref="DatagridViewCheckBoxHeaderCell.OnCheckBoxClicked"/> event,
    /// raised when the user clicks the header checkbox in a <see cref="CustomGridViewCheckBoxColumn"/>.
    /// </summary>
    /// <param name="columnIndex">The zero-based index of the column whose header was clicked.</param>
    /// <param name="state"><c>true</c> if the header checkbox was checked; <c>false</c> if unchecked.</param>
    public delegate void CheckBoxClickedHandler(int columnIndex, bool state);
}
