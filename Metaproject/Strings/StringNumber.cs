using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Strings
{
    public class StringNumber
    {

        #region <singleton>

        StringNumber() { }

        static StringNumber _instance;

        public static StringNumber Instance
        {
            get
            {
                if (null == _instance) _instance = new StringNumber();

                return _instance;
            }

        }

        #endregion

        #region <pub>
        public string GetWithNumberIncreased(string prefix, string str, string numberFormat)
        {
            int number = GetNumber(prefix, str);
            number++;

            return GetWithNumber(prefix, number, numberFormat);
        }

        public int GetNumber(string prefix, string str)
        {
            string numberPart = str.Replace(prefix, "");
            int number = -1;
            if (!int.TryParse(numberPart, out number))
                return 0;
            return number;
        }

        public int GetMaxNumber(string prefix, List<string> str)
        {
            List<int> numbers = str.Select(s => GetNumber(prefix, s)).ToList();
            if (numbers.Count == 0) return 0;

            return numbers.Max();
        }

        string GetWithNumber(string prefix, int number, string numberFormat)
        {
            string format = "{0:" + numberFormat + "}";
            string increased = prefix + " " + string.Format(format, number);
            return increased;
        }


        public string GetWithNumberIncreased(string prefix, List<string> strings, string numberFormat)
        {
            int max = GetMaxNumber(prefix, strings);
            max++;

            return GetWithNumber(prefix, max, numberFormat);

        }



        #endregion
    }
}