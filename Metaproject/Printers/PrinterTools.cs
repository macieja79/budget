using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Text;

namespace Metaproject.Printers
{
    public class PrinterTools
    {
        public static PrinterTools Instance {get; private set;}

        static PrinterTools()
        {
            Instance = new PrinterTools();
        }

        public List<string> GetPrinters()
        {
            PrinterSettings.StringCollection stringCollection = PrinterSettings.InstalledPrinters;
            List<string> printers = new List<string>();
            foreach (string printer in stringCollection)
            {
                printers.Add(printer);
            }

            return printers;
        }

        public Dictionary<string, string> GetPrinterProperties(string printer)
        {

           
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printer);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection coll = searcher.Get();


            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (ManagementObject obj in coll)
            {
                foreach (PropertyData property in obj.Properties)
                {
                    dict[property.Name] = property.Value.ToStringNull();
                }
            }

            return dict;




        }

      

        

    }
}
