using Metaproject.Collections;
using Metaproject.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
    public class LogReportItem
    {

		#region <const>
        public const string ITEM_SEPARATOR = "<|>";
        #endregion

        #region <static>


        static BiDictionary<LogReportItemType, string> _shortcutDict = new BiDictionary<LogReportItemType, string>();
        static string _headerPattern = "[WRN|00|23:12:20]";
        static LogReportItem()
        {
            _shortcutDict.Add(LogReportItemType.Error, "ERR");
            _shortcutDict.Add(LogReportItemType.Exception, "EXC");
            _shortcutDict.Add(LogReportItemType.Message, "MSG");
            _shortcutDict.Add(LogReportItemType.Warning, "WRN");
            _shortcutDict.Add(LogReportItemType.Unknown, "UNK");
        }

        public static string GetDate(DateTime dt, LogReportDateDisplayType typeOfDisplay)
        {
            string date = string.Empty;

            if (typeOfDisplay == LogReportDateDisplayType.DateTime)
                date = dt.ToString("u");
            else if (typeOfDisplay == LogReportDateDisplayType.Date)
                date = dt.ToString("yyyy-MM-dd");
            else if (typeOfDisplay == LogReportDateDisplayType.Time)
                date = dt.ToString("HH:mm:ss");

            return date;
        }

        public static string GetTypeDisplayAsString(LogReportItemType typeOfItem)
        {
            return _shortcutDict[typeOfItem];
        }

        public static LogReportItemType GetLogReportType(string typeStr)
        {
            return _shortcutDict[typeStr];
        }

        public static bool TryCreateFromString(string pattern, out LogReportItem item)
        {
            item = null;

            try
            {
                int headerLength = _headerPattern.Length;
                if (pattern.Length < headerLength + 2) return false;

                string header = pattern.Substring(0, headerLength);

                bool isPattern = pattern[0] == '[' && pattern[4] == '|' &&
                                 pattern[7] == '|' && pattern[16] == ']';

                if (!isPattern) return false;

                string type = MtString.GetSubString(pattern, 0, 4, false);
                string strLevel = MtString.GetSubString(pattern, 4, 7, false);
                string date = MtString.GetSubString(pattern, 7, 16, false);
                string tempMsg = pattern.Substring(18);
                string msg = tempMsg.Replace(ITEM_SEPARATOR, Environment.NewLine);


                LogReportItemType typeofReport = GetLogReportType(type);
                int level = int.Parse(strLevel);
                DateTime dt = DateTime.Parse(date);

                item = new LogReportItem
                {
                    TypeOfLogItem = typeofReport,
                    Level = level,
                    Date = dt,
                    Message = msg
                };

                return true;
            }
            catch(Exception exc)
            {

            }

            return false;

        }
        
		public static string CreateLine(LogReportItemType itemType, int level, DateTime dateTime, string message)
		{
			string PATTERN = "[{0}|{1:00}|{2}] {3}";

            string type = GetTypeDisplayAsString(itemType);
            string date = GetDate(dateTime, LogReportDateDisplayType.Time);

            string msg = message.Replace(Environment.NewLine, ITEM_SEPARATOR);

            string line = string.Format(PATTERN, type, level, date, msg);
            return line;
		}
		
		
		#endregion

        #region <props>
        public DateTime Date { get; set; }
        public int Level { get; set; }
        public string Message { get; set; }
        public LogReportItemType TypeOfLogItem { get; set; }
        #endregion

        #region <override>
        public string GetAsLine()
        {
			string line = CreateLine(TypeOfLogItem, Level, Date, Message);
			return line;


            
        }
        #endregion

    }
}
