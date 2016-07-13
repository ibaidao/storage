using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 数据表的主键类型
    /// </summary>
    public enum Generator : byte
    {
        /// <summary>
        /// GUID
        /// </summary>
        Guid,
        /// <summary>
        /// 默认-数据库自带方式
        /// </summary>
        Native,
        /// <summary>
        /// 序列-程序编写全局的序列使用
        /// </summary>
        Sequence,
        /// <summary>
        /// 程序赋值
        /// </summary>
        Assigned
    }

    /// <summary>
    /// 数据表的索引类型
    /// </summary>
    public enum IndexType : byte
    {
        /// <summary>
        /// 普通索引
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 唯一索引
        /// </summary>
        Unique = 2,

        /// <summary>
        /// 全文索引
        /// </summary>
        FullText = 4,

        /// <summary>
        /// 空间索引
        /// </summary>
        Spatial = 8,

        /// <summary>
        /// 多列索引
        /// </summary>
        MultiColumn = 16,

        /// <summary>
        /// 聚集索引
        /// </summary>
        Clustered = 32
    }
}
