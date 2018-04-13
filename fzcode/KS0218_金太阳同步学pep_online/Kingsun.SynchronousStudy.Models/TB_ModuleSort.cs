using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_ModuleSort")]
    public partial class TB_ModuleSort : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ModuleID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [FieldAttribute("ModuleName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 上级模块ID
        /// </summary>
        [FieldAttribute("SuperiorID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SuperiorID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [FieldAttribute("Sort", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Sort { get; set; }

        /// <summary>
        /// 所属教材ID
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitleID { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }


        public int ActiveState { get; set; }
    }
}
