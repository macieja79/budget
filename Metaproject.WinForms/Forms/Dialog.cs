using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Metaproject.Forms
{
    public static class Dialog
    {

        public static void ShowError(IWin32Window owner, string error)
        {
            MessageBox.Show(owner, error, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(IWin32Window owner, string title, string info)
        {
            MessageBox.Show(owner, info, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    
        public static bool ShowQuestion(IWin32Window owner, string question)
        {
            return MessageBox.Show(owner, question, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static DialogResult ShowQuestionWithCancel(IWin32Window owner, string question)
        {
            return MessageBox.Show(owner, question, string.Empty, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

    }
}
