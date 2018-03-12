using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Metaproject.Excel.Tools;
using Metaproject.Patterns;
using Metaproject.Strings;
using Microsoft.SqlServer.Server;

namespace Budget.Core.Tools
{
    public class TransactionParser : ILineParser<Transaction>
    {
        private readonly string _header;
        private readonly char _separator;
        private int _transactionDateIndex;

        public TransactionParser(string header, char separator)
        {
            _header = header;
            _separator = separator;

            Transaction template = new Transaction();
            List<string> items = header.SplitAndTrim(separator).ToList();
            _transactionDateIndex = items.IndexOf(template.GetName(() => template.TransactionDate));

        }

        public Transaction ParseFromLine(string line)
        {
            try
            {
                List<string> unparsed;
                Transaction iTransaction = new Transaction();
                ParseTools.GetParsed<Transaction>(iTransaction, _header, line, _separator, out unparsed);

                string[] valueItems = line.SplitAndTrim(_separator);
                string dateValue = valueItems[_transactionDateIndex];
                int dateValueInt;
                if (int.TryParse(dateValue, out dateValueInt))
                {
                    iTransaction.TransactionDate = ExcelTools.FromExcelSerialDate(dateValueInt);
                }

                return iTransaction;

            }
            catch (Exception)
            {


            }

            return null;

        }
    }
}
