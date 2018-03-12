using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metaproject.Dialog;
using Metaproject.Patterns;
using Metaproject.Patterns.Persistence;
using Metaproject.Reflection;

namespace Metaproject.Files
{
    public class FileRepository<T> : IFileRepository<T>
    {
        private readonly IFileSerializer<T> _serializer;
        private readonly IFileDialog _dialog;
        private string _path;
        private T _orginal;

        public FileRepository(T item, IFileSerializer<T> serializer, IFileDialog dialog) : this(serializer, dialog)
        {
            Item = item;
            _orginal = Activator.CreateInstance<T>();
            PropertyMapper.Instance.MapProperties(Item, _orginal, false);
        }

        public FileRepository(IFileSerializer<T> serializer, IFileDialog dialog)
        {
            _serializer = serializer;
            _dialog = dialog;
        }

        public T Item { get; private set; }

        public string Path => _path;

        public bool IsChanged
        {
            get
            {
              
                PropertyMapper.MapResult result = PropertyMapper.Instance.MapProperties(Item, _orginal, true);
                bool isChanged = !result.IsObjectEqual;
                return isChanged;
            }
        }

        public void Load(string path)
        {

            Item = _serializer.Load(path);
            _path = path;

            if (_orginal.IsNullObj())
                _orginal = Activator.CreateInstance<T>();
            
            

            PropertyMapper.Instance.MapProperties(Item, _orginal, false);
        }

        public void Save(string path)
        {

            

            _serializer.Save(Item, path);
            _path = path;
        }

        public void Save()
        {



            _serializer.Save(Item, _path);
        }


        public void Update(T item)
        {
            Item = item;
        }

        public void Load(LoadSaveDialogOptions options)
        {
            List<string> paths;
            bool isOk = _dialog.ShowLoadDialog(options, out paths);
            if (!isOk) return;
           
            string path = paths.FirstOrDefault();
            if (path.IsNull()) return;

            Load(path);

        }

        public void Save(LoadSaveDialogOptions options)
        {
            List<string> paths;
            bool isOk = _dialog.ShowSaveDialog(options, out paths);
            if (!isOk) return;

            string path = paths.FirstOrDefault();
            if (path.IsNull()) return;

            Save(path);
        }
    }
}
