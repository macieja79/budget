using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Metaproject.Files;
using Metaproject.Patterns;
using Metaproject.Patterns.Persistence;
using Rule = Budget.Core.Entities.Rule;


namespace Budget.Core.Import
{
    public interface IBudgetRepository 
    {
        IFileRepository<CategoryCollection> Categories { get; set; }
        IFileRepository<Options> Options { get; set; }
        IFileRepository<TransactionReportCollection> Reports { get; set; }
    }
}
