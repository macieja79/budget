using Metaproject.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Actions
{
    public class RefreshPivotTableAction : IAction<RefreshPivotTableAction.Data, ActionResult>
    {
        public class Data : ActionData
        {
            public IExcel Excel { get; set; }
        }

        public ActionResult Execute(Data data)
        {
            data.Excel.RefreshPivotTable();

            return ActionResult.Ok();
        }
    }
}
