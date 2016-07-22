package com.jyzn.common;

import java.io.IOException;

public interface IoWriter {
	/**
	 * д������
	 * @param data
	 */
	public void write(byte[] data) throws IOException;
	
	/**
	 * д������
	 * @param data
	 * @param offset
	 * @param length
	 */
	public void write(byte[] data, int offset, int length) throws IOException;
	
	/**
	 * д��һ֡����
	 * @param frame
	 */
	public void writeFrame(Frame frame) throws IOException;
	
}
