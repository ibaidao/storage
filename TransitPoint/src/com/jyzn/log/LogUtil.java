package com.jyzn.log;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class LogUtil {
	
	/**
	 * ������־ȫΪ�첽��־
	 */
	public static final void makeAllLoggersAsynchronous() {
		System.setProperty("Log4jContextSelector", "org.apache.logging.log4j.core.async.AsyncLoggerContextSelector");
	}
	
	/**
	 * ��ȡ��¼����������־��¼��
	 * @return
	 */
	public static Logger getAppLogger() {
		return LogManager.getLogger("app");
	}
	
	/**
	 * ��ȡ��¼ϵͳ������Ϣ����־��¼��
	 * @return
	 */
	public static Logger getSysLogger() {
		return LogManager.getLogger("sys");
	}
}
