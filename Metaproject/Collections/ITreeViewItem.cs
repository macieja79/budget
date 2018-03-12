using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace Metaproject.Collections
{
    public interface ITreeViewItem
    {
        string TreeDisplayName{ get; set; }
        IList<ITreeViewItem> Children { get;  }
        void RemoveTreeItem(ITreeViewItem item);
        void AddItem(ITreeViewItem item);
        void AddItem(int index, ITreeViewItem item);
    }

}
