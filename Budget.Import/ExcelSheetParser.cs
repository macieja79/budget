using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.Core.Import;
using Budget.Core.Tools;
using Metaproject.Excel.Tools;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Strings;

namespace Budget.Import
{
    public class ExcelSheetParser : ITransactionParser
    {
        public bool TryGetMonthReports(FileData data, out TransactionReport report)
        {
            report = null;

            try
            {
                string header = data.Lines.FirstOrDefault();
                if (header.IsNull()) return false;

                report = new TransactionReport() {Name = data.Name, Transactions = new List<Transaction>()};

                char separator = Convert.ToChar(data.Separator);
                TransactionParser parser = new TransactionParser(header, separator);

                for (int i = 1; i < data.Lines.Count; i++)
                {
                    string line = data.Lines[i];
                    Transaction iTransaction = parser.ParseFromLine(line);
                    report.Transactions.Add(iTransaction);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
