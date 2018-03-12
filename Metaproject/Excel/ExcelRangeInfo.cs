using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 


namespace Metaproject.Excel
{



    public class ExcelRangeInfo : ICloneable
    {

        public static ExcelRangeInfo CreateColumnRange(int col)
        {
            return new ExcelRangeInfo(-1, col);
        }

        public static ExcelRangeInfo CreateRowRange(int row)
        {
            return new ExcelRangeInfo(row, -1);
        }


        #region <ctors>
        public ExcelRangeInfo(int r0, int c0)
		{
			Start = new ExcelCellRangeInfo(r0, c0);
		}

		public ExcelRangeInfo(int r0, int c0, int r1, int c1)
		{
			Start = new ExcelCellRangeInfo(r0, c0);
			End = new ExcelCellRangeInfo(r1, c1);
		}

		public ExcelRangeInfo(ExcelCellRangeInfo start, ExcelCellRangeInfo end)
		{
			Start = start;
			End = end;
		}
		#endregion

		#region <props>

		public ExcelRangeInfoType TypeOfRange 
		{

			get
			{

				if (null != Start && null != End)
				{

					if (Start.Row == End.Row) return ExcelRangeInfoType.Horizontal;
					if (Start.Column == End.Column) return ExcelRangeInfoType.Vertical;
					return ExcelRangeInfoType.Box;
				}

				if (Start.Column < 1) return ExcelRangeInfoType.Row;
				if (Start.Row < 1) return ExcelRangeInfoType.Column;

				return ExcelRangeInfoType.Cell;

			}
		
		
		}

		public ExcelCellRangeInfo Start { get; set; }
		public ExcelCellRangeInfo End { get; set; }

		public string Range
        {
            get
            {
                if (TypeOfRange.HasValue(ExcelRangeInfoType.Cell))
                {
                    return string.Format("{0}", Start.Address);
                }
                else if (TypeOfRange.HasValue(ExcelRangeInfoType.Row, ExcelRangeInfoType.Column))
                {
                    return string.Format("{0}:{1}", Start.Address, Start.Address);
                }
                else
                {
                    return string.Format("{0}:{1}", Start.Address, End.Address);
                }
            }
        }

        public override string ToString()
        {
            return Range;
        }
        #endregion

        #region <ICloneable>
        public object Clone()
		{

			ExcelCellRangeInfo clonedStart = null;
			ExcelCellRangeInfo clonedEnd = null;

			if (null != Start) {
				clonedStart = new ExcelCellRangeInfo(Start.Row, Start.Column);
			}

			if (null != End) {
				clonedEnd = new ExcelCellRangeInfo(End.Row, End.Column);
			}

			return new ExcelRangeInfo(clonedStart, clonedEnd);

		}
		#endregion
	}
}
