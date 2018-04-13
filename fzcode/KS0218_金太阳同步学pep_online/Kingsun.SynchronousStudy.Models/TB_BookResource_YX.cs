using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_BookResource_YX")]
    public partial class TB_BookResource_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 资源地址
        /// </summary>
        [FieldAttribute("ResourceUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ResourceUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Column_1", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Column_1 { get; set; }

    }
}
