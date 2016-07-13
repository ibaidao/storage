package com.leej;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class LoggerServer {
	private ServerSocket socket;
	
	public LoggerServer() {
		
	}
	
	public void startServer() throws IOException {
		socket = new ServerSocket(12345);
		while (true) {
			System.out.print(".");
			Socket clientSocket = socket.accept();
			System.out.println();
			System.out.println("client connected");
			PrintLogger print = new PrintLogger(clientSocket);
			new Thread(print).start();
		}
	}
}
