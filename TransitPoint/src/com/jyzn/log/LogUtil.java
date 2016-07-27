package com.jyzn.log;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class LogUtil {
	
	/**
	 * 设置日志全为异步日志
	 */
	public static final void makeAllLoggersAsynchronous() {
		System.setProperty("Log4jContextSelector", "org.apache.logging.log4j.core.async.AsyncLoggerContextSelector");
	}
	
	/**
	 * 获取记录程序运行日志记录器
	 * @return
	 */
	public static Logger getAppLogger() {
		return LogManager.getLogger("app");
	}
	
	/**
	 * 获取记录系统运行信息的日志记录器
	 * @return
	 */
	public static Logger getSysLogger() {
		return LogManager.getLogger("sys");
	}
}
