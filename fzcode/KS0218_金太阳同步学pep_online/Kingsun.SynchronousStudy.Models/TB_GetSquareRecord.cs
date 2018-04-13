using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_GetSquareRecord")]
    public partial class TB_GetSquareRecord : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsEnableOss", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("NickName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TrueName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TrueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("NumberOfOraise", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int NumberOfOraise { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("PlayTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int PlayTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

        public string VideoID { get; set; }

    }
}
