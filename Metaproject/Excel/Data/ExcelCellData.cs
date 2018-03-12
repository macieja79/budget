using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Excel
{
    public class ExcelCellData
    {

        public ExcelCellData(int r, int c)
        {
            Range = new ExcelCellRangeInfo(r, c);
        }

        public ExcelCellRangeInfo Range { get; private set; }
        public string Value { get; set; }

    }
}
