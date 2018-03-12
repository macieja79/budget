using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Files
{
    public class FileNameInfo
    {
        public string FullName => $"{Name}.{Extension}";

        public string Name { get; set; }
        public string Extension { get; set; }

        public string Directory { get; set; }


    }
}
