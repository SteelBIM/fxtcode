using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.AppLibrary.Model
{
    [TableAttribute("V_FeeCombo")]
    public partial class V_FeeCombo : Kingsun.DB.Action
    {
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        [FieldAttribute("FeeName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FeeName { get; set; }

        [FieldAttribute("FeePrice", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? FeePrice { get; set; }

        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        [FieldAttribute("CreateDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        [FieldAttribute("CreateUser", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CreateUser { get; set; }

        [FieldAttribute("ModifyDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? ModifyDate { get; set; }

        [FieldAttribute("ModifyUser", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModifyUser { get; set; }

        [FieldAttribute("ComboID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboID { get; set; }

        [FieldAttribute("ComboName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ComboName { get; set; }

        [FieldAttribute("ComboTimes", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboTimes { get; set; }


        [FieldAttribute("AppleID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AppleID { get; set; }


        /// <summary>
        /// 状态 1-可用，0-禁用
        /// </summary>
        [FieldAttribute("FeeType", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FeeType { get; set; }
        /// <summary>
        /// 状态 1-可用，0-禁用
        /// </summary>
        [FieldAttribute("IsUse", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsUse { get; set; }
        /// <summary>
    }
}
