using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace MyLanguagePal.Shared.AssemblyExtensions
{
    public static class AssemblyExtensions
    {
        [CanBeNull]
        public static string GetAssemblyVersion([NotNull] this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (assembly.Location == null)
                return null; // Assembly is loaded from Byte[] (memory)                             

            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion;
        }
    }
}