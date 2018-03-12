using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Tests
{
    public class TestTool
    {

        #region <singleton>

        TestTool() { }

        static TestTool _instance;

        public static TestTool Instance
        {
            get
            {
                if (null == _instance) _instance = new TestTool();

                return _instance;
            }

        }

        #endregion

        #region <members>
        Random _random;
        #endregion

        #region <pub>

        public string GenerateRandomWord(int minNumberOfChars, int maxNumberOfChars)
        {
            if (null == _random) _random = new Random(DateTime.Now.Millisecond);

            int numberOfLetters = _random.Next(minNumberOfChars, maxNumberOfChars);

            int CHAR_A = 65;
            int CHAR_Z = 90;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numberOfLetters; i++)
            {
                sb.Append(Convert.ToChar(_random.Next(CHAR_A, CHAR_Z)));
            }

            return sb.ToString();

        }

        #endregion

    }
}
