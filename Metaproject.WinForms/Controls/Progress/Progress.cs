using Metaproject.Controls;
using Metaproject.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Metaproject.Controls
{
	public class Progress : IProgress, IDisposable
	{

		#region <singleton>

		Progress() { }

		static Progress _instance;

        public static Progress CreateProgress(string header)
        {
            if (null == _instance) _instance = new Progress();
            _instance.Show(header);
            return _instance;
       }
        
	   #endregion

		#region <members>
		MtProgress _progress;
        Counter _counter = null;
        int _counterSteps = -1;
		#endregion

        #region <pub>
        
        public void Show(string header)
        {
            if (null == _progress) _progress = new MtProgress();
            _progress.LabelHeader = header;
            _progress.ShowProgress(null);
        }

        public void Close()
        {
            _progress.CloseProgress();
            _counterSteps = -1;
        }
        
        public void SetUseCounter(int numberOfSteps)
        {
            _counterSteps = numberOfSteps;
        }

        #endregion

        #region <IProgress>

        public void Set(string caption, int steps)
		{
            _progress.LabelText = caption;

            if (_counterSteps > 0)
            {
                _counter = new Counter(_counterSteps, steps);
                _progress.Steps = _counterSteps;
            }
            else
            {
                _progress.Steps = steps;
            }
			
		}

		public void Step(string caption)
		{
            if (_counterSteps > 0)
            {
                if (!_counter.IsHit())
                    return;
            }

			if (null != caption)
				_progress.LabelText = caption;

			_progress.Step();
		}
		
		#endregion

        #region <IDisposable>

        public void Dispose()
        {
            Close();
        }
        #endregion
    }
}
