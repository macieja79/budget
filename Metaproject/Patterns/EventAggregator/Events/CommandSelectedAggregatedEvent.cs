using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns.EventAggregator.Events
{
    public class CommandSelectedAggregatedEvent
    {
        public static CommandSelectedAggregatedEvent Create(string id)
        {
            return new CommandSelectedAggregatedEvent() {CommandId = id};
        }

        public string CommandId { get; set; }
    }
}
