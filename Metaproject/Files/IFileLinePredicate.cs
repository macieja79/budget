using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Files
{
    public interface IFileLinePredicate
    {
        bool IsTrue(string fileLine);
        bool IsSimulation { get; }
    }
}
