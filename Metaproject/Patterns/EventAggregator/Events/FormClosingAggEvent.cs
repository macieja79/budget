using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Patterns.EventAggregator.Events
{
   public class ApplicationClosingAggEvent : AggregatedEvent
   {

       public bool IsCancel { get; set; }
   }
}
