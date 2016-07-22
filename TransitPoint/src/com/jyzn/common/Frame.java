package com.jyzn.common;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

import com.jyzn.utils.ChecksumUtils;

/**
 * 数据帧<br/>
 * 
 * |----Header----|----Length Field(2bytes)----|----Data Field----|----CRC Field----|<br/>
 * @author Leej 2016年7月18日
 *
 */
public class Frame {
	//默认缓存大小
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
	 * 清除帧中数据
	 */
	public void reset() {
		buffer.clear();
		char[] headers = configuration.getFrameHeader();
		for (int i = 0; i < headers.length; i ++) {
			buffer.put((byte)headers[i]);
		}
	}
	
	/**
	 * 设置长度域后面数据的长度（2字节），长度包括了信息长度和校验域长度。
	 * @param length
	 */
	public void setLengthField(short length) {
		buffer.putShort(length);
	}
	
	/**
	 * 获取帧中长度域的值，其值包括信息长度和校验与长度（字节）。
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
	 * 获取帧中的校验值
	 * @return 如果返回-1，表示帧不完整无校验值。
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
	 * 获取帧数据域中的数据
	 * @param bytes
	 * @return 数据的长度
	 */
	public int getDataFiled(byte[] bytes) {
		int length = getFrameLength() - getHeader().length - configuration.getChecksumBytes();
		if (bytes.length < length)
			return 0;
		System.arraycopy(buffer.array(), getHeader().length + 2, bytes, 0, length);
		return length;
	}
	
	/**
	 * 将需要发送的数据打包成一帧的形式。
	 * @param datas 待发送的数据
	 * @param offset 起始位置
	 * @param length 发送数据的长度
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
	 * 在帧中写入一字节。
	 * @param value
	 */
	public void put(byte value) {
		if (!buffer.hasRemaining()) {
			adjustBuffer(Byte.BYTES);
		}
		buffer.put(value);
	}
	
	/**
	 * 在帧中写入short类型数据，2字节
	 * @param value
	 */
	public void putShort(short value) {
		if (buffer.remaining() < Short.BYTES) {
			adjustBuffer(Short.BYTES);
		}
		
		buffer.putShort(value);
	}
	
	/**
	 * 在帧中写入int型数据，4字节
	 * @param value
	 */
	public void putInt(int value) {
		if (buffer.remaining() < Integer.BYTES) {
			adjustBuffer(Integer.BYTES);
		}
		
		buffer.putInt(value);
	}
	
	/**
	 * 在帧中写入多个字节
	 * @param data
	 */
	public void put(byte[] data) {
		put(data, 0, data.length);
	}
	
	/**
	 * 在帧中写入多个字节
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
	 * 获取该帧的长度
	 * @return
	 */
	public int getFrameLength() {
		return buffer.position();
	}
	
	/**
	 * 获取该帧的数据
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
	 * 获取帧头
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
