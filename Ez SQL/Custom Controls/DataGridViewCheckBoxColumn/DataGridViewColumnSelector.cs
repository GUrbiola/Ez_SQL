using System.Drawing;
using System.Windows.Forms;

namespace Ez_SQL.Custom_Controls
{
    /// <summary>
    /// Attaches a right-click column-visibility popup to a <see cref="DataGridView"/>.
    /// When the user right-clicks the top-left header cell (row -1, column 0), a
    /// <see cref="ToolStripDropDown"/> containing a <see cref="CheckedListBox"/> appears,
    /// listing every column by header text. Checking or unchecking an item immediately
    /// toggles the corresponding column's <see cref="DataGridViewColumn.Visible"/> property.
    /// </summary>
    class DataGridViewColumnSelector
    {
        /// <summary>The <see cref="DataGridView"/> to which this selector is currently attached.</summary>
        private DataGridView mDataGridView = null;
        /// <summary>The checklist that lists column names with visibility checkboxes.</summary>
        private CheckedListBox mCheckedListBox;
        /// <summary>The drop-down container that hosts the <see cref="mCheckedListBox"/>.</summary>
        private ToolStripDropDown mPopup;

        /// <summary>
        /// The max height of the popup
        /// </summary>
        public int MaxHeight = 300;
        /// <summary>
        /// The width of the popup
        /// </summary>
        public int Width = 200;

        /// <summary>
        /// Gets or sets the DataGridView to which the DataGridViewColumnSelector is attached
        /// </summary>
        public DataGridView DataGridView
        {
            get { return mDataGridView; }
            set
            {
                // If any, remove handler from current DataGridView 
                if (mDataGridView != null) mDataGridView.CellMouseClick -= new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
                // Set the new DataGridView
                mDataGridView = value;
                // Attach CellMouseClick handler to DataGridView
                if (mDataGridView != null) mDataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(mDataGridView_CellMouseClick);
            }
        }

        // When user right-clicks the cell origin, it clears and fill the CheckedListBox with
        // columns header text. Then it shows the popup. 
        // In this way the CheckedListBox items are always refreshed to reflect changes occurred in 
        // DataGridView columns (column additions or name changes and so on).
        void mDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                mCheckedListBox.Items.Clear();
                foreach (DataGridViewColumn c in mDataGridView.Columns)
                {
                    mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                }
                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < MaxHeight) ? PreferredHeight : MaxHeight;
                mCheckedListBox.Width = this.Width;
                mPopup.Show(mDataGridView.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        // The constructor creates an instance of CheckedListBox and ToolStripDropDown.
        // the CheckedListBox is hosted by ToolStripControlHost, which in turn is
        // added to ToolStripDropDown.
        /// <summary>
        /// Initializes the internal <see cref="CheckedListBox"/> and <see cref="ToolStripDropDown"/>
        /// used for the column-visibility popup. The <see cref="DataGridView"/> property must be set
        /// separately to activate the right-click behavior.
        /// </summary>
        public DataGridViewColumnSelector()
        {
            mCheckedListBox = new CheckedListBox();
            mCheckedListBox.CheckOnClick = true;
            mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck);

            ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;
            mPopup.Items.Add(mControlHost);
        }

        /// <summary>
        /// Initializes the selector and immediately attaches it to the specified <see cref="DataGridView"/>.
        /// </summary>
        /// <param name="dgv">The <see cref="DataGridView"/> to attach the column-visibility popup to.</param>
        public DataGridViewColumnSelector(DataGridView dgv)
            : this()
        {
            this.DataGridView = dgv;
        }

        // When user checks / unchecks a checkbox, the related column visibility is 
        // switched.
        void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            mDataGridView.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }
    }
}