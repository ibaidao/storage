using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Linq;

namespace Models.dbType
{
    /// <summary>
    /// 拆分-SQL
    /// </summary>
    internal class SqlPart
    {
        public string Sql { get; set; }
        public string SqlCount { get; set; }
        public string SqlOrderBy { get; set; }
        public string SqlNoSelect { get; set; }
    }

    /// <summary>
    /// 数据库方言 - 生成SQL
    /// </summary>
    internal abstract class DbDialect
    {
        #region 公共
        /// <summary>
        /// 数据库类型对应
        /// </summary>
        public ConcurrentDictionary<Type, string> _DataTypeMapper;

        #region 正则-拆分sql
        //查询列
        public readonly static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //排序
        public readonly static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        //distinct
        public readonly static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        /// <summary>
        /// 拆分sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlPart GetSqlPart(string sql)
        {
            var sqlPart = new SqlPart();
            //拆列
            var mColumns = rxColumns.Match(sql);
            //拆order by
            var mOrderBy = rxOrderBy.Match(sql);
            // Count(*)
            Group g = mColumns.Groups[1];
            sqlPart.SqlNoSelect = mOrderBy.Success ? sql.Substring(g.Index).Replace(mOrderBy.Value, "") : sql.Substring(g.Index);

            if (rxDistinct.IsMatch(sqlPart.SqlNoSelect))
            {
                sqlPart.SqlCount = "SELECT COUNT(" + g.Value.Trim() + ") AS RowsCount " + sqlPart.SqlNoSelect.Replace(g.Value, "");
            }
            else
            {
                sqlPart.SqlCount = "SELECT COUNT(*) AS RowsCount " + sqlPart.SqlNoSelect.Replace(g.Value, "");
            }
            sqlPart.SqlOrderBy = mOrderBy.Value;
            return sqlPart;
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public bool HasParamater(string sql, string paramName)
        {
            return Regex.IsMatch(sql, @"[?@:]" + paramName + "([^a-z0-9_]+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
        }
        #endregion

        #region INSERT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primarykey"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public virtual string GetInsertSql(string tableName, string primarykey, List<string> columnNames)
        {
            var format = "INSERT INTO {0} ({1}) VALUES ({2}) {3}";
            return string.Format(format,
                    tableName,
                    string.Join(",", columnNames),
                    string.Join(",", columnNames.Select(p => GetParamPrefix() + p)),
                    GetInsertReturnVal());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetInsertReturnVal()
        {
            return ";\nSELECT @@IDENTITY AS NewID;";
        }
        #endregion

        #region UPDATE
        public virtual string GetUpdateSql(string tableName, IEnumerable<string> columnNames, string where)
        {
            var format = "UPDATE {0} SET {1} {2}";
            return string.Format(format,
                    tableName,
                    string.Join(",", columnNames.Select(name => name + "=" + GetParamPrefix() + name)),
                    where,
                    GetParamPrefix());
        }
        #endregion

        #region DELETE
        public virtual string GetDeleteSql(string tableName, string where)
        {
            var format = "DELETE FROM {0} {1}";
            return string.Format(format,
                    tableName,
                    where);
        }
        #endregion

        #region SELECT
        public virtual string GetSelectSql(string tableName, string where)
        {
            if (where.IndexOf("-->>") == -1)
            {
                return string.Format("SELECT * FROM {0} {1}", tableName, where);
            }
            else
            {
                return string.Format("SELECT {2} FROM {0} {1}", tableName, where.Substring(0, where.IndexOf("-->>")), where.Substring(where.IndexOf("-->>") + 10));
            }
        }

        public virtual string GetSelectTopSql(string tableName, List<string> columns, string where, int top)
        {
            if (where.IndexOf("-->>") == -1)
            {
                return string.Format("SELECT TOP {2} *  FROM {0} {1}", tableName, where, top);
            }
            else
            {
                return string.Format("SELECT TOP {2} {3} FROM {0} {1}", tableName, where.Substring(0, where.IndexOf("-->>")), top, where.Substring(where.IndexOf("-->>") + 10));
            }
        }
        #endregion

        #endregion

        #region 重写方法
        /// <summary>
        /// 格式化--使用参数查询时，替换？ @  :
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract string FormatSql(string sql);
        /// <summary>
        /// 获得参数前缀
        /// </summary>
        /// <returns></returns>
        public abstract string GetParamPrefix();
        /// <summary>
        /// 获得分页查询
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <param name="sqlCount"></param>
        /// <returns></returns>
        public abstract string GetPageQuerySql(int skip, int take, string sql, out string sqlCount);
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public abstract string GetExistTableSql(string table);
        /// <summary>
        /// 获得表创建语句
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="columnInfos"></param>
        /// <returns></returns>
        public abstract string GetTableCtreateSql(TableMapper ti, List<ColumnMapper> columnInfos);
        /// <summary>
        /// 清空表数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract string GetTruncateSql(string tableName);
        #endregion

    }
}
