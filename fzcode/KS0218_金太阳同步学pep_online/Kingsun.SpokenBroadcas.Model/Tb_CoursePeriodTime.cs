using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_CoursePeriodTime")]
    /// <summary>
    /// 课时时间表
    /// </summary>
    public class Tb_CoursePeriodTime : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("CoursePeriodID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int CoursePeriodID { get; set; }
        /// <summary>
        /// 最多预约人数百分百
        /// </summary>
        [FieldAttribute("LimitNum", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int LimitNum { get; set; }
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
        /// 教师类型
        /// </summary>
        [FieldAttribute("TeacherType", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string TeacherType { get; set; }
       
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }
    }
}
