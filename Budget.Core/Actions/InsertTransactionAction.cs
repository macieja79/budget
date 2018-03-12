using Budget.Core.Entities;
using Metaproject.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Actions
{
    public class InsertTransactionAction : IAction<InsertTransactionAction.Data, ActionResult>
    {
        public class Data : ActionData
        {
            public List<TransactionReport> Reports { get; set; }
            public IExcel ExcelInstance { get; set; }
            public IComparer<Transaction> TransactionComparer { get; set; }
        }

        public ActionResult Execute(Data data)
        {
            data.ExcelInstance.InsertReportToNewSheet(data.Reports, data.TransactionComparer);
            return ActionResult.Ok();
        }
    }
}
