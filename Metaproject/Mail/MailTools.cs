using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.Mail
{
    public class MailTools
    {

        #region <singleton>

        MailTools() { }

        static MailTools _instance;

        public static MailTools Instance
        {
            get
            {
                if (null == _instance) _instance = new MailTools();
                return _instance;
            }
        }

        #endregion

        #region <pub>

        public void MailTo(string reciptient, string subject, string body)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            string fileName = string.Format("mailto:{0}?subject={1}&body={2}", reciptient, subject, body);
            proc.StartInfo.FileName = fileName;
            proc.Start();
        }

		public void MailTo(string recipient, string subject, string body, string attachmentPath)
		{
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			string fileName = string.Format("mailto:{0}?subject={1}&body={2}&Attachment={3}", recipient, subject, body, attachmentPath);
			proc.StartInfo.FileName = fileName;
			proc.Start();
		}

        #endregion

    }
}
