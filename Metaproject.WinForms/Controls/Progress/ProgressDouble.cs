using Metaproject.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Controls
{
    public class ProgressDouble : IProgressDouble, IDisposable
    {

        #region <singleton>

      

        static ProgressDouble _instance;

        public static ProgressDouble CreateProgress(string header, bool isSwapped = false)
        {
            if (null == _instance) _instance = new ProgressDouble();
            _instance.Show(header, isSwapped);
            return _instance;
        }


        #endregion

        #region <members>
        MtProgressDouble _progress;
        Counter _counter1 = null;
        Counter _counter2 = null;

        int _counterSteps1 = -1;
        int _counterSteps2 = -1;
        #endregion

        #region <pub>

        public void Show(string header, bool isSwapped = false)
        {
            if (null == _progress) _progress = new MtProgressDouble();
            _progress.LabelHeader = header;
            _progress.IsSwapped = isSwapped;
            _progress.ShowProgress(null);
        }

        public void Close()
        {
            _progress.CloseProgress();
            _counterSteps1 = -1;
        }

        public void SetUseCounter(int numberOfSteps1, int numberOfSteps2)
        {
            _counterSteps1 = numberOfSteps1;
            _counterSteps2 = numberOfSteps2;
        }

        public void CenterToControl(Control control)
        {
            _progress.CenterToControl(control);
        }

        #endregion

        #region <IProgress>

        public void Set(string caption, int steps)
        {
            _progress.LabelText = caption;

            if (_counterSteps1 > 0 && steps > 0)
            {
                _counter1 = new Counter(_counterSteps1, steps);
                _progress.Steps = _counterSteps1;
            }
            else
            {
                _progress.Steps = steps;
            }

        }

        public void Step(string caption)
        {
            if (_counterSteps1 > 0)
            {
                if (!_counter1.IsHit())
                    return;
            }

            if (null != caption)
                _progress.LabelText = caption;

            _progress.Step();
        }

        #endregion

        #region <IProgressDouble>

        public void Set2(string caption, int steps)
        {
            _progress.LabelText2 = caption;

            if (_counterSteps2 > 0 && steps > 0)
            {
                _counter2 = new Counter(_counterSteps2, steps);
                _progress.Steps2 = _counterSteps1;
            }
            else
            {
                _progress.Steps2 = steps;
            }

        }

        public void Step2(string caption)
        {
            if (_counterSteps2 > 0)
            {
                if (!_counter2.IsHit())
                    return;
            }

            if (null != caption)
                _progress.LabelText2 = caption;

            _progress.Step2();
        }

        #endregion


        #region <IDisposable>

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            Close();
            IsDisposed = true;
        }
        #endregion
    }
}
