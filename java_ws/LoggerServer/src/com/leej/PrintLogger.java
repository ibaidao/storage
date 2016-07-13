package com.leej;

import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.net.Socket;

public class PrintLogger implements Runnable {
	private Socket socket;
	
	public PrintLogger(Socket clientSocket) {
		socket = clientSocket;
	}

	@Override
	public void run() {
		// TODO Auto-generated method stub
		while (true) {
			try {
				InputStream in = socket.getInputStream();
//				ObjectInputStream oin = new ObjectInputStream(in);
//				Object object = null;
//				
//				object = oin.readObject();
//				System.out.println(object.toString());
				
				
				byte[] bytes = new byte[1024];
				
				int read = 0;
				while ((read = in.read(bytes)) != -1) {
					String str = new String(bytes, 0, read, "UTF-8");
					System.out.println(str);
				}
				
				in.close();
				socket.close();
				
				break;
				
			} 
			catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
