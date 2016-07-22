package com.jyzn.tcp;

import java.io.IOException;
import java.net.Socket;
import java.util.List;

import com.jyzn.common.BaseElement;
import com.jyzn.common.Frame;
import com.jyzn.common.FrameInfo;
import com.jyzn.common.IDisconnectedListener;
import com.jyzn.common.IFrameReceivedListener;
import com.jyzn.common.IRawDataReceivedListener;
import com.jyzn.common.ProtocolConfiguration;
import com.jyzn.status.FunCode;
import com.jyzn.status.TaskInfo;
import com.jyzn.status.VehicleStatus;

/**
 * 
 * @author Leej 2016Äê7ÔÂ19ÈÕ
 *
 */
public class SocketSession implements IRawDataReceivedListener, IFrameReceivedListener{
	private Socket socket;
	private ProtocolConfiguration configuration;
	private SocketReader reader;
	private SocketWriter writer;
	private IDisconnectedListener disconnectedListener;
	
	public SocketSession(Socket socket, ProtocolConfiguration configuration) {
		this.socket = socket;
		this.configuration = configuration;
	}
	
	public void setDisconnectedListener(IDisconnectedListener disconnectedListener) {
		this.disconnectedListener = disconnectedListener;
	}
	
	public void start() {
		if (reader == null) {
			try {
				reader = new SocketReader(socket.getInputStream());
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				reader = null;
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
				writer = new SocketWriter(socket.getOutputStream());
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				writer = null;
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
		
		try {
			socket.shutdownInput();
			socket.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}	
	}
	
	public SocketWriter getWriter() {
		return writer;
	}
	
	public Socket getSocket() {
		return socket;
	}
	
	public ProtocolConfiguration getProtocolConfiguration() {
		return configuration;
	}

	@Override
	public void onRawDataReceived(byte[] data, int length, boolean check) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onFrameReceived(Frame frame, boolean check) {
		// TODO Auto-generated method stub
		if (check) {
			List<BaseElement> elements = (new FrameInfo(frame)).getElements();
			for (BaseElement element : elements) {
				FunCode funCode = FunCode.getFunCode(element.code);
				if (funCode == FunCode.CURRENT_STATUS) {
					VehicleStatus status = new VehicleStatus();
					status.setByteArray(element.datas);
					System.out.println(status.toString());
				}
				else {
					TaskInfo taskInfo = new TaskInfo();
					taskInfo.setFunCode(funCode);
					taskInfo.setByteArray(element.datas);
					System.out.println(taskInfo.toString());
				}
				System.out.println();
			}
			System.out.println("<----------------------------------------------------->");
		}
	}
}
