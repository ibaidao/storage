using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Core;
using Models.dbType;

namespace Models.dbHandler
{
    /// <summary>
    /// 数据库实际操作
    /// </summary>
    internal sealed class DbHelper
    {
        //数据库连接
        private static string _providerName;
        private static DbProviderFactory _dbProviderFactory;
        private static string _connectionString;
        private static DbDialect _sqlDialect = null;
        
        #region 初始化
        /// <summary>
        /// 初始化 -- 
        /// </summary>
        /// <param name="connectionStringName"></param>
        internal DbHelper(string connectionStringName = "DataServer")
        {

            Configuration cc = ConfigurationManager.OpenExeConfiguration("D:\\storage\\JYZN\\Models\\App.config");
            ConnectionStringSettings config = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (config == null)
            {
                Logger.WriteLog("数据库连接未配置", null,  connectionStringName);
                throw new Exception("数据库连接未配置");
            }
            _connectionString = config.ConnectionString;
            _providerName = config.ProviderName;
            //默认
            if (_providerName == "")
                _providerName = "System.Data.SqlClient";
            //初始化数据库--方言
            _sqlDialect = CreateDialect(_providerName);

            _sqlDialect.GetDbName(_connectionString);
        }
        /// <summary>
        /// 初始化数据库方言
        /// </summary>
        /// <param name="_providerName"></param>
        /// <returns></returns>
        private static DbDialect CreateDialect(string _providerName)
        {
            try
            {
                //初始化数据库驱动
                _dbProviderFactory = DbProviderFactories.GetFactory(_providerName);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("初始化失败", ex, "是否将当前版本dll放入程序运行目录！");
            }
            //方言
            switch (_providerName)
            {
                case "System.Data.SqlClient":
                    return new SqlServerDialect();
                case "MySql.Data.MySqlClient":
                    return new MySqlDialect();
                case "System.Data.SQLite":
                    _connectionString = _connectionString.Replace("|Path|", Environment.CurrentDirectory);
                    return new SqliteDialect();
                default:
                    return new SqlServerDialect();

            }
        }

        #endregion

        #region 数据库连接

        /// <summary>
        /// 创建一个数据库连接
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            IDbConnection connection = null;
            try
            {
                connection = _dbProviderFactory.CreateConnection();
                connection.ConnectionString = _connectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Dispose();
                Logger.WriteLog("打开数据库连接失败", ex,  _connectionString);
                throw new Exception("创建数据库连接失败，无法继续");
            }
            return connection;
        }
        #endregion

        #region 初始化表
        internal void Initializer(Type t)
        {

            var tm = CacheMapper.GetTableMapper(t);
            //检查数据表是否存在，不存在则创建表
            string sql = _sqlDialect.GetExistTableSql(tm.TableName);
            if (!(Convert.ToInt32(ExecuteScalar(sql, null)) > 0))
            {
                var columnsMaper = CacheMapper.GetColumnsMapper(t);
                var columnInfos = columnsMaper.Where(c => c.ResultColumn == false).ToList();
                try
                {
                    var createSql = _sqlDialect.GetTableCtreateSql(tm, columnInfos);
                    ExecuteNonQuery(createSql, null);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("初始化表错误（创建表SQL）", ex, "检查：类成员属性");
                }
            }
        }
        #endregion

