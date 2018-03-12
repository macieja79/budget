using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Core.Import;

namespace Budget.UI
{
    public class TestDataProvider : ITransactionDataProvider
    {
        private readonly string _rawData;
        private readonly string _separator;

        public TestDataProvider(string rawData, string separator)
        {
            _rawData = rawData;
            _separator = separator;
        }

        public FileData GetTransactionData()
        {
            var list = _separator.ToList().ToArray();
            List<string> lines = _rawData.Split(list, StringSplitOptions.None).ToList();
            return new FileData() {Lines = lines};
        }
    }
}
