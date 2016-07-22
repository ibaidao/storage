package com.jyzn.serialport;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Enumeration;
import java.util.List;
import java.util.Properties;

import javax.comm.CommPortIdentifier;

/**
 * 
 * @author Leej 2016年7月22日
 *
 */
public class SerialPortUtils {
	public static final String PORT_NAME_KEY = "portName";
	public static final String BAUDRATE_KEY = "baudRate";
	public static final String FLOW_CONTROL_IN_KEY = "flowContrtolIn";
	public static final String FLOW_CONTROL_OUT_KEY = "flowControlOut";
	public static final String DATA_BITS_KEY = "databits";
	public static final String STOP_BITS_KEY = "stopbits";
	public static final String PARITY_KEY = "parity";
	
	@SuppressWarnings("unchecked")
	public static List<CommPortIdentifier> getSeriaPortIdentifier() {
		List<CommPortIdentifier> identifiers = new ArrayList<>();
		
		CommPortIdentifier identifier = null;
		
		Enumeration<CommPortIdentifier> enumeration = CommPortIdentifier.getPortIdentifiers();
		
		while (enumeration.hasMoreElements()) {
			identifier = enumeration.nextElement();
			if (identifier.getPortType() == CommPortIdentifier.PORT_SERIAL) {
				identifiers.add(identifier);
			}
		}
		
		return identifiers;
	}
	
	/**
	 * 保存串口的参数设置。使用java property的形式。
	 */
	public static void saveSerialPortConfiguration(SerialParameters parameters, String path) {
		Properties props;
		FileOutputStream fileOut = null;
		
		props = new Properties();
		props.put(PORT_NAME_KEY, parameters.getPortName());
		props.put(BAUDRATE_KEY, parameters.getBaudRateString());
		props.put(FLOW_CONTROL_IN_KEY, parameters.getFlowControlInString());
		props.put(FLOW_CONTROL_OUT_KEY, parameters.getFlowControlOutString());
		props.put(DATA_BITS_KEY, parameters.getDatabitsString());
		props.put(STOP_BITS_KEY, parameters.getStopbitsString());
		props.put(PARITY_KEY, parameters.getParity());
		
		try {
			fileOut = new FileOutputStream(path);
			props.store(fileOut, "SeriaPort Properties");
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			if (fileOut != null) {
				try {
					fileOut.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
	
	/**
	 * 加载串口设置的参数
	 * @param parameters
	 * @param fullPath
	 */
	public static void loadSerialPortConfiguration(SerialParameters parameters, String fullPath) {
		File file = new File(fullPath);
		FileInputStream fileIn = null;
		
		try {
			fileIn = new FileInputStream(file);
			Properties prop = new Properties();
			prop.load(fileIn);
			loadSerialPortConfiguration(parameters, prop);
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			if (fileIn != null) {
				try {
					fileIn.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
	
	/**
	 * 加载串口设置的参数
	 * @param parameters
	 * @param properties
	 */
	public static void loadSerialPortConfiguration(SerialParameters parameters, Properties properties) {
		parameters.setPortName(properties.getProperty(PORT_NAME_KEY));
		parameters.setBaudRate(properties.getProperty(BAUDRATE_KEY));
		parameters.setFlowControlIn(properties.getProperty(FLOW_CONTROL_IN_KEY));
		parameters.setFlowControlOut(properties.getProperty(FLOW_CONTROL_OUT_KEY));
		parameters.setDatabits(properties.getProperty(DATA_BITS_KEY));
		parameters.setStopbits(properties.getProperty(STOP_BITS_KEY));
		parameters.setParity(properties.getProperty(PARITY_KEY));
	}
}
