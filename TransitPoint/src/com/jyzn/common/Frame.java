package com.jyzn.common;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import com.jyzn.utils.ChecksumUtils;

/**
 * ����֡<br/>
 * 
 * |----Header----|----Length Field(2bytes)----|----Data Field----|----CRC Field----|<br/>
 * @author Leej 2016��7��18��
 *
 */
public class Frame {
	//Ĭ�ϻ����С
	private static final int DEFAULT_BUFFER_CAPACITY = 1024;
	private ByteBuffer buffer;
	private ProtocolConfiguration configuration;
	
	public Frame(ProtocolConfiguration configuration) {
		this(configuration, ByteBuffer.allocate(DEFAULT_BUFFER_CAPACITY));
	}
	
	public Frame(ProtocolConfiguration configuration, ByteBuffer byteBuffer) {
		this.buffer = byteBuffer;
		buffer.clear();
		buffer.order(ByteOrder.BIG_ENDIAN);
		char[] headers = configuration.getFrameHeader();
		for (int i = 0; i < headers.length; i ++) {
			buffer.put((byte)headers[i]);
		}
		this.configuration = configuration;
	}
	
	/**
	 * ���֡������
	 */
	public void reset() {
		buffer.clear();
		char[] headers = configuration.getFrameHeader();
		for (int i = 0; i < headers.length; i ++) {
			buffer.put((byte)headers[i]);
		}
	}
	
	/**
	 * ���ó�����������ݵĳ��ȣ�2�ֽڣ������Ȱ�������Ϣ���Ⱥ�У���򳤶ȡ�
	 * @param length
	 */
	public void setLengthField(short length) {
		buffer.putShort(length);
	}
	
	/**
	 * ��ȡ֡�г������ֵ����ֵ������Ϣ���Ⱥ�У���볤�ȣ��ֽڣ���
	 * @return
	 */
	public int getLengthField() {
		int pos = buffer.position();
		buffer.position(getHeader().length);
		short length = buffer.getShort();
		buffer.position(pos);
		
		return length & 0xFFFF;
	}
	
	/**
	 * ��ȡ֡�е�У��ֵ
	 * @return �������-1����ʾ֡��������У��ֵ��
	 */
	public int getCRCFiled() {
		if (getFrameLength() - getHeader().length - 2 - configuration.getChecksumBytes() < 0) {
			return -1;
		}
		buffer.position(getFrameLength() - configuration.getChecksumBytes());
		if (configuration.getChecksumBytes() == 1) {
			return buffer.get() & 0xFF;
		}
		else if (configuration.getChecksumBytes() == 4) {
			return buffer.getInt();
		}
		else {
			return buffer.getShort() & 0xFFFF;
		}
	}
	
	/**
	 * ��ȡ֡�������е�����
	 * @param bytes
	 * @return ���ݵĳ���
	 */
	public int getDataFiled(byte[] bytes) {
		int length = getFrameLength() - getHeader().length - configuration.getChecksumBytes();
		if (bytes.length < length)
			return 0;
		System.arraycopy(buffer.array(), getHeader().length + 2, bytes, 0, length);
		return length;
	}
	
	/**
	 * ����Ҫ���͵����ݴ����һ֡����ʽ��
	 * @param datas �����͵�����
	 * @param offset ��ʼλ��
	 * @param length �������ݵĳ���
	 */
	public void wrapperDatas(byte[] datas, int offset, int length) {
		if (buffer.position() != getHeader().length) {
			buffer.position(getHeader().length);
		}
		
		short dataLen = (short) (length + configuration.getChecksumBytes());
		putShort(dataLen);
		put(datas, offset, length);
		
		setCRCFiled(configuration.getChecksumType(), buffer, getHeader().length, length + 2);
	}
	
	public void setCRCFiled(int checksumType, ByteBuffer buffer, int offset, int length) {
		switch (checksumType) {
		case ProtocolConfiguration.TYPE_BCC:
			byte bcc = ChecksumUtils.bcc(buffer.array(), offset, length);
			buffer.put(bcc);
			break;
		case ProtocolConfiguration.TYPE_CRC32:
			int crc32 = (int) ChecksumUtils.crc32(buffer.array(), offset, length);
			buffer.putInt(crc32);
			break;
		case ProtocolConfiguration.TYPE_CRC16:
			default:
				short crc16 = (short) ChecksumUtils.crc16(buffer.array(), offset, length);
				buffer.putShort(crc16);
				break;
		}
	}
	
	/**
	 * ��֡��д��һ�ֽڡ�
	 * @param value
	 */
	public void put(byte value) {
		if (!buffer.hasRemaining()) {
			adjustBuffer(Byte.BYTES);
		}
		buffer.put(value);
	}
	
	/**
	 * ��֡��д��short�������ݣ�2�ֽ�
	 * @param value
	 */
	public void putShort(short value) {
		if (buffer.remaining() < Short.BYTES) {
			adjustBuffer(Short.BYTES);
		}
		
		buffer.putShort(value);
	}
	
	/**
	 * ��֡��д��int�����ݣ�4�ֽ�
	 * @param value
	 */
	public void putInt(int value) {
		if (buffer.remaining() < Integer.BYTES) {
			adjustBuffer(Integer.BYTES);
		}
		
		buffer.putInt(value);
	}
	
	/**
	 * ��֡��д�����ֽ�
	 * @param data
	 */
	public void put(byte[] data) {
		put(data, 0, data.length);
	}
	
	/**
	 * ��֡��д�����ֽ�
	 * @param data
	 * @param offset
	 * @param count
	 */
	public void put(byte[] data, int offset, int count) {
		if (count > buffer.remaining()) {
			adjustBuffer(count);
		}
		buffer.put(data, offset, count);
	}
	
	/**
	 * ��ȡ��֡�ĳ���
	 * @return
	 */
	public int getFrameLength() {
		return buffer.position();
	}
	
	/**
	 * ��ȡ��֡������
	 * @return
	 */
	public byte[] getFrameData() {
		return buffer.array();
	}
	
//	public int getChecksum() {
//		int crc = 0;
//		int oldPosition = buffer.position();
//		buffer.position(oldPosition - 2);
//		crc = buffer.getShort() & 0xffffffff;
//		return crc;
//	}
	
	/**
	 * ��ȡ֡ͷ
	 * @return
	 */
	public char[] getHeader() {
		return configuration.getFrameHeader();
	}
	
	public ByteBuffer getByteBuffer() {
		return buffer;
	}
	
	public ProtocolConfiguration getConfiguration() {
		return configuration;
	}
	
	public void dispose() {
		buffer = null;
	}
	
	private void adjustBuffer(int expectIncrement) {
		int oldCapacity = buffer.capacity();
		int newCapacity = (int) (oldCapacity * 1.5);
		if (newCapacity - oldCapacity < expectIncrement) {
			newCapacity = oldCapacity + expectIncrement;
		}
		byte[] array = buffer.array();
		buffer = ByteBuffer.allocate(newCapacity);
		buffer.order(ByteOrder.BIG_ENDIAN);
		buffer.put(array);
	}
}
