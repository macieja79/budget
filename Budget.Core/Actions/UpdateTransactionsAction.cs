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

    public class UpdateTransactionAction : IAction<UpdateTransactionAction.Data, ActionResult>
    {
        public class Data : ActionData
        {
            public List<TransactionReport> Reports { get; set; }
            public IExcel ExcelInstance { get; set; }
        }

        public ActionResult Execute(Data data)
        {
            data.ExcelInstance.UpdateReportCategories(data.Reports);
            return ActionResult.Ok();
        }
    }

}
