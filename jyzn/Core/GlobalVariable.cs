using System;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public static class GlobalVariable
    {
        /// <summary>
        /// 接收到的数据队列
        /// </summary>
        public static Queue<Protocol> InteractList
        {
            get
            {
                return Singleton<Queue<Protocol>>.GetInstance();
            }
        }
    }
}
