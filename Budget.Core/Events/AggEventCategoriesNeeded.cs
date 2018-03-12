using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Metaproject.Patterns.EventAggregator.Events;

namespace Budget.Core.Events
{
    public class AggEventCategoriesNeeded : AggregatedEvent
    {
        public CategoryCollection Categories { get; set; }
    }
}
