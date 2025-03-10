package com.jyzn.utils;

import java.util.zip.CRC32;

public class ChecksumUtils {
	private static int[] crc16Table = {0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7, 0x8108, 0x9129, 0xa14a,  
            0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef, 0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6, 0x9339, 0x8318, 0xb37b,  
            0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de, 0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485, 0xa56a, 0xb54b, 0x8528,  
            0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d, 0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4, 0xb75b, 0xa77a, 0x9719,  
            0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc, 0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823, 0xc9cc, 0xd9ed, 0xe98e,  
            0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b, 0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12, 0xdbfd, 0xcbdc, 0xfbbf,  
            0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a, 0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41, 0xedae, 0xfd8f, 0xcdec,  
            0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49, 0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70, 0xff9f, 0xefbe, 0xdfdd,  
            0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78, 0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f, 0x1080, 0x00a1, 0x30c2,  
            0x20e3, 0x5004, 0x4025, 0x7046, 0x6067, 0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e, 0x02b1, 0x1290, 0x22f3,  
            0x32d2, 0x4235, 0x5214, 0x6277, 0x7256, 0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d, 0x34e2, 0x24c3, 0x14a0,  
            0x0481, 0x7466, 0x6447, 0x5424, 0x4405, 0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c, 0x26d3, 0x36f2, 0x0691,  
            0x16b0, 0x6657, 0x7676, 0x4615, 0x5634, 0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab, 0x5844, 0x4865, 0x7806,  
            0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3, 0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a, 0x4a75, 0x5a54, 0x6a37,  
            0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92, 0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9, 0x7c26, 0x6c07, 0x5c64,  
            0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1, 0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8, 0x6e17, 0x7e36, 0x4e55,  
            0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0};
	
	private static int[] crc16Table2 = {0xF078, 0xE1F1, 0xD36A, 0xC2E3, 0xB65C, 0xA7D5,
            0x954E, 0x84C7, 0x7C30, 0x6DB9, 0x5F22, 0x4EAB, 0x3A14, 0x2B9D,
            0x1906, 0x088F, 0xE0F9, 0xF170, 0xC3EB, 0xD262, 0xA6DD, 0xB754,
            0x85CF, 0x9446, 0x6CB1, 0x7D38, 0x4FA3, 0x5E2A, 0x2A95, 0x3B1C,
            0x0987, 0x180E, 0xD17A, 0xC0F3, 0xF268, 0xE3E1, 0x975E, 0x86D7,
            0xB44C, 0xA5C5, 0x5D32, 0x4CBB, 0x7E20, 0x6FA9, 0x1B16, 0x0A9F,
            0x3804, 0x298D, 0xC1FB, 0xD072, 0xE2E9, 0xF360, 0x87DF, 0x9656,
            0xA4CD, 0xB544, 0x4DB3, 0x5C3A, 0x6EA1, 0x7F28, 0x0B97, 0x1A1E,
            0x2885, 0x390C, 0xB27C, 0xA3F5, 0x916E, 0x80E7, 0xF458, 0xE5D1,
            0xD74A, 0xC6C3, 0x3E34, 0x2FBD, 0x1D26, 0x0CAF, 0x7810, 0x6999,
            0x5B02, 0x4A8B, 0xA2FD, 0xB374, 0x81EF, 0x9066, 0xE4D9, 0xF550,
            0xC7CB, 0xD642, 0x2EB5, 0x3F3C, 0x0DA7, 0x1C2E, 0x6891, 0x7918,
            0x4B83, 0x5A0A, 0x937E, 0x82F7, 0xB06C, 0xA1E5, 0xD55A, 0xC4D3,
            0xF648, 0xE7C1, 0x1F36, 0x0EBF, 0x3C24, 0x2DAD, 0x5912, 0x489B,
            0x7A00, 0x6B89, 0x83FF, 0x9276, 0xA0ED, 0xB164, 0xC5DB, 0xD452,
            0xE6C9, 0xF740, 0x0FB7, 0x1E3E, 0x2CA5, 0x3D2C, 0x4993, 0x581A,
            0x6A81, 0x7B08, 0x7470, 0x65F9, 0x5762, 0x46EB, 0x3254, 0x23DD,
            0x1146, 0x00CF, 0xF838, 0xE9B1, 0xDB2A, 0xCAA3, 0xBE1C, 0xAF95,
            0x9D0E, 0x8C87, 0x64F1, 0x7578, 0x47E3, 0x566A, 0x22D5, 0x335C,
            0x01C7, 0x104E, 0xE8B9, 0xF930, 0xCBAB, 0xDA22, 0xAE9D, 0xBF14,
            0x8D8F, 0x9C06, 0x5572, 0x44FB, 0x7660, 0x67E9, 0x1356, 0x02DF,
            0x3044, 0x21CD, 0xD93A, 0xC8B3, 0xFA28, 0xEBA1, 0x9F1E, 0x8E97,
            0xBC0C, 0xAD85, 0x45F3, 0x547A, 0x66E1, 0x7768, 0x03D7, 0x125E,
            0x20C5, 0x314C, 0xC9BB, 0xD832, 0xEAA9, 0xFB20, 0x8F9F, 0x9E16,
            0xAC8D, 0xBD04, 0x3674, 0x27FD, 0x1566, 0x04EF, 0x7050, 0x61D9,
            0x5342, 0x42CB, 0xBA3C, 0xABB5, 0x992E, 0x88A7, 0xFC18, 0xED91,
            0xDF0A, 0xCE83, 0x26F5, 0x377C, 0x05E7, 0x146E, 0x60D1, 0x7158,
            0x43C3, 0x524A, 0xAABD, 0xBB34, 0x89AF, 0x9826, 0xEC99, 0xFD10,
            0xCF8B, 0xDE02, 0x1776, 0x06FF, 0x3464, 0x25ED, 0x5152, 0x40DB,
            0x7240, 0x63C9, 0x9B3E, 0x8AB7, 0xB82C, 0xA9A5, 0xDD1A, 0xCC93,
            0xFE08, 0xEF81, 0x07F7, 0x167E, 0x24E5, 0x356C, 0x41D3, 0x505A,
            0x62C1, 0x7348, 0x8BBF, 0x9A36, 0xA8AD, 0xB924, 0xCD9B, 0xDC12,
            0xEE89, 0xFF00};
	
