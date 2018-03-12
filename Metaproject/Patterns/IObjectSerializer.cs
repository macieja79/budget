using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public interface IObjectSerializer<T>
    {
        string Serialize(T t);
        T Deserialize(string str);
    }
}
