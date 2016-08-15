using System;
using System.Data;

namespace Models.dbHandler
{
    /// <summary>
    /// SQL语句查询实体
    /// </summary>
    internal class Query
    {
        public string Sql { get; set; }
        public CommandType CommandType { get; set; }
        public Func<IDataReader, object> Deserializer { get; set; }
        public Action<IDbCommand, object> ParamReader { get; set; }
        //-未使用
        public object Result { get; set; }
        public int Frequency { get; set; }

    }
}
