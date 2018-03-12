using System.Collections.Generic;
using Metaproject.Excel.Excel;

namespace Metaproject.Excel
{
	public class ExcelSheetData
	{
		public ExcelSheetData()
		{
			Rows = new List<ExcelRowData>();
			Borders = new List<ExcelBorderData>();
			Formats = new List<ExcelFormatData>();
            Cells = new List<ExcelCellData>();
            Formulas = new List<ExcelFormula>();
        }

		public List<ExcelRowData> Rows { get; set; }
		public List<ExcelBorderData> Borders { get; set; }
		public List<ExcelFormatData> Formats { get; set; }
        public List<ExcelCellData> Cells { get; set; }
        public List<ExcelFormula> Formulas { get; set; }
    }
}