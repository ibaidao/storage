using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public partial class Security
    {
        #region Base64
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            if (input == "") return "";
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            if (input == "") return "";
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }
        #endregion
    }
}
