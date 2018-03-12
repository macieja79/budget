using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public class FileLogger : ILogger
    {
        #region <members>
        string m_strFileName;
        object m_sync = new object();
        #endregion

        #region <ctor>
        public FileLogger(string fullPath)
        {
            m_strFileName = fullPath;
        }

        public FileLogger(string directory, string prefix)
        {
            string fileName = prefix + DateTime.Now.ToShortDateString() + ".log";
            m_strFileName = Path.Combine(directory, fileName);
        }
        #endregion

        #region <ILogger>
        public void Log(string message)
        {
            try
            {

                lock (m_sync)
                {
                    if (!File.Exists(m_strFileName)) File.Create(m_strFileName);
                    using (StreamWriter writer = File.AppendText(m_strFileName))
                    {
                        writer.WriteLine(message);
                    }
                }
            }
            catch (Exception exc)
            {

            }
        }

        public void Log(Exception exception)
        {
            Log(exception.Message);
        }


        public void Log(string message, int level)
        {
            // TODO: implementacja leveli
            Log(message);
        }

        public void Log(string message, int level, LogReportItemType type)
        {
            throw new NotImplementedException();
        }

        public void LogItems(params object[] items)
        {
            throw new NotImplementedException();
        }

        #endregion





      
    }
}
