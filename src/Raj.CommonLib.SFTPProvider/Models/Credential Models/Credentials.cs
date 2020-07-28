using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Models.Credential_Models
{
    /// <summary>
    /// Base credentials abstract class
    /// </summary>
    public abstract class Credentials
    {
        /// <summary>
        /// Domain or IP address of SFTP server
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Port of the SFTP server
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Username of SFTP user
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Base dir value, Helps to manage internally when same landing directory location for different types of users
        /// This manages dir and prevent user to access another directories which located in common landing directory
        /// Which helps when existing code was written using relative dir changes in the code
        /// </summary>
        public string BaseDir { get; set; } = "";
    }
}
