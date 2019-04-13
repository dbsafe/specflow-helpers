using Newtonsoft.Json.Linq;

namespace Specflow.Steps.Object.ExtensionMethods
{
    public static class JObjectExtensionMethods
    {
        public static void SetProperty(this JObject source, string propertyName, string propertyValue) => source[propertyName] = propertyValue;
        public static void SetProperty(this JObject source, string propertyName, decimal propertyValue) => source[propertyName] = propertyValue;

        public static void SetProperty(this JObject source, string propertyName, string[] propertyValue)
        {
            var prop = JArray.FromObject(propertyValue);
            source.Add(propertyName, prop);
        }
    }
}
