using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Log
{
	public interface IReportable
	{
		LogReport GetReport();
	}
}
