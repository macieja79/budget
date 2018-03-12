using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metaproject.Dialog;
using Metaproject.Forms;

namespace Metaproject.WinForms.Controls
{
    public partial class GridNavigator : UserControl, IStandardEventListener
    {
        public GridNavigator()
        {
           
            InitializeComponent();
            EventAttacher.Instance.AttachEvents(this, this);
        }

        private IGridController _gridController;
        public void AttachTreeView(IGridController gridController)
        {
            _gridController = gridController;
        }

        #region <IStandardEventListener>

        public void OnButtonClick(object sender, EventArgs e)
        {
            if (sender == btnAdd)
            {
                _gridController.Add();
                return;
            }

            if (sender == btnDel)
            {
                _gridController.Delete();
                return;
            }

            if (sender == btnCopy)
            {
                _gridController.Copy();
                return;
            }

            if (sender == btnPaste)
            {
                _gridController.Paste();
                return;
            }
        }

        public void OnCheckedChanged(object sender, EventArgs e)
        {
         
        }

        #endregion


    }
}
