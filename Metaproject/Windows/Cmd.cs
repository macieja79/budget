using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Windows
{
    public class Cmd : ICmd
    {
        public void ExecuteScript(string script)
        {
            string tempPath = @"c:\Temp\scr.bat";
            File.WriteAllText(tempPath, script);
            Process.Start(tempPath);
        }
    }
}
