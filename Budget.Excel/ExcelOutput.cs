using Budget.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Budget.Core.Entities;
using Metaproject.Excel;
using Metaproject.Reflection;
using System.Reflection;
using System.Runtime.InteropServices;
using Budget.Core.Events;
using Budget.Core.Excel;
using Budget.Core.Logic;
using Budget.Core.Tools;
using Metaproject.Dialog;
using Metaproject.Excel.Data;
using Metaproject.Excel.Excel;
using Metaproject.Excel.Formulas;
using Metaproject.Files;
using Metaproject.Graphic;
using Metaproject.Log;
using Metaproject.Patterns.EventAggregator;
using Metaproject.Collections;

namespace Budget.Excel
{
    public class ExcelOutput : IExcel
    {
        private readonly IEventAggregator _eventAggregator;
        private ILogger _logger;

        #region <const>

        const int COL_FIRST = 1;
        const int ROW_HEADERS = 1;
        const int ROW_FIRST_TRANSACTION = 2;


        private const string APPLICATION_ID = "METAPROJECT.BUDGET";

        #endregion

        #region <members>

        IExcelApp _app;

        #endregion

        #region <ctor>

        public ExcelOutput(IEventAggregator eventAggregator = null, ILogger logger = null)
        {
            _eventAggregator = eventAggregator;
            _logger = logger;

            if (_eventAggregator.IsNullObj()) _eventAggregator = EventAggregator.Empty;
            _eventAggregator.SubsribeEvent(this);

            if (_logger.IsNullObj()) _logger = new FakeLogger();

            Assembly assembly = Assembly.LoadFrom("Metaproject.Excel.dll");
            Type type = assembly.GetType("Metaproject.Excel.ExcelApp");
            _app = (IExcelApp) Activator.CreateInstance(type);
            _app.AttachAggregator(_eventAggregator, _logger);
        }

        #endregion

        #region <pub>

        public void Connect()
        {
            _app.Connect();
        }

        public void CreateInstance()
        {
            _app.CreateNewInstance();
        }

        #endregion

        #region <IExcel>

        public void InsertReportToNewSheet(List<TransactionReport> reports, IComparer<Transaction> transactionComparer)
        {
            //string code = Properties.Resources.VBAFunctionCode;
            //workBook.AddVbaModule(code);

            _app.SetScreenUpdating(false);

            try
            {
                IExcelWorkbook workBook = _app.GetActiveWorkbook();
                foreach (TransactionReport report in reports)
                {
                    InsertSingleReport(workBook, report, transactionComparer);
                }

                _app.SetDisplayGridlines(false);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _app.SetScreenUpdating(true);
            }

        }

        public void UpdateReportCategories(List<TransactionReport> reports)
        {
            IExcelWorkbook workBook = _app.GetActiveWorkbook();
            foreach (TransactionReport report in reports)
            {
                UpdateSingleReport(workBook, report);
            }
        }
        
        public void InsertPivotTableToSheet(int year, int month)
        {
            _app.Connect();

            IExcelWorkbook workbook = _app.GetActiveWorkbook();
            IExcelSheet sheet = workbook.GetActiveSheet();

            ExcelSheetData sheetData = sheet.GetSheetData();



            Transaction transaction = new Transaction();
            int firstCol = Transaction.GetIndex(() => transaction.Owner);
            int lastCol = Transaction.GetIndex(() => transaction.Amount);
            int numberOfRows = sheetData.Rows.Count;

            ExcelRangeInfo dataRange = new ExcelRangeInfo(1, firstCol, numberOfRows, lastCol + 1);
            ExcelRangeInfo insertRange = new ExcelRangeInfo(numberOfRows + 2, firstCol);

            string[] rows =
            {
                transaction.GetName(() => transaction.TransactionType),
                transaction.GetName(() => transaction.Category),
                transaction.GetName(() => transaction.SubCategory)
            };

            string[] columns =
            {
                transaction.GetName(() => transaction.Owner)
            };

            string[] values =
            {
                transaction.GetName(() => transaction.Amount)
            };

            PivotTableData data = new PivotTableData
            {
                Range = dataRange,
                InsertCell = insertRange,
                Columns = columns.ToList(),
                Rows = rows.ToList(),
                Values = values.ToList(),
                Name = "Test01",
                StyleName = "PivotStyleMedium2"
            };

            sheet.InsertPivotTable(data);

        }

