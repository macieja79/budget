using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Forms
{
    public class EventAttacher
    {

        public static EventAttacher Instance { get; private set; }
        
        static EventAttacher()
        {
            Instance = new EventAttacher();
        }

        #region <pub>

        public void AttachEvents(Control control, IStandardEventListener listener)
        {
            doAttachEvents(control, listener, true);
        }

        public void DetachEvents(Control control, IStandardEventListener listener)
        {
            doAttachEvents(control, listener, false);
        }

        #endregion

        #region <prv>

        void doAttachEvents(Control control, IStandardEventListener listener, bool isAttach)
        {
            attachToSingleControl(control, listener, isAttach);

            var strip = control as ToolStrip;
            if (strip != null)
            {
                foreach (ToolStripItem child in strip.Items)
                {
                    attachToSingleControl(child, listener, isAttach);
                }
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    doAttachEvents(child, listener, isAttach);
                }

            }

            
        }

        void attachToSingleControl(ToolStripItem control, IStandardEventListener listener, bool isAttach)
        {
            ToolStripButton button = control as ToolStripButton;
            if (null != button)
            {
                if (isAttach)
                    button.Click += listener.OnButtonClick;
                else
                    button.Click -= listener.OnButtonClick;
                return;
            }
        }

        void attachToSingleControl(Control control, IStandardEventListener listener, bool isAttach)
        {
            Button button = control as Button;
            if (null != button)
            {
                if (isAttach)
                    button.Click += listener.OnButtonClick;
                else
                    button.Click -= listener.OnButtonClick;
                return;
            }

            CheckBox checkBox = control as CheckBox;
            if (null != checkBox)
            {
                if (isAttach)
                    checkBox.CheckedChanged += listener.OnCheckedChanged;
                else
                    checkBox.CheckedChanged -= listener.OnCheckedChanged;
                return;
            }
        }

        #endregion


    }
}
