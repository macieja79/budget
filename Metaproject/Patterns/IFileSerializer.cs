using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public interface IFileSerializer< T>
    {
        void Save(T item, string path);
        T Load(string path);
    }
}
