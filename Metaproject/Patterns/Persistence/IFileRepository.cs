using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Metaproject.Dialog;


namespace Metaproject.Patterns.Persistence
{
    public interface IFileRepository<T>
    {
        T Item { get; }
        void Load(string path);
        void Save(string path);
        void Save(LoadSaveDialogOptions options);
        void Save();
        void Update(T item);

        void Load(LoadSaveDialogOptions options);
        string Path { get; }
        bool IsChanged { get;  }

    }
}
