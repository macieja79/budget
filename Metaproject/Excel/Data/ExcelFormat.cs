using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 

namespace Metaproject.Excel
{
	public class ExcelFormatData
	{

		#region <ctors>

		public ExcelFormatData()
		{
				Ranges = new List<ExcelRangeInfo>();
		}

		public ExcelFormatData(int c0, int r0, int c1, int r1) : this()
		{
			Ranges.Add(new ExcelRangeInfo(r0, c0, r1, c1));
		}

		public ExcelFormatData(ExcelRangeInfo range) : this()
		{

			Ranges.Add(range);
		}
		#endregion

		#region <props>
		public List<ExcelRangeInfo> Ranges { get; private set; }

		public System.Drawing.Color? Foreground { get; set; }
		public System.Drawing.Color? Background { get; set; }
		public bool? IsFontBold { get; set; }
		public bool? IsFontItalic { get; set; }
		public bool? IsWrapped { get; set; }
		public bool? IsMerged { get; set; }
		public bool? IsDiminishedToFit { get; set; }
		public int? FontSize { get; set; }
		public string NumberFormat { get; set; }
		public ExcelHorizontalAlignmentType? HorizontalAlignment { get; set; }
		public ExcelVertivalAlignmentType? Vertical { get; set; }

        public int[] ColumnWidths { get; set; }

        public bool? IsPrintArea { get; set; }
        

		public bool? IsCoumnAutofit { get; set; }
		#endregion

	}
}
