using Metaproject.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.WinForms
{
    public static class TableExtensions
    {
        public static DataGridView GetAsDataGridView<R, C, V>(this MtTable<R, C, V> table)
        {

            DataGridView grid = new DataGridView();
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToOrderColumns = false;
            grid.EnableHeadersVisualStyles = false;
            grid.EditMode = DataGridViewEditMode.EditProgrammatically;


            C[] columnHeaders = table.Headers;
            for (int iColumn = 0; iColumn < columnHeaders.Length; iColumn++)
            {
                C header = columnHeaders[iColumn];
                grid.Columns.Add("c" + iColumn.ToString(), header.ToString());
            }

            R[] rowHeaders = table.GetRowHeaders();
            foreach (R rowHeader in rowHeaders)
            {
                V[] values = table.GetRowValues(rowHeader);
                int iRow = grid.Rows.Add();

                grid.Rows[iRow].HeaderCell.Value = rowHeader.ToString();
                for (int i = 0; i < values.Length; i++)
                {
                    grid[i, iRow].Value = values[i];

                }
            }

            return grid;

        }

        public static Form GetTableAsForm<R, C, V>(this MtTable<R, C, V> table)
        {
            Form f = new Form();
            f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            DataGridView grid = table.GetAsDataGridView();
            grid.Dock = DockStyle.Fill;
            f.Controls.Add(grid);
            return f;
        }

        public static Control GetTableAsControl<R, C, V>(this MtTable<R, C, V> table)
        {
            Control c = new Control();
            DataGridView grid = table.GetAsDataGridView();
            c.Controls.Add(grid);
            c.Size = grid.Size;
            grid.Dock = DockStyle.Fill;
            return c;
        }

    }
}
