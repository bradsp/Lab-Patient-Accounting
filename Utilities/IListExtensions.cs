using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    /// Extensions for generic Lists
    /// </summary>
    public static class IListExtension
    {
        /// <summary>
        /// Add a list of items to another List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }
    }

}
