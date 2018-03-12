using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Import
{
    public interface IImport
    {
        List<ITransactionParser> GetParsers();
    }
}
