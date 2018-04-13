using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Account.Contract.ViewModel.SystemManage
{
    public class DataBaseTableFieldEntity
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string column { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string datatype { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int? length { get; set; }
        /// <summary>
        /// 允许空
        /// </summary>
        public string isnullable { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string identity { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string defaults { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string remark { get; set; }
    }
}
