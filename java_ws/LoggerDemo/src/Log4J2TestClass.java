import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class Log4J2TestClass {
	static Logger logger = LogManager.getLogger("test1Logger");
	
	public void log(int i) {
		logger.info("info log (" + i + ")");
	}
}
