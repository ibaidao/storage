﻿using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Utilities;

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
        /// 服务器接收时用的端口号
        /// </summary>
        private const Int32 SERVER_COMMUNICATE_PORT = 2580;

        /// <summary>
        /// 数据流结束标志
        /// </summary>
        private const Int32 STRAEM_SING_END = -1;        

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

        public static void StartListening()
        {
            KeepListening = true;
            listenThread = new Thread(Listening);
            listenThread.Start();
        }

        private static void StopListening()
        {
            KeepListening = false;
            //发送数据结束阻塞
            IPAddress[] ipList = Dns.GetHostAddresses(Dns.GetHostName());
            SendBuffer(ipList[1].ToString(),new byte[]{0x00});
        }

        /// <summary>
        /// 服务器监听线程
        /// </summary>
        public static Thread ServerSocketThread
        {
            get { return listenThread; }
        }

        /// <summary>
        /// 发送数据给设备
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <returns></returns>
        public static Models.ErrorCode SendBuffer(string deviceIP, byte[] content)
        {
            Models.ErrorCode sendSuc = Models.ErrorCode.OK;
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

                if (dictStream.ContainsKey(deviceIP))
                    dictStream.Remove(deviceIP);

                sendSuc = Models.ErrorCode.CommunicateDeviceError;
            }

            return sendSuc;
        }

        /// <summary>
        /// 开启循环监听
        /// </summary>
        private static void Listening()
        {
            TcpListener serverListen = new TcpListener(IPAddress.Any, SERVER_COMMUNICATE_PORT);
            serverListen.Start();

            while (KeepListening)
            {
                TcpClient serverReceive = serverListen.AcceptTcpClient();

                string clientIP = serverReceive.Client.RemoteEndPoint.ToString();
                //&*&*一台设备仅运行一个客户端，所以仅有一个链接（本机测试用不同端口测试需注释，上线时启用）
                //clientIP = clientIP.Substring(0, clientIP.IndexOf(':'));
                if (dictStream.ContainsKey(clientIP))
                    dictStream.Remove(clientIP);

                NetworkStream ns = serverReceive.GetStream();
                dictStream.Add(clientIP, ns);

                //子线程读取数据
                DataTransChild dtChild = new DataTransChild(clientIP, ns);
                Thread receiveThread = new Thread(new ParameterizedThreadStart(Receiving));
                receiveThread.Start(dtChild);
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
                    ReceiveByProtocol(dtChild.stream);
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
        private static void ReceiveByProtocol(NetworkStream ns)
        {
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
    }
}
