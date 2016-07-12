using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   /// <summary>
   /// 用户信息表
   /// </summary>
    [Serializable]
    [TableName("Users")]
    [PrimaryKey("ID", Generator.Native)]
    public sealed class Users
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        [ColumnName("ID")]
        public Int32 ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [ColumnName("Name")]
        public String Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [ColumnName("Sex")]
        public Boolean Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [ColumnName("Age")]
        public Int16 Age { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [ColumnName("Phone")]
        public String Phone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [ColumnName("Address")]
        public String Address { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [ColumnName("Job")]
        public String Job { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [ColumnName("Auth")]
        public String Auth { get; set; }
    }
}
