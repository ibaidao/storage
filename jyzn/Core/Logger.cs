using System;
using System.IO;

namespace Core
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object _lockLog = new object();//日志锁
        private static readonly object _lockMsg = new object();//客户端交互信息锁
        /// <summary>
        /// 日志路径
        /// </summary>
        private static readonly string _logPath = System.Environment.CurrentDirectory + "\\Logs\\";

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="ex">异常堆栈信息</param>
        /// <param name="proposal">工程师建议</param>
        public static void WriteLog(string message, Exception ex = null, string proposal = "")
        {
            lock (_lockLog)
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Log.txt";
                System.Text.StringBuilder strMsg = new System.Text.StringBuilder();

                strMsg.Append(string.Format("\n发生时间： {0}", DateTime.Now.ToString("dd HH:mm:ss:fff")));
                if (ex != null)
                {
                    if (ex.InnerException != null) { ex = ex.InnerException; }
                    strMsg.Append(string.Format("\n异常内容：{0}", message + "->" + ex.Message));
                    strMsg.Append(string.Format("\n详细信息：{0}", ex.StackTrace));
                }
                else
                {
                    strMsg.Append(string.Format("\n消息内容： {0}", message));
                }
                strMsg.Append(string.Format("\n可能原因：{0}", proposal));
                strMsg.Append("\n===================================================");

                WriteIntoFile(fileName, strMsg.ToString());
            }
        }

        /// <summary>
        /// 客户端服务器交互信息记录
        /// </summary>
        /// <param name="proto"></param>
        /// <param name="SendFlag">服务端发送(true)/接收(false)</param>
        public static void WriteInteract(Models.Protocol proto, bool SendFlag)
        {
            lock (_lockMsg)
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + "_Msg.txt";
                System.Text.StringBuilder strMsg = new System.Text.StringBuilder();

                strMsg.Append(SendFlag ? "\n发送给设备：" : "\n接收：");
                strMsg.Append(proto.DeviceIP);
                strMsg.Append("\n需要回执");
                strMsg.Append(proto.NeedAnswer);
                strMsg.Append("\n字节总数");
                strMsg.Append(proto.ByteCount);
                foreach (Models.Function function in proto.FunList)
                {
                    strMsg.Append(string.Format("\n名称：{0}；编码：{1}；ID：{2}；字节数：{3}；路径节点：", function.Name, function.Code, function.TargetInfo, function.DataCount));
                    foreach (Models.Location loc in function.PathPoint)
                    {
                        strMsg.Append(loc.ToString());
                    }
                }
                strMsg.Append("\n===================================================");
                
                WriteIntoFile(fileName, strMsg.ToString());
            }
        }

        private static void WriteIntoFile(string fileName, string strMsg)
        {
            //目录是否存在
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
            try
            {
                using (FileStream stream = new FileStream(_logPath + "\\" + fileName, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(stream);
                    sw.WriteLine(strMsg);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("--写日志错误" + e.Message);
            }
        }
    }
}