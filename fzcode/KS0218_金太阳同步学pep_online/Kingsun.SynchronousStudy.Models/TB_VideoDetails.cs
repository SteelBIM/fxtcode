using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_VideoDetails")]
    public partial class TB_VideoDetails : Kingsun.DB.Action
    {
        /// <summary>
        /// 视频难度程度
        /// </summary>
        [FieldAttribute("VideoDifficulty", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VideoDifficulty { get; set; }

        /// <summary>
        /// 视频状态
        /// </summary>
        [FieldAttribute("State", "((1))", EnumFieldUsage.CommonField, DbType.String)]
        public string State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 视频类别（1：活动）
        /// </summary>
        [FieldAttribute("VideoType", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string VideoType { get; set; }

        /// <summary>
        /// 完整视频
        /// </summary>
        [FieldAttribute("CompleteVideo", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CompleteVideo { get; set; }

        /// <summary>
        /// 背景音频
        /// </summary>
        [FieldAttribute("BackgroundAudio", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BackgroundAudio { get; set; }

        /// <summary>
        /// 视频封面
        /// </summary>
        [FieldAttribute("VideoCover", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoCover { get; set; }

        /// <summary>
        /// 视频简介
        /// </summary>
        [FieldAttribute("VideoDesc", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoTime", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTime { get; set; }

        /// <summary>
        /// 一级模块
        /// </summary>
        [FieldAttribute("FirstModular", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstModular { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        [FieldAttribute("SecondModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondModularID { get; set; }

        /// <summary>
        /// 二级模块
        /// </summary>
        [FieldAttribute("SecondModular", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondModular { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        [FieldAttribute("VideoTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 视频序号
        /// </summary>
        [FieldAttribute("VideoNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VideoNumber { get; set; }

        /// <summary>
        /// 静音视频
        /// </summary>
        [FieldAttribute("MuteVideo", null, EnumFieldUsage.CommonField, DbType.String)]
        public string MuteVideo { get; set; }

        /// <summary>
        /// 书籍名称
        /// </summary>
        [FieldAttribute("BookName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string BookName { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitleID { get; set; }

        /// <summary>
        /// 一级标题
        /// </summary>
        [FieldAttribute("FirstTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstTitle { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 二级标题
        /// </summary>
        [FieldAttribute("SecondTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondTitle { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstModularID { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

    }

    public class UserVideoDetails : Kingsun.DB.Action
    {
        public string VideoFileID { set; get; }
        public int NumberOfOraise { set; get; }
        public double TotalScore { set; get; }
        public string VideoReleaseAddress { set; get; }
        public int DialogueNumber { set; get; }
        public double DialogueScore { set; get; }
        public int IsEnableOss { set; get; }
        public DateTime CreateTime { set; get; }

    }


}
