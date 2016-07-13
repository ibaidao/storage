import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;

public class EchoSelectorProtocol implements TCPProtocol {
	private int bufSize = 0;
	
	public EchoSelectorProtocol(int bufSize) {
		this.bufSize = bufSize;
	}
	
	@Override
	public void handleAccept(SelectionKey key) throws IOException {
		// TODO Auto-generated method stub
		SocketChannel clientChannel = ((ServerSocketChannel)key.channel()).accept();
		clientChannel.configureBlocking(false);
		clientChannel.register(key.selector(), SelectionKey.OP_READ, ByteBuffer.allocate(bufSize));
	}

	@Override
	public void handleRead(SelectionKey key) throws IOException {
		// TODO Auto-generated method stub
		SocketChannel clientChannel = (SocketChannel) key.channel();
		ByteBuffer buffer = (ByteBuffer) key.attachment();
		long bytesRead = clientChannel.read(buffer);
		if (bytesRead == -1) {
			clientChannel.close();
		}
		else if (bytesRead > 0) {
			key.interestOps(SelectionKey.OP_READ | SelectionKey.OP_WRITE);
		}
	}

	@Override
	public void handleWrite(SelectionKey key) throws IOException {
		// TODO Auto-generated method stub
		ByteBuffer buffer = (ByteBuffer) key.attachment();
		buffer.flip();
		SocketChannel channel = (SocketChannel) key.channel();
		channel.write(buffer);
		if (!buffer.hasRemaining()) {
			key.interestOps(SelectionKey.OP_READ);
		}
		buffer.compact();
	}

}
