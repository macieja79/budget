using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Globalization;



namespace Metaproject.Strings
{

    public class MtDateTime
    {

        #region <constructors>
        public MtDateTime(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public MtDateTime()
        {
            DateTime = DateTime.Now;
        }
        #endregion

        #region <properties> 
        public DateTime DateTime;
        #endregion
    }

    public class MtStringBoolConverter
    {

        #region <members>
        string m_true = null;
        string m_false = null;
        #endregion

        #region <constructor>
        public MtStringBoolConverter(string trueAsString, string falseAsString)
        {
            m_true = trueAsString;
            m_false = falseAsString;
        }
        #endregion

        #region <public methods>
        public string Convert(bool value)
        {
            return value ? m_true : m_false;
        }

        public bool Convert(string value)
        {
            return (value == m_true);
        }
        #endregion

    }


	public static class MtString
    {

        #region <nested types>
        public enum StringType
        { 
            Text, 
            Integer, 
            Double
        }
        #endregion

        #region <public static methods>
        public static string Replace(string str, int start, int length, string newStr)
        {
            string prefix = str.Substring(0, start);
            string suffix = str.Substring(start + length);
            return prefix + newStr + suffix;
        }

        public static string[] SplitByString (string strToSplit, string separator)
        {

            if (string.IsNullOrEmpty(strToSplit) || string.IsNullOrEmpty(separator))
                return null;

            List<string> strings = new List<string>();
            string strToSearch = (string)strToSplit.Clone();
            
            restart:
            for (int i = 0; ; i++)
            {
                if (i > strToSearch.Length - separator.Length) break;

                string strToCompare = strToSearch.Substring(i, separator.Length);

                if (strToCompare == separator)
                {
                    string prefix = strToSearch.Substring(0, i);
                    string suffix = strToSearch.Substring(i + separator.Length);

                    strings.Add(prefix);

                    strToSearch = suffix;

                    if (strToSearch == "")
                        break;
                    else
                        goto restart;
                    
                }
            }
                       
            strings.Add(strToSearch);
            
            string[] stringsAsTable = new string[strings.Count];
            
            int j = 0;
            foreach (string s in strings)
            {
                stringsAsTable[j] = s; 
                j++;
            }

            return stringsAsTable;

        }

        public static double ConvertScientificNumber(string number)
        {

            number = PrepareNumber(number);

            string[] numbers = number.Split('E');

            if (numbers.Length != 2) return double.NaN;

            double baseNumber = double.NaN;
            double exp = double.NaN;

            string baseNumberString = numbers[0].Replace(',', '.');
            string expString = numbers[1];

            baseNumber = Convert.ToDouble(baseNumberString);
            exp = Convert.ToDouble(expString);

            if (double.TryParse(baseNumberString, out baseNumber) && double.TryParse(expString, out exp))
            {

                double result = baseNumber * Math.Pow(10, exp);
                return result;
            }

            return double.NaN;

        }

        public static string GetDoubleAsString(double value, int numberOfDecimals)
        {
            string format = GetDoubleValueFormat(numberOfDecimals);

            return string.Format(format, value);

        }

        public static string GetDoubleValueFormat(int numberOfDecimals)
        {
            string zeros = string.Empty;
            for (int i = 0; i < numberOfDecimals; i++)
            {
                if (i == 0) zeros += ".";
                zeros += "0";
            }

            string format = "{0:0" + zeros + "}";
            return format;
        }

        public static StringType GetStringType(string str)
        {

            if (str.Contains(".") || str.Contains(","))
            {
                double resultAsDouble;
                if (double.TryParse(str, out resultAsDouble))
                    return StringType.Double;
            }
            else
            {
                int resultAsInt;
                if (int.TryParse(str, out resultAsInt))
                    return StringType.Integer;
            }
            
            return StringType.Text;
            
        }

        public static string GetInBracket(string str)
        {
            return '(' + str + ')';

        }

        public static string ToFloatingPointString(double value)
        {
            return ToFloatingPointString(value, NumberFormatInfo.CurrentInfo);
        }

        private static readonly Regex rxScientific = new Regex(@"^(?<sign>-?)(?<head>\d+)(\.(?<tail>\d*?)0*)?E(?<exponent>[+\-]\d+)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        public static string ToFloatingPointString(double value, NumberFormatInfo formatInfo)
        {
            string result = value.ToString("r", NumberFormatInfo.InvariantInfo);
            Match match = rxScientific.Match(result);
            if (match.Success)
            {
                //Debug.WriteLine("Found scientific format: {0} => [{1}] [{2}] [{3}] [{4}]", result, match.Groups["sign"], match.Groups["head"], match.Groups["tail"], match.Groups["exponent"]);
                int exponent = int.Parse(match.Groups["exponent"].Value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
                StringBuilder builder = new StringBuilder(result.Length + Math.Abs(exponent));
                builder.Append(match.Groups["sign"].Value);
                if (exponent >= 0)
                {
                    builder.Append(match.Groups["head"].Value);
                    string tail = match.Groups["tail"].Value;
                    if (exponent < tail.Length)
                    {
                        builder.Append(tail, 0, exponent);
                        builder.Append(formatInfo.NumberDecimalSeparator);
                        builder.Append(tail, exponent, tail.Length - exponent);
                    }
                    else
                    {
                        builder.Append(tail);
                        builder.Append('0', exponent - tail.Length);
                    }
                }
                else
                {
                    builder.Append('0');
                    builder.Append(formatInfo.NumberDecimalSeparator);
                    builder.Append('0', (-exponent) - 1);
                    builder.Append(match.Groups["head"].Value);
                    builder.Append(match.Groups["tail"].Value);
                }
                result = builder.ToString();
            }
            return result;
        }

        public static string GetBetweenStrings(string originString, string start, string end)
        {

            string analysed = string.Empty;

            if (null == start)
            {
                analysed = originString;
            }
            else
            {
                string[] strings = SplitByString(originString, start);
                if (strings.Length <= 1) return null;

                analysed = strings[1];
            }

            if (null == end) return analysed.Trim();

            string[] nextStrings = SplitByString(analysed, end);

            if (nextStrings.Length == 0) return null;
            
            analysed = nextStrings[0];

            return analysed.Trim();
        
            
        }
        
        public static string GetListSeparated<T>(List<T> list, char separator)
        {
            string result = string.Empty;

            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i].ToString().Trim();
                result += s;

                if (i < list.Count - 1)
                    result += separator;
            }

            return result;

        }

        public static string GetSubString(string input, int start, int end, bool isWithIndexes)
        {
            try
            {
                if (isWithIndexes)
                {
                    int s = start;
                    int l = end - start + 1;
                    return input.Substring(s, l);
                }
                else
                {
                    int s = start + 1;
                    int l = end - start - 1;
                    return input.Substring(s, l);
                }
            }
            catch (Exception exc)
            {
                return null;
            }

        }
        #endregion

        #region <private>
        static string PrepareNumber(string number)
        {
            number = number.Trim();
            number = number.ToUpper();
            number = number.Replace("+", "");
            number = number.Replace(".", ",");
            if (number[0] == ',') number = "0" + number;

            return number;
        }
        #endregion

    }

    public class MtStrings : List<string>, ICloneable
    {
        
        #region ICloneable Members

        public object Clone()
        {
            MtStrings clonedStrings = new MtStrings();
            foreach (string s in this)
                clonedStrings.Add(s);

            return clonedStrings;
        }

        public string Joined
        {
            get
            {
                string j = string.Empty;
                foreach (string s in this)
                    j += s;

                return j;

            }
        }

        #endregion
    }
   
}

