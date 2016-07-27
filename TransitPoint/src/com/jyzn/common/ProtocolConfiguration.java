package com.jyzn.common;
/**
 * 
 * @author Leej 2016��7��19��
 *
 */
public class ProtocolConfiguration {
	/**
	 * У���8λ
	 */
	public static final int TYPE_BCC = 0;
	/**
	 * 8λCRC
	 */
	public static final int TYPE_CRC16 = 1;
	/**
	 * 16λCRC
	 */
	public static final int TYPE_CRC32 = 2;
	
	/**
	 * ����֡ͷ
	 */
	private char[] frameHeader = null;
	/**
	 * У������
	 */
	private int checkType = TYPE_CRC16;
	
	private String host;
	
	private int port;
	
	//��������
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
	 * ����֡ͷ
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
	 * ��ȡ֡ͷ
	 * @return
	 */
	public char[] getFrameHeader() {
		return frameHeader;
	}
	
	/**
	 * ����У��ʹ�õ�����
	 * @param type
	 */
	public void setChecksumType(int type) {
		this.checkType = type;
	}
	
	/**
	 * ��ȡʹ�õ�У������
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
