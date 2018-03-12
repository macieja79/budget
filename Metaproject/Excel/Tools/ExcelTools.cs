using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Excel.Tools
{
    public class ExcelTools
    {
        public static DateTime FromExcelSerialDate(int serialDate)
        {
            if (serialDate > 59) serialDate -= 1; 
            return new DateTime(1899, 12, 31).AddDays(serialDate);
        }
    }
}
