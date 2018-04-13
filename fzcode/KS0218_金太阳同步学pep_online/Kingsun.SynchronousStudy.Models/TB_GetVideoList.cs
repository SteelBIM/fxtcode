using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_GetVideoList")]
    public partial class TB_GetVideoList : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoType", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsEnableOss", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? VersionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoImageAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoImageAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoReleaseAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoReleaseAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TotalScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTitle { get; set; }

    }
}
