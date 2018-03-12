using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Excel
{
    public class ExcelSelectionChangedAggEvent
    {
        public bool IsSingleCell { get; set; }    
        public string RangeInfo { get; set; }
        public ExcelSelectionChangedAggEvent Previous { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

    }
}
