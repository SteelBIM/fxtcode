using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserStudyReport")]
    public partial class TB_UserStudyReport : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ClassID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("id", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StudentStudyCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StudentStudyCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ClassStudentCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ClassStudentCount { get; set; }

    }
}
