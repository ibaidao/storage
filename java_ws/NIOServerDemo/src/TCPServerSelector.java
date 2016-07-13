import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.ServerSocketChannel;
import java.util.Iterator;

public class TCPServerSelector {
	private static final int BUFSIZE = 256;
	private static final int TIMEOUT = 3000;
	
	public void createSelector() throws IOException {
		Selector selector = Selector.open();
		InetSocketAddress address = new InetSocketAddress(6666);
		
		ServerSocketChannel serverSocketChannel = ServerSocketChannel.open();
		serverSocketChannel.socket().bind(address);
		serverSocketChannel.configureBlocking(false);
		serverSocketChannel.register(selector, SelectionKey.OP_ACCEPT);
		
		
		TCPProtocol protocol = new EchoSelectorProtocol(BUFSIZE);
		
		while (true) {
			if (selector.select(TIMEOUT) == 0) {
				System.out.print(".");
				continue;
			}
			
			Iterator<SelectionKey> keyIt = selector.selectedKeys().iterator();
			while (keyIt.hasNext()) {
				SelectionKey key = keyIt.next();
				if (key.isAcceptable()) {
					protocol.handleAccept(key);
				}
				
				if (key.isReadable()) {
					protocol.handleRead(key);
				}
				
				if (key.isValid() & key.isWritable()) {
					protocol.handleWrite(key);
				}
				
				keyIt.remove();
			}
		}
	}
}
