using System;
using System.Collections.Generic;

namespace MyLanguagePal.Shared.Extensions.Enumerable.Manipulation
{
    public static class EnumerableManipulationExtensions
    {
        /// <summary>
        /// <para>Performs a cast to ICollection.</para>
        /// <para>Throws ArgumentNullException if collection is null.</para>
        /// <para>Throws ArgumentException if collection cannot be casted.</para>
        /// </summary>        
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var result = collection as ICollection<T>;
            if (result == null)
                throw new ArgumentException($"Collection '{collection.GetType()}' cannot be cast to '{typeof(ICollection<T>)}'");

            return result;
        }
    }
}