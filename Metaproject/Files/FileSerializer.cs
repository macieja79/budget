using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Metaproject.Patterns;

namespace Metaproject.Files
{
    public class FileSerializer<T> : IFileSerializer<T> where T : new()
    {
        private IObjectSerializer<T> _serializer;

        public FileSerializer(IObjectSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public T Load(string path)
        {
            if (!File.Exists(path))
            {
                return new T();
            }
            string objStr = File.ReadAllText(path);
            T t = _serializer.Deserialize(objStr);
            return t;
        }

        public void Save(T item, string path)
        {
            string objStr = _serializer.Serialize(item);
            File.WriteAllText(path, objStr);
        }
    }
}
