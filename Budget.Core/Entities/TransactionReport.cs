using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Entities
{
    public class TransactionReport
    {
        public string Name { get; set; }

        public string AccountNumber { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ParsedNumberOfTransactions { get; set; }
    }
}
