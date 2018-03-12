using Budget.Core.Entities;
using Metaproject.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Logic
{
    public class TransactionManager
    {

        public List<TransactionReport> GetMonthReports(List<TransactionReport> allReports)
        {
            
            List<Transaction> allTransactions = allReports
                .SelectMany(r => r.Transactions)
                .ToList();


            return GetMonthReports(allTransactions);

        }

        public List<TransactionReport> GetMonthReports(List<Transaction> allTransactions)
        {

            IEnumerable<IGrouping<int, Transaction>> transactionByMonth = allTransactions
                .GroupBy(t => t.TransactionDate.Month);

            int numberOfGroups = transactionByMonth.Count();

            List<TransactionReport> reports = transactionByMonth
                .Select(g => new TransactionReport() { Transactions = g.ToList() })
                .ToList();

            foreach (TransactionReport report in reports)
            {
                Transaction first = report.Transactions.FirstOrDefault();
                if (null != first)
                {
                    report.Name = GetReportName(first);
                    SortTransactionsByDate(report.Transactions);
                }
            }

            SortReportsByDate(reports);

            return reports;

        }

  

        public void RemoveInternalTransactions(List<TransactionReport> reports)
        {
            reports.ForEach(RemoveInternalTransaction);
        }

        public void RemoveInternalTransaction(TransactionReport report)
        {
            report.Transactions.RemoveWhere(r => r.Category == BudgetConst.CategoryInternal);
        }


   

        public string GetReportName(Transaction transaction)
        {
            int monthNumber = transaction.TransactionDate.Month;
            string monthName = DateTimeTools.GetMonthName(monthNumber).GetWithNoPolishLetters();

            int year = transaction.TransactionDate.Year;

            string sheetName = string.Format("{0} {1}", monthName, year);
            return sheetName;
        }

        class ReportComparer : IComparer<TransactionReport>
        {
            public int Compare(TransactionReport x, TransactionReport y)
            {
                Transaction first = x.Transactions.FirstOrDefault();
                Transaction second = y.Transactions.FirstOrDefault();

                if (null != first && null != second)
                {
                    return first.TransactionDate.CompareTo(second.TransactionDate);
                }
                
                return 0;
            }
        }

        class TransactionDateComparer : IComparer<Transaction>
        {
            public int Compare(Transaction x, Transaction y)
            {
                return x.TransactionDate.CompareTo(y.TransactionDate);
            }
        }

        void SortTransactionsByDate(List<Transaction> transactions)
        {
            transactions.Sort(new TransactionDateComparer());
        }

        private void SortReportsByDate(List<TransactionReport> reports)
        {
            reports.Sort(new ReportComparer());
        }




    }
}
