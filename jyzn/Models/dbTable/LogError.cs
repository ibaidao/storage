using System;

namespace Models
{
    /// <summary>
    /// 行为日志表
    /// </summary>
    [Serializable]
    public sealed class LogError
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        public Int32 ActorID { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public Int16 Type { get; set; }

        /// <summary>
        /// 操作步骤
        /// </summary>
        public String FunctionName { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public String Errors { get; set; }

        /// <summary>
        /// 行为时间
        /// </summary>
        public DateTime ActionTime { get; set; }
    }
}