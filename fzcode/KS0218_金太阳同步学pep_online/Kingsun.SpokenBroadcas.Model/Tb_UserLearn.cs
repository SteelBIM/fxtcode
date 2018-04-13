using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_UserLearn")]
    /// <summary>
    /// 用户上课记录表
    /// </summary>
    public class Tb_UserLearn : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [FieldAttribute("UserID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// CoursePeriodTimeID
        /// </summary>
        [FieldAttribute("CoursePeriodTimeID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int CoursePeriodTimeID { get; set; }
        /// <summary>
        /// 退出次数
        /// </summary>
        [FieldAttribute("OutTimes", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int OutTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [FieldAttribute("StartTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [FieldAttribute("EndTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.SeedField, DbType.String)]
        public string UserName { get; set; }
        /// <summary>
        /// TrueName
        /// </summary>
        [FieldAttribute("TrueName", null, EnumFieldUsage.SeedField, DbType.String)]
        public string TrueName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [FieldAttribute("TelePhone", null, EnumFieldUsage.SeedField, DbType.String)]
        public string TelePhone { get; set; }
        /// <summary>
        /// CourseID
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int CourseID { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        [FieldAttribute("CourseName", null, EnumFieldUsage.SeedField, DbType.String)]
        public string CourseName { get; set; }
        /// <summary>
        /// CoursePeriodID
        /// </summary>
        [FieldAttribute("CoursePeriodID", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int CoursePeriodID { get; set; }
        /// <summary>
        /// CoursePeriodName
        /// </summary>
        [FieldAttribute("CoursePeriodName", null, EnumFieldUsage.SeedField, DbType.String)]
        public string CoursePeriodName { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        [FieldAttribute("NewPrice", null, EnumFieldUsage.SeedField, DbType.Decimal)]
        public decimal NewPrice { get; set; }
        /// <summary>
        /// CourseStartTime
        /// </summary>
        [FieldAttribute("CourseStartTime", null, EnumFieldUsage.SeedField, DbType.String)]
        public string CourseStartTime { get; set; }
    }
}
