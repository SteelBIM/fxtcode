using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_Course")]
    public partial class TB_Course : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "", EnumFieldUsage.PrimaryKey, DbType.String)]
        public string ID { get; set; }

        /// <summary>
        /// 学期ID
        /// </summary>
        [FieldAttribute("TermID", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TermID { get; set; }

        /// <summary>
        /// 是否启用(默认禁用)
        /// </summary>
        [FieldAttribute("Disable", "((0))", EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? Disable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [FieldAttribute("ModifyDateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? ModifyDateTime { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        [FieldAttribute("GradeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string GradeName { get; set; }

        /// <summary>
        /// 学段名称
        /// </summary>
        [FieldAttribute("StageName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string StageName { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        [FieldAttribute("TermName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TermName { get; set; }

        /// <summary>
        /// 课程当前版本
        /// </summary>
        [FieldAttribute("Version", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Version { get; set; }

        /// <summary>
        /// 封皮图片URL
        /// </summary>
        [FieldAttribute("ImageURL", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImageURL { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [FieldAttribute("Creator", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Creator { get; set; }

        /// <summary>
        /// 书籍介绍
        /// </summary>
        [FieldAttribute("Description", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Description { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [FieldAttribute("CourseName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        [FieldAttribute("StageID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StageID { get; set; }

        /// <summary>
        /// 学科名称
        /// </summary>
        [FieldAttribute("SubjectName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SubjectName { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("EditionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? EditionID { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        [FieldAttribute("EditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EditionName { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 所属应用ID
        /// </summary>
        [FieldAttribute("AppID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? AppID { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }


    }
}
