using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using Metaproject.Reflection;

namespace System
{
    public static class StringExtensionMethods
    {


        public static List<string> GetNames(this object obj, params Expression<Func<object>>[] expressions)
        {
            List<string> names = new List<string>();
            foreach (var exp in expressions)
            {
                string name = ReflectionTools.GetName(exp);
                names.Add(name);
            }

            return names;
        }


        public static string GetName(this object obj, Expression<Func<object>> exp)
        {
            string name = ReflectionTools.GetName(exp);
            return name;

        }

    public static List<string> ToList (this StringCollection collection)
        {
        
            List<string> list = new List<string>();
            foreach (string ploter in collection)
                list.Add(ploter);

            return list;
        }

        

        public static string GetWithNoPolishLetters(this string str)
        {

            string without = str
            .Replace('ą', 'a')
            .Replace('Ą', 'A')
            .Replace('ć', 'c')
            .Replace('Ć', 'C')
            .Replace('ę', 'e')
            .Replace('Ę', 'E')
            .Replace('ł', 'l')
            .Replace('Ł', 'L')
            .Replace('ń', 'n')
            .Replace('Ń', 'N')
            .Replace('ó', 'o')
            .Replace('Ó', 'O')
            .Replace('ś', 's')
            .Replace('Ś', 'S')
            .Replace('ż', 'z')
            .Replace('Ż', 'Z')
            .Replace('ź', 'z')
            .Replace('Ź', 'Z');

            return without;

        }

        public static string[] SplitAndTrim(this string str, params char[] separator)
        {

            string[] items = str.Split(separator);
            string[] trimmed = new string[items.Length];
            for (int i = 0; i < items.Length; ++i)
            {
                trimmed[i] = items[i].Trim();
            }

            return trimmed;
        }


        public static bool In(this int value, params int[] array)
        {
            return (array.Contains(value));
        }

        public static bool IsNull (this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNull(this string[] list)
        {
            return (null == list || list.Length == 0);
        }

        public static string ToEmptyIfNull(this string str)
        {
            if (str == null) return "";
            return str;
        }

        public static List<string> SplitWithManySeparators(this string str, params char[] separators)
        {

            if (str.IsNull()) return new List<string>();

            int matched = 0;
            char matchedSeparator = char.MinValue;
            foreach (char separator in separators)
            {
                if (str.Contains(separator.ToString()))
                {
                    matched++;
                    matchedSeparator = separator;
                }
            }

            if (matched != 1)
            {
                return new List<string>() { str };
            }

            string[] items = str.Split(matchedSeparator);
            return items.ToList();
        }
    }

    
}
