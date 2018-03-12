using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metaproject.Collections;

namespace Metaproject.WinForms.TreeView
{
    public class TreeViewModelEvent
    {
        public enum ActionType
        {
            Selected,
            Removed
        }

        public ActionType TypeOfAction { get; set; }
        public ITreeViewItem Item { get; set; }
    }
}
