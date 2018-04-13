using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_InterestDubbingGame_UserInfo")]
    public partial class TB_InterestDubbingGame_UserInfo : Kingsun.DB.Action
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
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [FieldAttribute("ContactPhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        [FieldAttribute("VersionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionName { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        [FieldAttribute("ClassName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassName { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        [FieldAttribute("SchoolName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SchoolName { get; set; }
        /// <summary>
        /// 老师名称
        /// </summary>
        [FieldAttribute("TeacherName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeacherName { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        [FieldAttribute("GradeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string GradeName { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }

        /// <summary>
        /// 学校编号
        /// </summary>
        [FieldAttribute("SchoolID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SchoolID { get; set; }

        /// <summary>
        /// 年级编号
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int GradeID { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        [FieldAttribute("ClassID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int ClassID { get; set; }

        /// <summary>
        /// 教师编号
        /// </summary>
        [FieldAttribute("TeacherID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int TeacherID { get; set; }

        /// <summary>
        /// 报名时间
        /// </summary>
        [FieldAttribute("SignUpTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? SignUpTime { get; set; }
    }

    public class StatData
    {
        public int Total { get; set; }

        public int One { get; set; }

        public int Two { get; set; }

        public int Three { get; set; }

        public int Four { get; set; }

        public int Five { get; set; }

        public int Six { get; set; }
    }
}
