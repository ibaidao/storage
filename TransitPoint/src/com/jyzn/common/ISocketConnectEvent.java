package com.jyzn.common;

import com.jyzn.tcp.SocketSession;

public interface ISocketConnectEvent {

	public void onConnected(SocketSession session);
	public void onReconnectError();
}
