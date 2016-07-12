using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Utilities
{
    public partial class Security
    {
        #region MD5
        /// <summary>
        /// 对某个字符串进行加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Decrypt(string input)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(input);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));

            }
            return sb.ToString();
        }
        /// <summary>
        /// 对某个字符串进行加密 - OR
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5DecryptUKey(string input)
        {
            var des = DESEncrypt(input + ConfigurationManager.AppSettings["Key"]);
            return Md5Decrypt(des);
        }
        #endregion
    }
}
