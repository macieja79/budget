using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public class LogReportManager : ILogger
    {

        #region <ctor>
        public LogReportManager()
        {
            Reports = new List<LogReport>();
        }
        #endregion

        #region <members>
        LogReport _current;
        #endregion

        #region <props>
        public List<LogReport> Reports { get; private set; }

        public LogReport Current
        {
            get
            {
                if (null == _current) return Reports.LastOrDefault();
                return _current;
            }
            set
            {
                _current = value;
            }
        }
        #endregion

        #region <pub>
        public void ClearAllReports()
        {
            Reports.ForEach(r => r.Clear());
            Reports.Clear();
        }

        public LogReportPack GetReportPack()
        {
            LogReportPack pack = new LogReportPack();
            pack.Reports = Reports;
            return pack;
        }

        #endregion

        #region <ILogger>
        public void Log(string message)
        {
            Log(message, 0);
        }

        public void Log(string message, int level)
        {
            Log(message, level);

        }

        public void Log(Exception exception)
        {
            Log(exception.Message, 0, LogReportItemType.Exception);
        }

        public void Log(string message, int level, LogReportItemType type)
        {
            LogReportItem item = new LogReportItem()
            {
                Date = DateTime.Now,
                Level = level,
                Message = message,
                TypeOfLogItem = type
            };

            if (null != Current)
                Current.Items.Add(item);
        }

        public void LogItems(params object[] items)
        {
            throw new NotImplementedException();
        }

        #endregion



        
    }
}
