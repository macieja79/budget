using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Metaproject.Mail
{
    public class GmailSmtpClient
    {
        
        #region <singleton>

        GmailSmtpClient() { }

        static GmailSmtpClient _instance;

        public static GmailSmtpClient Instance
        {
            get
            {
                if (null == _instance) _instance = new GmailSmtpClient();
                return _instance;
            }
        }   

        #endregion

        #region <pub>
        public void SendMail(string recipient, string subject, string body, string path)
        {
            
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();
            
            try
            {
                Attachment attachment = new Attachment(path);

                msg.Subject = subject;
                msg.Body = body;
                msg.From = new MailAddress("metaprojectreports@gmail.com");
                msg.To.Add(recipient);
                msg.IsBodyHtml = true;

                msg.Attachments.Add(attachment);

                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("metaprojectreports@gmail.com", "metaproject2010");
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
