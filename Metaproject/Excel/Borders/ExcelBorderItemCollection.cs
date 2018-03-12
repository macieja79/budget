using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 

namespace Metaproject.Excel
{
	public class ExcelBorderItemCollection : List<ExcelBorderItem>
	{

		ExcelBorderItem this[ExcelBordersIndex index]
		{
			get
			{
				foreach (ExcelBorderItem border in this)
				{
					if (border.Index == index)
						return border;
				}

				return null;
			}
		}

		public void AddBorder(ExcelBordersIndex index, ExcelBorderWeight weight, ExcelLineStyle lineStyle)
		{

			ExcelBorderItem border = new ExcelBorderItem(index, weight, lineStyle);
			ExcelBorderItem existingBorder = this[border.Index];

			if (null == existingBorder)
			{
				Add(border);
			}
			else
			{
				existingBorder.LineStyle = border.LineStyle;
				existingBorder.Weight = border.Weight;
			}

		}
	
	}

	
}
