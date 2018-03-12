using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Controls
{
	public interface INavigatorListener
	{
		void OnButtonClicked(NavigatorButtonType type);
	}
}
