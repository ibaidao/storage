package com.jyzn.status;

import java.util.ArrayList;
import java.util.List;

import com.jyzn.common.BaseElement;
import com.jyzn.utils.Tools;

/**
 * 作业信息。包含：目标的ID（可以是货架、检货台、充电桩）；运行步骤。
 * @author Leej 2016年7月20日
 *
 */
public class TaskInfo implements IToByteArray, IByteArrayToObj{

	private FunCode funCode = null;
	//目标id，2字节
	private int targetId = 0;
	//步骤，每步包含（x,y,z)信息，占用5字节。
	private List<Coordinate> steps = new ArrayList<>();
	
	public TaskInfo() {
	}
	
	public void setFunCode(FunCode funCode) {
		this.funCode = funCode;
	}
	
	public void setTargetID(int id)	 {
		this.targetId = id;
	}
	
	public void addSteps(Coordinate coordinate) {
		steps.add(coordinate);
	}
	
	public FunCode getFunCode() {
		return funCode;
	}
	
	public int getTargetId() {
		return targetId;
	}
	
	public List<Coordinate> getSteps() {
		return steps;
	}

	@Override
	public byte[] toByteArray() {
		// TODO Auto-generated method stub
		byte[] array = new byte[2 + steps.size() * 5];
		toByteArray(array, 0);
		return array;
	}

	@Override
	public void toByteArray(byte[] array, int offset) {
		// TODO Auto-generated method stub
		Tools.putShort(array, offset, targetId);
		offset += 2;
		
		for (Coordinate coordinate : steps) {
			Tools.putShort(array, offset, coordinate.x);
			offset += 2;
			Tools.putShort(array, offset, coordinate.y);
			offset += 2;
			array[offset ++] = (byte)coordinate.z;
		}
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
	public void setByteArray(byte[] array) {
		// TODO Auto-generated method stub
		if (array == null || array.length < 2)
			return;
		int index = 0;
		targetId = Tools.getShort(array, index) & 0xFFFF;
		index += 2;
		
		Coordinate coordinate = null;
		steps.clear();
		while ((index + 5) <= array.length) {
			coordinate = new Coordinate(0, 0, 0);
			
			coordinate.x = Tools.getShort(array, index);
			index += 2;
			
			coordinate.y = Tools.getShort(array,  index);
			index += 2;
			
			coordinate.z = array[index ++] & 0xFF;
			
			steps.add(coordinate);
		}
	}
	
	@Override
	public String toString() {
		StringBuilder strBuilder = new StringBuilder();
		strBuilder.append(funCode);
		if (funCode != null) {
			strBuilder.append("(0x");
			strBuilder.append(String.format("%02X", funCode.getCode()));
			strBuilder.append(")\n");
		}else {
			strBuilder.append("(null)");
		}
		strBuilder.append(targetId);
		strBuilder.append("\n");
		int index = 1;
		for (Coordinate coordinate : steps) {
			strBuilder.append("step");
			strBuilder.append(index ++);
			strBuilder.append(":(");
			strBuilder.append(coordinate.x);
			strBuilder.append(" , ");
			strBuilder.append(coordinate.y);
			strBuilder.append(" , ");
			strBuilder.append(coordinate.z);
			strBuilder.append(")\n");
		}
		return strBuilder.toString();
	}
}
