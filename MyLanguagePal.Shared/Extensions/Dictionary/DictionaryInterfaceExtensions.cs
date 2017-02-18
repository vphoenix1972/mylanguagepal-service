using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePal.Shared.Extensions.Dictionary
{
    public static class DictionaryInterfaceExtensions
    {
        public static void Extend<T>([NotNull] this IDictionary<string, object> dict, [NotNull] T source, [CanBeNull] DictionaryExtendOptions options = null)
        {
            if (dict == null)
                throw new ArgumentNullException(nameof(dict));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var defaultOptions = new DictionaryExtendOptions();
            if (options != null)
            {
            }
            else
            {
                options = defaultOptions;
            }

            var sourceType = typeof(T);
            var sourceProperties = sourceType.GetProperties();

            foreach (var property in sourceProperties)
            {
                if (!property.CanRead)
                    continue;

                if (options.ShouldSkip != null && options.ShouldSkip(property))
                    continue;

                dict[property.Name] = property.GetValue(source);
            }
        }
    }
}