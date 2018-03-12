using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Forms
{
    public interface IStandardEventListener
    {
        void OnButtonClick(object sender, EventArgs e);
        void OnCheckedChanged(object sender, EventArgs e);
    }
}
