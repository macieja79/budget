using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.WinForms.TreeView
{
    public abstract class TreeViewModelItem<T> where T : TreeViewModelItem<T>
    {
        public abstract string Name { get; set; }
        public IList<TreeViewModelItem<T>> Children { get; set; }
        public abstract void RemoveTreeItem(TreeViewModelItem<T> item);


    }
}
