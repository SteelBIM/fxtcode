using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_FeeCombo")]
    public partial class TB_FeeCombo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Discount ", "((0))", EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal Discount  { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FeePrice", "((0))", EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? FeePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("State", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ImageUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ModifyUser", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModifyUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AppID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Month", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Month { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Type", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("AppleID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ComboType", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FeeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FeeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateUser", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CreateUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ModifyDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? ModifyDate { get; set; }

        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleID { get; set; }
    }
}
