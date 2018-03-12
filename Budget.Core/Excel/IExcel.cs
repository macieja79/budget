using Budget.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Excel;
using Metaproject.Excel;

namespace Budget.Core
{
    public interface IExcel
    {
        void InsertReportToNewSheet(List<TransactionReport> reports, IComparer<Transaction> transationComparer);
        void UpdateReportCategories(List<TransactionReport> reports);
        void InsertPivotTableToSheet(int year, int month);
        void RefreshPivotTable();
        List<string> GetSheetNames();
        IExcelSheet GetSheet(string sheetName);
        ExcelTransactionHitInfo GetTransactionHitInfo();
        void ClearFilters();
        void SetFilter(string header, string value);
        
    }
}
