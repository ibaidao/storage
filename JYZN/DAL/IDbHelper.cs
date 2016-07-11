namespace DAL
{
    public interface IDbHelper
    {
        /// <summary>
        /// 执行数据库查询，返回DataTable数据集
        /// </summary>
        /// <param name="strSql">对应SQL语句</param>
        /// <param name="cmdParams">参数</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable ExecuteQuery(string strSql, System.Data.Common.DbParameter[] cmdParams);
        
        /// <summary>
        /// 执行DML操作
        /// </summary>
        /// <param name="strSql">对应SQL语句</param>
        /// <param name="cmdParams">参数</param>
        /// <returns>影响的行数</returns>
        int ExecuteNonQuery(string strSql, System.Data.Common.DbParameter[] cmdParams);

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>
        /// <param name="sqlList"><Key,Value> = <SQL语句，对应参数></param>
        /// <returns>影响的行数</returns>
        int ExecuteSqlTran(System.Collections.Hashtable sqlList);

        /// <summary>
        /// 执行存储过程查询
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="cmdParams">存储过程参数</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable ExecuteQueryProcedure(string storeProcName,System.Data.Common.DbParameter[] cmdParams);

        /// <summary>
        /// 执行存储过程DML操作
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="cmdParams">存储过程参数</param>
        /// <returns>影响的行数</returns>
        int ExecuteNonQueryProcedure(string storeProcName, System.Data.Common.DbParameter[] cmdParams);

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">存储过程名或SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns>影响的行数</returns>
        int ExecuteNonQueryTrans(System.Data.Common.DbTransaction trans, System.Data.CommandType cmdType, string cmdText, params System.Data.Common.DbParameter[] cmdParams);
    }
}
