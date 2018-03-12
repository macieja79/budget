using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Excel
{
	public class ExcelBorderItem
	{
		public ExcelBordersIndex Index { get; set; }
		public ExcelBorderWeight Weight { get; set; }
		public ExcelLineStyle LineStyle { get; set; }

		public ExcelBorderItem(ExcelBordersIndex index, ExcelBorderWeight weight, ExcelLineStyle lineStyle)
		{

			Index = index;
			Weight = weight;
			LineStyle = lineStyle;
		}
	
	}
}
