using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Exceptions
{
    /// <summary>
    /// Base direcotry exception class
    /// </summary>
    public class BaseDirPermissionDeniedException : Exception
    {
        /// <summary>
        /// Base Dir Permissions denied exception constructor
        /// </summary>
        /// <param name="message"></param>
        public BaseDirPermissionDeniedException(string message) : base(message)
        {

        }
    }
}