        public void RefreshPivotTable()
        {
            _app.Connect();

            IExcelWorkbook workbook = _app.GetActiveWorkbook();
            IExcelSheet sheet = workbook.GetActiveSheet();
            sheet.RefreshPivotRable("Test");
        }

        public List<string> GetSheetNames()
        {
            IExcelWorkbook workbook = _app.GetActiveWorkbook();
            return workbook.GetWorksheetNames();
        }

        public IExcelSheet GetSheet(string sheetName)
        {
            IExcelWorkbook workbook = _app.GetActiveWorkbook();
            return workbook.GetSheet(sheetName);
        }

        public ExcelTransactionHitInfo GetTransactionHitInfo()
        {

            IExcelSheet sheet = _app.GetActiveWorkbook().GetActiveSheet();

            ExcelRangeInfo info = sheet.GetSelection();

            int selectedRow = info.Start.Row;

            Transaction transaction = GetTransaction(selectedRow);
            
            return new ExcelTransactionHitInfo {Transaction =  transaction};
        }

        Transaction GetTransaction(int rowNumber)
        {
            IExcelSheet sheet = _app.GetActiveWorkbook().GetActiveSheet();
            ExcelSheetData data = sheet.GetSheetData(1, rowNumber);


            if (data.Rows.Count < 2) return null;
            

            ExcelRowData headerRowData = data.Rows[0];
            
            ExcelRowData valueRowData = data.Rows[1];

            string separator = ";";
            string header = headerRowData.GetValuesInLine(separator);
            string values = valueRowData.GetValuesInLine(separator);

            TransactionParser parser = new TransactionParser(header, separator.FirstOrDefault());
            Transaction transaction = parser.ParseFromLine(values);
            return transaction;

        }

        public void ClearFilters()
        {

            IExcelSheet sheet = _app.GetActiveWorkbook().GetActiveSheet();
            sheet.ClearFilters();

        }

        public void SetFilter(string header, string value)
        {

            ExcelRangeInfo excelRangeInfo = GetTransactionRange();
            IExcelSheet sheet = _app.GetActiveWorkbook().GetActiveSheet();
            sheet.SetFilter(excelRangeInfo, COLUMN_CATEGORY, value);
        }

       

        #endregion

        #region <prv>

