using Metaproject.Patterns;
using Newtonsoft.Json;

namespace Metaproject.JSON
{
    public class JsonSerializer<T> : IObjectSerializer<T>
    {
        public string Serialize(T element)
        {
            var str = JsonConvert.SerializeObject(element, Formatting.Indented);
            return str;
        }

        public T Deserialize(string xmlStr)
        {
            var t = JsonConvert.DeserializeObject<T>(xmlStr);
            return t;
        }
    }
}