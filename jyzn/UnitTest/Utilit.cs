using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;

namespace UnitTest
{
    /// <summary>
    /// 工具类测试
    /// </summary>
    [TestClass]
    public class Utilit
    {
        [TestMethod]
        public void Encrypt()
        {
            string source = "Hello world";

            string target= Security.Base64Encrypt(source);
            Assert.AreEqual<string>(source, Security.Base64Decrypt(target));

            target = Security.DESEncrypt(source);
            Assert.AreEqual<string>(source, Security.DESDecrypt(target));

            target = Security.Md5Decrypt(source);
            Assert.AreEqual<string>(target, "3e25960a79dbc69b674cd4ec67a72c62");
        }

        [TestMethod]
        public void Regex()
        {
            string num="4343434343", unNum="Ad44";
            Assert.IsTrue(Regexp.IsNumeric(num));
            Assert.IsFalse(Regexp.IsNumeric(unNum));
            Assert.IsFalse(Regexp.IsChineseWords(unNum));
        }

        [TestMethod]
        public void Log()
        {
            Core.Logger.WriteLog(string.Format("单元测试{0}", Models.ErrorDescription.ExplainCode(Models.ErrorCode.OK)));
        }
    }
}
