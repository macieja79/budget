using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class EnumExtensions
    {
        public static bool HasValue(this Enum item, params Enum[] items)
        {
            return items.Any(i => i.Equals(item));
        }

    }
}
