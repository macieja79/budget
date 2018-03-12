using Budget.Core.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using System.IO;
using Metaproject.Strings;

namespace Budget.Import
{
    public class IngCsvFileParser : ITransactionParser
    {
        #region <members>
        string[] _items;
        const int ITEMS_COUNT = 21;
        #endregion

        #region <nested>

        class TransactionConverter
        {

            public List<Transaction> GetFromIngTransaction(List<TransactionING> ingTransactions)
            {

                List<Transaction> transactions = new List<Transaction>();
                foreach (TransactionING ing in ingTransactions)
                {
                    if (null == ing)
                        continue;

                    Transaction t = GetFromIng(ing);
                    transactions.Add(t);

                }

                return transactions;


            }

            Transaction GetFromIng(TransactionING ing)
            {

                Transaction t = new Transaction
                {

                    TransactionDate = ing.TransactionDate,
                    Amount = ing.Amount,
                    CounterPartData = ing.CounterPartData,
                    Currency = ing.Currency,
                    Details = ing.Details,
                    Title = ing.Title,
                    TransactionNumber = ing.TransactionNumber,
                    Owner = ing.AccountName,
                    AccountNumber = ing.AccountNumber
                };

                return t;
            }

            string ExtractAccountNumber(string counterPartData)
            {

                if (string.IsNullOrEmpty(counterPartData)) return "";

                var items = counterPartData.Split(' ');

                foreach (string item in items)
                {
                    if (item.Length == 26)
                    {

                        if (item.All(c => char.IsDigit(c)))
                            return item;
                    }
                }

                return "";


            }
        }

        class TransactionING
        {

            // Data transakcji; Data ksiŕgowania; Dane kontrahenta; Tytu│;Szczegˇ│y;Nr transakcji; Kwota transakcji(waluta rachunku); Waluta;Kwota blokady/zwolnienie blokady; Waluta;Kwota p│atnoťci w walucie;Waluta;;;;;;;;; 

            public DateTime TransactionDate { get; set; }
            public DateTime BookingDate { get; set; }
            public string CounterPartData { get; set; }
            public string Title { get; set; }
            public string Details { get; set; }
            public string TransactionNumber { get; set; }
            public decimal Amount { get; set; }
            public string Currency { get; set; }
            public decimal BlockAmount { get; set; }
            public string Currency2 { get; set; }
            public decimal CurrencyAmount { get; set; }
            public string Currency3 { get; set; }
            public string AccountName { get; set; }
            public string AccountNumber { get; set; }
        }

        #endregion

        #region <ITransactionParser>
        public bool TryGetMonthReports(FileData data, out TransactionReport report)
        {

            report = new TransactionReport();

            string header = data.Lines.First();
            bool canParse = CheckIfCanParse(header);
            if (!canParse) return false;

            int startIndex = GetStartIndex(data);
            int count = data.Lines.Count - startIndex - 4;
            List<string> transactionLines = data.Lines.Skip(startIndex + 1).Take(count).ToList();

            List<TransactionING> ingTransactions = new List<TransactionING>();
            foreach (string line in transactionLines)
            {
                var transaction = GetFromLine(line);
                ingTransactions.Add(transaction);
            }

            TransactionConverter converter = new TransactionConverter();
            report.Transactions = converter.GetFromIngTransaction(ingTransactions);
            return true;
        }

        private int GetStartIndex(FileData data)
        {
            for (int i = 0; i < data.Lines.Count; i++)
            {
                string line = data.Lines[i];
                if (line.IsNull()) continue;

                if (line.StartsWith("Data transakcji;"))
                    return i;

            }

            return -1;

        }

        private bool CheckIfCanParse(string header)
        {
            if (header.Contains("ING Bank")) return true;
            return false;
        }

        #endregion

        #region <prv>

        TransactionING GetFromLine(string line)
        {
            var items = line.Split(';');
            _items = items;

            int count = items.Length;
            if (count != ITEMS_COUNT)
            {
                return null;
            }

            TransactionING transaction = new TransactionING
            {
                TransactionDate = GetDate(items[0]),
                BookingDate = GetDate(items[1]),
                CounterPartData = GetStr(items[2]),
                AccountNumber = GetAccountNumber(items[4]),
                Title = GetStr(items[3]),
                Details = GetStr(items[6]),
                TransactionNumber = items[7],
                Amount = ParseTools.ParseDecimal(items[8]),
                Currency = items[9],
                BlockAmount = ParseTools.ParseDecimal(items[10]),
                Currency2 = items[11],
                CurrencyAmount = ParseTools.ParseDecimal(items[12]),
                Currency3 = items[13],
                AccountName = items[14]
            };

            return transaction;
        }

        string GetStr(string input)
        {
            if (input.IsNull()) return input;

            string output = input.Replace("\"", "").Trim();
            return output;

        }

        string GetAccountNumber(string input)
        {
            if (input.IsNull()) return input;
            string output = input.Replace("'", "").Trim();
            return output;
        }

        DateTime GetDate(string v)
        {
            if (string.IsNullOrEmpty(v)) return DateTime.MinValue;

            var items = v.Split('-');
            int year = int.Parse(items[0]);
            int month = int.Parse(items[1]);
            int day = int.Parse(items[2]);

            return new DateTime(year, month, day);
        }



        #endregion
    }
}
