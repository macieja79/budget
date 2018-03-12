using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Finances
{
    public class FinancesTools
    {
        public static string GetAccountNumberWithSpaces(string accountNumber)
        {
            if (accountNumber.IsNull()) return accountNumber;

            List<string> parts = new List<string>();
            string checkSum = accountNumber.Substring(0, 2);
            parts.Add(checkSum);
            
            int start = 2;
            int size = 4;
            
            for (int i = 0; i < 6; i++)
            {
                string part = accountNumber.Substring(start, size);
                parts.Add(part);
                start += size;
            }

            string formatted = string.Format("{0} {1} {2} {3} {4} {5} {6}", parts.ToArray());
            return formatted;
        }
    }

}
