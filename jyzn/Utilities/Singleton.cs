using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingletonE<T> where T : new()
    {
        private static readonly T instance = new T();

        public static T GetInstance()
        {
            return instance;
        }
    }
    /// <summary>
    /// 线程安全的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T> where T : class,new()
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }

}
