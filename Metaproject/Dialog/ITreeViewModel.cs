using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Dialog
{
    public interface ITreeViewModel
    {
        void Expand();
        void Collapse();
        void AddSelected();
        void DeleteSelected();
    }
}
