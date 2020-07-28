using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Models.Credential_Models
{
    /// <summary>
    /// Passowrd Base Credential Model
    /// </summary>
    public class PasswordBaseCredentials: Credentials
    {
        /// <summary>
        /// Password of user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Set true if server uses Keyboard ineractive authentication machanism
        /// </summary>
        public bool IsKeyboardInteractive { get; set; }
    }
}
