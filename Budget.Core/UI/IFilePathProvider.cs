using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.UI
{
    public interface IFilePathProvider
    {
        List<string> GetFilePaths();
    }
}
