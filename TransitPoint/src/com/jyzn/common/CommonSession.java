package com.jyzn.common;

import java.util.HashMap;

public abstract class CommonSession {
	private HashMap<String, IRawDataReceivedListener> recvListeners = new HashMap<>();
	
	public void addRawDataReceivedListener(String key, IRawDataReceivedListener listener) {
		if (!recvListeners.containsKey(key)) {
			recvListeners.put(key, listener);
		}
	}
	
	public void removeRawDataReceivedListener(String key) {
		if (recvListeners.containsKey(key))
			recvListeners.remove(key);
	}
}
