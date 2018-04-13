using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserHearResources")]
    public partial class TB_UserHearResources : Kingsun.DB.Action
    {
        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        [FieldAttribute("IsEnableOss", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SecondTitleID { get; set; }

        /// <summary>
        /// 获赞数量
        /// </summary>
        [FieldAttribute("NumberOfOraise", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int NumberOfOraise { get; set; }

        /// <summary>
        /// 跟读次数
        /// </summary>
        [FieldAttribute("PlayTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int PlayTimes { get; set; }

        /// <summary>
        /// 录音成绩
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double TotalScore { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AverageScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double AverageScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int FirstTitleID { get; set; }

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
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldAttribute("SerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 子序号
        /// </summary>
        [FieldAttribute("TextSerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int TextSerialNumber { get; set; }

        /// <summary>
        /// 文件服务器ID
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

    }

    [TableAttribute("TB_UserHearResources_YX")]
    public partial class TB_UserHearResources_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        [FieldAttribute("IsEnableOss", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SecondTitleID { get; set; }

        /// <summary>
        /// 获赞数量
        /// </summary>
        [FieldAttribute("NumberOfOraise", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int NumberOfOraise { get; set; }

        /// <summary>
        /// 跟读次数
        /// </summary>
        [FieldAttribute("PlayTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int PlayTimes { get; set; }

        /// <summary>
        /// 录音成绩
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double TotalScore { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AverageScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double AverageScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int FirstTitleID { get; set; }

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
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldAttribute("SerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 子序号
        /// </summary>
        [FieldAttribute("TextSerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int TextSerialNumber { get; set; }

        /// <summary>
        /// 文件服务器ID
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

    }
}
