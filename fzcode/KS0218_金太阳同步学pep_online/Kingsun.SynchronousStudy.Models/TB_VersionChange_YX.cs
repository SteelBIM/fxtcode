using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_VersionChange_YX")]
    public partial class TB_VersionChange_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 模块版本号
        /// </summary>
        [FieldAttribute("ModuleVersion", "('V1.0.0')", EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleVersion { get; set; }

        /// <summary>
        /// 更新描述
        /// </summary>
        [FieldAttribute("UpdateDescription", "('初始版本')", EnumFieldUsage.CommonField, DbType.String)]
        public string UpdateDescription { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [FieldAttribute("State", "((1))", EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? State { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        [FieldAttribute("IsUpdate", "((0))", EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? IsUpdate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 增量包MD5
        /// </summary>
        [FieldAttribute("IncrementalPacketMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string IncrementalPacketMD5 { get; set; }

        /// <summary>
        /// 一级标题
        /// </summary>
        [FieldAttribute("FirstTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string FirstTitle { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        [FieldAttribute("SecondTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 二级标题
        /// </summary>
        [FieldAttribute("SecondTitle", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SecondTitle { get; set; }

        /// <summary>
        /// 模块地址
        /// </summary>
        [FieldAttribute("ModuleAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleAddress { get; set; }

        /// <summary>
        /// MD5值
        /// </summary>
        [FieldAttribute("MD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string MD5 { get; set; }

        /// <summary>
        /// 增量包地址
        /// </summary>
        [FieldAttribute("IncrementalPacketAddress", null, EnumFieldUsage.CommonField, DbType.String)]
        public string IncrementalPacketAddress { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 主模块ID
        /// </summary>
        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ModuleID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [FieldAttribute("BooKID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? BooKID { get; set; }

        /// <summary>
        /// 教材名称
        /// </summary>
        [FieldAttribute("TeachingNaterialName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingNaterialName { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        [FieldAttribute("ModuleName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleName { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        [FieldAttribute("FirstTitleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstTitleID { get; set; }

    }
}
