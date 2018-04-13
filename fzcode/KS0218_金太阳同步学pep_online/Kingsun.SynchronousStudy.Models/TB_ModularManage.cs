using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_ModularManage")]
    public partial class TB_ModularManage : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ActiveState", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ActiveState { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [FieldAttribute("SuperiorID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SuperiorID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Level", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Level { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? State { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [FieldAttribute("ModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ModularID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [FieldAttribute("ModularName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModularName { get; set; }

    }
}
