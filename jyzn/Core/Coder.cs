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
        #region 协议中常量定义
        /// <summary>
        /// 协议起止标志
        /// </summary>
        public const Byte PROTOCOL_REMARK_START = 0x3C, PROTOCOL_REMARK_END = 0x3E;

        /// <summary>
        /// 协议头总字节数
        /// </summary>
        public const Int16 PROTOCOL_HEAD_BYTES_COUNT = PROTOCOL_HEAD_RESERVE_BYTES + PROTOCOL_HEAD_FUNCTION_BYTES * FUNCTION_COUNT_ONCE;

        /// <summary>
        /// 每次通讯最多可发送命令数
        /// </summary>
        private const Int16 FUNCTION_COUNT_ONCE = 4;

        /// <summary>
        /// 协议头保留总字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_RESERVE_BYTES = 2;

        /// <summary>
        /// 协议头每个命令字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_FUNCTION_BYTES = 3;

        /// <summary>
        /// 协议数据坐标位置前字节数
        /// </summary>
        private const Int16 PROTOCOL_BODY_PRE_BYTES = 2;

        /// <summary>
        /// 每个位置占用字节总数
        /// </summary>
        private const Int16 PROTOCOL_BODY_LOCATION_DIMENSION_BYTES = 5;

        /// <summary>
        /// 每个位置显示维度总数
        /// </summary>
        private const Int16 PROTOCOL_BODY_LOCATION_DIMENSION_COUNT = 3;

        /// <summary>
        /// 协议校验码字节数
        /// </summary>
        private const Int16 PROTOCOL_PARITY_BYTES = 2;

        /// <summary>
        /// 起止标志位总字节数
        /// </summary>
        private const Int16 PROTOCOL_START_END_REMARK = 1;
        #endregion

        /// <summary>
        /// 解码单次通信数据头
        /// </summary>
        /// <param name="data">14字节的文件头</param>
        /// <returns></returns>
        public static Protocol DecodeHead(byte[] data)
        {
            short byteIdx = PROTOCOL_START_END_REMARK + PROTOCOL_HEAD_RESERVE_BYTES;
            Protocol info = new Protocol();

            info.BodyByteCount = 0;
            info.FunList = new List<Function>(FUNCTION_COUNT_ONCE);
            for (int i = 0; i < FUNCTION_COUNT_ONCE; i++)
            {
                Function f = new Function();
                f.Code = (FunctionCode)data[byteIdx];
                f.DataCount = (short)(data[byteIdx + 1] << 8 | data[byteIdx + 2]);

                info.BodyByteCount += f.DataCount;
                info.FunList.Add(f);

                byteIdx += PROTOCOL_HEAD_FUNCTION_BYTES;
            }

            return info;
        }

        /// <summary>
        /// 解码单次通信数据主体
        /// </summary>
        /// <param name="data">数据字节流</param>
        /// <param name="info">协议对象（已实例化）</param>
        /// <returns>是否解码成功</returns>
        public static bool DecodeBody(byte[] data, Protocol info)
        {
            int byteIdx = PROTOCOL_START_END_REMARK + PROTOCOL_HEAD_BYTES_COUNT;
            if (!Utilities.Crc.CheckCRC(data, byteIdx + info.BodyByteCount))
            {
                Logger.WriteLog("数据校验失败");
                return false;
            }
            Location loc;

            for (int i = 0; i < info.FunList.Count; i++)
            {
                Function func = info.FunList[i];
                func.TargetInfo = data[byteIdx] << 8 | data[byteIdx + 1];
                byteIdx += PROTOCOL_BODY_PRE_BYTES;

                func.PathPoint = new List<Location>();
                int nodeCount = (func.DataCount - PROTOCOL_BODY_PRE_BYTES) / PROTOCOL_BODY_LOCATION_DIMENSION_BYTES;

                for (int j = 0; j < nodeCount; j++)
                {
                    loc = new Location();
                    loc.XPos = data[byteIdx] << 8 | data[byteIdx + 1];
                    loc.YPos = data[byteIdx + 2] << 8 | data[byteIdx + 3];
                    loc.ZPos = data[byteIdx + 4];
                    func.PathPoint.Add(loc);

                    byteIdx += PROTOCOL_BODY_LOCATION_DIMENSION_BYTES;
                }
            }
            return true;
        }

        /// <summary>
        /// 对象编码为数据流
        /// </summary>
        /// <param name="info">待编码对象</param>
        /// <param name="data">字节流</param>
        public static void EncodeInfo(Protocol info, byte[] data)
        {
            //准备缓存区
            int byteCount = PROTOCOL_HEAD_BYTES_COUNT;
            for (int i = 0; i < info.FunList.Count; i++)
            {
                byteCount += info.FunList[i].DataCount;
            }
            byteCount += PROTOCOL_PARITY_BYTES;
            byteCount += PROTOCOL_START_END_REMARK * 2;
            if (data == null || data.Length < byteCount)
                data = new byte[byteCount];
            //开始编码
            int byteHeadIdx = 0, byteBodyIdx = PROTOCOL_START_END_REMARK + PROTOCOL_HEAD_BYTES_COUNT;
            data[byteHeadIdx] = PROTOCOL_REMARK_START;
            byteHeadIdx += PROTOCOL_START_END_REMARK + PROTOCOL_HEAD_RESERVE_BYTES;
            for (int i = 0; i < info.FunList.Count; i++)
            {
                data[byteHeadIdx] = (byte)info.FunList[i].Code;
                data[byteHeadIdx + 1] = (byte)(info.FunList[i].DataCount >> 8);
                data[byteHeadIdx + 2] = (byte)(info.FunList[i].DataCount);

                EncodeBody(info.FunList[i], data, byteBodyIdx);

                byteHeadIdx += PROTOCOL_HEAD_FUNCTION_BYTES;
                byteBodyIdx += info.FunList[i].DataCount;
            }
            int crcCode = Utilities.Crc.CRC16(data, byteCount - PROTOCOL_PARITY_BYTES - PROTOCOL_START_END_REMARK);
            data[byteBodyIdx] = (byte)(crcCode >> 8);
            data[byteBodyIdx + 1] = (byte)crcCode;
            data[byteBodyIdx + 2] = PROTOCOL_REMARK_END;
        }

        /// <summary>
        /// 为单个功能编码
        /// </summary>
        /// <param name="func"></param>
        /// <param name="data"></param>
        /// <param name="start">距离前面字节数</param>
        private static void EncodeBody(Function func, byte[] data, int start)
        {
            int byteIdx = start;
            data[byteIdx] = (byte)(func.TargetInfo >> 8);
            data[byteIdx + 1] = (byte)func.TargetInfo;
            byteIdx += PROTOCOL_BODY_PRE_BYTES;
            for (int i = 0; i < func.PathPoint.Count; i++)
            {
                data[byteIdx] = (byte)(func.PathPoint[i].XPos >> 8);
                data[byteIdx + 1] = (byte)func.PathPoint[i].XPos;

                data[byteIdx + 2] = (byte)(func.PathPoint[i].YPos >> 8);
                data[byteIdx + 3] = (byte)func.PathPoint[i].YPos;

                data[byteIdx + 4] = (byte)func.PathPoint[i].ZPos;

                byteIdx += PROTOCOL_BODY_LOCATION_DIMENSION_BYTES;
            }
        }
    }
}
