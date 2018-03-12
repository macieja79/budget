using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Progress
{
    public interface IProgressDouble : IProgress
    {
        void Set2(string caption, int steps);
        void Step2(string caption);    
    }
}
