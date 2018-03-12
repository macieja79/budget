using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Reflection
{
    public static class ReflectionExtensions
    {

        public static bool IsGenericList(this PropertyInfo info)
        {
            bool isList = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
            return isList;
        }

    }
}
