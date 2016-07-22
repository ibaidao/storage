package com.jyzn.common;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * 帧的内容
 * @author Leej 2016年7月20日
 *
 */
public class FrameInfo {
	private Frame  frame;
	private List<BaseElement> elements;
	private boolean isNeedAck = false;

	public FrameInfo(Frame frame) {
		this.frame = frame;
		elements = new ArrayList<>();
	}
	
	public void setFrame(Frame frame) {
		this.frame = frame;
		elements.clear();
	}
	
	public boolean isNeedAck() {
		return isNeedAck;
	}
	
	public List<BaseElement> getElements() {
		return elements;
	}
	
	public boolean unpack() {
		int datasLen = frame.getLengthField();
		int dataStartPos = frame.getHeader().length + 2;
		ByteBuffer buffer = frame.getByteBuffer();
		buffer.position(dataStartPos);
		
		short ack = buffer.getShort();
		isNeedAck = ack == 0x02? true : false;
		
		byte code1 = buffer.get();
		int dataLen4Code1 = buffer.getShort() & 0xFFFF;
		
		byte code2 = buffer.get();
		int dataLen4Code2 = buffer.getShort() & 0xFFFF;
		
		byte code3 = buffer.get();
		int dataLen4Code3 = buffer.getShort() & 0xFFFF;
		
		byte code4 = buffer.get();
		int dataLen4Code4 = buffer.getShort() & 0xFFFF;
		
		
		if (dataLen4Code1 != 0) {
			BaseElement item = getData(buffer, dataLen4Code1);
			item.code = code1;
			elements.add(item);
		}
		
		if (dataLen4Code2 != 0) {
			BaseElement item = getData(buffer, dataLen4Code2);
			item.code = code2;
			elements.add(item);
		}
		
		if (dataLen4Code3 != 0) {
			BaseElement item = getData(buffer, dataLen4Code3);
			item.code = code3;
			elements.add(item);
		}
		
		if (dataLen4Code4 != 0) {
			BaseElement item = getData(buffer, dataLen4Code4);
			item.code = code4;
			elements.add(item);
		}
		
		int endPos = buffer.position();
		
		return (endPos - dataStartPos) == datasLen ? true : false;
	}
	
	private BaseElement getData(ByteBuffer byteBuffer, int length) {
		byte[] data = new byte[length];
		byteBuffer.get(data);
		
		BaseElement item = new BaseElement();
		item.datas = data;
		
		return item;
	}
}
