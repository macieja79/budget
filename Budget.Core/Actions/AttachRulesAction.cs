using Budget.Core.Entities;
using Metaproject.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Actions
{
    public class AttachRulesAction : IAction<AttachRulesAction.Data, ActionResult>
    {
        public class Data : ActionData
        {

            public List<Transaction> Transactions { get; set; }
         
            public CategoryCollection Categories { get; set; }

            public bool IsToOverrideExisting { get; set; }
        }



        public ActionResult Execute(Data data)
        {
            foreach (var transaction in data.Transactions)
            {
                
                data.Categories.AttachCategoryName(transaction, data.IsToOverrideExisting);
            }

            return ActionResult.Ok();
        }
    }
}
