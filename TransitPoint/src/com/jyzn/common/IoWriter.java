package com.jyzn.common;

import java.io.IOException;

public interface IoWriter {
	/**
	 * 写入数据
	 * @param data
	 */
	public void write(byte[] data) throws IOException;
	
	/**
	 * 写入数据
	 * @param data
	 * @param offset
	 * @param length
	 */
	public void write(byte[] data, int offset, int length) throws IOException;
	
	/**
	 * 写入一帧数据
	 * @param frame
	 */
	public void writeFrame(Frame frame) throws IOException;
	
}
