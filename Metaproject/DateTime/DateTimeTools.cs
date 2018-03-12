using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System
{
    public class DateTimeTools
    {

        static Dictionary<int, string> _months = new Dictionary<int, string>()
        {
            {1, "Styczeń" },
            {2, "Luty" },
            {3, "Marzec" },
            {4, "Kwiecień" },
            {5, "Maj" },
            {6, "Czerwiec" },
            {7, "Lipiec" },
            {8, "Sierpień" },
            {9, "Wrzesień" },
            {10, "Październik" },
            {11, "Listopad" },
            {12, "Grudzień" },
        };

        public static string GetMonthName(int monthNumber)
        {
            string name = _months[monthNumber];
            return name;
        }


    }
}
