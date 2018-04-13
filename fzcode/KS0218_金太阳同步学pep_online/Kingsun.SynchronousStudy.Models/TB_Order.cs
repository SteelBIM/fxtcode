using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_Order")]
    public partial class TB_Order : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("State", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("PayWay", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PayWay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FeeComboID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? FeeComboID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseID { get; set; }

        /// <summary>
        /// 是否使用优惠卷
        /// </summary>
        [FieldAttribute("IsDiscount", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsDiscount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("OrderID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OrderID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TotalMoney", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? TotalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        [FieldAttribute("SourceType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int SourceType { get; set; }

        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleID { get; set; }
    }
}
