package com.jyzn.status;

import com.jyzn.common.BaseElement;

/**
 * 
 * @author Leej 2016Äê7ÔÂ19ÈÕ
 *
 */
public interface IToByteArray {

	public byte[] toByteArray();
	public void toByteArray(byte[] array, int offset);
	
	public BaseElement toBaseElement();
}
