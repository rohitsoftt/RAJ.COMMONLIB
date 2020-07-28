using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider
{
    /// <summary>
    /// SFTPClientProvider interface
    /// </summary>
    public interface ISFTPClientProvider
    {
        /// <summary>
        /// Use to upload file
        /// </summary>
        /// <param name="localFilePath"></param>
        /// <param name="localFileName"></param>
        /// <param name="remoteFileName"></param>
        /// <returns></returns>
        void UploadFile(string localFilePath, string localFileName, string remoteFileName = null);
        /// <summary>
        /// Use to upload file
        /// </summary>
        /// <param name="localFileNameWithPath"></param>
        /// <param name="remoteFileName"></param>
        /// <returns></returns>
        void UploadFile(string localFileNameWithPath, string remoteFileName = null);
        /// <summary>
        /// Use to download file
        /// </summary>
        /// <param name="remoteFileName"></param>
        /// <param name="localPath"></param>
        /// <param name="localFileName"></param>
        /// <returns></returns>
        void DownloadFile(string remoteFileName, string localPath, string localFileName = null);
        /// <summary>
        /// Use to download file
        /// </summary>
        /// <param name="remoteFileName"></param>
        /// <param name="localFileWithPath"></param>
        /// <returns></returns>
        void DownloadFile(string remoteFileName, string localFileWithPath);
        /// <summary>
        /// Use to change relative directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ChangeRelativeDirectory(string path);
        /// <summary>
        /// Use to change absolute directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ChangeAbsoluteDirectory(string path);
        /// <summary>
        /// Helps to get list of file names
        /// </summary>
        /// <param name="absoluteDirPath"></param>
        /// <param name="files"></param>
        void GetFilesList(string absoluteDirPath, ref IList<string> files);
        /// <summary>
        /// Helps to get list of files
        /// </summary>
        /// <param name="absoluteDirPath"></param>
        /// <param name="files"></param>
        void GetFilesListWithPath(string absoluteDirPath, ref IList<string> files);
    }
}
