using System;
using System.Reflection;
using JetBrains.Annotations;

namespace MyLanguagePal.Shared.Extensions.Dictionary
{
    public class DictionaryExtendOptions
    {
        [CanBeNull]
        public Func<PropertyInfo, bool> ShouldSkip { get; set; }

        public static DictionaryExtendOptions CreateIsSameOrSubClass([NotNull] Type baseClass)
        {
            if (baseClass == null)
                throw new ArgumentNullException(nameof(baseClass));

            return new DictionaryExtendOptions()
            {
                ShouldSkip = info => info.DeclaringType == null || !info.DeclaringType.IsSameOrSubclass(baseClass)
            };
        }
    }
}