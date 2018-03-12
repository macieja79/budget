using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 

namespace Metaproject.Excel
{
	public class ExcelSupport
	{
		public static string GetCellSymbol(int r, int c)
		{
            string col = "";
            if (c >= 0) col = GetColumnSymbol(c);

            string row = "";
            if (r >= 0) row = r.ToString();

			string cellSymb = col + row;
			return cellSymb;
		}

		public static string GetColumnSymbol(int nr)
		{

			int n1 = nr / 26;
			int n2 = nr % 26;

			if (n2 == 0)
			{

				n1--;
				n2 = 26;
			}

			string s1 = "";
			string s2 = "";

			if (n1 > 0)
			{
				s1 = char.ConvertFromUtf32(64 + n1);

			}

			if (n2 > 0)
			{
				s2 = char.ConvertFromUtf32(64 + n2);

			}

			string outStr = s1 + s2;

			return outStr;

		}



	}
}
