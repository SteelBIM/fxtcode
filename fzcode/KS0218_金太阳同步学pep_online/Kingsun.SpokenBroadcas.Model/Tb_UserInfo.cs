using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("Tb_UserInfo")]
    public class Tb_UserInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [FieldAttribute("UserID", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        [FieldAttribute("UserName", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string UserName { get; set; }
        /// <summary>
        /// NickName
        /// </summary>
        [FieldAttribute("NickName", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string NickName { get; set; }
        /// <summary>
        /// TrueName
        /// </summary>
        [FieldAttribute("TrueName", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string TrueName { get; set; }
        /// <summary>
        /// UserImage
        /// </summary>
        [FieldAttribute("UserImage", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string UserImage { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        [FieldAttribute("UserRoles", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int UserRoles { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [FieldAttribute("TelePhone", null,  EnumFieldUsage.SeedField, DbType.String)]
        public string TelePhone { get; set; }
        /// <summary>
        /// 是否第一次登陆
        /// </summary>
        [FieldAttribute("isFirstLog", null,  EnumFieldUsage.SeedField, DbType.Int32)]
        public int isFirstLog { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateTime", null,  EnumFieldUsage.SeedField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }

    }
}
