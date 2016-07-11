using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Collections;

namespace DAL
{
    public class MysqlHelper:IDbHelper
    {
        private static readonly string DBConnectionString;

        /// <summary>
        /// 命令执行准备
        /// </summary>
        /// <param name="command">命令执行对象</param>
        /// <param name="conn">连接对象</param>
        /// <param name="trans">事务标志</param>
        /// <param name="cmdType">命令类型（存储过程/SQL语句)</param>
        /// <param name="cmdText">存储过程名或SQL语句</param>
        /// <param name="cmdParms">参数数组</param>
        private void PrepareCommand(DbCommand command, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            command.Connection = conn;

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            if (trans != null)
            {
                command.Transaction = trans;
            }

            if (cmdParms != null)
            {
                foreach (DbParameter param in cmdParms)
                {
                    command.Parameters.Add(param);
                }
            }
        }

        /// <summary>
        /// 数据库初始化
        /// </summary>
        /// <returns>执行结果</returns>
        static MysqlHelper()
        {
            DBConnectionString = "Database='JYZN';Data Source='.';User Id='root';Password='';charset='utf8';pooling=true";
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="strSql">对应SQL语句</param>
        /// <param name="cmdParams">参数</param>
        /// <returns>返回数据集</returns>
        public DataTable ExecuteQuery(string strSql, DbParameter[] cmdParams)
        {
            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection conn = new MySqlConnection(DBConnectionString))
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, strSql, cmdParams);

                DataSet ds = new DataSet();

                try
                {
                    DbDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds, "tmpDataSet");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw (ex);
                }

                cmd.Parameters.Clear();

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 执行DML操作
        /// </summary>
        /// <param name="strSql">对应SQL语句</param>
        /// <param name="cmdParams">参数</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string strSql, DbParameter[] cmdParams)
        {
            DbCommand cmd = new MySqlCommand();

            using (DbConnection conn = new MySqlConnection(DBConnectionString))
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, strSql, cmdParams);

                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                return val;
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>
        /// <param name="sqlList"><Key,Value> = <SQL语句，对应参数></param>
        /// <returns>影响的行数</returns>
        public int ExecuteSqlTran(System.Collections.Hashtable sqlList)
        {
            int val = 0;
            DbCommand cmd = new MySqlCommand();

            using (DbConnection conn = new MySqlConnection(DBConnectionString))
            {
                DbTransaction mysqlTrans = conn.BeginTransaction();

                try
                {
                    foreach (DictionaryEntry item in sqlList)
                    {
                        PrepareCommand(cmd, conn, mysqlTrans, CommandType.Text, item.Key.ToString(), (DbParameter[])item.Value);

                        val += cmd.ExecuteNonQuery();

                        cmd.Parameters.Clear();
                    }
                    mysqlTrans.Commit();
                }
                catch (Exception ex)
                {
                    mysqlTrans.Rollback();
                    Console.WriteLine(ex.Message);
                    throw (ex);
                }
            }
            return val;
        }

        /// <summary>
        /// 执行存储过程查询
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="cmdParams">存储过程参数</param>
        /// <returns>返回数据集</returns>
        public DataTable ExecuteQueryProcedure(string storeProcName, DbParameter[] cmdParams)
        {

            MySqlCommand cmd = new MySqlCommand();

            using (MySqlConnection conn = new MySqlConnection(DBConnectionString))
            {
                PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, storeProcName, null);

                DataSet ds = new DataSet();

                try
                {
                    DbDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds, "tmpDataSet");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw (ex);
                }

                cmd.Parameters.Clear();

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 执行存储过程DML操作
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="cmdParams">存储过程参数</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQueryProcedure(string storeProcName, DbParameter[] cmdParams)
        {
            DbCommand cmd = new MySqlCommand();

            using (DbConnection conn = new MySqlConnection(DBConnectionString))
            {
                PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, storeProcName, cmdParams);

                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                return val;

            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="cmdType">命令类型（存储过程或SQL语句）</param>
        /// <param name="cmdText">存储过程名或SQL语句</param>
        /// <param name="cmdParams">参数数组</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQueryTrans(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] cmdParams)
        {
            DbCommand cmd = new MySqlCommand();

            DbTransaction mysqlTrans = (MySqlTransaction)trans;

            PrepareCommand(cmd, trans.Connection, mysqlTrans, cmdType, cmdText, cmdParams);

            int val = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            return val;
        }
    }
}
