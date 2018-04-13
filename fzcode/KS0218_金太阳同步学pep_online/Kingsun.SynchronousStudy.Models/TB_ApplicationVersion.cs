using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_ApplicationVersion")]
    public partial class TB_ApplicationVersion : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 上传者
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 更新文件地址
        /// </summary>
        [FieldAttribute("FileAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FileAddress { get; set; }

        /// <summary>
        /// 更新文件MD5值
        /// </summary>
        [FieldAttribute("FileMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FileMD5 { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool State { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        [FieldAttribute("MandatoryUpdate", null, EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool MandatoryUpdate { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        [FieldAttribute("VersionID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionID { get; set; }

        /// <summary>
        /// 1-IOS版本；2-Android版本
        /// </summary>
        [FieldAttribute("VersionType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int VersionType { get; set; }

        /// <summary>
        /// 应用版本号
        /// </summary>
        [FieldAttribute("VersionNumber", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionNumber { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        [FieldAttribute("VersionDescription", null, EnumFieldUsage.CommonField, DbType.String)]
        public string VersionDescription { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        [FieldAttribute("isEnabled", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int isEnabled { get; set; }

    }

    public class AppUpDate
    {
        public string Type { set; get; }
        public string AppID { set; get; }
        public string VersionNumber { get; set; }
        public string UserId { get; set; }
        public int DownloadChannel { get; set; }
    }
}
