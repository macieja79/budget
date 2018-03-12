using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;

namespace Budget.Core.UI
{
    public interface  IReportSelector
    {
        List<TransactionReport> GetSelectedReports(List<TransactionReport> allReports);
    }
}
