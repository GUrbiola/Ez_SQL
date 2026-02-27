using System.Windows.Forms;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// A <see cref="DataGridViewCheckBoxColumn"/> subclass that adds a master checkbox to its header cell.
    /// The constructor replaces the default header cell with a <see cref="DatagridViewCheckBoxHeaderCell"/>
    /// and subscribes to its <see cref="DatagridViewCheckBoxHeaderCell.OnCheckBoxClicked"/> event.
    /// When the header checkbox is toggled, every row in the column is set to the same checked state,
    /// acting as a select-all / deselect-all control.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.DataGridViewCheckBoxColumn))]
    public class CustomGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        #region Constructor
        /// <summary>
        /// Initializes the column, installs the <see cref="DatagridViewCheckBoxHeaderCell"/> as the header cell,
        /// sets the default column width to 50, and wires the header-checkbox click handler.
        /// </summary>
        public CustomGridViewCheckBoxColumn()
        {
            DatagridViewCheckBoxHeaderCell datagridViewCheckBoxHeaderCell = new DatagridViewCheckBoxHeaderCell();

            this.HeaderCell = datagridViewCheckBoxHeaderCell;
            this.Width = 50;

            //this.DataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvList_CellFormatting);
            datagridViewCheckBoxHeaderCell.OnCheckBoxClicked += new CheckBoxClickedHandler(datagridViewCheckBoxHeaderCell_OnCheckBoxClicked);

        }
        #endregion

        #region Methods
        void datagridViewCheckBoxHeaderCell_OnCheckBoxClicked(int columnIndex, bool state)
        {
            DataGridView.RefreshEdit();
            foreach (DataGridViewRow row in this.DataGridView.Rows)
            {
                //if (!row.Cells[columnIndex].ReadOnly)
                //{
                    row.Cells[columnIndex].Value = state;
                //}
            }
            DataGridView.RefreshEdit();
        }
        #endregion
    }
}