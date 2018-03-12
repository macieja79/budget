using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Excel
{
	public class GridToExcelSheetConverter
	{


		public ExcelSheetData CreateFromGrid(DataGridView grid)
		{

			ExcelSheetData data = new ExcelSheetData();

			int rowCount = grid.Rows.Count;
			int colCount = grid.Columns.Count;

			for (int r = 0; r < rowCount; r++)
			{
				List<object> values = new List<object>();
				for (int c = 0; c < colCount; c++)
				{
					values.Add(grid[c, r].Value);
				}

				ExcelRowData rowData = new ExcelRowData(1, r + 1, values.ToArray());
				data.Rows.Add(rowData);
			}

			ExcelBorderData borderData = new ExcelBorderData(1, 1, rowCount, colCount);
			borderData.Borders.Add(new ExcelBorderItem(ExcelBordersIndex.xlAround, ExcelBorderWeight.xlThick, ExcelLineStyle.xlDouble));
			borderData.Borders.Add(new ExcelBorderItem(ExcelBordersIndex.xlInside, ExcelBorderWeight.xlHairline, ExcelLineStyle.xlContinuous));
			data.Borders.Add(borderData);

			return data;

		}

	
	}
}
