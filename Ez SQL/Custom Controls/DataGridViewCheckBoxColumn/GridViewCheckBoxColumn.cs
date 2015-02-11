using System.Windows.Forms;

namespace Ez_SQL.Custom_Controls
{
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.DataGridViewCheckBoxColumn))]
    public class CustomGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        #region Constructor
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