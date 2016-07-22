package com.jyzn.utils;

public class Tools {

	public static short getShort(byte[] array, int startPos) {
		
		short value = (short) ((array[startPos] << 8) & 0xFF00);
		value |= (array[startPos + 1] & 0x00FF);
		
		return value;
	}
	
	public static void putShort(byte[] array, int pos, int value) {
		array[pos] = (byte) ((value >> 8)  & 0x00FF);
		array[pos + 1] = (byte) (value & 0x00FF);
	}
}
