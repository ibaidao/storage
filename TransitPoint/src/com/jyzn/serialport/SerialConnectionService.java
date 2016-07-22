package com.jyzn.serialport;

import javax.comm.CommPortIdentifier;
import javax.comm.CommPortOwnershipListener;
import javax.comm.NoSuchPortException;
import javax.comm.PortInUseException;
import javax.comm.SerialPort;
import javax.comm.UnsupportedCommOperationException;

import com.jyzn.common.ProtocolConfiguration;

public class SerialConnectionService implements CommPortOwnershipListener{
	private SerialParameters parameters;
	private boolean open = false;
	private CommPortIdentifier portId;
	private SerialSession session;
	private ProtocolConfiguration configuration;
	
	public SerialConnectionService(SerialParameters parameters, ProtocolConfiguration configuration) {
		this.parameters = parameters;
		this.configuration = configuration;
		open = false;
	}
	
	public void openConnection() throws SerialConnectionException {
		try {
			portId = CommPortIdentifier.getPortIdentifier(parameters.getPortName());
		} catch (NoSuchPortException e) {
			// TODO Auto-generated catch block
			throw new SerialConnectionException(e.getMessage());
		}
		
		SerialPort serialPort = null;
		try {
			serialPort = (SerialPort) portId.open("SerialPort", 30000);
		} catch (PortInUseException e) {
			// TODO Auto-generated catch block
			throw new SerialConnectionException(e.getMessage());
		}
		
		try {
			setConnectionParameters(serialPort, parameters);
		} catch (SerialConnectionException e) {
			// TODO: handle exception
			serialPort.close();
			throw e;
		}
		
		serialPort.notifyOnDataAvailable(true);
		serialPort.notifyOnBreakInterrupt(true);
		
		try {
			serialPort.enableReceiveTimeout(30);
		} catch (UnsupportedCommOperationException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		if (serialPort != null)
			session = new SerialSession(serialPort, configuration);
		
		portId.addPortOwnershipListener(this);
		
		open = true;
	}
	
	public void setConnectionParameters(SerialPort port, SerialParameters parameters) throws SerialConnectionException {
		int oldBaudRate = port.getBaudRate();
		int oldDatabits = port.getDataBits();
		int oldStopbits = port.getStopBits();
		int oldParity = port.getParity();
		
		try {
			port.setSerialPortParams(parameters.getBaudRate(), parameters.getDatabits(),
					parameters.getStopbits(), parameters.getParity());
		} catch (UnsupportedCommOperationException e) {
			// TODO Auto-generated catch block
			parameters.setBaudRate(oldBaudRate);
			parameters.setDatabits(oldDatabits);
			parameters.setStopbits(oldStopbits);
			parameters.setParity(oldParity);
			throw new SerialConnectionException("Unspported parameter");
		}
		
		//…Ë÷√¡˜øÿ÷∆
		try {
			port.setFlowControlMode(parameters.getFlowControlIn() | parameters.getFlowControlOut());
		} catch (UnsupportedCommOperationException e) {
			// TODO Auto-generated catch block
			throw new SerialConnectionException("Unspported flow control");
		}
	}
	
	public void closeConnection() {
		if (!open)
			return;
		
		if (session != null) {
			session.close();
			portId.removePortOwnershipListener(this);
		}
		
		open = false;
	}
	
	public void sendBreak() {
		session.sendBreak();
	}
	
	public boolean isOpen() {
		return open;
	}
	
	@Override
	public void ownershipChange(int arg0) {
		// TODO Auto-generated method stub
		
	}
}
