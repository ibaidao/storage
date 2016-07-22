package com.jyzn.common;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Iterator;

/**
 * 
 * @author Leej 2016年7月19日
 *
 */
public abstract class CommonReader implements IoReader {
	private IDisconnectedListener disconnectedListener;
	protected HashMap<String, IRawDataReceivedListener> recvListeners = new HashMap<>();
	protected HashMap<String, IFrameReceivedListener> frameReceivedListener = new HashMap<>();
	protected InputStream in;
	protected boolean isReaderStart = false;
	protected boolean isDone = false;
	private Thread readThread = null;
	protected boolean endOfStream = false;
	
	public CommonReader(InputStream inputStream) {
		this.in = inputStream;
	}
	
	public void setDisconnectedListener(IDisconnectedListener disconnectedListener) {
		this.disconnectedListener = disconnectedListener;
	}
	
	public void addRawDataReceivedListener(String key, IRawDataReceivedListener listener) {
		if (!recvListeners.containsKey(key)) {
			recvListeners.put(key, listener);
		}
	}
	
	public void removeRawDataReceivedListener(String key) {
		if (recvListeners.containsKey(key))
			recvListeners.remove(key);
	}
	
	public void addFrameReceivedListener(String key, IFrameReceivedListener listener) {
		if (!frameReceivedListener.containsKey(key))
			frameReceivedListener.put(key, listener);
	}
	
	public void removeFrameReceivedListener(String key) {
		if (frameReceivedListener.containsKey(key))
			frameReceivedListener.remove(key);
	}
	
	public boolean isEndOfStream() {
		return endOfStream;
	}
	
	public void startReader() {
		if (isReaderStart)
			return;
		isDone = false;
		readThread = new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				read(readThread);
			}
		});
		readThread.setName("IO Reader: " + getClass().getSimpleName());
		readThread.start();
		isReaderStart = true;
		System.out.println("reader start");
	}
	
	public void stopReader() {
		isDone = true;
		if (readThread != null) {
			readThread.interrupt();
			try {
				readThread.join(500);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			readThread = null;
		}
		
		isReaderStart = false;
	}
	
	@Override
	public Frame readFrameData(ProtocolConfiguration configuration) throws IOException {
		// TODO Auto-generated method stub
		Frame frame = new Frame(configuration);
		boolean readSuc = readFrameData(frame);
		if (!readSuc)
			frame = null;
		return frame;
	}

	/**
	 * 读取一帧数据。会阻塞直到一帧数据读完，或数据流结束，或异常出现。
	 * @throws IOException 
	 */
	@Override
	public boolean readFrameData(Frame frame) throws IOException {
		// TODO Auto-generated method stub
		char[] headers = frame.getConfiguration().getFrameHeader();
		int readDataLen = 0;
		int dataLen = 0;
		int headerIndex = 0;
		boolean readSuc = false;
		int data = 0;
		RecvState recvState = RecvState.RECV_IDLE;
		endOfStream = false;
		while (!readSuc) {
			data = in.read();
			if (data == -1) {
				endOfStream = true;
				readSuc = false;
				break;
			}
			
			switch (recvState) {
			case RECV_IDLE:
				if (data == headers[0]) {
					recvState = RecvState.RECV_HEADER;
					headerIndex ++;
					if (headerIndex == headers.length) {//包头就一个字节的情况
						recvState = RecvState.RECV_LENGTH_HIGH;
						headerIndex = 0;
					}
				}
				else {
					headerIndex = 0;
				}
				break;
			case RECV_HEADER:
				if (data == headers[headerIndex ++]) {
					if (headerIndex == headers.length) {
						recvState = RecvState.RECV_LENGTH_HIGH;
						headerIndex = 0;
					}
				}
				else {
					recvState = RecvState.RECV_IDLE;
					headerIndex = 0;
				}
				break;
			case RECV_LENGTH_HIGH:
				dataLen = (data << 8) & 0xFF00;
				recvState = RecvState.RECV_LENGTH_LOW;
				frame.put((byte)data);
				break;
			case RECV_LENGTH_LOW:
				dataLen |= (data & 0x00FF);
				recvState = RecvState.RECV_DATA;
				frame.put((byte)data);
				System.out.println("datas length: " + dataLen);
				break;
			case RECV_DATA:
				frame.put((byte)data);
				readDataLen ++;
				if (readDataLen == dataLen) {
					readSuc = true;
					recvState = RecvState.RECV_IDLE;
				}
				break;
			}
		}
		
		return readSuc;
	}
	
	protected enum RecvState {
		RECV_HEADER,
		RECV_LENGTH_HIGH,
		RECV_LENGTH_LOW,
		RECV_DATA,
		RECV_IDLE;
	}
	
	protected void rawDataReceived(byte[] data, int length, boolean check) {
		if (recvListeners != null && recvListeners.size() > 0) {
			Iterator<IRawDataReceivedListener> iterator = recvListeners.values().iterator();
			while (iterator.hasNext()) {
				IRawDataReceivedListener listener = iterator.next();
				listener.onRawDataReceived(data, length, check);
			}
		}
	}
	
	protected void frameReceived(Frame frame, boolean check) {
		if (frameReceivedListener != null && frameReceivedListener.size() > 0) {
			for (IFrameReceivedListener listener : frameReceivedListener.values()) {
				listener.onFrameReceived(frame, check);
			}
		}
	}
	
	/**
	 * 如果读数据业务出现了异常或达到数据流的结尾，返回false代表与服务器（对方）断开了连接。
	 * @return
	 */
	public abstract boolean doReaderTask();
	
	private void read(Thread thread) {
		while (!isDone && thread == readThread && !readThread.isInterrupted()) {
			boolean suc = doReaderTask();
			if (!suc) {
				if (disconnectedListener != null)
					disconnectedListener.onDisconnected();
				break;
			}
		}
	}
}
