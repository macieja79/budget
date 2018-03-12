using Metaproject.Log;
using Metaproject.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Patterns
{
    public class ActionData
    {

        ILogger _iLogger { get; set; }
        IProgress _iProgress { get; set; }

        public ILogger ILogger
        {
            get
            {
                if (null == _iLogger)
                    _iLogger = new FakeLogger();
                return _iLogger;
            }
            set
            {
                _iLogger = value;    
            }
        }

        public IProgress IProgress
        {
            get
            {
                if (null == _iProgress)
                    _iProgress = FakeProgress.Instance;
                return _iProgress;
            }
            set
            {
                _iProgress = value;
            }
        }



    }
}
