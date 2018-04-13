using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    /// <summary>
    /// 班级学习数据视图
    /// </summary>
    [TableAttribute("View_LRClassListData")]
    public partial class View_LRClassListData : Kingsun.DB.Action
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserID { get; set; }

        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [FieldAttribute("ClassLongID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? ClassLongID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [FieldAttribute("ClassName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassName { get; set; }


    }
}
