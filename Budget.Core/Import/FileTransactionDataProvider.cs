using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Budget.Core.Import;
using System.IO;

namespace Budget.Core.Import
{
    public class FileTransactionDataProvider : ITransactionDataProvider
    {


        

        public string FilePath { get; set; }

        #region ITransactionDataProvider
        public FileData GetTransactionData()
        {
            var lines = File.ReadAllLines(FilePath, Encoding.Default);
            return new FileData {Lines = lines.ToList(), Separator = ";"};
        }
        #endregion
    }
}
