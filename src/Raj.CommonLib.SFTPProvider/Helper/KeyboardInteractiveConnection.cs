using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Helper
{
    /// <summary>
    /// Keyboard interactive authentication type helper
    /// </summary>
    internal class KeyboardInteractiveConnection
    {
        /// <summary>
        /// Password
        /// </summary>
        private static string Password { get; set; }
        internal static ConnectionInfo GetKeyboardInteractiveConnection(string host, int port, string username, string password)
        {
            Password = password;
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            ConnectionInfo conInfo = new ConnectionInfo(host: host, port: port, username: username, keybAuth);
            return conInfo;
        }
        internal static KeyboardInteractiveAuthenticationMethod GetKeyboardInteractiveMethod(string username, string password)
        {
            Password = password;
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(username);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            return keybAuth;
        }
        /// <summary>
        /// Handles key event to enter password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = Password;
                }
            }
        }
    }
}
