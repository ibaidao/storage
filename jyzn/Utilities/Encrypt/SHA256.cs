using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Utilities
{
    public partial class Security
    {
        #region SHA256
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns></returns>
        public static string SHA256(string regNum)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一个SHA256
            byte[] source = Encoding.Default.GetBytes(regNum);//将字符串转为Byte[]
            byte[] crypto = sha256.ComputeHash(source);//进行SHA256加密
            return Convert.ToBase64String(crypto);//把加密后的字符串从Byte[]转为字符串
        }
        #endregion
    }
}
