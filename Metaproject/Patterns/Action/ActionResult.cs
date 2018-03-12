using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public class ActionResult
    {

        public static ActionResult Ok()
        {
            return new ActionResult() {IsOk = true};
        }

        public bool IsOk { get; set; }
    }
}
