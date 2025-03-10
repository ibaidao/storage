﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 实体类跟数据表属性 映射
    /// </summary>
    internal class TableMapper
    {
        /// <summary>
        /// Poco类 类型
        /// </summary>
        public Type PocoType
        {
            get;
            set;
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey
        {
            get;
            set;
        }

        /// <summary>
        /// 主键-生成方式
        /// </summary>
        public Generator Generator
        {
            get;
            set;
        }

        /// <summary>
        /// 索引
        /// </summary>
        public List<string>[] IndexKey
        {
            get;
            set;
        }

        /// <summary>
        /// 索引类型
        /// </summary>
        public IndexType[] IndexType
        {
            get;
            set;
        }

        /// <summary>
        /// 从实体类获得表对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TableMapper FromPoco(Type t)
        {
            var ti = new TableMapper();
            ti.PocoType = t;
            //获得表名
            var a = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            ti.TableName = a.Length == 0 ? t.Name : (a[0] as TableNameAttribute).Value;

            //获得主键
            a = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            ti.PrimaryKey = a.Length == 0 ? "ID" : (a[0] as PrimaryKeyAttribute).Name;
            //主键索引方式
            ti.Generator = a.Length == 0 ? Generator.Native : (a[0] as PrimaryKeyAttribute).Generator;

            //解析索引信息
            a = t.GetCustomAttributes(typeof(IndexKeyAttribute), true);
            if (a.Length > 0)
            {
                ti.IndexKey = new List<string>[a.Length];
                ti.IndexType = new IndexType[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    //多个多列索引
                    List<string> indexList = new List<string>();
                    foreach(string s in (a[i] as IndexKeyAttribute).indexList)
                        indexList.Add(s);
                    ti.IndexKey[i] = indexList;
                    ti.IndexType[i] = (a[i] as IndexKeyAttribute).IndexType;
                }
            }
            return ti;
        }
    }
}
