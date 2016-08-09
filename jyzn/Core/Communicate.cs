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
        private const Int16 COMMUNICATION_TIME_OUT = 1000;

        /// <summary>
        /// 通讯数据默认缓存大小
        /// </summary>
        private const Int16 DATA_BODY_DEFAULT_MAX = 1024;

        /// <summary>
        /// 设备接收时用的端口号
        /// </summary>
        private const Int32 DEVICE_COMMUNICATE_PORT = 2580;

        /// <summary>
        /// 拣货平台接收时用的端口号
        /// </summary>
        private const Int32 PICK_STATION_COMMUNICATE_PORT = 2583;

        /// <summary>
        /// 服务器接收时用的端口号
        /// </summary>
        private const Int32 SERVER_COMMUNICATE_PORT = 2588;

        /// <summary>
        /// 数据流结束标志
        /// </summary>
        private const Int32 STRAEM_SING_END = -1;

        private const String SERVER_IP_ADDRESS = "192.168.1.105";

        /// <summary>
        /// 耗时计时器
        /// </summary>
        private static Stopwatch sw = new Stopwatch();

        private static bool KeepListening = true;

        /// <summary>
        /// 通过IP映射跟设备的连接
        /// </summary>
        private static Dictionary<string, NetworkStream> dictStream = new Dictionary<string, NetworkStream>();
        
        private static Thread listenThread;

        /// <summary>
        /// 接收数据线程间的通信数据
        /// </summary>
        private class DataTransChild
        {
            /// <summary>
            /// 当前设备的IP
            /// </summary>
            public string IP;

            /// <summary>
            /// 当前通信的连接
            /// </summary>
            public NetworkStream stream;

            public DataTransChild(string ip, NetworkStream ns)
            {
                IP = ip;
                stream = ns;
            }
        }
        #endregion

        public static void StartListening(Models.StoreComponentType currentSystem)
        {
            KeepListening = true;
            int port = GetPortBySystemType(currentSystem);

            listenThread = new Thread(new ParameterizedThreadStart(Listening));
            listenThread.Start(port);
        }

        private static void StopListening()
        {
            KeepListening = false;
            //发送数据结束阻塞
            SendBuffer2Server(new byte[] { 0x00 });
        }

        /// <summary>
        /// 服务器监听线程
        /// </summary>
        public static Thread ServerSocketThread
        {
            get { return listenThread; }
        }

        /// <summary>
        /// 发送数据给客户端
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="clientType">客户端类型</param>
        /// <returns></returns>
        public static Models.ErrorCode SendBuffer2Client(Models.Protocol protocol,Models.StoreComponentType clientType)
        {
            byte[] data = null;
            Coder.EncodeByteData(protocol, ref data);

            return SendBuffer2Client(protocol.DeviceIP, data, clientType);
        }

        /// <summary>
        /// 发送数据给设备
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <param name="clientType">客户端类型</param>
        /// <returns></returns>
        public static Models.ErrorCode SendBuffer2Client(string deviceIP, byte[] content, Models.StoreComponentType clientType)
        {
            int port = GetPortBySystemType(clientType);

            return SendBuffer(deviceIP, port, content);
        }

        /// <summary>
        /// 设备发送数据给服务器
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static Models.ErrorCode SendBuffer2Server(Models.Protocol protocol)
        {
            byte[] data = null;
            Coder.EncodeByteData(protocol, ref data);

            return SendBuffer2Server(data);
        }

        /// <summary>
        /// 设备发送数据给服务器
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <returns></returns>
        public static Models.ErrorCode SendBuffer2Server(byte[] content)
        {
            return SendBuffer(SERVER_IP_ADDRESS, SERVER_COMMUNICATE_PORT, content);
        }

        /// <summary>
        /// 发送数据给设备
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <param name="PortNum">端口号</param>
        /// <returns></returns>
        private static Models.ErrorCode SendBuffer(string deviceIP, int PortNum, byte[] content)
        {
            Models.ErrorCode sendSuc = Models.ErrorCode.OK;

            IPEndPoint IpPoint = new System.Net.IPEndPoint(IPAddress.Parse(deviceIP), PortNum);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            NetworkStream ns = null;
            try
            {
                serverSocket.Connect(IpPoint); 
                ns = new NetworkStream(serverSocket);
                ns.Write(content, 0, content.Length);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("数据发送失败", ex);
                sendSuc = Models.ErrorCode.CommunicateDeviceError;
            }
            finally
            {
                serverSocket.Close();
                if (ns != null)
                {
                    ns.Close();
                    ns.Dispose();
                }
                serverSocket = null;
            }
            return sendSuc;
        }

        /// <summary>
        /// 开启循环监听
        /// </summary>
        /// <param name="threadPara"></param>
        private static void Listening(object threadPara)
        {
            int port = Convert.ToInt32(threadPara);
            TcpListener serverListen = new TcpListener(IPAddress.Any, port);
            serverListen.Start();

            while (KeepListening)
            {
                TcpClient serverReceive = serverListen.AcceptTcpClient();

                string clientIP = serverReceive.Client.RemoteEndPoint.ToString();
                clientIP = clientIP.Substring(0, clientIP.IndexOf(':'));
                if (dictStream.ContainsKey(clientIP))
                    dictStream.Remove(clientIP);

                NetworkStream ns = serverReceive.GetStream();
                dictStream.Add(clientIP, ns);


                DataTransChild dtChild = new DataTransChild(clientIP, ns);
                ReceiveByProtocol(dtChild);
                //子线程读取数据
                //Thread receiveThread = new Thread(new ParameterizedThreadStart(Receiving));
                //receiveThread.Start(dtChild);
            }
        }
        
        /// <summary>
        /// 接收设备发来的数据
        /// </summary>
        private static void Receiving(object obj)
        {
            DataTransChild dtChild = obj as DataTransChild;
            //重复使用已建立的连接
            while (true)
            {
                try
                {
                    //ReceiveByProtocol(dtChild);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("通讯异常，连接中断", ex,string.Format("设备IP：{0}", dtChild.IP));

                    dictStream.Remove(dtChild.IP);
                    dtChild.stream.Close();
                    dtChild.stream.Dispose();
                    dtChild.stream = null;

                    //中断连接后，下次新建连接通信
                    break;
                }
            }
        }

        /// <summary>
        /// 根据协议读取数据
        /// </summary>
        private static void ReceiveByProtocol(DataTransChild dataTrans)
        {
            NetworkStream ns = dataTrans.stream;
             byte[] byteHead = new byte[Coder.PROTOCOL_PACKAGE_SIZE_BYTES];
            List<byte> dataDiscarded = new List<byte>();
            int byteCheck = 0;

            while ((byteCheck = ns.ReadByte()) != STRAEM_SING_END)
            {
                if (byteCheck == Coder.PROTOCOL_REMARK_START)
                    break;
                dataDiscarded.Add((byte)byteCheck);
            }
            if (dataDiscarded.Count != 0)
            {
                Logger.WriteLog("接收到的无效数据：", null, System.Text.Encoding.Default.GetString(dataDiscarded.ToArray()));
                dataDiscarded.Clear();
            }

            if (!ReadBuffer(ns, Coder.PROTOCOL_PACKAGE_SIZE_BYTES, byteHead))
            {
                Logger.WriteLog("数据头读取超时：",null, System.Text.Encoding.Default.GetString(byteHead));
                return;
            }
            int byteCount = byteHead[0] << 8 | byteHead[1];
            byte[] byteBody = new byte[byteCount];            
            if (!ReadBuffer(ns, byteCount, byteBody))
            {
                Logger.WriteLog("数据主体读取超时：", null, System.Text.Encoding.Default.GetString(byteBody));
                return;
            }
            Models.Protocol info = new Models.Protocol();
            info.SourceStream = byteBody;
            info.DeviceIP = dataTrans.IP;
            if (!Coder.DecodeByteData(info, byteBody))
            {
                Logger.WriteLog("数据解码失败：", null, System.Text.Encoding.Default.GetString(byteBody));
                return;
            }
            GlobalVariable.InteractQueue.Enqueue(info);
        }

        /// <summary>
        /// 读取数据流
        /// </summary>
        /// <param name="bt">读取到的数据流</param>
        /// <param name="nCount">待读取字节数</param>
        /// <returns>是否读取成功</returns>
        private static bool ReadBuffer(NetworkStream ns, int nCount, byte[] bt)
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

        /// <summary>
        /// 获取系统端口号
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private static int GetPortBySystemType(Models.StoreComponentType itemType)
        {
            int port = SERVER_COMMUNICATE_PORT;
            switch (itemType)
            {
                case Models.StoreComponentType.Devices:
                    port = DEVICE_COMMUNICATE_PORT;
                    break;
                case Models.StoreComponentType.PickStation:
                    port = PICK_STATION_COMMUNICATE_PORT;
                    break;
                default: break;
            }

            return port;
        }
    }
}
