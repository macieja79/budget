using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Metaproject.Files;

namespace Metaproject.DataGrid
{
    public class GridToCsvConverter
    {


        public static void Convert(DataGridView grid, string file)
        {


            using (CsvFileWriter writer = new CsvFileWriter(file))
            {
                CsvRow csvRow = new CsvRow();

                for (int r = 0; r < grid.Rows.Count; ++r)
                {

                    if (!grid.Rows[r].Visible) continue;

                    csvRow.Clear();

                    for (int c = 0; c < grid.Columns.Count; ++c)
                    {

                        int c0 = grid.Columns[c].DisplayIndex;

                        object objValue = grid[c0, r].Value;

                        string value = (null != objValue) ? objValue.ToString() : string.Empty;


                        csvRow.Add(value);
                    }

                    writer.WriteRow(csvRow);

                }
            }

        }


    }
}
