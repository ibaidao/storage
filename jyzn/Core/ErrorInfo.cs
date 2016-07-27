using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 异常信息描述
    /// </summary>
    public struct ErrorDescription
    {
        private const string UnKownTrouble = "未识别的异常";
        private const string DatabaseHandler = "数据库执行失败";
        private const string AddDuplicateItem = "增加重复记录";

        /// <summary>
        /// 解析错误编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ExplainCode(ErrorCode code)
        {
            string result = string.Empty;
            switch (code)
            {
                case ErrorCode.UnKownTrouble: result = UnKownTrouble; break;
                case ErrorCode.DatabaseHandler: result = DatabaseHandler; break;
                case ErrorCode.AddDuplicateItem: result = AddDuplicateItem; break;
                default: break;
            }
            return result;
        }
    }

    /// <summary>
    /// 异常信息编码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 无异常
        /// </summary>
        OK = 0x00,

        UnKownTrouble ,

        DatabaseHandler = 0x300,

        AddDuplicateItem = 0x301
    }
}
