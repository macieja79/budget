using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public class FakeLogger : ILogger
    {

        #region <singleton>

        // FakeLogger() { }

        static FakeLogger _instance;

        public static FakeLogger Instance
        {
            get
            {
                if (null == _instance) _instance = new FakeLogger();
                return _instance;
            }
        }   

        #endregion




        public void Log(string message)
        { }

        public void Log(string message, int level)
        { }

        public void Log(Exception exception)
        { }

        public void Log(string message, int level, LogReportItemType type)
        { }

        public void LogItems(params object[] items)
        {
            throw new NotImplementedException();
        }
    }
}
