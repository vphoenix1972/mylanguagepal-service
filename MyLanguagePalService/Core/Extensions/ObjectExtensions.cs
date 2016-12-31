using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLanguagePalService.Core.Extensions
{
    public static class ObjectExtensions
    {
        [NotNull]
        public static string ToJson(this object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        [CanBeNull]
        public static T FromJObjectTo<T>([CanBeNull] this object obj)
            where T : class
        {
            var tobj = obj as T;
            if (tobj != null)
                return tobj;

            var jobj = obj as JObject;
            if (jobj != null)
                return jobj.ToObject<T>();

            var jarr = obj as JArray;
            if (jarr != null)
                return jarr.ToObject<T>();

            return null;
        }

        public static string GetClassName(this object obj)
        {
            return obj.GetType().Name;
        }
    }
}