package com.jyzn.common;

import java.io.IOException;

public interface IoReader {

	/**
	 * ��ȡһ֡����
	 * @return
	 * @throws IOException 
	 */
	public Frame readFrameData(ProtocolConfiguration configuration) throws IOException;
	/**
	 * ��ȡһ֡����
	 * @param frame
	 * @return
	 * @throws IOException 
	 */
	public boolean readFrameData(Frame frame) throws IOException;
}
