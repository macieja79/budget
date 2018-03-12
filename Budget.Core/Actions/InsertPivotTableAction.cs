using Metaproject.Patterns;

namespace Budget.Core.Actions
{
    public class InsertPivotTableAction : IAction<InsertPivotTableAction.Data, ActionResult>
    {
        public ActionResult Execute(Data data)
        {
            data.Excel.InsertPivotTableToSheet(-1, -1);
            return ActionResult.Ok();
        }

        public class Data : ActionData
        {
            public IExcel Excel { get; set; }
        }
    }
}