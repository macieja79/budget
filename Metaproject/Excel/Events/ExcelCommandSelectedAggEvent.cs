﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Excel
{
    public class ExcelCommandSelectedAggEvent
    {
        public string CommandId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
