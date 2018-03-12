using Budget.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Entities;
using Budget.Core.Import;
using Metaproject.Dialog;
using Metaproject.Patterns.Persistence;

namespace Budget.UI
{
    public class ReportDisplay : IOutput
    {
        public void ShowReports(List<TransactionReport> reports)
        {
            FormReportMain form = new FormReportMain(null, null);
            
        }

        public bool EditRules(List<Rule> rules)
        {
            return true;       
        }

        public bool EditCategories(IFileRepository<CategoryCollection> categories, out CategoryCollection edited, out string path)
        {
            throw new NotImplementedException();
        }

        public bool EditCategories(CategoryCollection categories, out CategoryCollection edited, out string path)
        {
            throw new NotImplementedException();
        }

        public IFilePathProvider FilePathProvider { get; set; }
        public IRulesRepository RulesProvider { get; set; }
        public ITransactionParser TransactionParser { get; set; }
        public IFileDialog FileDialog { get; set; }
        public void AttachMenu(MenuItem menu)
        {
           
        }

        public void ClearCurrentItem()
        {
            throw new NotImplementedException();
        }

        public Transaction GetSelectedTransaction()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
