using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raj.CommonLib.EmailClient
{
    public class EmailCredentials
    {
        /// <summary>
        /// Username of the email
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password of the email
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// store in comma seprated format
        /// </summary>
        public string CCEmails { get; set; }
        /// <summary>
        /// To meail
        /// </summary>
        public string ToEmails { get; set; }
        /// <summary>
        /// SMTP of the email server
        /// </summary>
        public string SMTP { get; set; }
        /// <summary>
        /// Port of the SMTP server
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// From name to send email
        /// </summary>
        public string FromName { get; set; }
        /// <summary>
        /// From email address 
        /// </summary>
        public string FromEmailAddress { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public string CreatedDate { get; set; }
        /// <summary>
        /// Reply to emails comma(",") seprated list
        /// </summary>
        public string ReplyToEmails { get; set; }
        /// <summary>
        /// Get email credentails
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static EmailCredentials GetCredentials()
        {
            return new EmailCredentials
            {
                Username = Properties.EmailResource.Username,
                Password = Properties.EmailResource.Password,
                CCEmails = Properties.EmailResource.CCEmailIds,
                ToEmails = Properties.EmailResource.ToEmailIds,
                ReplyToEmails = Properties.EmailResource.ReplyToEmails,
                SMTP = Properties.EmailResource.SMTP,
                Port = Properties.EmailResource.SMTPPort,
                FromName = Properties.EmailResource.FromEmailDisplayName,
                FromEmailAddress = Properties.EmailResource.FromEmailId
            };
        }
    }
}
