using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Controls
{
	public class Counter
	{

		int _counter = 0;
		int _hitNumber = 0;

		public Counter(int numberOfSteps, int total)
		{
			NumberOfSteps = numberOfSteps;
			Total = total;

			_hitNumber = (Total / NumberOfSteps);

		}


		public bool IsHit()
		{
			_counter++;

			if (_counter == _hitNumber)
			{
				_counter = 0;
				return true;
			}

			return false;
		}


		public int NumberOfSteps { get; set; }
		public int Total { get; set; }


	}
}
