using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 

namespace Metaproject.Excel
{
	public class ExcelRowData
	{

		public ExcelRowData(int col, int row, params object[] values)
		{

			Values = new List<object>();
			Range = new ExcelRangeInfo(row, col);
			foreach (object val in values)
				Values.Add(val);
		}

		public ExcelRangeInfo Range { get; set; }
		public List<object> Values { get; set; }

	    public string GetValuesInLine(string separator)
	    {
	        return string.Join(separator, Values);
	    }
	}
}
