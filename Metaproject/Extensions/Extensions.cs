using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class Extensions
    {

        public static string ToStringNull(this object obj, string nullValue = "")
        {
            if (null == obj)
                return nullValue;

            return obj.ToString();

        }

        public static List<string> ToStringDict<T,U>(this Dictionary<T,U> dict)
        {
            List<string>  list = new List<string>();
            foreach (T key in dict.Keys)
            {
                U val = dict[key];
                string formatted = string.Format("{0}={1}", key, val);
                list.Add(formatted);
            }

            return list;
        }


    }
}
