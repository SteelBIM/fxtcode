using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    [TableAttribute("TB_UUMSUser")]
    public partial class TB_UUMSUser : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("State", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsAvatarTrue", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsAvatarTrue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("GradeStartYear", null, EnumFieldUsage.CommonField, DbType.String)]
        public string GradeStartYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SubjectName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SubjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AeraID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AeraID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("PersonID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PersonID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StageID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? StageID { get; set; }

        /// <summary>
        /// 性别 。1代表男2代表女
        /// </summary>
        [FieldAttribute("Gender", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AppID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("LastLogContry", null, EnumFieldUsage.CommonField, DbType.String)]
        public string LastLogContry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SchoolID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SchoolID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("GradeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? GradeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AvatarUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UploadAvatarDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? UploadAvatarDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ProID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ProID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CityID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CityID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Host", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Host { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("RegDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("LoginNum", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? LoginNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("LastLoginDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Grade", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Grade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SchoolName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SchoolName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Email", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Address", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Telephone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Telephone { get; set; }

        /// <summary>
        /// 在第二次迭代中该字段用来标识用户
        ///1.普通用户 12.老师 26.学生
        /// </summary>
        [FieldAttribute("UserType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TrueName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TrueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("PassWord", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PassWord { get; set; }

    }
}
