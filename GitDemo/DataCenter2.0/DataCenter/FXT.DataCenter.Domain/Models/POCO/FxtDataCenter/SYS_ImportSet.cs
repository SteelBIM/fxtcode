using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_ImportSet
    {

        /// <summary>
        /// 批量导入字段对应表
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 导入类型
        /// </summary>
        public int ImportType { get; set; }
        /// <summary>
        /// 外部字段名字
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 系统字段
        /// </summary>
        public string SysColumnName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int? ColumnLen { get; set; }
        /// <summary>
        /// 字段小数位数
        /// </summary>
        public int? ColumnDLen { get; set; }
        /// <summary>
        /// 系统表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }

    }
}
