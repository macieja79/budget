using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string message, int level);
        void Log(Exception exception);
        void Log(string message, int level, LogReportItemType type);
        void LogItems(params object[] items);
    }
}
