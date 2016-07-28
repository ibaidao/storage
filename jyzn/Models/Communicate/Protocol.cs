using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 单个数据包格式
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// 协议命令信息
        /// </summary>
        public List<Function> FunList
        {
            get;
            set;
        }

        /// <summary>
        /// 协议总字节数（自动统计）
        /// </summary>
        public Int32 ByteCount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需要回执
        /// </summary>
        public Boolean NeedAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// 通讯数据（前期调试）
        /// </summary>
        public byte[] SourceStream
        {
            get;
            set;
        }
    }
}
