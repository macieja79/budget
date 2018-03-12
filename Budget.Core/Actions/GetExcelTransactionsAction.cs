using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Budget.Core.Import;
using Metaproject.Excel;
using Metaproject.Patterns;

namespace Budget.Core.Actions
{
    public class GetExcelTransactionsAction : IAction<GetExcelTransactionsAction.Data, GetTransactionsAction.Result>
    {
        public class Data : ActionData
        {
            public List<string> SheetFileNames { get; set; }
            public IExcel Excel { get; set; }
            public List<ITransactionParser> Parsers { get; set; }
            public string Separator { get; set; }
        }

        public GetTransactionsAction.Result Execute(Data data)
        {
            List<FileData> fileDatas = new List<FileData>();

            string separator = data.Separator ?? ";";

            foreach (string sheetName in data.SheetFileNames)
            {
                IExcelSheet sheet = data.Excel.GetSheet(sheetName);
                ExcelTransactionDataProvider excelDataProvider = new ExcelTransactionDataProvider(sheet, separator);
                FileData iFileData = excelDataProvider.GetTransactionData();
                fileDatas.Add(iFileData);
            }

            GetTransactionsAction.Data getTransactionData = new GetTransactionsAction.Data()
            {
                FileDatas = fileDatas,
                Parsers = data.Parsers
            };

            GetTransactionsAction transactionAction = new GetTransactionsAction();
            GetTransactionsAction.Result result = transactionAction.Execute(getTransactionData);
            return result;
        }
    }
}