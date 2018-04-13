using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace CBSS.Framework.Contract
{
    [TableAttribute("TB_UserStudyCurriculum")]
    public partial class TB_UserStudyCurriculum : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ClassShortID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ClassShortID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TextbookVersion", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TextbookVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("JuniorGrade", null, EnumFieldUsage.CommonField, DbType.String)]
        public string JuniorGrade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TeachingBooks", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingBooks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BreelID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BreelID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("id", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StudentStudyCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StudentStudyCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CourseCategory", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        public string Type { get; set; }
    }
}
