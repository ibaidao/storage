package com.jyzn.common;

import com.jyzn.status.Coordinate;
import com.jyzn.status.FunCode;
import com.jyzn.status.TaskInfo;
import com.jyzn.status.VehicleStatus;
import com.jyzn.utils.Tools;

public class EntityWrapper {

	public static void wrap(BaseElement element, TaskInfo taskInfo) {
		taskInfo.setFunCode(FunCode.getFunCode(element.code));
		
		byte[] array = element.datas;
		taskInfo.setTargetID(Tools.getShort(array, 0) & 0xFFFF);
		
		Coordinate coordinate = null;
		int index = 1;
		while ((index + 5) < array.length) {
			coordinate = new Coordinate(0, 0, 0);
			
			coordinate.x = Tools.getShort(array, index) & 0xFFFF;
			index += 2;
			
			coordinate.y = Tools.getShort(array,  index) & 0xFFFF;
			index += 2;
			
			coordinate.z = array[index];
			index ++;
			
			taskInfo.addSteps(coordinate);
		}
	}
	
	public static void wrap(BaseElement baseElement, VehicleStatus status) {
		byte[] datas = baseElement.datas;
		if (datas == null || datas.length < 7)
			return;
		
	}
}
