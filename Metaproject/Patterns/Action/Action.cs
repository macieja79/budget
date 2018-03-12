using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public interface IAction<D, R> where D : ActionData 
                                   where R : ActionResult
    {
        R Execute(D data);
    }
}
