using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserTicket")]
    public partial class TB_UserTicket : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid ID { get; set; }

        /// <summary>
        /// 使用情况，0表示未使用，1表示已使用, 2已过期
        /// </summary>
        [FieldAttribute("Status", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Status { get; set; }

        /// <summary>
        /// 用户获取使用券时间
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// APP版本
        /// </summary>
        [FieldAttribute("AppID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [FieldAttribute("OrderID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OrderID { get; set; }

        /// <summary>
        /// 使用券ID
        /// </summary>
        [FieldAttribute("TicketID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TicketID { get; set; }

    }
}
