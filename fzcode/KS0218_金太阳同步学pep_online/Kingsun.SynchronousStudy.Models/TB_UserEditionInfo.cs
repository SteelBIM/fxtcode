using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserEditionInfo")]
    public partial class TB_UserEditionInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

    }
}
