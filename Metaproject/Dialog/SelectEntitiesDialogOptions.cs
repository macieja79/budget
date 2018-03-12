using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Dialog
{
    public class SelectEntitiesDialogOptions
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public List<string> Options { get; set; }
        public List<string> Checked { get; set; }
    }

}

