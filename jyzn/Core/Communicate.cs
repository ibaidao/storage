using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// 设备通信
    /// </summary>
    public class Communicate
    {
        #region 变量定义
        /// <summary>
        /// 数据接收超时时间（ms）
        /// </summary>
        private const Int32 COMMUNICATION_TIME_OUT = 1000;

        /// <summary>
        /// 设备接收时用的端口号
        /// </summary>
        private const Int32 DEVICE_COMMUNICATE_PORT = 2580;

        /// <summary>
        /// 服务器接收时用的端口号
        /// </summary>
        private const Int32 SERVER_COMMUNICATE_PORT = 2580;
        
        private byte[] byteHead = new byte[Coder.PROTOCOL_HEAD_BYTES_COUNT];
        private byte[] byteBody = new byte[1024];

        /// <summary>
        /// 耗时计时器
        /// </summary>
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 通过IP映射跟设备的连接
        /// </summary>
        private Dictionary<string, NetworkStream> dictStream = new Dictionary<string, NetworkStream>();

        private TcpListener serverListen = null;

        private Thread listenThread;
        #endregion

        public Communicate()
        {
        }

        public void StartListening()
        {
            serverListen = new TcpListener(IPAddress.Any, SERVER_COMMUNICATE_PORT);
            serverListen.Start();

            listenThread = new Thread(Listening);
            listenThread.Start();
        }

        /// <summary>
        /// 服务器监听线程
        /// </summary>
        public Thread ServerSocketThread
        {
            get { return this.listenThread; }
        }

        /// <summary>
        /// 开始监听，并接受端口数据
        /// </summary>
        private void Listening()
        {
            while (true)
            {
                TcpClient serverReceive = serverListen.AcceptTcpClient();

                string clientIP = serverReceive.Client.RemoteEndPoint.ToString();
                //一台设备仅运行一个客户端，所以仅有一个链接（测试阶段用不同端口测试需注释）
                clientIP = clientIP.Substring(0, clientIP.IndexOf(':'));

                NetworkStream ns = serverReceive.GetStream();
                if (!dictStream.ContainsKey(clientIP))
                    dictStream.Add(clientIP, ns);

                Thread receiveThread = new Thread(new ParameterizedThreadStart(Receiving));
                receiveThread.Start(ns);
            }
        }

        /// <summary>
        /// 接收设备发来的消息
        /// </summary>
        private void Receiving(object obj)
        {
            try
            {
                NetworkStream ns = (NetworkStream)obj;
                while (ns.ReadByte() != Coder.PROTOCOL_REMARK_START) ;

                if (!ReadBuffer(ns, Coder.PROTOCOL_HEAD_BYTES_COUNT, byteHead))
                {
                    Console.WriteLine("数据头读取超时：", System.Text.Encoding.Default.GetString(byteHead));
                    return;
                }

                Protocol info = Coder.DecodeHead(byteHead);

                if (!ReadBuffer(ns, info.BodyByteCount + 2, byteBody))
                {
                    Console.WriteLine("数据主体读取超时：", System.Text.Encoding.Default.GetString(byteBody));
                    return;
                }

                info.SourceStream = byteBody;
                GlobalVariable.InteractQueue.Enqueue(info);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("通讯异常，监听结束", ex);

                //重新启动端口监听
                serverListen.Stop();
                serverListen.Start();
            }
        }

        /// <summary>
        /// 发送数据给设备
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <returns></returns>
        public bool SendBuffer(string deviceIP, byte[] content)
        {
            bool sendSuc = false;
            Socket serverSocket = null;
            NetworkStream ns = dictStream.ContainsKey(deviceIP) ? dictStream[deviceIP] : null;
            try
            {
                if (ns == null)
                {
                    IPEndPoint IpPoint = new System.Net.IPEndPoint(IPAddress.Parse(deviceIP), DEVICE_COMMUNICATE_PORT);
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Connect(IpPoint);
                    ns = new NetworkStream(serverSocket);
                    
                    dictStream.Add(deviceIP, ns);
                }

                ns.Write(content, 0, content.Length);

                sendSuc = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("数据发送失败", ex);

                serverSocket.Close();
                if (ns != null)
                {
                    ns.Close();
                    ns.Dispose();
                }
                serverSocket = null;

                sendSuc = false;
            }

            return sendSuc;
        }

        /// <summary>
        /// 读取数据流
        /// </summary>
        /// <param name="bt">读取到的数据流</param>
        /// <param name="nCount">待读取字节数</param>
        /// <returns>是否读取成功</returns>
        private bool ReadBuffer(NetworkStream ns, int nCount, byte[] bt)
        {
            byte[] inread = new byte[nCount];
            int already_read = 0, this_read = 0;

            sw.Reset();
            sw.Start();
            while (already_read < nCount && ns.CanRead && sw.ElapsedMilliseconds < COMMUNICATION_TIME_OUT)
            {
                this_read = ns.Read(bt, already_read, nCount - already_read);
                already_read = already_read + this_read;
                if (already_read < nCount)
                {//若还有数据未读，则休息10毫秒等待数据
                    Thread.Sleep(10);
                }
            }
            sw.Stop();
            return nCount == already_read;
        }
    }
}
