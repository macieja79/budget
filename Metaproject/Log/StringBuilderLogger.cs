using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
	public class StringBuilderLogger : ILogger
	{

		StringBuilder _sb = new StringBuilder();

		public void Log(string message)
		{
			_sb.AppendLine(message);
		}

		public void Log(string message, int level)
		{
			Log(message);
		}

		public void Log(Exception exception)
		{
			Log(exception.Message);
		}

		public void Log(string message, int level, LogReportItemType type)
		{
			Log(message);
		}

	    public void LogItems(params object[] items)
	    {
	        throw new NotImplementedException();
	    }

	    public override string ToString()
		{
			return _sb.ToString();
		}
	}
}
