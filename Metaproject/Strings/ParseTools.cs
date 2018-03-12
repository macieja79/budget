using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Metaproject.Strings
{
    public class ParseTools
    {


        public static string ToUpper(string str)
        {
            if (str.IsNull()) return null;
            return str.ToUpper();
        }

        public static string ToLower(string str)
        {
            if (str.IsNull()) return null;
            return str.ToLower();
        }


        public static decimal ParseDecimal(string value)
        {
            value = value.Trim();
            string toParse = GetPreparedToParse(value);

            decimal parsed = decimal.Zero;
            if (decimal.TryParse(toParse, out parsed))
            {
                return parsed;
            }

            return parsed;
        }

        public static double ParseDouble(string value)
        {
            value = value.Trim();
            string toParse = GetPreparedToParse(value);

            double parsed = double.NaN;
            if (double.TryParse(toParse, out parsed))
            {
                return parsed;
            }

            return parsed;
        }

        public static long ParseLong(string v)
        {
            if (string.IsNullOrEmpty(v)) return 0;
            return long.Parse(v);
        }

        public static int ParseInt(string v)
        {
            if (string.IsNullOrEmpty(v)) return 0;
            return int.Parse(v);
        }

        public static T GetParsed<T>(T instance, string header, string line, char separator, out List<string> unparsed )
        {
            List<string> headers = header.Split(separator).ToList();
            List<string> values = line.Split(separator).ToList();
            unparsed = new List<string>();
            
            PropertyInfo[] properites = instance.GetType().GetProperties();
            foreach (PropertyInfo property in properites)
            {

                if (!property.CanWrite) continue;



                string propertyName = property.Name;
                int index = headers.IndexOf(propertyName);
                if (index == -1) continue;

                string value = values[index];

                if (property.PropertyType.FullName == "System.String")
                {
                    property.SetValue(instance, value, null);
                    continue;
                }

                if (property.PropertyType.FullName == "System.Int32")
                {

                    property.SetValue(instance, int.Parse(value), null);
                    continue;
                }

                if (property.PropertyType.FullName == "System.Decimal")
                {

                    property.SetValue(instance, decimal.Parse(value), null);
                    continue;
                    }

                unparsed.Add(propertyName);

            }

            return instance;
        }

        static string GetPreparedToParse(string value)
        {
            char systemSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            char currentSeparator = value.LastOrDefault(c => !Char.IsDigit(c));

            if (currentSeparator != char.MinValue)
            {
                value = value.Replace(currentSeparator, systemSeparator);
            }
            return value;
        }

        static string GetValue(List<string> headers, List<string> values, string name)
        {
            int index = headers.IndexOf(name);
            if (index == -1) return null;
            return values[index];
        }



    }
}
