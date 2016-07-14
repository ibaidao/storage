using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Crc
    {
        /// <summary>
        /// CRC校验算法
        /// </summary>
        /// <param name="bytes">待校验数据流</param>
        /// <param name="bytesLength"></param>
        /// <returns>校验码</returns>
        public static int CRC16(byte[] bytes, int bytesLength)
        {
            byte CRC16Lo = 0, CRC16Hi = 0;
            byte CL = 0, CH = 0;
            byte SaveHi = 0, SaveLo = 0;
            int i = 0, j = 0, result = 0;

            CRC16Lo = 0xFF;
            CRC16Hi = 0xFF;
            CL = 0x01;
            CH = 0xA0;
            for (i = 0; i < bytesLength; i++)
            {
                CRC16Lo ^= bytes[i];	// 每一个数据与CRC寄存器进行异或
                for (j = 0; j <= 7; j++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi >>= 1;	//高位右移一位
                    CRC16Lo >>= 1;	//低位右移一位
                    if (0 != (SaveHi & 0x01))
                    {	//如果高位字节最后一位为1, 则低位字节右移后前面补1
                        CRC16Lo |= 0x80;
                    }
                    if (0 != (SaveLo & 0x01))
                    {	//如果LSB为1, 则与多项式码进行异或
                        CRC16Hi ^= CH;
                        CRC16Lo ^= CL;
                    }
                }
            }
            result = (int)((CRC16Lo << 8) + CRC16Hi);
            return result;
        }

        /// <summary>
        /// CRC校验数据的有效性
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytesLength"></param>
        /// <returns></returns>
        public static bool CheckCRC(byte[] bytes, int bytesLength)
        {

            int w = CRC16(bytes, bytesLength);
            int w1 = bytes[bytesLength] << 8 | bytes[bytesLength + 1];
            return w == w1;
        }
    }
}
