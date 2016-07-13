import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class LoggerDemo {

	public static void main(String[] args) {
		loggerDemo();
		System.exit(1);
	}
	
	public static void loggerDemo() {
		Log4J2TestClass test1 = new Log4J2TestClass();
		Log4j2TestClass2 test2 = new Log4j2TestClass2();
		int i = 0;
		while(i < 1000000) {
			test1.log(i);
			test2.log(i);
			i ++;
		}
	}
	
	public static void logger2() {
		Logger logger = LogManager.getLogger(LoggerDemo.class.getName());
		logger.info("info msg");
		logger.warn("warn info");
		logger.debug("debug msg");
		logger.fatal("fatal msg");
		logger.error("error msg");
		logger.trace("trace msg");
	}
	
//	public static void logger() {
//		//建立一个Logger实例
//		Logger logger = Logger.getLogger("com.foo");
//		//设置logg的级别，通常在配置文件设置
//		logger.setLevel(Level.INFO);
//		
//		Logger loggerBar = Logger.getLogger("com.foo.Bar");
//		//下面这个请求可用，warn等级大于info
//		logger.warn("low full level");
//		//下面这个请求不可用，debug等级小于info
//		logger.debug("");
//		//命名为com.foo.Bar的Logger实例会继承命名为com.foo的级别，因此下面这个请求可用
//		loggerBar.info("info msg");
//		//下面这个请求不可用，因为DEBUG等级小于info
//		loggerBar.debug("debug msg");
//		
//	}
	
//	public static void demo() {
//		//step1  建立Logger实例
//		Logger logger = Logger.getLogger("loggerDemo");
//		
//		//step2 读取配置文件
//		//BasicConfigurator.configure();//自动快速的使用缺省Log4j环境
//		//PropertyConfigurator.configure("path");//读取使用Java的特性文件编写的配置文件
//		//DOMConfigurator.configure("path");//读取XML形式的配置文件
////		PropertyConfigurator.configure("log.properties");
//		
//		String pattern = "Milliseconds since program start:%r%n";
//		pattern += "Classname of caller:%C%n";
//		pattern += "Date in ISO8601 format:%d{ISO8601}%n";
//		pattern += "Location of log event:%l%n";
//		pattern += "Message:%m%n%n";
//		
//		PatternLayout layout = new PatternLayout(pattern);
//		//step3 插入日志信息
////		logger.debug("debug msg");
////		logger.info("info msg");
////		logger.warn("warn msg");
////		logger.error("error msg");
////		logger.fatal("fatal msg");
//		logger.info("begin to use log4j");
//		
//	}
}
