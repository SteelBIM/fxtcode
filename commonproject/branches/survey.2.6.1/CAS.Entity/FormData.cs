using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CAS.Entity
{
    public class FormData
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string cfield { get; set; }
        /// <summary>
        /// 字段的值
        /// </summary>
        public string value { get; set; }
    }
    public class Field : FormData
    {
        /// <summary>
        /// 字段类型
        /// </summary>
        public SqlDbType type { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public string operatoin { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string alias { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string parame { get; set; }
        /// <summary>
        /// 查询字符串
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 1:只插入参数，0：插入sql和参数
        /// </summary>
        public int insertype { get; set; }
    }
}
