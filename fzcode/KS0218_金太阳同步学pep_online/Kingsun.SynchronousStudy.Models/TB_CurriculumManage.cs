using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_CurriculumManage")]
    public partial class TB_CurriculumManage : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 书本全称
        /// </summary>
        [FieldAttribute("TeachingNaterialName", null, EnumFieldUsage.CommonField, DbType.String)]
        public String TeachingNaterialName { get; set; }

        /// <summary>
        /// 教材学段
        /// </summary>
        [FieldAttribute("EducationLevel", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EducationLevel { get; set; }

        /// <summary>
        /// 学段ID
        /// </summary>
        [FieldAttribute("StageID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StageID { get; set; }

        /// <summary>
        /// 教材科目
        /// </summary>
        [FieldAttribute("CourseCategory", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseCategory { get; set; }

        /// <summary>
        /// 科目ID
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        /// <summary>
        /// 教材版本
        /// </summary>
        [FieldAttribute("TextbookVersion", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TextbookVersion { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 所属年级
        /// </summary>
        [FieldAttribute("JuniorGrade", null, EnumFieldUsage.CommonField, DbType.String)]
        public string JuniorGrade { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 教材书册
        /// </summary>
        [FieldAttribute("TeachingBooks", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingBooks { get; set; }

        /// <summary>
        /// 册别ID
        /// </summary>
        [FieldAttribute("BreelID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BreelID { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>
        [FieldAttribute("CourseCover", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseCover { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 书本ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

    }
}
