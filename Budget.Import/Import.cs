using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Import;

namespace Budget.Import
{
    public class ImportImpl : IImport
    {
        public List<ITransactionParser> GetParsers()
        {
            List<ITransactionParser> parsers = new List<ITransactionParser>();
            parsers.Add(new IngCsvFileParser());
            parsers.Add(new MojeFinanseTransactionParser());
            parsers.Add(new ExcelSheetParser());
            return parsers;
        }
    }
}
