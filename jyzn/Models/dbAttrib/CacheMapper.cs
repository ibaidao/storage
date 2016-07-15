using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Core;

namespace Models
{
    /// <summary>
    /// 实体类 跟 数据库 关系映射
    /// </summary>
    internal class CacheMapper
    {

        static ConcurrentDictionary<Type, DbType> _typeMapper;
        static ConcurrentDictionary<Type, TableMapper> _tableMapperCache = new ConcurrentDictionary<Type, TableMapper>();
        static ConcurrentDictionary<Type, List<ColumnMapper>> _columnsMapperCache = new ConcurrentDictionary<Type, List<ColumnMapper>>();

        static CacheMapper()
        {
            _typeMapper = new ConcurrentDictionary<Type, DbType>();
            _typeMapper[typeof(byte)] = DbType.Byte;
            _typeMapper[typeof(sbyte)] = DbType.SByte;
            _typeMapper[typeof(short)] = DbType.Int16;
            _typeMapper[typeof(ushort)] = DbType.UInt16;
            _typeMapper[typeof(int)] = DbType.Int32;
            _typeMapper[typeof(uint)] = DbType.UInt32;
            _typeMapper[typeof(long)] = DbType.Int64;
            _typeMapper[typeof(ulong)] = DbType.UInt64;
            _typeMapper[typeof(float)] = DbType.Single;
            _typeMapper[typeof(double)] = DbType.Double;
            _typeMapper[typeof(decimal)] = DbType.Decimal;
            _typeMapper[typeof(bool)] = DbType.Boolean;
            _typeMapper[typeof(string)] = DbType.String;
            _typeMapper[typeof(char)] = DbType.StringFixedLength;
            _typeMapper[typeof(Guid)] = DbType.Guid;
            _typeMapper[typeof(DateTime)] = DbType.DateTime;
            _typeMapper[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            _typeMapper[typeof(TimeSpan)] = DbType.Time;
            _typeMapper[typeof(byte[])] = DbType.Binary;
            _typeMapper[typeof(byte?)] = DbType.Byte;
            _typeMapper[typeof(sbyte?)] = DbType.SByte;
            _typeMapper[typeof(short?)] = DbType.Int16;
            _typeMapper[typeof(ushort?)] = DbType.UInt16;
            _typeMapper[typeof(int?)] = DbType.Int32;
            _typeMapper[typeof(uint?)] = DbType.UInt32;
            _typeMapper[typeof(long?)] = DbType.Int64;
            _typeMapper[typeof(ulong?)] = DbType.UInt64;
            _typeMapper[typeof(float?)] = DbType.Single;
            _typeMapper[typeof(double?)] = DbType.Double;
            _typeMapper[typeof(decimal?)] = DbType.Decimal;
            _typeMapper[typeof(bool?)] = DbType.Boolean;
            _typeMapper[typeof(char?)] = DbType.StringFixedLength;
            _typeMapper[typeof(Guid?)] = DbType.Guid;
            _typeMapper[typeof(DateTime?)] = DbType.DateTime;
            _typeMapper[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            _typeMapper[typeof(TimeSpan?)] = DbType.Time;
            _typeMapper[typeof(object)] = DbType.Object;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DbType GetDbType(Type t)
        {
            DbType dbType;
            if (!_typeMapper.TryGetValue(t, out dbType))
            {
                Logger.WriteLog("数据类型异常");
            }
            return dbType;
        }
        
        /// <summary>
        /// 获得实体类中的表信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableMapper GetTableMapper(Type t)
        {
            TableMapper ti;
            if (!_tableMapperCache.TryGetValue(t, out ti))
            {
                ti = TableMapper.FromPoco(t);
                _tableMapperCache[t] = ti;
            }
            return ti;
        }

        /// <summary>
        /// 获得实体类中的列信息
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static List<ColumnMapper> GetColumnsMapper(object o)
        {
            //如果是list 一类
            if (o is IList)
            {
                o = (o as IList)[0];
            }
            Type t = (o is Type) ? o as Type : o.GetType();
            //列名
            List<ColumnMapper> columnNames;
            if (!_columnsMapperCache.TryGetValue(t, out columnNames))
            {
                columnNames = new List<ColumnMapper>();
                var properties = t.GetProperties();
                foreach (var pi in properties)
                {
                    ColumnMapper ci = ColumnMapper.FromProperty(pi);
                    columnNames.Add(ci);
                }
                _columnsMapperCache[o.GetType()] = columnNames;
            }
            return columnNames;
        }
    }
}
