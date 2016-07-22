package com.jyzn.serialport;

public class SerialConnectionException extends Exception {

	/**
	 * 
	 */
	private static final long serialVersionUID = 8865465427L;

	public SerialConnectionException(String str) {
		super(str);
	}
	
	public SerialConnectionException() {
		super();
	}
}