        private void InsertSingleReport(IExcelWorkbook workBook, TransactionReport report, IComparer<Transaction> transactionComparer)
        {
            ExcelSheetData sheetData = new ExcelSheetData();
            TransactionManager transactionManager = new TransactionManager();
            ColorConverter converter = new ColorConverter();

            var firstTransaction = report.Transactions.First();
            List<string> headers = Transaction.GetTransactionHeaders().ToList();
            sheetData.Rows.Add(new ExcelRowData(COL_FIRST, ROW_HEADERS, headers.ToArray()));

            int col_LAST = COL_FIRST + headers.Count - 1;
            int col_CATEGORY = COL_FIRST + headers.IndexOf(ReflectionTools.GetName(() => firstTransaction.Category));
            int col_AMOUNT = COL_FIRST + headers.IndexOf(ReflectionTools.GetName(() => firstTransaction.Amount));
            int col_ACCOUNT = COL_FIRST + headers.IndexOf(ReflectionTools.GetName(() => firstTransaction.AccountNumber));
            int col_TRANSACTION_NUMBER = COL_FIRST +
                                         headers.IndexOf(
                                             ReflectionTools.GetName(() => firstTransaction.TransactionNumber));
            int col_TRANSACTION_COMMENT = COL_FIRST + headers.IndexOf(
                                             ReflectionTools.GetName(() => firstTransaction.Comment));

            int iTransactionRow = 1;
            foreach (var transaction in report.Transactions)
            {
                iTransactionRow++;
                object[] values = ReflectionTools
                    .GetPropertyValues(transaction, headers).ToArray();
                sheetData.Rows.Add(new ExcelRowData(COL_FIRST, iTransactionRow, values));

                if (transaction.Color.IsNotNull())
                {

                    ExcelRangeInfo iRange = new ExcelRangeInfo(iTransactionRow, COL_FIRST, iTransactionRow, col_LAST);
                    ExcelFormatData colorFormat = new ExcelFormatData(iRange);
                    colorFormat.Background = (Color)converter.ConvertFromString(transaction.Color);
                    sheetData.Formats.Add(colorFormat);
                }
            }

            int row_LAST_TRANSACTION = ROW_FIRST_TRANSACTION + report.Transactions.Count - 1;
            int row_SUBTOTAL = row_LAST_TRANSACTION + 1;


            int row_SummaryTable = row_SUBTOTAL + 2;
            iTransactionRow = row_SummaryTable;

            Tree<string> categoryTree = GetCategoryTree(report.Transactions);
            foreach (var categoryItem in categoryTree.Children)
            {
                string category = categoryItem.Value;
                var subCategories = categoryItem.Children.Select(i => i.Value);

                foreach (string subCategory in subCategories)
                {
                    ExcelCellData categoryCell = new ExcelCellData(iTransactionRow, col_CATEGORY);
                    categoryCell.Value = category;

                    ExcelCellData subCategoryCell = new ExcelCellData(iTransactionRow, COLUMN_SUB_CATEGORY);
                    subCategoryCell.Value = subCategory;

                    

                }

            }


            ExcelRangeInfo transactionsRange =
                new ExcelRangeInfo(ROW_FIRST_TRANSACTION, col_CATEGORY, row_LAST_TRANSACTION, col_AMOUNT);

            ExcelRangeInfo subTotalRange = new ExcelRangeInfo(ROW_FIRST_TRANSACTION, col_AMOUNT, row_LAST_TRANSACTION, col_AMOUNT);
            ExcelFormula subTotalFormula = ExcelFormulaHelper.GetSubTotalSum(row_SUBTOTAL, col_AMOUNT, subTotalRange);
            sheetData.Formulas.Add(subTotalFormula);
            ExcelBorderData subTotalBorderData = new ExcelBorderData(subTotalFormula.Range);
            subTotalBorderData.Borders.AddBorder(ExcelBordersIndex.xlAround, ExcelBorderWeight.xlMedium, ExcelLineStyle.xlContinuous);
            sheetData.Borders.Add(subTotalBorderData);

            ExcelBorderData borderTable = new ExcelBorderData(COL_FIRST, ROW_HEADERS, col_LAST, row_LAST_TRANSACTION);
            borderTable.Borders.Add(new ExcelBorderItem(ExcelBordersIndex.xlInside, ExcelBorderWeight.xlHairline, ExcelLineStyle.xlContinuous));
            borderTable.Borders.Add(new ExcelBorderItem(ExcelBordersIndex.xlAround, ExcelBorderWeight.xlMedium, ExcelLineStyle.xlContinuous));
            sheetData.Borders.Add(borderTable);

            ExcelRangeInfo columnAccountRange = ExcelRangeInfo.CreateColumnRange(col_ACCOUNT);
            ExcelFormatData columnAccountFormat = new ExcelFormatData(columnAccountRange);
            columnAccountFormat.NumberFormat = "@";
            sheetData.Formats.Add(columnAccountFormat);

            ExcelRangeInfo columnTransactionNumber = ExcelRangeInfo.CreateColumnRange(col_TRANSACTION_NUMBER);
            ExcelFormatData columnTransactionFormat = new ExcelFormatData(columnTransactionNumber);
            columnTransactionFormat.NumberFormat = "0";
            sheetData.Formats.Add(columnTransactionFormat);

            IExcelSheet sheet = workBook.CreateSheet(report.Name);
            sheet.InsertSheetData(sheetData);

            int columnCount = headers.Count;
            sheet.SetColumnsAutoFit(1, columnCount);
            sheet.SetAutoFilter(ROW_HEADERS, COL_FIRST, ROW_HEADERS, col_LAST);
            
        }

