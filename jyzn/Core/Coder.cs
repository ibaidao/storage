using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 协议编码解码
    /// </summary>
    public class Coder
    {
        /// <summary>
        /// 每次通讯最多可发送命令数
        /// </summary>
        private const Int16 FUNCTION_COUNT_ONCE = 4;

        /// <summary>
        /// 协议头属性总字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_BYTES = 2;

        /// <summary>
        /// 协议头每个命令字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_FUNCTION_BYTES = 3;

        /// <summary>
        /// 协议校验码字节数
        /// </summary>
        private const Int16 PROTOCOL_PARITY_BYTES = 1;

        /// <summary>
        /// 解码文件头
        /// </summary>
        /// <param name="data">14字节的文件头</param>
        /// <returns></returns>
        public static Protocol DecodeHead(byte[] data)
        {
            short byteIdx = PROTOCOL_HEAD_BYTES;
            Protocol info = new Protocol();

            info.BodyByteCount = 0;
            info.FunList = new List<Function>(FUNCTION_COUNT_ONCE);
            for (int i = 0; i < FUNCTION_COUNT_ONCE; i++)
            {
                Function f = new Function();
                f.Code = (FunctionCode)data[byteIdx];
                f.DataCount = data[byteIdx + 1];
                f.DataCount = (short)(f.DataCount << 8 + data[byteIdx + 2]);

                info.BodyByteCount += f.DataCount;
                info.FunList.Add(f);
            }

            return info;
        }
    }
}
