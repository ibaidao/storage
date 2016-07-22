package com.jyzn.common;

import java.nio.ByteBuffer;

import com.jyzn.status.VehicleStatus;

/**
 * 一帧数据最多只能包含4个功能以及对应的数据。
 * @author Leej 2016年7月20日
 *
 */
public class FrameWrapper {

	public static void wrap4Test(VehicleStatus status, Frame frame) {
		frame.reset();
		ByteBuffer buffer = frame.getByteBuffer();
		int lengthFiledStartPos = buffer.position();
		//先跳过长度域
		buffer.position(lengthFiledStartPos + 2);
		//数据
		buffer.putShort((short)0);//no need ack
		buffer.put((byte)status.workStatus.ordinal());
		buffer.put((byte)status.powerLeft);
		buffer.putShort((short)status.coordinate.x);
		buffer.putShort((short)status.coordinate.y);
		buffer.put((byte)status.coordinate.z);
		
		int lastPos = buffer.position();
		int datasLen = lastPos + frame.getConfiguration().getChecksumBytes() - (lengthFiledStartPos + 2);
		buffer.position(lengthFiledStartPos);
		//设置长度域
		buffer.putShort((short)datasLen);
		
		buffer.position(lastPos);
		
		frame.setCRCFiled(frame.getConfiguration().getChecksumType(), 
				buffer, lengthFiledStartPos, datasLen);
	}
	
	/**
	 * @param frame
	 * @param baseElements datas need to be added to the frame。只会加入前4个
	 */
	public static void wrap(Frame frame, boolean ack, BaseElement... baseElements) {
		frame.reset();
		ByteBuffer buffer = frame.getByteBuffer();
		int lenthFieldPos = buffer.position();
		//先跳过长度域
		buffer.position(lenthFieldPos + 2);
		//是否需要应答
		buffer.putShort((short)(ack ? 0x02 : 0x00));
		
		int posInCodeArea = buffer.position();
		int posInDataArea = posInCodeArea + 4 * 3;
		int index = 0;
		while (index < 4 && index < baseElements.length) {
			//功能码
			BaseElement baseElement = baseElements[index];
			buffer.position(posInCodeArea);
			buffer.put((byte)baseElement.code);
			buffer.putShort((short)baseElement.datas.length);
			posInCodeArea = buffer.position();
			
			////功能码对应的数据
			buffer.position(posInDataArea);
			buffer.put(baseElement.datas);
			posInDataArea = buffer.position();
			
			index ++;
		}
		
		//如果不够4个，多于的功能码位用0填充
		buffer.position(posInCodeArea);
		while (index < 4) {
			buffer.put((byte)0x00);
			buffer.putShort((short)0x00);
			index ++;
		}
		
		//设置长度域
		int datasLen = posInDataArea - lenthFieldPos;
		buffer.position(lenthFieldPos);
		buffer.putShort((short)datasLen);
		
		buffer.position(posInDataArea);
		//设置crc
		frame.setCRCFiled(frame.getConfiguration().getChecksumType(), 
				buffer, lenthFieldPos, datasLen);
	}
}
