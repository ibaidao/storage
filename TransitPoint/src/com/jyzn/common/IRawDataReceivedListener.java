package com.jyzn.common;

public interface IRawDataReceivedListener {

	public void onRawDataReceived(byte[] data, int length, boolean error);
}
