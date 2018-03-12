using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Metaproject.Log
{

    public class OutputLogger : ILogger
    {

        public void Log(string message)
        {
            try
            {
                Debug.WriteLine(message);
            }
            catch (Exception)
            {
                /* ignored */
            }
        }

        public void Log(Exception exception)
        {
            try
            {
                if (exception == null) return;
                Log(exception.Message);
            }
            catch (Exception)
            {
                /* ignored */
            }
        }

        public void Log(string message, int level)
        {
            try
            {
                Debug.WriteLine(message);
            }
            catch (Exception)
            {
                /* ignored */
            }
        }

        public void Log(string message, int level, LogReportItemType type)
        {
            
        }

        public void LogItems(params object[] items)
        {
            try
            {
                if (null == items) return;
                List<string> strs = new List<string>();
                foreach (object obj in items)
                {
                    if (null == obj) continue;
                    string str = obj.ToString();
                    strs.Add(str);
                }

                string message = string.Join(" ", strs);
                Log(message);
            }
            catch (Exception)
            {
                /* ignored */
            }
        }



    }
}
