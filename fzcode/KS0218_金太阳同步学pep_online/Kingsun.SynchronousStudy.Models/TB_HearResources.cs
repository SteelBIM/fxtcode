using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_HearResources")]
    public partial class TB_HearResources : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [FieldAttribute("RoleName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string RoleName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [FieldAttribute("Picture", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Picture { get; set; }

        /// <summary>
        /// 跟读次数
        /// </summary>
        [FieldAttribute("RepeatNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? RepeatNumber { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        [FieldAttribute("SecondModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondModularID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldAttribute("SerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SerialNumber { get; set; }

        /// <summary>
        /// 子序号
        /// </summary>
        [FieldAttribute("TextSerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TextSerialNumber { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [FieldAttribute("TextDesc", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TextDesc { get; set; }

        /// <summary>
        /// 音频
        /// </summary>
        [FieldAttribute("AudioFrequency", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AudioFrequency { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 书籍ID
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

        /// <summary>
        /// 二级模块英文名
        /// </summary>
        [FieldAttribute("ModularEN", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModularEN { get; set; }
    }




    [TableAttribute("TB_HearResources_YX")]
    public partial class TB_HearResources_YX : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [FieldAttribute("RoleName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string RoleName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [FieldAttribute("Picture", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Picture { get; set; }

        /// <summary>
        /// 跟读次数
        /// </summary>
        [FieldAttribute("RepeatNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? RepeatNumber { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        [FieldAttribute("FirstModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        [FieldAttribute("SecondModularID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SecondModularID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldAttribute("SerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SerialNumber { get; set; }

        /// <summary>
        /// 子序号
        /// </summary>
        [FieldAttribute("TextSerialNumber", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? TextSerialNumber { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [FieldAttribute("TextDesc", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TextDesc { get; set; }

        /// <summary>
        /// 音频
        /// </summary>
        [FieldAttribute("AudioFrequency", null, EnumFieldUsage.CommonField, DbType.String)]
        public string AudioFrequency { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int? ID { get; set; }

        /// <summary>
        /// 书籍ID
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

        /// <summary>
        /// 二级模块英文名
        /// </summary>
        [FieldAttribute("ModularEN", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModularEN { get; set; }
    }
}
