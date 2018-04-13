using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.AppLibrary.Model
{
    [TableAttribute("TB_ComboType")]
    public partial class TB_ComboType : Kingsun.DB.Action
    {
        /// <summary>
        /// 套餐编号
        /// </summary>
        [FieldAttribute("ComboID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ComboID { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        [FieldAttribute("ComboName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ComboName { get; set; }

        /// <summary>
        /// 套餐时间
        /// </summary>
        [FieldAttribute("ComboTimes", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ComboTimes { get; set; }

        /// <summary>
        /// 是否可用 1-可用，0-禁用
        /// </summary>
        [FieldAttribute("IsUse", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? IsUse { get; set; }

    }
}
