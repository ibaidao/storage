using System;
using System.Collections.Generic;
using Models;

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
        public const Byte PROTOCOL_REMARK_START = 0x3C;

        /// <summary>
        /// 起始标志位总字节数
        /// </summary>
        private const Int16 PROTOCOL_START_END_REMARK = 1;

        /// <summary>
        /// 包数据长度占用字节数
        /// </summary>
        public const Int16 PROTOCOL_PACKAGE_SIZE_BYTES = 2;

        /// <summary>
        /// 每次通讯发送命令数
        /// </summary>
        private const Int16 FUNCTION_COUNT_ONCE = 4;

        /// <summary>
        /// 协议头保留字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_RESERVE_BYTES = 2;

        /// <summary>
        /// 协议头每个命令字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_FUNCTION_BYTES = 3;

        /// <summary>
        /// 协议头总字节数
        /// </summary>
        private const Int16 PROTOCOL_HEAD_BYTES_COUNT = PROTOCOL_HEAD_RESERVE_BYTES + PROTOCOL_HEAD_FUNCTION_BYTES * FUNCTION_COUNT_ONCE;

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
        /// 是否需要回执的数据位（低字节的低位第二位）
        /// </summary>
        private const Int16 PROTOCOL_ANSWER_FLAG_POSITION = 2;
        #endregion

        /// <summary>
        /// 解码单次通信数据主体
        /// </summary>
        /// <param name="data">数据字节流(从文件属性开始)</param>
        /// <param name="info">协议对象（已实例化）</param>
        /// <returns>是否解码成功</returns>
        public static bool DecodeByteData(Protocol info, byte[] data)
        {
            if (!Utilities.Crc.CheckCRC(data, 0, data.Length - PROTOCOL_PARITY_BYTES))
            {
                Logger.WriteLog("数据校验失败");
                return false;
            }

            DecodeInfo(info, data, data.Length);
            return true;
        }

        /// <summary>
        /// 对象编码为数据流
        /// </summary>
        /// <param name="info">待编码对象</param>
        /// <param name="data">字节流</param>
        public static void EncodeByteData(Protocol info, ref byte[] data)
        {
            EncodeInfo(info, ref data);

            int noCheckByte = PROTOCOL_START_END_REMARK + PROTOCOL_PACKAGE_SIZE_BYTES;//不参与校验的字节头
            int checkLocation = info.ByteCount - PROTOCOL_PARITY_BYTES;//无校验码的总长度
            int crcCode = Utilities.Crc.CRC16(data, noCheckByte, checkLocation - noCheckByte);
            data[checkLocation] = (byte)(crcCode >> 8);
            data[checkLocation + 1] = (byte)crcCode;
        }

        /// <summary>
        /// 解码单次通信数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="data"></param>
        /// <param name="dataCount">总字节数</param>
        private static void DecodeInfo(Protocol info, byte[] data, int dataCount)
        {
            short byteHeadIdx = PROTOCOL_HEAD_RESERVE_BYTES, byteBodyIdx = PROTOCOL_HEAD_BYTES_COUNT ;
            int nodeCount, contentLength = 0;
            //数据头解析
            info.NeedAnswer = (data[byteHeadIdx - 1] & 1 << (PROTOCOL_ANSWER_FLAG_POSITION - 1)) > 0 ;
            info.ByteCount = dataCount;
            info.FunList = new List<Function>(FUNCTION_COUNT_ONCE);
            for (int i = 0; i < FUNCTION_COUNT_ONCE; i++)
            {
                contentLength = data[byteHeadIdx + 1] << 8 | data[byteHeadIdx + 2];
                nodeCount = (contentLength - PROTOCOL_BODY_PRE_BYTES) / PROTOCOL_BODY_LOCATION_DIMENSION_BYTES;
                if (nodeCount < 1) continue;

                Function func = new Function();
                func.Code = (FunctionCode)data[byteHeadIdx];
                func.DataCount = (short)contentLength;
                byteHeadIdx += PROTOCOL_HEAD_FUNCTION_BYTES;
                //数据内容解析
                func.TargetInfo = data[byteBodyIdx] << 8 | data[byteBodyIdx + 1];
                byteBodyIdx += PROTOCOL_BODY_PRE_BYTES;
                func.PathPoint = new List<Location>();
                
                for (int j = 0; j < nodeCount; j++)
                {
                    Location loc = new Location();
                    loc.XPos = data[byteBodyIdx] << 8 | data[byteBodyIdx + 1];
                    loc.YPos = data[byteBodyIdx + 2] << 8 | data[byteBodyIdx + 3];
                    loc.ZPos = data[byteBodyIdx + 4];
                    func.PathPoint.Add(loc);
                    byteBodyIdx += PROTOCOL_BODY_LOCATION_DIMENSION_BYTES;
                }
                info.FunList.Add(func);
            }
        }

        /// <summary>
        /// 对象编码为数据流
        /// </summary>
        /// <param name="info">待编码对象</param>
        /// <param name="data">字节流</param>
        private static void EncodeInfo(Protocol info,ref byte[] data)
        {
            //准备缓存区
            int dataCount = PROTOCOL_HEAD_BYTES_COUNT;
            int byteCount, noCheckByte = PROTOCOL_START_END_REMARK + PROTOCOL_PACKAGE_SIZE_BYTES;
            for (int i = 0; i < info.FunList.Count; i++)
            {
                info.FunList[i].DataCount = (short)(info.FunList[i].PathPoint.Count * 5 + 2);
                dataCount += info.FunList[i].DataCount;
            }
            dataCount += PROTOCOL_PARITY_BYTES;
            byteCount = noCheckByte + dataCount;
            info.ByteCount = byteCount;
            if (data == null || data.Length < byteCount)
                data = new byte[byteCount];

            //开始编码
            int byteHeadIdx = 0, byteBodyIdx = noCheckByte + PROTOCOL_HEAD_BYTES_COUNT;
            data[byteHeadIdx] = PROTOCOL_REMARK_START;
            data[byteHeadIdx + 1] = (byte)(dataCount >> 8);
            data[byteHeadIdx + 2] = (byte)dataCount;
            byteHeadIdx += PROTOCOL_START_END_REMARK + PROTOCOL_PACKAGE_SIZE_BYTES;
            data[byteHeadIdx + 1] |= (byte)(info.NeedAnswer ? 1 << (PROTOCOL_ANSWER_FLAG_POSITION - 1) : 0);
            byteHeadIdx += PROTOCOL_HEAD_RESERVE_BYTES; 
            for (int i = 0; i < info.FunList.Count; i++)
            {
                data[byteHeadIdx] = (byte)info.FunList[i].Code;
                data[byteHeadIdx + 1] = (byte)(info.FunList[i].DataCount >> 8);
                data[byteHeadIdx + 2] = (byte)(info.FunList[i].DataCount);
                EncodeBody(info.FunList[i], data, byteBodyIdx);

                byteHeadIdx += PROTOCOL_HEAD_FUNCTION_BYTES;
                byteBodyIdx += info.FunList[i].DataCount;
            }
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

            if (func.PathPoint == null || func.PathPoint.Count == 0)
                return;

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
