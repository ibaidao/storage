import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketException;
import java.nio.ByteBuffer;
import java.nio.channels.SocketChannel;

public class NIOClient {

	public static void main(String[] args) throws IOException {
		String server = "";
		int serverPort = 6666;
		
		SocketChannel clientChannel = SocketChannel.open();
		clientChannel.configureBlocking(false);
		
		if (!clientChannel.connect(new InetSocketAddress(server, serverPort))) {
			
			while (!clientChannel.finishConnect()) {
				System.out.print(".");
			}
		}
		
		ByteBuffer writeBuf = ByteBuffer.wrap("Hello".getBytes());
		ByteBuffer readBuf = ByteBuffer.allocate(10);
		int totalBytesRecved = 0;
		int byteRecved = 0;
		
		while (totalBytesRecved < 10) {
			if (writeBuf.hasRemaining()) {
				clientChannel.write(writeBuf);
			}
			
			if ((byteRecved = clientChannel.read(readBuf)) == -1) {
				throw new SocketException("Connection closed prematurely");
			}
			
			totalBytesRecved += byteRecved;
			System.out.print(".");
		}
		
		System.out.println("Received: " + new String(readBuf.array(), 0 , totalBytesRecved)); 
		clientChannel.close();
	}
}
