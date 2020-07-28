using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Models.Credential_Models
{
    /// <summary>
    /// Key and Password credential model
    /// </summary>
    public class KeyAndPasswordBaseCredentials: Credentials
    {
        /// <summary>
        /// Password of user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Set true if server uses Keyboard ineractive authentication machanism
        /// </summary>
        public bool IsKeyboardInteractive { get; set; }
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
