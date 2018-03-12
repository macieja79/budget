using Metaproject.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.DataGrid
{
    public class DataGridSearcher
    {
        DataGridView _grid;
        
        public DataGridSearcher(DataGridView grid)
        {
            _grid = grid;
        }

        public void SearchHorizontally(string pattern)
        {

            search(pattern, true);
        }

        public void SearchVertically(string pattern)
        {
            search(pattern, false);

        }

        void search(string pattern, bool isHorizontal)
        {

         

            int c = 0;
            int r = 0;

            DataGridViewCell cell = getLeftUpperSelectedCell();
            _grid.ClearSelection();

            if (null != cell)
            {
                r = cell.RowIndex;
                c = cell.ColumnIndex;
            }

            int nCol = _grid.Columns.Count;
            int nRow = _grid.Rows.Count;
            int repeats = nCol * nRow - 1;

            for (int i = 0; i < repeats; ++i)
            {


                if (isHorizontal)
                    incrementHorizontally(ref r, ref c, nCol, nRow);
                else
                    incrementVertically(ref r, ref c, nCol, nRow);

                bool isFound = searchGrid(pattern, c, r);
                if (isFound)
                {
                    return;
                }

            }
        }

        void incrementHorizontally(ref int r, ref int c, int nCol, int nRow)
        {
            ++c;
            if (c == nCol)
            {
                c = 0;
                ++r;
            }

            if (r == nRow)
                r = 0;

        }

        void incrementVertically(ref int r, ref int c, int nCol, int nRow)
        {
            ++r;
            if (r == nRow)
            {
                r = 0;
                ++c;
            }

            if (c == nCol)
                c = 0;

        }

        bool searchGrid(string pattern, int c, int r)
        {
            if (!(_grid[c, r].Value is string)) return false;
            string cellValue = (string)_grid[c, r].Value;
            if (StrComparer.Instance.Compare(pattern, cellValue, true, true))
            {
                _grid[c, r].Selected = true;
                _grid.FirstDisplayedScrollingColumnIndex = c;
                _grid.FirstDisplayedScrollingRowIndex = r;
                return true;
            }

            return false;
        }

        DataGridViewCell getLeftUpperSelectedCell()
        {

            List<DataGridViewCell> selected = _grid.GetCellList()
                .Where(c => c.Selected).ToList();

            if (selected.Count == 0) return _grid[0, 0];

            int minCol = selected.Select(c => c.ColumnIndex).Min();
            int minRow = selected.Select(c => c.RowIndex).Min();

            return _grid[minCol, minRow];
        }

    }
}
