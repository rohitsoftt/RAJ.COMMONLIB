using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Models.Credential_Models
{
    /// <summary>
    /// Key Base Crednetials Model
    /// </summary>
    public class KeyBaseCredentials: Credentials
    {
        /// <summary>
        /// Fill path of private key file
        /// </summary>
        public string KeyFilePath { get; set; }
        /// <summary>
        /// PassPhrase of the key
        /// </summary>
        public string PassPhrase { get; set; }
    }
}
