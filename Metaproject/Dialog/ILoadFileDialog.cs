using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Dialog
{
    public interface IFileDialog
    {
        bool ShowLoadDialog(LoadSaveDialogOptions options, out List<string> paths);
        bool ShowSaveDialog(LoadSaveDialogOptions options, out List<string> paths);
        bool ShowSelectEntitiesDialog(SelectEntitiesDialogOptions options, out List<string> selected);

        bool ShowQuestionDialog(string question, string caption);

    }
}
