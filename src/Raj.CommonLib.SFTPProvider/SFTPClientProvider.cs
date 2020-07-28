using Raj.CommonLib.SFTPProvider.Exceptions;
using Raj.CommonLib.SFTPProvider.Helper;
using Raj.CommonLib.SFTPProvider.Models.Credential_Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider
{
    /// <summary>
    /// SFTP clinet provider extended SftpClient
    /// </summary>
    public class SFTPClientProvider : SftpClient, ISFTPClientProvider
    {
        /// <summary>
        /// After connect to the server, auto-redirect to this location as working directory
        /// </summary>
        public string BaseDir { get; }
        /// <summary>
        /// Domain or Ip of the sftp server
        /// </summary>
        public string Host { get; }
        /// <summary>
        /// Identifier of the SFTP connection, use when multiple SFTP provider objects needs to handle
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentials"></param>
        public SFTPClientProvider(PasswordBaseCredentials credentials)
            : base(GetSftpConnection(credentials.Host, credentials.Port, credentials.Username, credentials.Password, credentials.IsKeyboardInteractive))
        {
            BaseDir = GetBaseDirValue(credentials.BaseDir) ?? "/";
            Host = credentials.Host;
        }
        /// <summary>
        /// Using username and keyFile
        /// </summary>
        /// <param name="credentials"></param>
        public SFTPClientProvider(KeyBaseCredentials credentials)
            : base(GetSftpConnection(credentials.Host, credentials.Username, credentials.Port, new PrivateKeyFile[] { GetPrivateKeyFile(credentials.KeyFilePath, credentials.PassPhrase) }))
        {
            BaseDir = GetBaseDirValue(credentials.BaseDir) ?? "/";
            Host = credentials.Host;
        }
        /// <summary>
        /// Username, password and keyfiles
        /// </summary>
        /// <param name="credentials"></param>
        public SFTPClientProvider(KeyAndPasswordBaseCredentials credentials)
            : base(GetSftpConnection(credentials.Host, credentials.Username, credentials.Password, credentials.Port, credentials.IsKeyboardInteractive, new PrivateKeyFile[] { GetPrivateKeyFile(credentials.KeyFilePath, credentials.PassPhrase) }))
        {
            BaseDir = GetBaseDirValue(credentials.BaseDir) ?? "/";
            Host = credentials.Host;
        }
        /// <summary>
        /// Username, Password and PrivateKeyfiles array
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="privateKeyfiles"></param>
        public SFTPClientProvider(PasswordBaseCredentials credentials, PrivateKeyFile[] privateKeyfiles)
            : base(GetSftpConnection(credentials.Host, credentials.Username, credentials.Password, credentials.Port, credentials.IsKeyboardInteractive, privateKeyfiles))
        {
            BaseDir = GetBaseDirValue(credentials.BaseDir) ?? "/";
            Host = credentials.Host;
        }
        /// <summary>
        /// Username and PrivateKeyFiles
        /// </summary>
        /// <param name="host">Domain or Ip of the sftp server</param>
        /// <param name="port">SFTP service port</param>
        /// <param name="username">Username of sftp user</param>
        /// <param name="privateKeyfiles">Renci.SshNet.PrivateKeyFile in array</param>
        /// <param name="baseDir">Base dir/Working dir, to restrict the access</param>
        public SFTPClientProvider(string host, int port, string username, PrivateKeyFile[] privateKeyfiles, string baseDir = null)
            : base(GetSftpConnection(host, username, port, privateKeyfiles))
        {
            BaseDir = GetBaseDirValue(baseDir) ?? "/";
            Host = host;
        }
        /// <summary>
        /// Using connectionInfo
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <param name="baseDir">Base dir/Working dir, to restrict the access</param>
        public SFTPClientProvider(ConnectionInfo connectionInfo, string baseDir = null)
            : base(connectionInfo)
        {
            BaseDir = GetBaseDirValue(baseDir) ?? "/";
            Host = connectionInfo.Host;
        }
        /// <summary>
        /// Get private key file
        /// </summary>
        /// <param name="keyFilePath">
        /// Absolute private key file path
        /// </param>
        /// <param name="passPhrase">
        /// Key passPhrase
        /// </param>
        /// <returns></returns>
        private static PrivateKeyFile GetPrivateKeyFile(string keyFilePath, string passPhrase)
        {
            PrivateKeyFile pk;
            if (passPhrase != null)
            {
                pk = new PrivateKeyFile(keyFilePath, passPhrase);
            }
            else
            {
                pk = new PrivateKeyFile(keyFilePath);
            }
            return pk;
        }
        private string GetBaseDirValue(string baseDir)
        {
            if (!string.IsNullOrEmpty(baseDir) && baseDir.Trim(' ').Length > 0)
            {
                baseDir = baseDir.Trim('/').Trim('\\');
                baseDir = "/" + baseDir;
                return baseDir;
            }
            else
            {
                return null;
            }
        }
        private static ConnectionInfo GetSftpConnection(string host, int port, string username, string password, bool isKeyboardInteractive)
        {
            IList<AuthenticationMethod> AuthMethods = new List<AuthenticationMethod>();
            if (isKeyboardInteractive)
            {
                return KeyboardInteractiveConnection.GetKeyboardInteractiveConnection(host, port, username, password);
            }
            else
            {
                var passwordAuth = new PasswordAuthenticationMethod(username, password);
                AuthMethods.Add(passwordAuth);
                return new ConnectionInfo(host, port, username, AuthMethods.ToArray());
            }
        }
        /// <summary>
        /// Get SFTP connection
        /// </summary>
        /// <param name="host">Domain or IP address</param>
        /// <param name="username">Username of SFTP user</param>
        /// <param name="port">Port of SFTP server</param>
        /// <param name="privateKeyFiles"></param>
        /// <returns>Renci.SshNet.ConnectionInfo</returns>
        private static ConnectionInfo GetSftpConnection(string host, string username, int port, PrivateKeyFile[] privateKeyFiles)
        {
            return new ConnectionInfo(host, port, username, AuthticationMethods(username, privateKeyFiles));
        }
        private static ConnectionInfo GetSftpConnection(string host, string username, string password, int port, bool isKeyboardInteractive, PrivateKeyFile[] privateKeyFiles)
        {
            return new ConnectionInfo(host, port, username, AuthticationMethods(username, password, isKeyboardInteractive, privateKeyFiles));
        }
        /// <summary>
        /// Use to connect with baseDir
        /// </summary>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">base dir permission denied</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPathNotFoundException">path was not found on the remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        public new void Connect()
        {
            if (!IsConnected)
            {
                base.Connect();
                try
                {
                    ChangeDirectory(BaseDir);
                }
                catch
                {
                    Disconnect();
                    throw;
                }
            }
        }

        private static AuthenticationMethod[] AuthticationMethods(string username, PrivateKeyFile[] privateKeyFiles)
        {
            IList<AuthenticationMethod> AuthMethods = new List<AuthenticationMethod>();
            foreach (var privateKeyFile in privateKeyFiles)
            {
                var privateKeyAuthenticationMethod = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
                AuthMethods.Add(privateKeyAuthenticationMethod);

            }
            return AuthMethods.ToArray();
        }
        private static AuthenticationMethod[] AuthticationMethods(string username, string password, bool isInteractive, PrivateKeyFile[] privateKeyFiles)
        {
            IList<AuthenticationMethod> AuthMethods = new List<AuthenticationMethod>();
            if (isInteractive)
            {
                AuthMethods.Add(KeyboardInteractiveConnection.GetKeyboardInteractiveMethod(username, password));
            }
            else
            {
                var passwordAuth = new PasswordAuthenticationMethod(username, password);
                AuthMethods.Add(passwordAuth);
            }
            foreach (var privateKeyFile in privateKeyFiles)
            {
                var privateKeyAuthenticationMethod = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
                AuthMethods.Add(privateKeyAuthenticationMethod);
            }
            return AuthMethods.ToArray();
        }
        /// <summary>
        /// Use to upload file
        /// </summary>
        /// <param name="localFilePath">Path of local file</param>
        /// <param name="localFileName">Name of the local file</param>
        /// <param name="remoteFileName">Name of remote file, if not mention then will keep same as a local file</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">path is null or contains only whitespace characters.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to upload the file was denied by the remote host.
        ///     -or-
        ///     A SSH command was denied by the server.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        /// <remarks>Method calls made by this method to input, may under certain conditions result
        ///    in exceptions thrown by the stream.</remarks>
        public void UploadFile(string localFilePath, string localFileName, string remoteFileName = null)
        {
            remoteFileName = remoteFileName ?? localFileName;
            string filePath = localFilePath + $"\\{localFileName}";
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BufferSize = 1024;
                base.UploadFile(fs, remoteFileName);
            }
        }
        /// <summary>
        /// Use to upload file
        /// </summary>
        /// <param name="localFileNameWithPath"></param>
        /// <param name="remoteFileName"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">path is null or contains only whitespace characters.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to upload the file was denied by the remote host.
        ///     -or-
        ///     A SSH command was denied by the server.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        /// <remarks>Method calls made by this method to input, may under certain conditions result
        ///     in exceptions thrown by the stream.</remarks>
        public void UploadFile(string localFileNameWithPath, string remoteFileName = null)
        {
            remoteFileName = remoteFileName ?? Path.GetFileName(localFileNameWithPath);
            using (FileStream fs = new FileStream(localFileNameWithPath, FileMode.Open))
            {
                BufferSize = 1024;
                Console.WriteLine(WorkingDirectory);
                UploadFile(fs, remoteFileName);
            }
        }
        /// <summary>
        /// Download file by file name
        /// </summary>
        /// <param name="remoteFileName">Remote file name</param>
        /// <param name="localPath">Local file path</param>
        /// <param name="localFileName">Local file name</param>
        /// <returns></returns>
        /// /// <exception cref="System.ArgumentException">path is null or contains only whitespace characters.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to upload the file was denied by the remote host.
        ///     -or-
        ///     A SSH command was denied by the server.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        /// <remarks>Method calls made by this method to input, may under certain conditions result
        ///     in exceptions thrown by the stream.</remarks>
        public void DownloadFile(string remoteFileName, string localPath, string localFileName = null)
        {
            if (string.IsNullOrEmpty(localFileName))
            {
                localFileName = remoteFileName;
            }
            CheckAndCreatePath(localPath);
            string filePath = localPath + $@"\{localFileName}";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BufferSize = 1024;
                DownloadFile(remoteFileName, fs);
            }
        }
        /// <summary>
        /// To download file by remote file name
        /// </summary>
        /// <param name="remoteFileName">Name of remote file</param>
        /// <param name="localFileWithPath">Local file with full path</param>
        /// <returns></returns>
        /// /// <exception cref="System.ArgumentException">path is null or contains only whitespace characters.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to upload the file was denied by the remote host.
        ///     -or-
        ///     A SSH command was denied by the server.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        /// <remarks>Method calls made by this method to input, may under certain conditions result
        ///    in exceptions thrown by the stream.</remarks>
        public void DownloadFile(string remoteFileName, string localFileWithPath)
        {
            CheckAndCreatePath(localFileWithPath, true);
            using (FileStream fs = new FileStream(localFileWithPath, FileMode.Create))
            {
                BufferSize = 1024;
                DownloadFile(remoteFileName, fs);
            }
        }
        /// <summary>
        /// To change by reletive directory
        /// </summary>
        /// <param name="path">Remote location path</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to change directory denied by remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPathNotFoundException">path was not found on the remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        public bool ChangeRelativeDirectory(string path)
        {
            ChangeDirectory(path);
            return true;
        }
        /// <summary>
        /// To change directory by absolute path
        /// </summary>
        /// <param name="path">Absolute path of remote location</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to change directory denied by remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPathNotFoundException">path was not found on the remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        public bool ChangeAbsoluteDirectory(string path)
        {
            if (CheckPathSame(path))
            {
                return true;
            }
            if (WorkingDirectory != BaseDir)
            {
                if (WorkingDirectory != "/")
                    ChangeDirectory("/");
                if (BaseDir != "/")
                    ChangeDirectory(BaseDir);
            }
            ChangeDirectory(path);
            return true;
        }
        /// <summary>
        /// To get list of files only with name
        /// </summary>
        /// <param name="absoluteDirPath">absoulte path of remote location</param>
        /// <param name="files">List of only file name</param>
        public void GetFilesList(string absoluteDirPath, ref IList<string> files)
        {
            string workingDir = WorkingDirectory;
            if (WorkingDirectory != BaseDir)
            {
                if (WorkingDirectory != "/")
                    ChangeDirectory("/");
                ChangeDirectory(BaseDir);
            }
            foreach (var entry in ListDirectory(absoluteDirPath))
            {

                if (entry.IsDirectory)
                {
                    //ListDirectory(entry.FullName, ref files);
                }
                else
                {
                    files.Add(entry.Name);
                }
            }
            if (workingDir != WorkingDirectory)
            {
                ChangeDirectory("/");
                ChangeDirectory(workingDir.Trim('/'));
            }
        }
        /// <summary>
        /// Get files with file path from remote directory
        /// </summary>
        /// <param name="absoluteDirPath">absolute path of the remote location</param>
        /// <param name="files">list of file names with full path</param>
        public void GetFilesListWithPath(string absoluteDirPath, ref IList<string> files)
        {
            string workingDir = WorkingDirectory;
            if (WorkingDirectory != BaseDir)
            {
                if (WorkingDirectory != "/")
                    ChangeDirectory("/");
                ChangeDirectory(BaseDir);
            }
            foreach (var entry in ListDirectory(absoluteDirPath))
            {

                if (entry.IsDirectory)
                {
                    //ListDirectory(entry.FullName, ref files);
                }
                else
                {
                    files.Add(entry.FullName);
                }
            }
            if (workingDir != WorkingDirectory)
            {
                ChangeDirectory("/");
                ChangeDirectory(workingDir.Trim('/'));
            }
        }
        /// <summary>
        /// If directory path is not exist then creates directory path
        /// </summary>
        /// <param name="path">local path</param>
        /// <param name="withFileName">true if given path is with file name</param>
        public static void CheckAndCreatePath(string path, bool withFileName = false)
        {
            if (withFileName)
            {
                string filename = Path.GetFileName(path);
                path = path.Replace(filename, "");
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// To check whether current working directory and change request with absolute path 
        /// whether they are same or not
        /// </summary>
        /// <param name="absoluteDirPath"></param>
        /// <returns></returns>
        private bool CheckPathSame(string absoluteDirPath)
        {
            string workingDir = WorkingDirectory;
            bool isSame = false;
            if (workingDir.Trim('/') != absoluteDirPath.Trim('/'))
            {
                if (BaseDir != "/")
                {
                    if (workingDir.Length >= BaseDir.Length)
                    {
                        workingDir = workingDir.Substring(BaseDir.Length);
                        if (workingDir.Trim('/') == absoluteDirPath.Trim('/'))
                        {
                            isSame = true;
                        }
                    }
                }
            }
            else
            {
                isSame = true;
            }
            return isSame;
        }
        /// <summary>
        /// To Change directory
        /// </summary>
        /// <exception cref="BaseDirPermissionDeniedException">
        /// throws access dinied exception if try to change path before base dir
        /// </exception>
        /// <exception cref="System.ArgumentNullException">path is null.</exception>
        /// <exception cref="Renci.SshNet.Common.SshConnectionException">Client is not connected.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPermissionDeniedException">Permission to change directory denied by remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SftpPathNotFoundException">path was not found on the remote host.</exception>
        /// <exception cref="Renci.SshNet.Common.SshException">A SSH error where System.Exception.Message is the message from the remote host.</exception>
        /// <exception cref="System.ObjectDisposedException">The method was called after the client was disposed.</exception>
        /// <param name="path">
        /// path to change directory
        /// </param>
        public new void ChangeDirectory(string path)
        {
            if (BaseDir != "/")
            {
                if (path.Contains(".."))
                {
                    List<string> bd = BaseDir.Split('/').ToList();
                    bd.RemoveAll(RemoveItem);
                    List<string> wd = WorkingDirectory.Split('/').ToList();
                    wd.RemoveAll(RemoveItem);
                    List<string> pt = path.Split('/').ToList();
                    pt.RemoveAll(RemoveItem);
                    int allowedCount = wd.Count - bd.Count;
                    if (bd.Count > 0)
                    {
                        foreach (var i in pt)
                        {
                            if (i == "..")
                            {
                                allowedCount -= 1;
                            }
                            else
                            {
                                allowedCount += 1;
                            }
                            if (allowedCount < 0)
                            {
                                throw new BaseDirPermissionDeniedException("Permission to change directory denied by base dir path");
                            }
                        }
                    }
                }
            }
            base.ChangeDirectory(path);
        }

        /// <summary>
        /// Used to remove empty strings from the list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool RemoveItem(string item)
        {
            if (item == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
