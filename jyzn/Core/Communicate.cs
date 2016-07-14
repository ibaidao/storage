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
        /// 设备通讯超时时间（ms）
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

        /// <summary>
        /// 耗时计时器
        /// </summary>
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// 通过IP映射跟设备的连接
        /// </summary>
        private Dictionary<string, NetworkStream> dictStream = new Dictionary<string, NetworkStream>();

        private Socket ListenSocket = null;

        private byte[] byteHead = new byte[14];
        private byte[] byteBody = new byte[1024];
        #endregion

        public Communicate()
        {
            IPHostEntry myEntry = Dns.GetHostEntry(Dns.GetHostName());
            TcpListener ServerListen = new TcpListener(myEntry.AddressList[0], SERVER_COMMUNICATE_PORT);
            ServerListen.Start();
            ListenSocket = ServerListen.AcceptSocket();
        }

        /// <summary>
        /// 接收设备发来的消息
        /// </summary>
        public void Receive()
        {
            NetworkStream ns = null;
            try
            {
                //1.监听端口
                ns = new NetworkStream(ListenSocket);

                while (ns.ReadByte() != Coder.PROTOCOL_REMARK_START) ;

                if (!ReadBuffer(ns, 14, byteHead))
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
                GlobalVariable.InteractList.Enqueue(info);
                //        if (UCList.ParaOver.thExecute.ThreadState == ThreadState.Suspended)
                //              UCList.ParaOver.thExecute.Resume();

            }
            catch (Exception ex)
            {
                //Configs.BaseConfigs.PrintError(ex);
                //
                //2009-11-13,异常处理 ,重新启动端口监听
                //ServerListen.Stop();
                //ServerListen.Start();
            }

            //10.关闭通信
            finally
            {
                //ns.Close();
                //ns.Dispose();
                //ListenSocket.Close();
                //ListenSocket = null;

                //ServerListen.Stop();
                //ServerListen = null;
            }
        }

        /// <summary>
        /// 发送数据给设备
        /// </summary>
        /// <param name="deviceIP">设备IP</param>
        /// <param name="content">待发送数据</param>
        /// <returns></returns>
        public bool Send(string deviceIP, byte[] content)
        {
            bool sendSuc = false;
            Socket serverSocket = null;
            NetworkStream ns = dictStream[deviceIP];
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
        public bool ReadBuffer(NetworkStream ns, int nCount, byte[] bt)
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
