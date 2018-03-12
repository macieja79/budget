using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Dialog
{
    public interface ISaveFileDialog
    {
        bool ShowSaveDialog(LoadSaveDialogOptions options, out List<string> paths);
    }
}
