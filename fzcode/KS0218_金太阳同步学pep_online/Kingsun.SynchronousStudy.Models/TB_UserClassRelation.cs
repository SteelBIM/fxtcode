using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserClassRelation")]
    public partial class TB_UserClassRelation : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 班级长ID
        /// </summary>
        [FieldAttribute("ClassLongID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? ClassLongID { get; set; }

        /// <summary>
        /// 8位班级短ID
        /// </summary>
        [FieldAttribute("ClassShortID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassShortID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        [FieldAttribute("ClassName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TrueName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TrueName { get; set; }

    }
}
