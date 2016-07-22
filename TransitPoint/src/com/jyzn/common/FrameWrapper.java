package com.jyzn.common;

import java.nio.ByteBuffer;

import com.jyzn.status.VehicleStatus;

/**
 * һ֡�������ֻ�ܰ���4�������Լ���Ӧ�����ݡ�
 * @author Leej 2016��7��20��
 *
 */
public class FrameWrapper {

	public static void wrap4Test(VehicleStatus status, Frame frame) {
		frame.reset();
		ByteBuffer buffer = frame.getByteBuffer();
		int lengthFiledStartPos = buffer.position();
		//������������
		buffer.position(lengthFiledStartPos + 2);
		//����
		buffer.putShort((short)0);//no need ack
		buffer.put((byte)status.workStatus.ordinal());
		buffer.put((byte)status.powerLeft);
		buffer.putShort((short)status.coordinate.x);
		buffer.putShort((short)status.coordinate.y);
		buffer.put((byte)status.coordinate.z);
		
		int lastPos = buffer.position();
		int datasLen = lastPos + frame.getConfiguration().getChecksumBytes() - (lengthFiledStartPos + 2);
		buffer.position(lengthFiledStartPos);
		//���ó�����
		buffer.putShort((short)datasLen);
		
		buffer.position(lastPos);
		
		frame.setCRCFiled(frame.getConfiguration().getChecksumType(), 
				buffer, lengthFiledStartPos, datasLen);
	}
	
	/**
	 * @param frame
	 * @param baseElements datas need to be added to the frame��ֻ�����ǰ4��
	 */
	public static void wrap(Frame frame, boolean ack, BaseElement... baseElements) {
		frame.reset();
		ByteBuffer buffer = frame.getByteBuffer();
		int lenthFieldPos = buffer.position();
		//������������
		buffer.position(lenthFieldPos + 2);
		//�Ƿ���ҪӦ��
		buffer.putShort((short)(ack ? 0x02 : 0x00));
		
		int posInCodeArea = buffer.position();
		int posInDataArea = posInCodeArea + 4 * 3;
		int index = 0;
		while (index < 4 && index < baseElements.length) {
			//������
			BaseElement baseElement = baseElements[index];
			buffer.position(posInCodeArea);
			buffer.put((byte)baseElement.code);
			buffer.putShort((short)baseElement.datas.length);
			posInCodeArea = buffer.position();
			
			////�������Ӧ������
			buffer.position(posInDataArea);
			buffer.put(baseElement.datas);
			posInDataArea = buffer.position();
			
			index ++;
		}
		
		//�������4�������ڵĹ�����λ��0���
		buffer.position(posInCodeArea);
		while (index < 4) {
			buffer.put((byte)0x00);
			buffer.putShort((short)0x00);
			index ++;
		}
		
		//���ó�����
		int datasLen = posInDataArea - lenthFieldPos;
		buffer.position(lenthFieldPos);
		buffer.putShort((short)datasLen);
		
		buffer.position(posInDataArea);
		//����crc
		frame.setCRCFiled(frame.getConfiguration().getChecksumType(), 
				buffer, lenthFieldPos, datasLen);
	}
}
