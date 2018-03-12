using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Import
{
    public interface ITransactionDataProvider
    {
        FileData GetTransactionData();
    }
}
