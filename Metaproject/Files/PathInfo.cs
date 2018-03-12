using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Metaproject.Strings;

namespace Metaproject.Files
{
    [DebuggerDisplay("{FullPath}")]
    public class PathInfo
    {


        public enum PathType
        {
            NotExists,
            File, 
            Directory
        }


        private readonly List<string> _itemsList;

        public PathInfo(string path)
        {
            _itemsList = GetSplited(path);
        }
		
        public string FullPath => string.Join(Path.DirectorySeparatorChar.ToString(), _itemsList);
        
        public bool IsExist => File.Exists(FullPath);

        public string FileName
        {
            get
            {
                if (TypeOfPath != PathType.File) return null;
                string fileName = _itemsList.Last();
                return fileName;
            }
        }

        public string DirectoryName
        {
            get
            {
                if (TypeOfPath == PathType.Directory)
                    return FullPath;

                if (TypeOfPath == PathType.NotExists) return null;

                var directoryItems = _itemsList.GetRange(0, _itemsList.Count - 1);

                return string.Join(Path.DirectorySeparatorChar.ToString(), directoryItems);

            }


        }



        public PathInfo Down(int count)
        {
            for (var i = 0; i < count; i++)
                Down();

            return this;
        }

        public PathInfo Down()
        {
            _itemsList.RemoveAt(_itemsList.Count - 1);
            return this;
        }

        public PathInfo Up(string pathPart)
        {
            var items = GetSplited(pathPart);
            _itemsList.AddRange(items);
            return this;
        }

        private List<string> GetSplited(string path)
        {
            char[] separators = {Path.DirectorySeparatorChar, '/', '\\'};
            var items = path.Split(separators).ToList();
            return items;
        }

        public PathType TypeOfPath
        {
            get
            {
                string fullPath = FullPath;
                if (File.Exists(fullPath)) return PathType.File;
                if (Directory.Exists(fullPath)) return PathType.Directory;
                return PathType.NotExists;
            }
        }

        public FileNameInfo GetFileName()
        {
            if (TypeOfPath != PathType.File) return null;
            string fileName = FileName;

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extensionWithDot = Path.GetExtension(fileName);
            string extension = extensionWithDot.Replace(".", "");

            return new FileNameInfo
            {
                Directory = DirectoryName,
                Name = nameWithoutExtension,
                Extension = extension
            };
        }
    }
}
