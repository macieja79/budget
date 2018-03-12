using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.DataGrid
{
    public class GridModelRow<T>
    {

        protected T _item;

        public GridModelRow(T t)
        {
            _item = t;
        }

        public T Item
        {
            get
            {
                return _item;
            }
        }

    }
}
