using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.UndoRedo
{
	public interface IOperation
	{
		void Do();
		void Undo();
		void Redo();
	}
}
