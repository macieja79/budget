using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns.EventAggregator.Events
{
    public class AggregatedEvent
    {
        public string Name { get; set; }
        public bool IsHandled { get; set; }
    }
}
