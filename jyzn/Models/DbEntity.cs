using System;
using Core;
using Models.dbHandler;

namespace Models
{
    /// <summary>
    /// 各数据表外表调用实体
    /// </summary>
    public static class DbEntity
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public static DataAccess<Users> DUser
        {
            get
            {
                return Singleton<DataAccess<Users>>.GetInstance();
            }
        }
    }
}
