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
//		//����һ��Loggerʵ��
//		Logger logger = Logger.getLogger("com.foo");
//		//����logg�ļ���ͨ���������ļ�����
//		logger.setLevel(Level.INFO);
//		
//		Logger loggerBar = Logger.getLogger("com.foo.Bar");
//		//�������������ã�warn�ȼ�����info
//		logger.warn("low full level");
//		//����������󲻿��ã�debug�ȼ�С��info
//		logger.debug("");
//		//����Ϊcom.foo.Bar��Loggerʵ����̳�����Ϊcom.foo�ļ��������������������
//		loggerBar.info("info msg");
//		//����������󲻿��ã���ΪDEBUG�ȼ�С��info
//		loggerBar.debug("debug msg");
//		
//	}
	
//	public static void demo() {
//		//step1  ����Loggerʵ��
//		Logger logger = Logger.getLogger("loggerDemo");
//		
//		//step2 ��ȡ�����ļ�
//		//BasicConfigurator.configure();//�Զ����ٵ�ʹ��ȱʡLog4j����
//		//PropertyConfigurator.configure("path");//��ȡʹ��Java�������ļ���д�������ļ�
//		//DOMConfigurator.configure("path");//��ȡXML��ʽ�������ļ�
////		PropertyConfigurator.configure("log.properties");
//		
//		String pattern = "Milliseconds since program start:%r%n";
//		pattern += "Classname of caller:%C%n";
//		pattern += "Date in ISO8601 format:%d{ISO8601}%n";
//		pattern += "Location of log event:%l%n";
//		pattern += "Message:%m%n%n";
//		
//		PatternLayout layout = new PatternLayout(pattern);
//		//step3 ������־��Ϣ
////		logger.debug("debug msg");
////		logger.info("info msg");
////		logger.warn("warn msg");
////		logger.error("error msg");
////		logger.fatal("fatal msg");
//		logger.info("begin to use log4j");
//		
//	}
}
