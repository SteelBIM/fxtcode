using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserInfoExtend")]
    public partial class TB_UserInfoExtend : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [FieldAttribute("EquipmentID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string EquipmentID { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [FieldAttribute("DeviceType", null, EnumFieldUsage.CommonField, DbType.String)]
        public string DeviceType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [FieldAttribute("IPAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string IPAddress { get; set; }

    }
}
