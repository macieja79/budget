using System;
using System.IO;
using System.Xml.Serialization;
using Metaproject.Patterns;

namespace Metaproject.Xml
{
    public class XmlSerializer<T> : IObjectSerializer<T>
    {
        public string Serialize(T element)
        {
            var serializer = new XmlSerializer(typeof(T));

            var str = string.Empty;

            using (TextWriter tw = new StringWriter())
            {
                serializer.Serialize(tw, element);
                str = tw.ToString();
            }

            return str;
        }

        public T Deserialize(string str)
        {
            var serializer = new XmlSerializer(typeof(T));

            var t = default(T);

            using (TextReader tw = new StringReader(str))
            {
                try
                {
                    t = (T) serializer.Deserialize(tw);
                }
                catch (Exception exc)
                {
                    return t;
                }
            }

            return t;
        }
    }
}