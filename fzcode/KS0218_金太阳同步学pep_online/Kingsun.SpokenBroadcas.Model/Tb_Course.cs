using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_Course")]
    /// <summary>
    /// 课程表
    /// </summary>
    public class Tb_Course : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        [FieldAttribute("Name", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Name { get; set; }

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
        /// 课程类型，0为电影专题
        /// </summary>
        [FieldAttribute("Type", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int Type { get; set; }
        /// <summary>
        /// 课程描述
        /// </summary>
        [FieldAttribute("Summary", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Summary { get; set; }
        /// <summary>
        /// 课程使用人群
        /// </summary>
        [FieldAttribute("Groups", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string Groups { get; set; }
        /// <summary>
        /// 课时数量
        /// </summary>
        [FieldAttribute("Num", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int Num { get; set; }
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
        /// 开课日期
        /// </summary>
        [FieldAttribute("OpenDate", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 课程状态(0:禁用 1:启用)
        /// </summary>
        [FieldAttribute("Status", null, EnumFieldUsage.SeedField, DbType.Int32)]
        public int Status { get; set; }
        /// <summary>
        /// 直播间地址
        /// </summary>
        [FieldAttribute("StudioUrl", null, EnumFieldUsage.SeedField, DbType.String)]
        public string StudioUrl { get; set; }
        /// <summary>
        /// 直播间口令
        /// </summary>
        [FieldAttribute("StudioCommand", null, EnumFieldUsage.SeedField, DbType.String)]
        public string StudioCommand { get; set; }
    }
}
