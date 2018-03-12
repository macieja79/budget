using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Metaproject.Strings
{
    public class StrComparer
    {

        #region <singleton>

        StrComparer() { }

        static StrComparer _instance;

        public static StrComparer Instance
        {
            get
            {
                if (null == _instance) _instance = new StrComparer();

                return _instance;
            }

        }

        #endregion

        #region <pub>
        public bool Compare(string str1, string str2, bool isUseWildcards, bool isIgnoreCase)
        {

            if (null == str1 && null == str2) return true;
            if (null == str1 || null == str2) return false;

            string tr1 = str1.Trim();
            string tr2 = str2.Trim();

            if (isIgnoreCase)
            {
                tr1 = tr1.ToLower();
                tr2 = tr2.ToLower();
            }

            if (isUseWildcards)
            {
                string regexPattern = wildcardToRegex(tr1);
                bool isMatch = Regex.IsMatch(tr2, regexPattern);
                return isMatch;
            }
            else
            {
                bool isMatch = string.Equals(tr1, tr2);
                return isMatch;
            }


        }
        #endregion

        #region <prv>
        string wildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
            Replace("\\*", ".*").
            Replace("\\?", ".") + "$";
        }
        #endregion

    }
}
