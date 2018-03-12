using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Excel.Excel;

namespace Metaproject.Excel.Formulas
{
    public class ExcelFormulaHelper
    {


        public static ExcelFormula GetSubTotalSum(int row, int column, ExcelRangeInfo range)
        {
            string expression = $"=SUBTOTAL(9;{range.Range})";

            return new ExcelFormula
            {
                Expression = expression,
                Range = new ExcelRangeInfo(row, column)
            };
        }

        public static ExcelFormula GetSum(int row, int column, ExcelRangeInfo range)
        {
            string expression = $"=SUM({range.Range})";

            return new ExcelFormula
            {
                Expression = expression,
                Range = new ExcelRangeInfo(row, column)
            };
         
        }

        


    }

}
