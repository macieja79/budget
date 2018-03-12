using Budget.Core.Entities;
using Budget.Core.Import;
using Metaproject.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Actions
{

    public class GetTransactionsAction : IAction<GetTransactionsAction.Data, GetTransactionsAction.Result>
    {

        public class Data : ActionData
        {
            public List<ITransactionParser> Parsers { get; set; }
            public List<FileData> FileDatas { get; set; }
        }

        public class Result : ActionResult
        {
            public List<TransactionReport> Reports { get; set; }
        }

        public Result Execute(Data data)
        {
            Result actionResult = new Result() {Reports = new List<TransactionReport>()};
            
            foreach (FileData fileData in data.FileDatas)
            {
                TransactionReport report;
                foreach (ITransactionParser parser in data.Parsers)
                {
                    bool isParsed = parser.TryGetMonthReports(fileData, out report);
                    if (isParsed)
                    {
                        actionResult.Reports.Add(report);
                        break;
                    }
                }
            }

            return actionResult;

        }

    }

}
