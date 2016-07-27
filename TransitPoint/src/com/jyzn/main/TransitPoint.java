package com.jyzn.main;

import java.io.IOException;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

import javax.comm.CommPortIdentifier;

import org.apache.logging.log4j.Level;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import com.jyzn.common.Frame;
import com.jyzn.common.FrameWrapper;
import com.jyzn.common.ISocketConnectEvent;
import com.jyzn.common.ProtocolConfiguration;
import com.jyzn.log.LogUtil;
import com.jyzn.serialport.SerialPortUtils;
import com.jyzn.status.Coordinate;
import com.jyzn.status.FunCode;
import com.jyzn.status.TaskInfo;
import com.jyzn.status.VehicleStatus;
import com.jyzn.status.VehicleStatus.WorkStatus;
import com.jyzn.tcp.SocketConnectionService;
import com.jyzn.tcp.SocketSession;

public class TransitPoint{
	private static Timer timer;
	private static Frame testFrame;
	
	private static ISocketConnectEvent socketConnectEvent = new ISocketConnectEvent() {
		
		@Override
		public void onReconnectError() {
			// TODO Auto-generated method stub
			System.out.println("reconnect error");
			if (timer != null) {
				timer.cancel();
				timer = null;
			}
		}

		@Override
		public void onConnected(final SocketSession session) {
			// TODO Auto-generated method stub
			if (timer != null) {
				timer.cancel();
				timer = null;
			}
			
			timer = new Timer();
			timer.schedule(new TimerTask() {
				
				@Override
				public void run() {
					// TODO Auto-generated method stub
					if (testFrame == null) {
						ProtocolConfiguration configuration = new ProtocolConfiguration();
						configuration.setFrameHeader('<');
						testFrame = new Frame(configuration);
//						byte[] datas = "i am machine".getBytes();
//						testFrame.wrapperDatas(datas, 0, datas.length);
						
//						VehicleStatus status = new VehicleStatus();
//						status.workStatus = WorkStatus.LINE_UP;
//						status.powerLeft = 90;
//						status.coordinate.x = 10;
//						status.coordinate.y = 20;
//						status.coordinate.z = 30;
//						FrameWrapper.wrap4Test(status, testFrame);
						
						VehicleStatus status = new VehicleStatus();
						status.workStatus = WorkStatus.LINE_UP;
						status.powerLeft = 90;
						status.coordinate.x = 10;
						status.coordinate.y = 20;
						status.coordinate.z = 30;
						
						VehicleStatus status2 = new VehicleStatus();
						status2.workStatus = WorkStatus.CHARGING;
						status2.powerLeft = 92;
						status2.coordinate.x = 11;
						status2.coordinate.y = 21;
						status2.coordinate.z = 31;
						
						TaskInfo taskInfo = new TaskInfo();
						taskInfo.setFunCode(FunCode.CARRY_SHELVES_BACKTO_STORAGE_AREA);
						taskInfo.setTargetID(10);
						taskInfo.addSteps(new Coordinate(20, 10, 1));
						taskInfo.addSteps(new Coordinate(30, 104, 1));
						taskInfo.addSteps(new Coordinate(50, 3, 1));
						taskInfo.addSteps(new Coordinate(60, 20, 1));
						taskInfo.addSteps(new Coordinate(-20, -10, 1));
						taskInfo.addSteps(new Coordinate(-30, -104, 1));
						taskInfo.addSteps(new Coordinate(-50, -3, 1));
						taskInfo.addSteps(new Coordinate(-60, -20, 1));
						
						TaskInfo taskInfo2 = new TaskInfo();
						taskInfo2.setFunCode(FunCode.FOUND_SHELVES);
						taskInfo2.addSteps(new Coordinate(1, -1, 1));
						taskInfo2.addSteps(new Coordinate(2, -2, 0));
						taskInfo2.addSteps(new Coordinate(3, -3, 0));
						taskInfo2.addSteps(new Coordinate(4, -4, 1));
						
						FrameWrapper.wrap(testFrame, false, status.toBaseElement(), 
								status2.toBaseElement(), taskInfo.toBaseElement(), taskInfo2.toBaseElement());
					}
					
					try {
						if (session != null) {
							session.getWriter().writeFrame(testFrame);
						}
						else {
							System.out.println("session is null!");
						}
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}, 1000, 1000);
		}
	};
	
	public static void main(String[] args) {
//		VehicleStatus status = new VehicleStatus();
//		status.workStatus = WorkStatus.LINE_UP;
//		status.powerLeft = 90;
//		status.coordinate.x = 10;
//		status.coordinate.y = 20;
//		status.coordinate.z = 30;
//		
//		VehicleStatus status2 = new VehicleStatus();
//		status2.workStatus = WorkStatus.CHARGING;
//		status2.powerLeft = 92;
//		status2.coordinate.x = 11;
//		status2.coordinate.y = 21;
//		status2.coordinate.z = 31;
//		
//		TaskInfo taskInfo = new TaskInfo();
//		taskInfo.setFunCode(FunCode.CARRY_SHELVES_BACKTO_STORAGE_AREA);
//		taskInfo.setTargetID(10);
//		taskInfo.addSteps(new Coordinate(20, 10, 1));
//		taskInfo.addSteps(new Coordinate(30, 104, 1));
//		taskInfo.addSteps(new Coordinate(50, 3, 1));
//		taskInfo.addSteps(new Coordinate(60, 20, 1));
//		taskInfo.addSteps(new Coordinate(-20, -10, 1));
//		taskInfo.addSteps(new Coordinate(-30, -104, 1));
//		taskInfo.addSteps(new Coordinate(-50, -3, 1));
//		taskInfo.addSteps(new Coordinate(-60, -20, 1));
//		
//		ProtocolConfiguration cfg = new ProtocolConfiguration();
//		cfg.setFrameHeader('0');
//		Frame frame = new Frame(cfg);
//		
////		FrameWrapper.wrap(status, frame);
//		
//		FrameWrapper.wrap(frame, false, status.toBaseElement(), 
//				status2.toBaseElement(), taskInfo.toBaseElement());
//		
//		byte[] datas = frame.getFrameData();
//		int len = frame.getFrameLength();
//		System.out.println("header len: " + cfg.getFrameHeader().length);
//		System.out.println("length: " + len);
//		for (int i = 0; i < len; i ++) {
//			System.out.print(String.format("%2X", datas[i] & 0xFF) + " ");
//		}
//		
//		System.out.println();
//		for (int i = 0; i < len; i ++) {
//			System.out.print(String.format("%2d", datas[i] & 0xFF) + " ");
//		}
//		System.out.println();
//		
//		FrameInfo frameInfo = new FrameInfo(frame);
//		frameInfo.unpack();
//		List<BaseElement> elements = frameInfo.getElements();
//		VehicleStatus dStatus = new VehicleStatus();
//		dStatus.setByteArray(elements.get(0).datas);
//		System.out.println(dStatus.toString());
//		dStatus.setByteArray(elements.get(1).datas);
//		System.out.println(dStatus.toString());
//		
//		TaskInfo restoreInfo = new TaskInfo();
//		BaseElement element = elements.get(2);
//		restoreInfo.setFunCode(FunCode.getFunCode(element.code));
//		restoreInfo.setByteArray(element.datas);
//		System.out.println(restoreInfo);
		
		int i = 0;
		if (i == 0)
			return;
				
		try {
			System.out.println("Begin to connect server...");
			InetAddress address = InetAddress.getLocalHost();
			ProtocolConfiguration configuration = new ProtocolConfiguration();
			configuration.setFrameHeader('<');
//			configuration.setHost(address.getHostName());
			configuration.setHost("192.168.137.193");
			configuration.setPort(12345);
			SocketConnectionService service = new SocketConnectionService(configuration);
			service.setSocketConnectEvent(socketConnectEvent);
			service.connect2Server();
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
