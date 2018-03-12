using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Metaproject.Patterns.EventAggregator.Events;

namespace Metaproject.WinForms
{
    public class ComboValueSelectedAggEvent : AggregatedEvent
    {

        public object EditedObject { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

}
