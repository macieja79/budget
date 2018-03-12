using System.Collections.Generic;

namespace Metaproject.Database
{
    public interface IDatabaseTreeElement
    {
        string Name { get; set; }
        List<IDatabaseTreeElement> Children { get; }
    }
}