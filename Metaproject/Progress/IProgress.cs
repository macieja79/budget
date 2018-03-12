using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Progress
{
	public interface IProgress
	{
	
		void Set(string caption, int steps);
		void Step(string caption);
		
	}
}
