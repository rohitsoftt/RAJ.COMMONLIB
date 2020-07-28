using Raj.CommonLib.SFTPProvider.Models.Credential_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raj.CommonLib.SFTPProvider.UnitTests.static_data
{
    public class SFTPDetails
    {
        public const string Username = "xxxx";
        public const string Password = "xxxxx";
        public const string Host = "xxxx.xxxx.com";
        public const int Port = 22;
        public const string PassPhrase = "";
        public const string KeyFilePath = "";
        public const string BaseDir = "";



        public static SFTPClientProvider GetSFTPClientProvider(bool isKeyboardInteractive=false, bool isPasswordBase=true, bool isKeyBase=false)
        {
            if(isPasswordBase && isKeyBase)
            {
                KeyAndPasswordBaseCredentials credentials = new KeyAndPasswordBaseCredentials
                {
                    Username = SFTPDetails.Username,
                    Host = SFTPDetails.Host,
                    Port = SFTPDetails.Port,
                    KeyFilePath = SFTPDetails.KeyFilePath,
                    BaseDir = SFTPDetails.BaseDir,
                    Password = SFTPDetails.Password,
                    IsKeyboardInteractive = isKeyboardInteractive,
                };
                return new SFTPClientProvider(credentials);
            }
            else if (isKeyBase)
            {
                KeyBaseCredentials credentials = new KeyBaseCredentials
                {
                    Username = SFTPDetails.Username,
                    Host = SFTPDetails.Host,
                    Port = SFTPDetails.Port,
                    KeyFilePath = SFTPDetails.KeyFilePath,
                    BaseDir = SFTPDetails.BaseDir
                };
                return new SFTPClientProvider(credentials);
            }
            else
            {
                PasswordBaseCredentials credentials = new PasswordBaseCredentials
                {
                    Username = SFTPDetails.Username,
                    Password = SFTPDetails.Password,
                    Host = SFTPDetails.Host,
                    Port = SFTPDetails.Port,
                    IsKeyboardInteractive = isKeyboardInteractive,
                    BaseDir = SFTPDetails.BaseDir
                };
                return new SFTPClientProvider(credentials);
            }
        }
    }
}
