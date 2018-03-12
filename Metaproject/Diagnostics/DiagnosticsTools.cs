using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Metaproject.Diagnostics
{
    public class DiagnosticsTools
    {
        
        #region <singleton>

        DiagnosticsTools() { }

        static DiagnosticsTools _instance;

        public static DiagnosticsTools Instance
        {
            get
            {
                if (null == _instance) _instance = new DiagnosticsTools();
                return _instance;
            }
        }   

        #endregion

        public bool IsProcesRunning(string processName)
        {

            string name = processName.ToLower().Trim();

            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                string iName = p.ProcessName.ToLower().Trim();
                if (iName == name)
                {
                    return true;
                }
            }

            return false;  

        }

    
    }
}
