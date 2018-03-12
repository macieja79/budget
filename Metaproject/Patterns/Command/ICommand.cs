using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Patterns.Command
{
    public interface ICommand
    {
        void Run();
        string ID { get; }
    }
}
