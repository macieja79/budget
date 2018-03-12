using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;

namespace Budget.UI.ViewModels
{
    public class TransactionDropDownData
    {
        public Transaction Transaction { get; set; }
        public string PickedProperty { get; set; }
    }

}
