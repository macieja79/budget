using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Patterns
{
    public interface ILineParser<out T>
    {
        T ParseFromLine(string line);
    }
}