	public static int crc16_2(byte[] datas, int offset, int length)	 {
		int crc16 = 0;
		for (int i = offset; i < length + offset; i ++) {
			crc16 = (crc16 >> 8) ^ crc16Table2[(crc16 ^ datas[i]) & 0xFF];
		}
		return crc16;
	}
	
	public static int crc16(byte[] datas, int offset, int length) {
		int crc16 = 0;
		for (int i = offset; i < length + offset; i ++) {
			crc16 = (crc16 >> 8) ^ crc16Table[(crc16 ^ datas[i]) & 0xFF];
		}
		return crc16;
	}
	
	public static short crc16_3(byte[] datas, int offset, int length) {
		int crc = 0;
		int xDaPoly = 0;
		int j = 0, xDaBit = 0;
		xDaPoly = 0xA001;//x ** 16 + x ** 15 + x ** 2 + 1
		
		for (int i = offset; i < offset + length; i ++) {
			crc ^= datas[i];
			for (j = 0; j < 8; j ++) {
				xDaBit = (int)(crc & 0x01);
				crc >>= 1;
				if (xDaBit == 1) {
					crc ^= xDaPoly;
				}
			}
		}
		
		return (short)(crc & 0xFFFF);
	}
	
	public static long crc32(byte[] datas) {
		return crc32(datas, 0, datas.length);
	}

	public static long crc32(byte[] datas, int offset, int length)	{
		CRC32 crc32 = new CRC32();
		crc32.update(datas, offset, length);
		return crc32.getValue();
	}
	
	public static byte bcc(byte[] datas, int offset, int length) {
		byte bcc = 0;
		for (int i = offset; i < offset + length; i ++) {
			bcc ^= datas[i];
		}
		
		return bcc;
	}
}
