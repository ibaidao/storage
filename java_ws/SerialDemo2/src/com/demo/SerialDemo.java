package com.demo;

import java.net.Inet4Address;
import java.net.Inet6Address;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.util.Enumeration;

import javax.comm.CommPortIdentifier;

public class SerialDemo {
	
	public static void main(String[] args) {
//		listSerialPort();
		listInetAddress();
	}
	
	public static void  listSerialPort() {
		CommPortIdentifier identifier = null;
		
		Enumeration<?> enumeration = CommPortIdentifier.getPortIdentifiers();
		while (enumeration.hasMoreElements()) {
			identifier = (CommPortIdentifier) enumeration.nextElement();
			if (identifier != null && identifier.getPortType() == CommPortIdentifier.PORT_SERIAL) {
				System.out.println("find serial port:");
				System.out.println(identifier.getName());
			}
			
		}
	}
	
	/**
	 * Get the network interfaces and associated addresses for this host
	 */
	public static void listInetAddress() {
		
		try {
			Enumeration<NetworkInterface> interfaceList = NetworkInterface.getNetworkInterfaces();
			if (interfaceList == null) {
				System.out.println("No interfaces found!!");
			}
			else {
				while (interfaceList.hasMoreElements()) {
					NetworkInterface networkInterface = interfaceList.nextElement();
					System.out.println("interface --> " + networkInterface.getName() + " : ");
					Enumeration<InetAddress> addrList = networkInterface.getInetAddresses();
					if (!addrList.hasMoreElements()) {
						System.out.println("\t(No addresses for this interface)");
					}
					while (addrList.hasMoreElements()) {
						InetAddress address = addrList.nextElement();
						System.out.println("\tAddress" + (address instanceof Inet4Address ? "(v4)" : (address instanceof Inet6Address? "(v6)":"?")));
						System.out.println(": " + address.getHostAddress());
					}
				}
			}
		} catch (SocketException e) {
			// TODO Auto-generated catch block
			System.out.println("Error getting network interfaces:" + e.getMessage());
		}
	}
}
