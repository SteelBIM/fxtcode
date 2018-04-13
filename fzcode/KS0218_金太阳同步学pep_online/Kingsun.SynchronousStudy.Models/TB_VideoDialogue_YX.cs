using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_VideoDialogue_YX")]
    public partial class TB_VideoDialogue_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 对白结束时间
        /// </summary>
        [FieldAttribute("EndTime", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EndTime { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        [FieldAttribute("ActiveID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ActiveID { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        [FieldAttribute("SecondModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondModularID { get; set; }

        /// <summary>
        /// 对白
        /// </summary>
        [FieldAttribute("DialogueText", null, EnumFieldUsage.CommonField, DbType.String)]
        public string DialogueText { get; set; }

        /// <summary>
        /// 对白顺序号
        /// </summary>
        [FieldAttribute("DialogueNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? DialogueNumber { get; set; }

        /// <summary>
        /// 对白开始时间
        /// </summary>
        [FieldAttribute("StartTime", null, EnumFieldUsage.CommonField, DbType.String)]
        public string StartTime { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        [FieldAttribute("VideoID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VideoID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitleID { get; set; }

    }
}
 