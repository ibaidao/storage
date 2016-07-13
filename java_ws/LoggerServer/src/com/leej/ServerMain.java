package com.leej;

import java.io.IOException;

public class ServerMain {

	public static void main(String[] args) {
		LoggerServer server = new LoggerServer();
		try {
			server.startServer();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
}
