using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public struct Protocol
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
        /// 协议数据位字节数
        /// </summary>
        public Int16 BodyByteCount
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
