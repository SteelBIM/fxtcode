using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_ModuleConfiguration")]
    public partial class TB_ModuleConfiguration : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 教材书册
        /// </summary>
        [FieldAttribute("TeachingNaterialName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingNaterialName { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        [FieldAttribute("FirstTitileID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitileID { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        [FieldAttribute("FirstTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstTitle { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 单元
        /// </summary>
        [FieldAttribute("SecondTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondTitle { get; set; }

        /// <summary>
        /// 是否在Mod被软删除（0：未删除，1：已删除）
        /// </summary>
        [FieldAttribute("State", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? State { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("BookID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BookID { get; set; }

        public int StartingPage { get; set; }
        public int EndingPage { get; set; }
    }
}
