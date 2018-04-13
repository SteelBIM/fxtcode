using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_TeacherInfo")]
    public partial class TB_TeacherInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [FieldAttribute("City", null, EnumFieldUsage.CommonField, DbType.String)]
        public string City { get; set; }

        /// <summary>
        /// 地区ID
        /// </summary>
        [FieldAttribute("AreaID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? AreaID { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [FieldAttribute("Area", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Area { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        [FieldAttribute("SchoolID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SchoolID { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        [FieldAttribute("SchoolName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string SchoolName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 学科ID
        /// </summary>
        [FieldAttribute("SubjectID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? SubjectID { get; set; }

        /// <summary>
        /// 学科
        /// </summary>
        [FieldAttribute("Subject", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Subject { get; set; }

        /// <summary>
        /// 省份ID
        /// </summary>
        [FieldAttribute("ProvinceID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ProvinceID { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [FieldAttribute("Province", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Province { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        [FieldAttribute("CityID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int CityID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int UserID { get; set; }

    }
}
