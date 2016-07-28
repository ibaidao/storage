using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Utilities;

namespace Core
{
    /// <summary>
    /// 底层算法用到的全局变量
    /// </summary>
    public static class GlobalVariable
    {
        /// <summary>
        /// 接收到的数据队列
        /// </summary>
        public static Queue<Protocol> InteractQueue
        {
            get
            {
                return Singleton<Queue<Protocol>>.GetInstance();
            }
        }
    }
}
