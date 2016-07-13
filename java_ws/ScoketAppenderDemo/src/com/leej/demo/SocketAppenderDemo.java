package com.leej.demo;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class SocketAppenderDemo {
	static Logger logger = LogManager.getRootLogger();

	public static void main(String[] args) {
		logger();
		
		logger.info("log exit");
	}
	
	public static void logger() {
		int i = 0;
		while (i < 100) {
			logger.trace("trace " + i);
			logger.debug("debug " + i);
			logger.info("info " + i);
			logger.warn("warn " + i);
			logger.error("error " + i);
			logger.fatal("fatal " + i);
			
			i ++;
		}
		
		logger.traceExit();
	}
}
