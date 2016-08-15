using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Models.dbHandler
{
    /// <summary>
    /// 数据库访问封装 - 仓储实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccess<T> where T : class
    {
        #region 初始化
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private static DbHelper _dbContext;

        //默认
        public DataAccess()
        {
            //默认配置数据库连接名
            _dbContext = new DbHelper("DataServer");
            _dbContext.Initializer(typeof(T));
        }

        public DataAccess(string dbCofig)
        {
            _dbContext = new DbHelper(dbCofig);
            _dbContext.Initializer(typeof(T));

        }
        #endregion

        #region 添加、修改、删除
        /// <summary>
        /// 添加 - 单个/多个
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object Insert(object entity)
        {
            return _dbContext.Insert<T>(entity);
        }

        /// <summary>
        /// 更新 - 单个/多个
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(object entity)
        {
            return _dbContext.Update<T>(entity);
        }
        /// <summary>
        /// 更新 - 按条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        public int Update(string where, object paramaters)
        {
            return _dbContext.Update<T>(where, paramaters);
        }

        /// <summary>
        /// 删除 - 主键值 - 单个/多个
        /// <example>
        /// Delete(new { ID=1 }),Delete(List-ID),
        /// </example>
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int Delete(object uid)
        {
            return _dbContext.Delete<T>(uid);
        }
        /// <summary>
        /// 删除 - 所有：清空表内容
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int Delete()
        {
            return _dbContext.Delete<T>();
        }
        /// <summary>
        /// 删除 - 按条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        public int Delete(string where, object paramaters)
        {
            return _dbContext.Delete<T>(where, paramaters);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 主键查询 -Entity
        /// </summary>   
        /// <param name="idValue">主键值</param>
        /// <returns></returns>
        public T GetSingleEntity(object idValue)
        {
            return _dbContext.GetSingleEntity<T>(idValue);
        }
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingleEntity(string where, object parameters)
        {
            return _dbContext.GetSingleEntity<T>(where, parameters);
        }
        /// <summary>
        /// 所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetEntityList()
        {
            return _dbContext.GetEntityList<T>(" 1=1 ", null).ToList();
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetEntityList(string where, object parameters)
        {
            return _dbContext.GetEntityList<T>(where, parameters).ToList();
        }
        /// <summary>
        ///  分页查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="parameters"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public List<T> GetEntityList(string where, object parameters, int pageIndex, int pageSize, out int recordCount)
        {
            return _dbContext.GetEntityList<T>(where, parameters, pageIndex, pageSize, out recordCount).ToList();
        }
        #endregion

        #region 其他写法
        /// <summary>
        /// where
        /// </summary>
        private static KeyValuePair<string, object> _where;
        private static int _skip;
        private static int _take;
        /// <summary>
        /// Where 条件
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public DataAccess<T> Where(Expression<Func<T, bool>> exp)
        {
            _where = new WhereExpression().ToSql(exp.Body);
            return this;
        }

        public DataAccess<T> OrderyBy(int skip)
        {
            _skip = skip;
            return this;
        }

        public DataAccess<T> Skip(int skip)
        {
            _skip = skip;
            return this;
        }
        public DataAccess<T> Take(int take)
        {
            _take = take;
            return this;
        }
        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            if (_take > 0)
            {
                return _dbContext.GetEntityList<T>(_where.Key, _where.Value, _skip, _take).ToList();
            }
            else
            {
                return GetEntityList(_where.Key, _where.Value);
            }
        }
        /// <summary>
        /// Fist
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            return GetSingleEntity(_where.Key, _where.Value);
        }
        /// <summary>
        /// FistDefault
        /// </summary>
        /// <returns></returns>
        public T FirstDefault()
        {
            T t = First();
            if (t == null)
            {
                t = Activator.CreateInstance<T>();
            }
            return t;
        }
        #endregion
    }


    /// <summary>
    /// 数据库访问封装 - Sql
    /// </summary>
    public class DataAccess
    {
        #region 初始化
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private static DbHelper _dbContext;
        //默认
        public DataAccess()
        {
            //默认配置数据库连接名
            _dbContext = new DbHelper("DataServer");
        }

        public DataAccess(string dbCofig)
        {
            _dbContext = new DbHelper(dbCofig);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, object parameters)
        {
            return _dbContext.ExecuteNonQuery(sql, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object parameters)
        {
            return _dbContext.ExecuteReader(sql, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql)
        {
            return ExecuteScalar<T>(sql, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object parameters)
        {
            return (T)_dbContext.ExecuteScalar(sql, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql)
        {
            return Query<T>(sql, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            return _dbContext.Query<T>(sql, parameters);
        }

        public IEnumerable<T> Query<T>(string view, string where, object parameters, int pageIndex, int pageSize, out int recordCount)
        {
            return _dbContext.Query<T>(view, where, parameters, pageIndex, pageSize, out recordCount);
        }
        #endregion
    }
}
