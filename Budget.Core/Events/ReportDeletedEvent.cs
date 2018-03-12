using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Events
{
    public class ReportDeletedEvent
    {
        public string ReportName { get; set; }
    }
}
