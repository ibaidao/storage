package com.jyzn.status;

import com.jyzn.common.BaseElement;
import com.jyzn.utils.Tools;

/**
 * 
 * @author Leej 2016��7��19��
 *
 */
public class VehicleStatus implements IToByteArray, IByteArrayToObj{
	public FunCode funCode = FunCode.CURRENT_STATUS;
	public static enum WorkStatus {
		IDLE,				//����
		BREAK_DOWN,			//����
		CHARGING,			//���
		TAKING_SHELVES,		//ȡ����
		TO_PICKING_TABLE,	//�ͼ��̨
		LINE_UP,			//�Ŷ�
		BACK_SHELVES;		//��λ����
	}
	
	/**
	 * ����״̬
	 */
	public WorkStatus workStatus = WorkStatus.IDLE;
	/**
	 * ʣ�����
	 */
	public int powerLeft = 100;
	/**
	 * ����
	 */
	public Coordinate coordinate = new Coordinate(0, 0, 0);
	
	@Override
	public byte[] toByteArray() {
		// TODO Auto-generated method stub
		byte[] array = new byte[1 + 1 + 5];
		toByteArray(array, 0);
		return array;
	}
	@Override
	public void toByteArray(byte[] array, int offset) {
		// TODO Auto-generated method stub
		array[offset ++] = (byte)workStatus.ordinal();
		array[offset ++] = (byte)powerLeft;
		Tools.putShort(array, offset, coordinate.x);
		offset += 2;
		Tools.putShort(array, offset, coordinate.y);
		offset += 2;
		array[offset ++] = (byte) coordinate.z;
	}
	@Override
	public BaseElement toBaseElement() {
		// TODO Auto-generated method stub
		BaseElement baseElement = new BaseElement();
		baseElement.code = funCode.getCode();
		baseElement.datas = toByteArray();
		return baseElement;
	}
	@Override
	public void setByteArray(byte[] datas) {
		// TODO Auto-generated method stub
		if (datas == null || datas.length < 7)
			return;
		int index = 0;
		workStatus = getWorkStatus(datas[index ++]);
		powerLeft = datas[index ++];
		coordinate.x = Tools.getShort(datas, index);
		index += 2;
		coordinate.y = Tools.getShort(datas, index);
		index += 2;
		coordinate.z = datas[index ++] & 0xFF;
	}
	
	private WorkStatus getWorkStatus(int value) {
		WorkStatus workStatus = null;
		switch (value) {
		case 1:
			workStatus = WorkStatus.BREAK_DOWN;
			break;
		case 2:
			workStatus = WorkStatus.CHARGING;
			break;
		case 3:
			workStatus = WorkStatus.TAKING_SHELVES;
			break;
		case 4:
			workStatus = WorkStatus.TO_PICKING_TABLE;
			break;
		case 5:
			workStatus = WorkStatus.LINE_UP;
			break;
		case 6:
			workStatus = WorkStatus.BACK_SHELVES;
			break;
		case 0:
			default:
			workStatus = WorkStatus.IDLE;
			break;
		}
		
		return workStatus;
	}
	
	@Override
	public String toString() {
		String str = workStatus.toString() + "\n" +
					 powerLeft + "\n" + 
					 coordinate.x + "\n" +
					 coordinate.y + "\n" + 
					 coordinate.z + "\n";
		
		return str;
	}
}
