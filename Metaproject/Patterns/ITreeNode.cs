using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public interface ITreeNode
    {
        string Name { get; set; }
        List<ITreeNode> GetChildren();
    }

}
