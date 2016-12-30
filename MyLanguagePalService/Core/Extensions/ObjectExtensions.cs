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
            var jobj = (JObject)obj;

            return jobj?.ToObject<T>();
        }

        public static string GetClassName(this object obj)
        {
            return obj.GetType().Name;
        }
    }
}