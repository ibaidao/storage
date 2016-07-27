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
        public const string UnKownTrouble = "未识别的异常";
        public const string DatabaseHandler = "数据库执行失败";
        public const string AddDuplicateItem = "增加重复记录";
        public const string PathWithinOneAxis = "路径不能是斜线";
        public const string PathAlreadyExists = "路径已经存在";

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
                case ErrorCode.PathWithinOneAxis: result = PathWithinOneAxis; break;
                case ErrorCode.PathAlreadyExists: result = PathAlreadyExists; break;                    

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
        #region 常规通用
        /// <summary>
        /// 无异常
        /// </summary>
        OK = 0x00,

        UnKownTrouble,

        AddDuplicateItem,

        #endregion


        #region 数据库相关

        DatabaseHandler = 0x300,

        #endregion

        #region 仓库相关

        PathWithinOneAxis = 0x400,
        PathAlreadyExists
        #endregion

    }
}