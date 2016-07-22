package com.jyzn.tcp;

import java.io.IOException;
import java.io.InputStream;

import com.jyzn.common.CommonReader;
import com.jyzn.common.Frame;
import com.jyzn.common.ProtocolConfiguration;
import com.jyzn.utils.ChecksumUtils;

/**
 * 
 * @author Leej 2016Äê7ÔÂ19ÈÕ
 *
 */
public class SocketReader extends CommonReader {
	private Frame frame;
	private ProtocolConfiguration configuration;
	
	public SocketReader(InputStream inputStream) {
		super(inputStream);
		// TODO Auto-generated constructor stub
	}
	
	public void createFrame(ProtocolConfiguration configuration) {
		this.configuration = configuration;
		frame = new Frame(configuration);
	}
	
	public void dispose() {
		frame.dispose();
	}

	@Override
	public boolean doReaderTask() {
		// TODO Auto-generated method stub
		if (frame == null) {
			createFrame(configuration == null? ProtocolConfiguration.getDefaultProtocolConfiguration() : configuration);
		}
		boolean suc = false;
		boolean disconnect = false;
		try {
			suc = readFrameData(frame);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			disconnect = true;
		}
		disconnect |= isEndOfStream();
		if (disconnect) {
			return false;
		}
		
		boolean checksumCorrect = false;
		if (suc) {
			int checksum = ChecksumUtils.crc16(frame.getFrameData(), frame.getHeader().length, frame.getFrameLength() - frame.getHeader().length - configuration.getChecksumBytes());
			int recvChecksum = (frame.getCRCFiled() & 0xFFFF);
			if (checksum == recvChecksum) {
				checksumCorrect = true;
			}
			else {
				System.out.println("checksum error");
			}
		}
		
		rawDataReceived(frame.getFrameData(), frame.getFrameLength(), checksumCorrect);
		frameReceived(frame, checksumCorrect);
		
		frame.reset();
		return true;
	}
}
