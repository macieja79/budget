using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Metaproject.Log
{
    public class LogReport
	{

		#region <ctors>
		public LogReport()
        {
            CreationDate = DateTime.Now;
            Items = new List<LogReportItem>();

        }

        public LogReport(string name) : this()
        {
            Name = name;
        }
		#endregion

		#region <props>
		public string Name { get; set; }
        public string SubName { get; set; }
        public DateTime CreationDate { get; set; }
        public List<LogReportItem> Items { get; set; }

		public LogReportItemType MostCriticalType
		{

			get
			{
				LogReportItemType[] types = 
				{ 
					LogReportItemType.Exception, 
					LogReportItemType.Error, 
					LogReportItemType.Warning, 
					LogReportItemType.Message 
				};

				foreach (LogReportItemType checkedType in types)
				{
					if (Items.Any(i => i.TypeOfLogItem == checkedType))
						return checkedType;

				}

				return LogReportItemType.Message;

			}


		}
		#endregion

		#region <pub>



		public void Clear()
        {
            Items.Clear();
        }

		const string HEADER_PATTERN = "<{0}|{1}>";
		
		public string GetHeader()
		{


			string header = CreateHeader(CreationDate, Name);
			return header;
            
            
		}

		#endregion

		#region <static>

		public static string CreateHeader(DateTime creationDate, string name)
		{

			string dt = creationDate.ToString("s");

            string header = string.Format(HEADER_PATTERN, dt, name);

            return header;
			

		}


        static string PATTERN = @"\<(?<date>.{19})\|(?<name>.*)\>";

        public static bool TryCreateFromHeader(string header, out LogReport report)
        {
            report = null;
            Match match = Regex.Match(header, PATTERN);

            Group dateGroup = match.Groups["date"];
            Group nameGroup = match.Groups["name"];

            if (!(dateGroup.Success && nameGroup.Success)) return false;

            string dateStr = dateGroup.Value;
            string nameStr = nameGroup.Value;

            DateTime dt = DateTime.Parse(dateStr);

            report = new LogReport { Name = nameStr, CreationDate = dt };

            return true;

		}
		#endregion

	}
}
