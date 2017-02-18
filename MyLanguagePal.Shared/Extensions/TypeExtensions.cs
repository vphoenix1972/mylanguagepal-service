using System;
using JetBrains.Annotations;

namespace MyLanguagePal.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSameOrSubclass([NotNull] this Type type, [NotNull] Type other)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return type.IsSubclassOf(other) ||
                   type == other;
        }
    }
}