        #region 底层私有方法 - 执行数据库的操作
        /// <summary>
        /// 执行sql - 返回第一行-第一列值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object ExecuteScalar(Query query, object parameters)
        {
            //执行-使用using
            using (IDbConnection conn = CreateConnection())
            {
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    using (IDbCommand cmd = Command.PrepareCommand(conn, tran, query))
                    {
                        try
                        {
                            if ((parameters is IEnumerable && !(parameters is string || parameters is IEnumerable<KeyValuePair<string, object>>)))
                            {
                                var retVal = 0;
                                #region 分支：默认不启用
                                /* - 批量新增返回所有的自增ID
                                var retVal = new List<object>();
                                bool isFirst = true;
                                foreach (var param in (IEnumerable)parameters)
                                {
                                    if (!isFirst)
                                    {
                                        cmd.Parameters.Clear();
                                    }
                                    else
                                    {
                                        isFirst = false;
                                    }
                                    query.ParamReader(cmd, param);
                                    retVal.Add(cmd.ExecuteScalar());
                                }
                                tran.Commit();
                                return retVal;
                                 * */
                                #endregion
                                bool isFirst = true;
                                foreach (var param in (IEnumerable)parameters)
                                {
                                    if (!isFirst)
                                    {
                                        cmd.Parameters.Clear();
                                    }
                                    else
                                    {
                                        isFirst = false;
                                    }
                                    query.ParamReader(cmd, param);
                                    retVal += cmd.ExecuteNonQuery();
                                }
                                tran.Commit();
                                return retVal;
                            }
                            else
                            {
                                object retVal;
                                query.ParamReader(cmd, parameters);
                                retVal = cmd.ExecuteScalar();
                                tran.Commit();
                                return retVal;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("执行数据库语句失败-（ExecuteScalar）", ex, cmd.CommandText+ string.Join(",", (from IDataParameter parameter in cmd.Parameters select parameter.ParameterName + "=" + parameter.Value).ToArray()));
                            return -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行sql - 返回影响条数
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(Query query, object parameters)
        {
            var retVal = 0;
            //执行-使用using
            using (IDbConnection conn = CreateConnection())
            {
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    using (IDbCommand cmd = Command.PrepareCommand(conn, tran, query))
                    {
                        try
                        {
                            //multi
                            if ((parameters is IEnumerable && !(parameters is string || parameters is IEnumerable<KeyValuePair<string, object>>)))
                            {
                                bool isFirst = true;
                                foreach (var param in (IEnumerable)parameters)
                                {
                                    if (!isFirst)
                                    {
                                        cmd.Parameters.Clear();
                                    }
                                    else
                                    {
                                        isFirst = false;
                                    }
                                    query.ParamReader(cmd, param);
                                    retVal += cmd.ExecuteNonQuery();
                                }
                                tran.Commit();
                            }
                            else
                            {
                                query.ParamReader(cmd, parameters);
                                retVal = cmd.ExecuteNonQuery();
                                tran.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("执行数据库语句失败-（ExecuteNonQuery）", ex, cmd.CommandText+ string.Join(",", (from IDataParameter parameter in cmd.Parameters select parameter.ParameterName + "=" + parameter.Value).ToArray()));
                            retVal = -1;
                        }

                        return retVal;
                    }
                }
            }
        }
        /// <summary>
        /// 执行sql - 返回IDataReader
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private IDataReader ExecuteReader(Query query, object parameters)
        {
            IDataReader dr = null;
            //执行-使用using
            IDbConnection conn = CreateConnection();
            using (IDbCommand cmd = Command.PrepareCommand(conn, null, query))
            {
                try
                {
                    query.ParamReader(cmd, parameters);
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("执行数据库语句失败-（ExecuteReader）", ex, cmd.CommandText+ string.Join(",", (from IDataParameter parameter in cmd.Parameters select parameter.ParameterName + "=" + parameter.Value).ToArray()));
                }
                return dr;
            }
        }

        #endregion

        #region 公共方法 - 外部可调用的数据库操作

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal int ExecuteNonQuery(string sql, object parameters)
        {
            //创建一个查询
            var id = new Unique(sql, _connectionString, parameters == null ? null : parameters.GetType(), null);
            var query = Command.CreateQuery(id, _sqlDialect);
            //调用
            return ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal object ExecuteScalar(string sql, object parameters)
        {
            //创建一个查询
            var id = new Unique(sql, _connectionString, parameters == null ? null : parameters.GetType(), null);
            var query = Command.CreateQuery(id, _sqlDialect);
            //调用
            return ExecuteScalar(query, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal IDataReader ExecuteReader(string sql, object parameters)
        {
            //创建一个查询
            var id = new Unique(sql, _connectionString, parameters == null ? null : parameters.GetType(), null);
            var query = Command.CreateQuery(id, _sqlDialect);
            //调用
            return ExecuteReader(query, parameters);
        }
        #endregion

        #region Create-新增
        /// <summary>
        /// 添加 - 单个/多个 -（同一类型）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">可以是一个Poco,也可以是IEnumrate<Poco></param>
        /// <returns></returns>
        internal object Insert<TEntity>(object entity)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            return Insert(tm, entity);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private object Insert(TableMapper tm, object entity)
        {
            //列-成员
            var columnsMaper = CacheMapper.GetColumnsMapper(tm.PocoType);
            //列名
            var columnNames = columnsMaper.Where(c => c.ResultColumn == false).Select(o => o.ColumnName).ToList();
            //使用-数据库自增时
            if (tm.Generator == Generator.Native)
            {
                columnNames.Remove(tm.PrimaryKey);
            }
            //SQL
            var sql = _sqlDialect.GetInsertSql(tm.TableName, tm.PrimaryKey, columnNames);
            //创建一个查询
            var id = new Unique(sql, _connectionString, entity.GetType(), tm.PocoType);
            var query = Command.CreateQuery(id, _sqlDialect);
            //执行
            return ExecuteScalar(query, entity);
        }
        #endregion

        #region Read-读取
        /// <summary>
        /// 根据主键获得对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="idValue"></param>
        /// <returns></returns>
        internal TEntity GetSingleEntity<TEntity>(object idValue)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            var where = string.Format(" WHERE {0}={1}{0}", tm.PrimaryKey, _sqlDialect.GetParamPrefix());
            return GetSingleEntity<TEntity>(where, new KeyValuePair<string, object>(tm.PrimaryKey, idValue));
        }
        /// <summary>
        /// 根据where条件获得对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal TEntity GetSingleEntity<TEntity>(string where, object parameters)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            //列-成员
            var columns = CacheMapper.GetColumnsMapper(tm.PocoType).Where(c => c.ResultColumn == false).Select(o => o.ColumnName).ToList();
            var sql = _sqlDialect.GetSelectTopSql(tm.TableName, columns, where, 1);
            return Query<TEntity>(sql, parameters).FirstOrDefault();
        }
        /// <summary>
        ///  根据where条件获得List
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal IEnumerable<TEntity> GetEntityList<TEntity>(string where, object parameters)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            var sql = _sqlDialect.GetSelectSql(tm.TableName, where);
            return Query<TEntity>(sql, parameters);
        }

        internal IEnumerable<TEntity> GetEntityList<TEntity>(string where, object parameters, int skip, int take)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            var columns = CacheMapper.GetColumnsMapper(tm.PocoType).Where(c => c.ResultColumn == false).Select(o => o.ColumnName).ToList();
            var sql = _sqlDialect.GetSelectSql(tm.TableName, where);
            var sqlCount = "";
            var pageSql = _sqlDialect.GetPageQuerySql(skip, take, sql, out sqlCount);
            return Query<TEntity>(pageSql, parameters);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        internal IEnumerable<TEntity> GetEntityList<TEntity>(string where, object parameters, int pageIndex, int pageSize, out int recordCount)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            var sql = _sqlDialect.GetSelectSql(tm.TableName, where);
            var sqlCount = "";
            var pageSql = _sqlDialect.GetPageQuerySql((pageIndex - 1) * pageSize, pageSize, sql, out sqlCount);
            recordCount = Convert.ToInt32(ExecuteScalar(sqlCount, parameters));
            return Query<TEntity>(pageSql, parameters);
        }

        internal IEnumerable<TEntity> Query<TEntity>(string view, string where, object parameters, int pageIndex, int pageSize, out int recordCount)
        {
            var sqlCount = "";
            var pageSql = _sqlDialect.GetPageQuerySql((pageIndex - 1) * pageSize, pageSize, view, out sqlCount);
            recordCount = Convert.ToInt32(ExecuteScalar(sqlCount, parameters));
            return Query<TEntity>(pageSql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal IEnumerable<TEnity> Query<TEnity>(string sql, object parameters)
        {
            var type = typeof(TEnity);
            //创建一个查询
            var id = new Unique(sql, _connectionString, (parameters == null ? null : parameters.GetType()), type);
            var query = Command.CreateQuery(id, _sqlDialect);
            //DataReader
            var dr = ExecuteReader(query, parameters);
            //空值
            if (dr == null)
            {
                yield break;
            }
            using (dr)
            {
                if (query.Deserializer == null)
                {
                    query.Deserializer = SqlMapper.GetDeserializer(type, dr);
                }
                while (true)
                {
                    TEnity poco;
                    try
                    {
                        if (!dr.Read())
                            yield break;
                        poco = (TEnity)query.Deserializer(dr);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("查询数据失败（实例化）", ex, query.Sql);
                        yield break;
                    }
                    yield return poco;
                }
            }
        }

        #endregion

        #region Update-修改

        /// <summary>
        /// 更新 - 单个/多个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal int Update<TEntity>(object entity)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            string where = string.Format(" WHERE {0} ={1}{0}", tm.PrimaryKey, _sqlDialect.GetParamPrefix());
            return Update(tm, entity, where);
        }
        /// <summary>
        /// 更新 - 单个/多个
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal int Update<TEntity>(string where, object entity)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            return Update(tm, entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int Update(TableMapper tm, object entity, string where)
        {
            //列-成员
            var columnsMaper = CacheMapper.GetColumnsMapper(tm.PocoType);
            //列名
            var columnNames = columnsMaper.Where(c => c.ResultColumn == false && c.PropertyInfo.GetValue(entity,null) != null).Select(o => o.ColumnName).ToList();
            //使用-数据库自增时
            if (tm.Generator == Generator.Native)
            {
                columnNames.Remove(tm.PrimaryKey);
            }
            //SQL
            var sql = _sqlDialect.GetUpdateSql(tm.TableName, columnNames, where);
            //创建一个查询
            var id = new Unique(sql, _connectionString, entity.GetType(), tm.PocoType);
            var query = Command.CreateQuery(id, _sqlDialect);
            //执行
            return ExecuteNonQuery(query, entity);
        }
        #endregion

        #region Delete-删除
        /// <summary>
        /// 删除 - 主键值 - 单个/多个
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="uid"></param>
        /// <returns></returns>
        internal int Delete<TEntity>(object uid)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            string where = string.Format(" WHERE {0} ={1}{0}", tm.PrimaryKey, _sqlDialect.GetParamPrefix());
            return Delete(tm, where, uid);
        }
        /// <summary>
        /// 删除 - 其他
        /// </summary>
        /// <param name="where"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int Delete<TEntity>(string where, object paramaters)
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            return Delete(tm, where, paramaters);
        }
        /// <summary>
        /// 清空表内容
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        internal int Delete<TEntity>()
        {
            var tm = CacheMapper.GetTableMapper(typeof(TEntity));
            //SQL
            var sql = _sqlDialect.GetTruncateSql(tm.TableName);
            //执行
            return ExecuteNonQuery(sql, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tm"></param>
        /// <param name="where"></param>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        private int Delete(TableMapper tm, string where, object paramaters)
        {
            //SQL
            var sql = _sqlDialect.GetDeleteSql(tm.TableName, where);
            //创建一个查询
            var id = new Unique(sql, _connectionString, paramaters.GetType(), tm.PocoType);
            var query = Command.CreateQuery(id, _sqlDialect);
            //执行
            return ExecuteNonQuery(query, paramaters);
        }
        #endregion
    }
}
