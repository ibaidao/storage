import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class Log4j2TestClass2 {
	
	static Logger logger = LogManager.getLogger("test2Logger");
	
	public void log(int i) {
		logger.info("info log2 (" + i + ")");
	}
}
