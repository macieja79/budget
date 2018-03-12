using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace System
{
    public class ObjTools
    {
        public static bool IsNotNull(params object[] obj)
        {
            foreach (object o in obj)
            {
                if (o.IsNullObj()) return false;
            }

            return true;
            
        }

        public static bool IsAnyNull(params object[] obj)
        {
            return obj.Any(o => o.IsNullObj());
        }

        public static bool AreEqual(params object[] obj)
        {
            int n = obj.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (obj[i] != obj[j]) return false;

                }

            }
            return true;
        }

        public static bool AreNotEqual(params object[] obj)
        {
            int n = obj.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (obj[i] == obj[j]) return false;

                }

            }
            return true;
        }

    }
}
