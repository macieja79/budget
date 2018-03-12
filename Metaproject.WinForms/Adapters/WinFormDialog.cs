using Metaproject.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Metaproject.Files;
using Metaproject.Forms;
using DialogResult = System.Windows.Forms.DialogResult;

namespace Metaproject.WinForms.Adapters
{
    public class WinFormDialog : IFileDialog
    {
       
        public bool ShowLoadDialog(LoadSaveDialogOptions options, out List<string> paths)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = options.Filter;
                System.Windows.Forms.DialogResult formsResult = dialog.ShowDialog();

                paths = new List<string>();
                paths.AddRange(dialog.FileNames.ToList());

                bool isOk = formsResult == DialogResult.OK;
                return isOk;
            }
        }

        public bool ShowSaveDialog(LoadSaveDialogOptions options, out List<string> paths)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = options.Filter;
                System.Windows.Forms.DialogResult formsResult = dialog.ShowDialog();

                paths = new List<string>();
                paths.AddRange(dialog.FileNames.ToList());

                bool isOk = formsResult == DialogResult.OK;
                return isOk;
            }
        }

        public bool ShowSelectEntitiesDialog(SelectEntitiesDialogOptions options, out List<string> selected)
        {

            using (CheckedListForm form = new CheckedListForm())
            {
                form.Text = options.Caption;
                form.InitializeDialog(options.Description, options.Options, options.Checked, this, this);
                System.Windows.Forms.DialogResult formsResult = form.ShowDialog();

                selected = new List<string>();
                bool isOk = formsResult == DialogResult.OK;
                if (isOk)
                {
                    selected = form.GetSelectedItems();
                }

                return isOk;
            }
        }

        public bool ShowQuestionDialog(string question, string caption)
        {
            var result = MessageBox.Show(question, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}
