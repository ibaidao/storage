package com.jyzn.serialport;

import java.io.IOException;

import javax.comm.SerialPort;

import com.jyzn.common.Frame;
import com.jyzn.common.IDisconnectedListener;
import com.jyzn.common.IFrameReceivedListener;
import com.jyzn.common.IRawDataReceivedListener;
import com.jyzn.common.ProtocolConfiguration;

public class SerialSession implements IRawDataReceivedListener, IFrameReceivedListener {
	private SerialPort serialPort;
	private ProtocolConfiguration configuration;
	private SerialReader reader;
	private SerialWriter writer;
	private IDisconnectedListener disconnectedListener;
	
	public SerialSession(SerialPort serialPort, ProtocolConfiguration configuration) {
		this.serialPort = serialPort;
		this.configuration = configuration;
	}
	
	public void start() {
		if (reader == null) {
			try {
				reader = new SerialReader(serialPort.getInputStream());
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				reader = null;
				serialPort.close();
			}
		}
		
		if (reader != null) {
			reader.createFrame(configuration);
			reader.addRawDataReceivedListener(getClass().getSimpleName(), this);
			reader.addFrameReceivedListener(getClass().getSimpleName(), this);
			reader.setDisconnectedListener(disconnectedListener);
			reader.startReader();
		}
		
		if (writer == null) {
			try {
				writer = new SerialWriter(serialPort.getOutputStream());
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				writer = null;
				serialPort.close();
			}
		}
		
		if (writer != null) {
			writer.startWriter();
		}
	}
	
	public void close() {
		if (reader != null) {
			reader.stopReader();
		}
		reader.dispose();
		reader = null;
		
		if (writer != null) {
			writer.stopWriter();
		}
		
		writer = null;
		
		serialPort.close();
	}
	
	public void sendBreak() {
		serialPort.sendBreak(1000);
	}
	
	public SerialWriter getWriter() {
		return writer;
	}
	
	public SerialPort getSerialPort() {
		return serialPort;
	}
	
	public ProtocolConfiguration getProtocolConfiguration() {
		return configuration;
	}

	public void setDisconnectedListener(IDisconnectedListener disconnectedListener) {
		this.disconnectedListener = disconnectedListener;
	}
	
	@Override
	public void onFrameReceived(Frame frame, boolean check) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onRawDataReceived(byte[] data, int length, boolean error) {
		// TODO Auto-generated method stub
		
	}

}
