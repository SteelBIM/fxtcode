using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserStudyDirectory")]
    public partial class TB_UserStudyDirectory : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ClassStudentCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ClassStudentCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ClassID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstModular", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstModular { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StudentStudyCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StudentStudyCount { get; set; }

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
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VideoNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstModularID { get; set; }

    }
}
