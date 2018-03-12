using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public class LogReportIO
    {
        
        #region <singleton>

        LogReportIO() { }

        static LogReportIO _instance;

        public static LogReportIO Instance
        {
            get
            {
                if (null == _instance) _instance = new LogReportIO();
                return _instance;
            }
        }   

        #endregion

        #region <pub>
        public LogReportPack CreateFromLines(string[] lines)
        {
            LogReportPack pack = new LogReportPack();
            LogReport parsedReport = null;
            LogReport currentReport = null; 
            
            LogReportItem item = null;
            
            foreach (string line in lines)
            {
                if (LogReport.TryCreateFromHeader(line, out parsedReport))
                {
                    currentReport = parsedReport;
                    pack.Reports.Add(currentReport);
                    continue;
                }

                if (LogReportItem.TryCreateFromString(line, out item))
                {
                    if (null != currentReport)
                    {
                        currentReport.Items.Add(item);
                    }
                }
            }

            return pack;

        }

        public LogReportPack CreateFromFile(string path)
        {
            if (!File.Exists(path)) return null;

            string[] lines = File.ReadAllLines(path);

            LogReportPack pack = CreateFromLines(lines);

            return pack;

        }

        public string[] GetAsLines(LogReportPack reportPack)
        {
            List<string> lines = new List<string>();
            foreach (LogReport report in reportPack.Reports)
            {

                lines.Add(report.GetHeader());
                foreach (LogReportItem item in report.Items)
                {
                    lines.Add(item.GetAsLine());
                }
            }

            return lines.ToArray();

        }

        public void SaveToFile(LogReportPack pack, string path)
        {
            string[] lines = GetAsLines(pack);
            File.WriteAllLines(path, lines);
        }

        #endregion

    }
}
