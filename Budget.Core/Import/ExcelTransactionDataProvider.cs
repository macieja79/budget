using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Excel;

namespace Budget.Core.Import
{
    public class ExcelTransactionDataProvider : ITransactionDataProvider
    {
        private IExcelSheet _sheet;
        private readonly string _separator;

        public ExcelTransactionDataProvider(IExcelSheet sheet, string separator)
        {
            _sheet = sheet;
            _separator = separator;
        }

        public FileData GetTransactionData()
        {
            List<string> lines = new List<string>();
            ExcelSheetData sheetData = _sheet.GetSheetData();

            foreach (var row in sheetData.Rows)
            {
                string line = string.Join(_separator, row.Values);
                lines.Add(line);
            }

            return new FileData() {Lines = lines, Name = _sheet.Name, Separator = _separator};
        }
    }
}
