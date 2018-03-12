using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Excel.Data
{
    public class PivotTableData
    {
        public string Name { get; set; }
        public ExcelRangeInfo Range { get; set; }
        public ExcelRangeInfo InsertCell { get; set; }
        public List<string> Rows { get; set; }
        public List<string> Columns { get; set; }
        public List<string> Values { get; set; }
        public string StyleName { get; set; }
    }
}
