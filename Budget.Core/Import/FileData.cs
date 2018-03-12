using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Import
{
    public class FileData
    {
        public string Name { get; set; }
        public List<string> Lines { get; set; }
        public string Separator { get; set; }
    }
}
