package com.jyzn.tcp;

import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

import com.jyzn.common.IDisconnectedListener;
import com.jyzn.common.ISocketConnectEvent;
import com.jyzn.common.ProtocolConfiguration;

/**
 * 
 * @author Leej 2016年7月19日
 *
 */
public class SocketConnectionService implements IDisconnectedListener {
	private ProtocolConfiguration configuration;
	private SocketSession session;
	private boolean reconnectServiceStart = false;
	private ISocketConnectEvent connectEvent;
	
	public SocketConnectionService(ProtocolConfiguration configuration) {
		this.configuration = configuration;
	}
	
	public void setSocketConnectEvent(ISocketConnectEvent connectEvent) {
		this.connectEvent = connectEvent;
	}
	
	public SocketSession getSocketSessiong() {
		return session;
	}
	
	public void connect2Server(String host, int port) {
		try {
			Socket socket = new Socket(host, port);
			if (session != null) {
				session.close();
				session = null;
			}
			
			SocketSession newSession = new SocketSession(socket, configuration);
			newSession.setDisconnectedListener(this);
			newSession.start();
			session = newSession;
			
			if (connectEvent != null)
				connectEvent.onConnected(newSession);
			
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public void connect2Server() {
		connect2Server(configuration.getHost(), configuration.getPort());
	}
	
	public void close() {
		if (session != null)
			session.close();
		
		session = null;
	}

	@Override
	public void onDisconnected() {
		// TODO Auto-generated method stub
		//断开连接，需要重新连接。如果进行重新连接的次数超过了规定的次数，进行警告。
		close();//关闭对话
		startReconnectService();
	}
	
	private void startReconnectService() {
		if (reconnectServiceStart)
			return;
		new Thread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				System.out.println("reconnect service start...");
				int count = 0;
				while (count < configuration.getReconnectTimes()) {
					try {
						Socket socket = new Socket(configuration.getHost(), configuration.getPort());
						SocketSession newSession = new SocketSession(socket, configuration);
						newSession.setDisconnectedListener(SocketConnectionService.this);
						newSession.start();
						
						session = newSession;
						reconnectServiceStart = false;
						
						if (connectEvent != null)
							connectEvent.onConnected(newSession);
						
						break;
						
					} catch (UnknownHostException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
						count ++;
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
						count ++;
					}
					
					try {
						Thread.sleep(1000);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
				if (count >= configuration.getReconnectTimes()) {
					if (connectEvent != null)
						connectEvent.onReconnectError();
				}
			}
		}).start();
		reconnectServiceStart = true;
	}
}
