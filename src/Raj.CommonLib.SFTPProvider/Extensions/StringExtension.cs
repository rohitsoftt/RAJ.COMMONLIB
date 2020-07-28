using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raj.CommonLib.SFTPProvider.Extensions
{
    /// <summary>
    /// String extension method
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Checks string contains char
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A boolean value, true if null or only spaces else returns false</returns>
        public static bool IsNullOrWhiteSpaces(this string value)
        {
            //Removes leading and trailing
            string newValue = value.Trim(' ');
            if (string.IsNullOrEmpty(newValue))
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
