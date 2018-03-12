using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Excel
{
    public class ExcelBorderData
    {
        public ExcelBorderData(int c0, int r0, int c1, int r1)
        {
            Range = new ExcelRangeInfo(r0, c0, r1, c1);
            Borders = new ExcelBorderItemCollection();
        }

        public ExcelBorderData(ExcelRangeInfo range)
        {
            Range = range;
            Borders = new ExcelBorderItemCollection();
        }

        public ExcelRangeInfo Range { get; set; }
        public ExcelBorderItemCollection Borders { get; set; }


    }
}
