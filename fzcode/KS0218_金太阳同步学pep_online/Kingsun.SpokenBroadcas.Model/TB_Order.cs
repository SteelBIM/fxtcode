using Kingsun.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Model
{
    [TableAttribute("TB_Order")]
    public partial class TB_Order : Kingsun.DB.Action
    {
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        [FieldAttribute("OrderID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string OrderID { get; set; }

        /// <summary>
        /// 课时时间ID
        /// </summary>
        [FieldAttribute("CoursePeriodTimeID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? CoursePeriodTimeID { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        [FieldAttribute("Count", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Count { get; set; }
        /// <summary>
        /// 购买总价
        /// </summary>
        [FieldAttribute("TotalMoney", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? TotalMoney { get; set; }

        [FieldAttribute("CompleteDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CompleteDate { get; set; }

        [FieldAttribute("State", "((0))", EnumFieldUsage.CommonField, DbType.String)]
        public string State { get; set; }

        [FieldAttribute("PayWay", null, EnumFieldUsage.CommonField, DbType.String)]
        public string PayWay { get; set; }

        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }
    }
}
