using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class ExtensionMethods
    {

        public static bool IsIndexInRange(this ICollection collection, int index)
        {
            return index >= 0 && index < collection.Count;
        }

        public static bool IsIndexOutOfRange(this ICollection collection, int index)
        {
            return !IsIndexInRange(collection, index);
        }



        public static List<T> AsList<T>(this T t)
        {
            List<T> list = new List<T>();
            list.Add(t);
            return list;
        }

        public static T[] AsArray<T>(this T t)
        {
            T[] array = new T[1];
            array[0] = t;
            return array;
        }

        public static void MoveDown<T>(this IList<T> list, T element)
        {
            int index = list.IndexOf(element);
            if (index >= list.Count - 1) return;


            T swap = list[index];
            list[index] = list[index + 1];
            list[index + 1] = swap;
        }

        public static void MoveUp<T>(this IList<T> list, T element)
        {
            int index = list.IndexOf(element);
            if (index < 1) return;

            T swap = list[index];
            list[index] = list[index - 1];
            list[index - 1] = swap;
        }


        public static T Previous<T>(this List<T> list, T element)
        {
            if (list.IndexOf(element) == 0) return default(T);
            else return (list[list.IndexOf(element) - 1]);
        }

        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            foreach (T t in elements)
                list.Add(t);
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (null == list || list.Count == 0);

        }

        public static void RemoveIfContains<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
                list.Remove(item);

        }

        public static void RemoveWhere<T>(this List<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                bool isFulfiled = predicate(list[i]);
                if (isFulfiled)
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

        }

        public static void AddIfNotContains<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);

        }

        public static List<KeyValuePair<K, V>> ToKeyValueList<K, V>(this Dictionary<K, V> dict)
        {
            List<KeyValuePair<K, V>> collection = new List<KeyValuePair<K, V>>();
            foreach (K key in dict.Keys)
            {
                collection.Add(new KeyValuePair<K, V>(key, dict[key]));
            }

            return collection;
        }
        
        public static IEnumerable<GroupResult<TElement>> GroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();

                //reduce the list recursively until zero
                var nextSelectors = groupSelectors.Skip(1).ToArray();
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult<TElement>
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            else
            {
                return null;
            }
        }

    }
}
