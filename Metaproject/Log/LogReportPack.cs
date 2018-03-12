using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Metaproject.Log
{
	public class LogReportPack
	{

		#region <const>
		const string HEADER_PATTERN = "{{0}}";
        const string PATTERN = @"\{(?<name>.*)\}";
		#endregion

		#region <ctor>
		public LogReportPack()
		{
			Reports = new List<LogReport>();
		}
		#endregion

		#region <props>
		public string Name { get; set; }
		public List<LogReport> Reports { get; set; }
		#endregion

		#region <pub>
		public string GetAsLine()
		{
			string header = CreateHeader(Name);
			return header;
		}
		#endregion

		#region <static>

		public static string CreateHeader(string name)
		{
            string header = string.Format(HEADER_PATTERN, name);
            return header;
		}

		public static bool TryCreateFromHeader(string header, out LogReportPack report)
		{
			
			report = null;
			Match match = Regex.Match(header, PATTERN);
			Group nameGroup = match.Groups["name"];

			if (!(nameGroup.Success))
				return false;

			string nameStr = nameGroup.Value;

			report = new LogReportPack { Name = nameStr };

			return true;

		}

		#endregion


	}
}
