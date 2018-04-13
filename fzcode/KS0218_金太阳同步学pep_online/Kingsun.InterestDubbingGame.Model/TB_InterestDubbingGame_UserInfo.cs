using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    [TableAttribute("TB_InterestDubbingGame_UserInfo")]
    public class TB_InterestDubbingGame_UserInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }


        /// <summary>
        /// UserName
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 地区ID
        /// </summary>
        [FieldAttribute("AreaID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int AreaID { get; set; }
        /// <summary>
        /// 地区名
        /// </summary>
        [FieldAttribute("AreaName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AreaName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int State { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("UserImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserImage { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [FieldAttribute("ContactPhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }
        /// <summary>
        /// 年级名称
        /// </summary>
        [FieldAttribute("VersionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("SignUpTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime SignUpTime { get; set; }
        /// <summary>
        /// SchoolID
        /// </summary>
        [FieldAttribute("SchoolID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SchoolID { get; set; }
        /// <summary>
        /// SchoolName
        /// </summary>
        [FieldAttribute("SchoolName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SchoolName { get; set; }
        /// <summary>
        /// GradeID
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }
        /// <summary>
        /// 年级名称
        /// </summary>
        [FieldAttribute("GradeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string GradeName { get; set; }
        /// <summary>
        /// ClassID
        /// </summary>
        [FieldAttribute("ClassID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassID { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        [FieldAttribute("ClassName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassName { get; set; }
        /// <summary>
        /// 老师ID
        /// </summary>
        [FieldAttribute("TeacherID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeacherID { get; set; }
        /// <summary>
        /// 教师名称
        /// </summary>
        [FieldAttribute("TeacherName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeacherName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }
    }


    public class Redis_InterestDubbingGame_UserInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>

        public int ID { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>

        public string UserID { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// UserName
        /// </summary>

        public string UserName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>

        public string ContactPhone { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>

        public int VersionID { get; set; }
        /// <summary>
        /// 年级名称
        /// </summary>

        public string VersionName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public string SignUpTime { get; set; }
        /// <summary>
        /// SchoolID
        /// </summary>

        public int? SchoolID { get; set; }
        /// <summary>
        /// SchoolName
        /// </summary>

        public string SchoolName { get; set; }
        /// <summary>
        /// GradeID
        /// </summary>

        public int? GradeID { get; set; }
        /// <summary>
        /// 年级名称
        /// </summary>

        public string GradeName { get; set; }
        /// <summary>
        /// ClassID
        /// </summary>

        public string ClassID { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>

        public string ClassName { get; set; }
        /// <summary>
        /// 老师ID
        /// </summary>

        public string TeacherID { get; set; }
        /// <summary>
        /// 教师名称
        /// </summary>

        public string TeacherName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>       
        public string CreateTime { get; set; }
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
        public int Other { get; set; }
    }
}
