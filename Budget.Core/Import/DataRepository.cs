using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Metaproject.Files;
using Metaproject.Patterns.Persistence;

namespace Budget.Core.Import
{
    public class BudgetRepository : IBudgetRepository
    {
        public IFileRepository<CategoryCollection> Categories { get; set; }
        public IFileRepository<Options> Options { get; set; }
        public IFileRepository<TransactionReportCollection> Reports { get; set; }
    }
}
