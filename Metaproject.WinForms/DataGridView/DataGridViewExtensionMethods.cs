using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Metaproject.DataGrid
{
	public static class DataGridViewExtensionMethods
	{


		public static List<DataGridViewCell> GetCellList (this DataGridView grid)
		{
			List<DataGridViewCell> cells = new List<DataGridViewCell>();
			
			for (int c = 0; c < grid.ColumnCount; c++)
			{
				for (int r = 0; r < grid.RowCount; r++)
				{
					cells.Add(grid[c, r]);
				}
			}

			return cells;

		}

        public static List<DataGridViewRow> ToList(this DataGridViewRowCollection collection)
        {

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in collection)
                list.Add(row);

            return list;
        }

		public static List<DataGridViewColumn> ToList(this DataGridViewColumnCollection collection)
		{
			List<DataGridViewColumn> list = new List<DataGridViewColumn>();
			foreach (DataGridViewColumn column in collection) 
			list.Add(column);

			return list;

		}
	
        public static void AutoFit(this DataGridViewColumn column)
        {
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            int width = column.Width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            column.Width = width;
        }

        public static void AutoFit(this DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
                column.AutoFit();
        }

	    public static void SetImageColumn(this DataGridView grid, string columnName)
	    {
	        DataGridViewImageColumn imageColumn = grid.Columns[columnName] as DataGridViewImageColumn;
	        if (imageColumn.IsNullObj()) return;

	        imageColumn.DefaultCellStyle.NullValue = null;
	    }

	}
}
