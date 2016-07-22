package com.jyzn.common;

import java.io.IOException;

public interface IoReader {

	/**
	 * 读取一帧数据
	 * @return
	 * @throws IOException 
	 */
	public Frame readFrameData(ProtocolConfiguration configuration) throws IOException;
	/**
	 * 读取一帧数据
	 * @param frame
	 * @return
	 * @throws IOException 
	 */
	public boolean readFrameData(Frame frame) throws IOException;
}
