using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Metaproject.Log;

namespace Metaproject.WinForms
{
    public class TextBoxLogger : ILogger
    {
        private readonly TextBox _box;

        public TextBoxLogger(TextBox box)
        {
            _box = box;
        }

        public void Log(string message)
        {
            _box.Text += $"{message}{Environment.NewLine}";
        }

        public void Log(string message, int level)
        {
            throw new NotImplementedException();
        }

        public void Log(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Log(string message, int level, LogReportItemType type)
        {
            throw new NotImplementedException();
        }

        public void LogItems(params object[] items)
        {
            throw new NotImplementedException();
        }
    }
}
