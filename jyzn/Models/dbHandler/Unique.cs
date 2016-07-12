using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.dbHandler
{
    /// <summary>
    /// SQL语句唯一判断工具
    /// </summary>
    internal class Unique : IEquatable<Unique>
    {
        /// <summary>
        /// 
        /// </summary>
        public int HashCode { get; private set; }
        public string Sql { get; private set; }
        public Type Type { get; private set; }
        public string ConnString { get; private set; }
        public Type ParametersType { get; private set; }

        /// <summary>
        /// 唯一执行ID
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="parametersType"></param>
        /// <param name="type"></param>
        public Unique(string sql, string connectionString, Type parametersType, Type type)
        {
            this.ConnString = connectionString;
            this.Sql = sql;
            //实体类，or key-Values
            this.Type = type;
            this.ParametersType = parametersType;
            unchecked
            {
                HashCode = 19;
                HashCode = HashCode * 23 + (type == null ? 0 : type.GetHashCode());
                HashCode = HashCode * 23 + (connectionString == null ? 0 : connectionString.GetHashCode());
                HashCode = HashCode * 23 + (sql == null ? 0 : sql.GetHashCode());
                HashCode = HashCode * 23 + (parametersType == null ? 0 : parametersType.GetHashCode());
            }
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
        public bool Equals(Unique other)
        {
            return
                other != null &&
                Type == other.Type &&
                Sql == other.Sql &&
                ConnString == other.ConnString &&
                ParametersType == other.ParametersType;
        }
    }
}
