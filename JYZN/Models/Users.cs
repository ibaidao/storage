using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public sealed class Users
    {
        public const string FieldID = "ID";
        public const string FieldName = "Name";
        public const string FieldSex = "Sex";
        public const string FieldAge = "Age";
        public const string FieldPhone = "Phone";
        public const string FieldAddress = "Address";
        public const string FieldJob = "Job";
        public const string FieldAuth = "Auth";

        /// <summary>
        /// 人员ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Boolean Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public Int16 Age { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public String Job { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public String Auth { get; set; }

        public static Users ExplainData(System.Data.DataRow item)
        {
            Users objUsers = new Users();

            objUsers.ID = Convert.ToInt32(item[FieldID]);
            objUsers.Name = item[FieldName].ToString();
            objUsers.Sex = Convert.ToBoolean(item[FieldSex]);
            objUsers.Age = Convert.ToInt16(item[FieldAge]);
            objUsers.Phone = item[FieldPhone].ToString();
            objUsers.Address = item[FieldAddress].ToString();
            objUsers.Phone = item[FieldJob].ToString();
            objUsers.Address = item[FieldAuth].ToString();

            return objUsers;
        }
    }
}