        private Tree<string> GetCategoryTree(List<Transaction> reportTransactions)
        {
            Tree<string> tree = new Tree<string>();
            List<string> categories = reportTransactions
                .Select(t => t.Category)
                .Distinct()
                .ToList();

            foreach (string category in categories)
            {
                Tree<string> categoryItem = new Tree<string>(category);
                categoryItem.AddChildren(
                    reportTransactions
                        .Where(t => t.Category == category)
                        .Select(t => t.SubCategory)
                        .Distinct()
                        .ToArray()
                    );
            }

            return tree;
        }

        private void UpdateSingleReport(IExcelWorkbook workBook, TransactionReport report)
        {
            ExcelSheetData sheetData = new ExcelSheetData();
            List<string> headers = Transaction.GetTransactionHeaders().ToList();
            ColorConverter converter = new ColorConverter();
            int col_LAST = COL_FIRST + headers.Count - 1;
            Transaction template = new Transaction();
            int col_CATEGORY = COL_FIRST + headers.IndexOf(ReflectionTools.GetName(() => template.Category));
            int col_SUB_CATEGORY = COL_FIRST + headers.IndexOf(ReflectionTools.GetName(() => template.SubCategory));

            int iTransactionRow = 2;
            foreach (var transaction in report.Transactions)
            {
                ExcelCellData categoryData = new ExcelCellData(iTransactionRow, col_CATEGORY);
                categoryData.Value = transaction.Category;
                sheetData.Cells.Add(categoryData);

                ExcelCellData subCategoryData = new ExcelCellData(iTransactionRow, col_SUB_CATEGORY);
                subCategoryData.Value = transaction.SubCategory;
                sheetData.Cells.Add(subCategoryData);

                Color color = Color.White;
                if (transaction.Color.IsNotNull())
                {
                    color = (Color)converter.ConvertFromString(transaction.Color);
                }

                ExcelRangeInfo iRange = new ExcelRangeInfo(iTransactionRow, COL_FIRST, iTransactionRow, col_LAST);
                ExcelFormatData colorFormat = new ExcelFormatData(iRange);
                colorFormat.Background = color;
                sheetData.Formats.Add(colorFormat);

                iTransactionRow++;
            }

            IExcelSheet sheet = workBook.GetSheet(report.Name);
            sheet.InsertSheetData(sheetData);
          
        }
        #endregion

        private int COLUMN_CATEGORY = 4;
        private int COLUMN_SUB_CATEGORY = 5;
            
      

        int GetLastRowNumber()
        {
            IExcelSheet sheet = _app.GetActiveWorkbook().GetActiveSheet();
            for (int r = ROW_FIRST_TRANSACTION;; r++)
            {
                var value = sheet.GetValue(r, COL_FIRST);
                if (value.IsNullObj())
                    return r - 1;
            }

            return -1;
        }

        ExcelRangeInfo GetTransactionRange()
        {
            int firstRow = ROW_FIRST_TRANSACTION;
            int lastRow = GetLastRowNumber();
            int firstColumn = COL_FIRST;

            int headersCount = Transaction.GetTransactionHeaders().Length;
            int lastColumn = firstColumn + headersCount - 1;

            return new ExcelRangeInfo(firstRow, firstColumn, lastRow, lastColumn);


        }

      
    }
}
