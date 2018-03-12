using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.Core.Import;
using Metaproject.Strings;
using Microsoft.SqlServer.Server;

namespace Budget.Import
{
    public class MojeFinanseTransactionParser : ITransactionParser
    {

        class TransactionItem
        {
            public DateTime DataOperacji { get; set; }
            public int Typ { get; set; }
            public string Tytul { get; set; }
            public decimal Kwota { get; set; }
            public string Notatka { get; set; }
            public string Kategoria { get; set; }
            public string PodKategoria { get; set; }
            public string NazwaKonta { get; set; }
        }


        public bool TryGetMonthReports(FileData data, out TransactionReport report)
        {

            report = new TransactionReport() {Transactions = new List<Transaction>()};

            string header = data.Lines.FirstOrDefault();
            bool canParse = CheckIfValidFile(header);
            if (!canParse) return false;
            

            for (int i = 1; i < data.Lines.Count; i++)
            {
                string line = data.Lines[i];
                TransactionItem item = GetFromLine(line);
                if (item.IsNullObj()) continue;

                Transaction transaction = CreateFromTransactionItem(item);
                if (transaction.IsNullObj()) continue;
                
                report.Transactions.Add(transaction);
            }

            return true;

        }

        bool CheckIfValidFile(string header)
        {
            const int ExpectedNumberOfItems = 8;

            string[] items = header.Split(';');

            if (items.Length != ExpectedNumberOfItems) return false;

            return true;

        }

        TransactionItem GetFromLine(string line)
        {
            if (line.IsNull()) return null;

            string[] splited = line.Split(';');

            TransactionItem item = new TransactionItem()
            {
                DataOperacji = GetDate(splited[0]),
                Typ =  ParseTools.ParseInt(splited[1]),
                Tytul = splited[2],
                Kwota = ParseTools.ParseDecimal(splited[3]),
                Notatka = splited[4],
                PodKategoria = splited[5],
                Kategoria = splited[6],
                NazwaKonta = splited[7]
            };

            return item;

        }


        DateTime GetDate(string v)
        {
            if (string.IsNullOrEmpty(v)) return DateTime.MinValue;

            var items = v.Split('-');
            int day = int.Parse(items[0]);
            int month = int.Parse(items[1]);
            int year = int.Parse(items[2]);

            return new DateTime(year, month, day);
        }

        Transaction CreateFromTransactionItem(TransactionItem item)
        {

            decimal coeff = item.Typ == 1 ? -1m : 1m;

            Transaction tr = new Transaction
            {
                TransactionDate = item.DataOperacji,
                Amount = item.Kwota * coeff,
                Currency = "PLN",
                Details = "GOTOWKA",
                CounterPartData = item.Notatka,
                Title = item.Tytul,
                Owner = item.NazwaKonta,
                Category = ParseTools.ToUpper(item.Kategoria),
                SubCategory = ParseTools.ToUpper(item.PodKategoria)
            };

            return tr;


        }



    }
}
