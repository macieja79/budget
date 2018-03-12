using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 

namespace Metaproject.Excel
{
	public class ExcelCellRangeInfo
	{


		public ExcelCellRangeInfo(int row, int col)
		{
			Column = col;
			Row = row;
		}


		public int Column { get; set; }
		public int Row { get; set; }

		public string Address
		{
			get
			{

				return ExcelSupport.GetCellSymbol(Row, Column);
			}
		}
	
	
	}
}
