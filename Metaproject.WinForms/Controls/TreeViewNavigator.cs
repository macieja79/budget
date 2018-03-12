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
    public partial class TreeViewNavigator : UserControl, IStandardEventListener
    {
        public TreeViewNavigator()
        {
            InitializeComponent();
            EventAttacher.Instance.AttachEvents(this, this);
        }

        private ITreeViewModel _treeView;
        public void AttachTreeView(ITreeViewModel treeViewModel)
        {
            _treeView = treeViewModel;
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            if (sender == btnExp)
            {
                _treeView.Expand();
            }

            if (sender == btnCollapse)
            {
                _treeView.Collapse();
            }

            if (sender == btnAdd)
            {
                _treeView.AddSelected();
            }

            if (sender == btnDel)
            {
                _treeView.DeleteSelected();
            }
        }

    

        public void OnCheckedChanged(object sender, EventArgs e)
        {
         
        }

     
    }
}
