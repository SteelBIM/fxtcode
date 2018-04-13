using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.AppLibrary.Model
{
    [TableAttribute("TB_OrderDetails")]
    public partial class TB_OrderDetails : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("OrderID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? OrderID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TotalMoney", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? TotalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FeeComboID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? FeeComboID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StartDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EndDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ByEditionName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ByEditionName { get; set; }



    }
}
