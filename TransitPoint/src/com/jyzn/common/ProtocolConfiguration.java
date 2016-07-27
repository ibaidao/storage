package com.jyzn.common;
/**
 * 
 * @author Leej 2016年7月19日
 *
 */
public class ProtocolConfiguration {
	/**
	 * 校验和8位
	 */
	public static final int TYPE_BCC = 0;
	/**
	 * 8位CRC
	 */
	public static final int TYPE_CRC16 = 1;
	/**
	 * 16位CRC
	 */
	public static final int TYPE_CRC32 = 2;
	
	/**
	 * 定义帧头
	 */
	private char[] frameHeader = null;
	/**
	 * 校验类型
	 */
	private int checkType = TYPE_CRC16;
	
	private String host;
	
	private int port;
	
	//重连次数
	private int reconnectTimes = 10; 
	
	public ProtocolConfiguration() {
		
	}
	
	public void setReconnectTimes(int times) {
		this.reconnectTimes = times;
	}
	
	public int getReconnectTimes() {
		return reconnectTimes;
	}
	
	public void setHost(String host) {
		this.host = host;
	}
	
	public void setPort(int port) {
		this.port = port;
	}
	
	public String getHost() {
		return host;
	}
	
	public int getPort() {
		return port;
	}
	 
	/**
	 * 设置帧头
	 * @param headrs
	 */
	public void setFrameHeader(char... header) {
		if (header == null) {
			frameHeader = new char[1];
			frameHeader[0] = '<';
		}
		else {
			this.frameHeader = header;
		}
	}
	
	/**
	 * 获取帧头
	 * @return
	 */
	public char[] getFrameHeader() {
		return frameHeader;
	}
	
	/**
	 * 设置校验使用的类型
	 * @param type
	 */
	public void setChecksumType(int type) {
		this.checkType = type;
	}
	
	/**
	 * 获取使用的校验类型
	 * @return
	 */
	public int getChecksumType() {
		return checkType;
	}
	
	public int getChecksumBytes() {
		if (checkType == TYPE_CRC32) {
			return 4;
		}
		else if (checkType == TYPE_CRC16) {
			return 2;
		}
		else if (checkType == TYPE_BCC) {
			return 1;
		}
		
		return 2;
	}
	
	public static final ProtocolConfiguration getDefaultProtocolConfiguration() {
		ProtocolConfiguration configuration = new ProtocolConfiguration();
		configuration.setFrameHeader('<');
		configuration.setChecksumType(ProtocolConfiguration.TYPE_CRC16);
		return configuration;
	}
}
