using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.IBS.Model
{
    [TableAttribute("Tb_UserInfo")]
    public partial class Tb_UserInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否第一次登陆
        /// </summary>
        [FieldAttribute("isLogState", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string isLogState { get; set; }

        /// <summary>
        /// 是否是oss文件（0：否，1：是）
        /// </summary>
        [FieldAttribute("IsEnableOss", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsEnableOss { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("IsUser", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsUser { get; set; }

        /// <summary>
        /// 用户注册的App版本
        /// </summary>
        [FieldAttribute("AppId", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("NickName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserRoles", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TrueName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TrueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

    }
}
