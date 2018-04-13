using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_CoursePeriod")]
    /// <summary>
    /// 课时表
    /// </summary>
    public class Tb_CoursePeriod : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        [FieldAttribute("CourseID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int CourseID { get; set; }

        /// <summary>
        /// 课时名称
        /// </summary>
        [FieldAttribute("Name", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Name { get; set; }
        /// <summary>
        /// 课时描述
        /// </summary>
        [FieldAttribute("Summary", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Summary { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        [FieldAttribute("Price", null, EnumFieldUsage.SeedField, DbType.Decimal)]
        public decimal Price { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        [FieldAttribute("NewPrice", null,  EnumFieldUsage.SeedField, DbType.Decimal)]
        public decimal NewPrice { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [FieldAttribute("Image", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Image { get; set; }
        /// <summary>
        /// 大图
        /// </summary>
        [FieldAttribute("BigImage", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string BigImage { get; set; }
        /// <summary>
        /// 直播提前几分钟进入
        /// </summary>
        [FieldAttribute("AheadMinutes", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int AheadMinutes { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        [FieldAttribute("AppointNum", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int AppointNum { get; set; }
        /// <summary>
        /// 显示预约人数
        /// </summary>
        [FieldAttribute("AppointNumShow", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int AppointNumShow { get; set; }
        /// <summary>
        /// 最多预约人数
        /// </summary>
        [FieldAttribute("LimitNum", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int LimitNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 课时状态(0:禁用 1:启用)
        /// </summary>
        [FieldAttribute("Status", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int Status { get; set; }
    }
}
