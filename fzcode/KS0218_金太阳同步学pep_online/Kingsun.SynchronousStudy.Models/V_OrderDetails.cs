using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("V_OrderDetails")]
    public partial class V_OrderDetails : Kingsun.DB.Action
    {
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [FieldAttribute("ProductName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ProductName { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        [FieldAttribute("ProductCount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ProductCount { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [FieldAttribute("ProductMoney", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? ProductMoney { get; set; }

        /// <summary>
        /// 支付金钱
        /// </summary>
        [FieldAttribute("TotalMoney", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? TotalMoney { get; set; }


        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.String)]
        public string State { get; set; }

        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? UserID { get; set; }



        [FieldAttribute("Month", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Month { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <remarks>支付方式</remarks>
        [FieldAttribute("PayWay", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PayWay { get; set; }

        [FieldAttribute("TeachingNaterialName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingNaterialName { get; set; }
        

        /// <summary>
        /// 支付方式ID
        /// </summary>
        /// <remarks>支付方式ID</remarks>
        [FieldAttribute("PayWayID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? PayWayID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 套餐ID
        /// </summary>
        [FieldAttribute("FeeComboID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? FeeComboID { get; set; }

        [FieldAttribute("StartDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? StartDate { get; set; }

        [FieldAttribute("EndDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// 购买版本
        /// </summary>
        [FieldAttribute("ByEditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ByEditionName { get; set; }

        [FieldAttribute("FeeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FeeName { get; set; }

        [FieldAttribute("ComboType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboType { get; set; }

        [FieldAttribute("FeePrice", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? FeePrice { get; set; }

        [FieldAttribute("ComboName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ComboName { get; set; }

        [FieldAttribute("ComboTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboTimes { get; set; }

        [FieldAttribute("OrderID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OrderID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AppleOrderID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppleOrderID { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }
        /// <summary>
        /// Tb_UserInfo表的CreateTime
        /// </summary>
        [FieldAttribute("CreateTime", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// TB_UserClassRelation表的ClassShortID
        /// </summary>
        [FieldAttribute("ClassShortID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ClassShortID { get; set; }
    }
}
