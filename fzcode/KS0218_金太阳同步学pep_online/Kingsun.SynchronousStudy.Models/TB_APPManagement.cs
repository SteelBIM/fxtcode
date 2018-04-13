using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_APPManagement")]
    public partial class TB_APPManagement : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        [FieldAttribute("VersionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionName { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [FieldAttribute("CreatePerson", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CreatePerson { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string ID { get; set; }

    }
}
