using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Controls
{
    public class DoubleBufferedGrid : DataGridView
    {
        public DoubleBufferedGrid()
        {
            DoubleBuffered = true;
       }


    }
}
