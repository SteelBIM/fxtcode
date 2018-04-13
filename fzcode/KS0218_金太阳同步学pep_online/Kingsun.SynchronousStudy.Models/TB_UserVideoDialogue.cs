using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserVideoDialogue")]
    public partial class TB_UserVideoDialogue : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoType", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string VideoType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户表主键ID
        /// </summary>
        [FieldAttribute("UserVideoID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserVideoID { get; set; }

        /// <summary>
        /// 对白序号
        /// </summary>
        [FieldAttribute("DialogueNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int DialogueNumber { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        [FieldAttribute("DialogueScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double DialogueScore { get; set; }

        /// <summary>
        /// 文件服务器视频ID（用户配音的）
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

    }

    [TableAttribute("TB_UserVideoDialogue_YX")]
    public partial class TB_UserVideoDialogue_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoType", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string VideoType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户表主键ID
        /// </summary>
        [FieldAttribute("UserVideoID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserVideoID { get; set; }

        /// <summary>
        /// 对白序号
        /// </summary>
        [FieldAttribute("DialogueNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int DialogueNumber { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        [FieldAttribute("DialogueScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double DialogueScore { get; set; }

        /// <summary>
        /// 文件服务器视频ID（用户配音的）
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

    }
}
