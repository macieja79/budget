using Metaproject.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Progress
{
    public class FakeProgress : IProgress
    {
        
        #region <singleton>

        FakeProgress() { }

        static FakeProgress _instance;

        public static FakeProgress Instance
		{
			get
			{
                if (null == _instance) _instance = new FakeProgress();
				
				return _instance;
			}

		}
        
	   #endregion

        #region <IProgress>
        public void Show(string header)
        {
         
        }

        public void Set(string caption, int steps)
        {
          
        }

        public void Step(string caption)
        {
           
        }

        public void Close()
        {
           
        }
        #endregion
        
    }
}
