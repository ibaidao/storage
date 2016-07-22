package com.jyzn.common;

import java.io.IOException;
import java.io.OutputStream;
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;

/**
 * 
 * @author Leej 2016Äê7ÔÂ19ÈÕ
 *
 */
public abstract class CommonWriter implements IoWriter {
	protected OutputStream os;
	protected boolean isWriteStarted = false;
	protected boolean isDone = false;
	private Thread writeThread;
	private final BlockingQueue<Frame> queue;
	
	public CommonWriter(OutputStream outputStream) {
		this.os = outputStream;
		queue = new ArrayBlockingQueue<>(5, true);
	}
	
	public synchronized boolean sendFrame(Frame frame) {
		boolean addQueue = false;
		
		if (!isDone && frame != null) {
			addQueue = queue.offer(frame);
			
			synchronized (queue) {
				queue.notify();
			}
		}
		
		return addQueue;
	}
	
	public void startWriter() {
		if (isWriteStarted)
			return;
		isDone = false;
		writeThread = new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				write(writeThread);
			}
		});
		writeThread.setName("IO Writer: " + this.getClass().getSimpleName());
		writeThread.start();
		isWriteStarted = true;
	}
	
	public void stopWriter() {
		isDone = true;
		
		synchronized (queue) {
			queue.notifyAll();
		}
		
		if (writeThread != null) {
			writeThread.interrupt();
			try {
				writeThread.join(500);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			writeThread = null;
		}
		
		isWriteStarted = false;
	}
	
	@Override
	public void write(byte[] data) throws IOException {
		// TODO Auto-generated method stub
		write(data, 0, data.length);
	}

	@Override
	public void write(byte[] data, int offset, int length) throws IOException {
		// TODO Auto-generated method stub
		os.write(data, offset, length);
	}

	@Override
	public void writeFrame(Frame frame) throws IOException {
		// TODO Auto-generated method stub
		int len = frame.getFrameLength();
		byte[] data = frame.getFrameData();
		write(data, 0, len);
	}
	
	public abstract void doWriteTask();

	private void write(Thread thread) {
		while (!isDone && writeThread == thread && !writeThread.isInterrupted()) {
			//doWriteTask();
			Frame frame = nextFrame();
			if (frame != null) {
				try {
					writeFrame(frame);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
		
		while (queue != null && !queue.isEmpty()) {
			try {
				writeFrame(queue.remove());
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	private Frame nextFrame() {
		Frame frame = null;
		
		while (!isDone && (frame = queue.poll()) == null) {
			try {
				synchronized (queue) {
					queue.wait();
				}
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		
		return frame;
	}
}
