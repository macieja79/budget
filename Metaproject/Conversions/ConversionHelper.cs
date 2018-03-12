using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaproject.Conversions
{
    public class ConversionHelper
    {

        public static string BytesToString(byte[] bytes, string separator)
        {
            var strArray = bytes.Select(b => b.ToString());
            string str = string.Join(separator, strArray);
            return str;
        }

        public static byte[] StringToBytes(string str, string separator)
        {

            //char charSeparator = string
            string[] array = {separator};
            string[] items = str.Split(array, StringSplitOptions.None);

            byte[] bytesArray = items.Select(byte.Parse).ToArray();
            return bytesArray;
        }

    }
}
