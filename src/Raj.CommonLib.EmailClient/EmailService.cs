using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Raj.CommonLib.EmailClient
{
    class EmailService
    {
        /// <summary>
        /// Use to send any type of email
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> SendEmail(EmailCredentials emailCredentials, string subject, string body, string[] attachmentPath = null)
        {
            var credentials = new NetworkCredential(emailCredentials.Username, emailCredentials.Password);
            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress(emailCredentials.FromEmailAddress, emailCredentials.FromName),
                Subject = subject,
                Body = body,
                Priority = MailPriority.High,
                BodyEncoding = Encoding.UTF8
        };
            mail.IsBodyHtml = true;
            mail.To.Add(emailCredentials.ToEmails);
            mail.ReplyToList.Add(emailCredentials.ReplyToEmails);
            string[] CCEamailIDs = !string.IsNullOrEmpty(emailCredentials.CCEmails) ? emailCredentials.CCEmails.Split(',') : null;
            foreach (string CCEmail in CCEamailIDs)
            {
                mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
            }

            // If not null the attachemnt
            if (attachmentPath != null && attachmentPath.Length > 0)
            {
                foreach (var attachment in attachmentPath)
                {
                    var attachmentt = new System.Net.Mail.Attachment(attachment);
                    mail.Attachments.Add(attachmentt);
                }
            }

            // Smtp client
            var client = new SmtpClient()
            {
                Port = Int32.Parse(emailCredentials.Port),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailCredentials.SMTP,
                EnableSsl = true,
                Credentials = credentials,
                
            };
            client.ServicePoint.MaxIdleTime = 0;
            client.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
            try
            {
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
