using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.Core.Extensions
{
    public static class ListExtensions
    {
        public static void AssertListAndItemsAreNotNull<T>([NotNull] this IList<T> list, [NotNull] string listName = "list")
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (listName == null)
                throw new ArgumentNullException(nameof(listName));

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == null)
                    throw new ArgumentNullException($"{listName}[{i}]");
            }
        }
    }
}