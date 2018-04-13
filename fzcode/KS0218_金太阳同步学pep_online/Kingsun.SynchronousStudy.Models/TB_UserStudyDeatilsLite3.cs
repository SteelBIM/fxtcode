using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserStudyDeatilsLite3")]
    public partial class TB_UserStudyDeatilsLite3 : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("DubTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int DubTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondTitle { get; set; }

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
        [FieldAttribute("IsEnableOss", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int FirstModularID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstModular", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstModular { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int FirstTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SecondTitleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VideoNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TotalScore", null, EnumFieldUsage.CommonField, DbType.Double)]
        public double TotalScore { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("VideoFileID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VideoFileID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("id", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int id { get; set; }

    }
}
