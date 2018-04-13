using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserVideoDetails_YX")]
    public partial class TB_UserVideoDetails_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 获赞数量
        /// </summary>
        [FieldAttribute("NumberOfOraise", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? NumberOfOraise { get; set; }

        /// <summary>
        /// 播放次数
        /// </summary>
        [FieldAttribute("PlayTimes", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PlayTimes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionType", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoType", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string VideoType { get; set; }

        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        [FieldAttribute("IsEnableOss", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsEnableOss { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [FieldAttribute("ClassId", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassId { get; set; }

        /// <summary>
        /// 文件服务器视频ID
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

        /// <summary>
        /// 视频发布地址
        /// </summary>
        [FieldAttribute("VideoReleaseAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoReleaseAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoImageAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoImageAddress { get; set; }

        /// <summary>
        /// 配音总成绩
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double? TotalScore { get; set; }

        /// <summary>
        /// 完成状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("column1", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? column1 { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 视频序号
        /// </summary>
        [FieldAttribute("VideoNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VideoNumber { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        [FieldAttribute("VideoID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VideoID { get; set; }
    }
}
