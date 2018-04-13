using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserStatistics")]
    public partial class TB_UserStatistics : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// AppID
        /// </summary>
        [FieldAttribute("AppID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppID { get; set; }

        /// <summary>
        /// 使用时长
        /// </summary>
        [FieldAttribute("UseTime", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UseTime { get; set; }

        /// <summary>
        /// 七日内使用次数
        /// </summary>
        [FieldAttribute("Number", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Number { get; set; }

        /// <summary>
        /// 最后启动时间
        /// </summary>
        [FieldAttribute("LoginTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserID { get; set; }

    }
}
