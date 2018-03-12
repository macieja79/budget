using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.DataGrid
{
    public interface IDataGridItem<T>
    {
        T GetCloned();
        void Update(T t); 
    }
}
