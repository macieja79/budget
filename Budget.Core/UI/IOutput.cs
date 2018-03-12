using Budget.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Import;
using Metaproject.Dialog;
using Metaproject.Patterns.Persistence;

namespace Budget.Core.UI
{
    public interface IOutput
    {
        void ShowReports(List<TransactionReport> reports);
        bool EditRules(List<Rule> rules);
        bool EditCategories(IFileRepository<CategoryCollection> categories, out CategoryCollection edited, out string path);

        IFilePathProvider FilePathProvider { get; set; }
        ITransactionParser TransactionParser { get; set; }
        IFileDialog FileDialog { get; set; }

        void AttachMenu(MenuItem menu);
        void ClearCurrentItem();
        Transaction GetSelectedTransaction();
        void Refresh();
    }
}
