using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.UI.ViewModels;
using Metaproject.Patterns.EventAggregator.Events;

namespace Budget.UI.Events
{
    public class EventTransactionDropped : AggregatedEvent
    {
        public TransactionDropDownData TransactionDropDownData { get; set; }
    }
}