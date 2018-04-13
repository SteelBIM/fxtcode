using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudyHopeChina.Web
{
    [TableAttribute("tb_UserInfo")]
    public partial class tb_UserInfo : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Userid", null, EnumFieldUsage.PrimaryKey, DbType.String)]
        public string Userid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Username", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Parentname", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Parentname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Phone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Teachername", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Teachername { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Teacherphone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Teacherphone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Schoolname", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Schoolname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Period", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Period { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Grade", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Grade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Isjoin", "((0))", EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool? Isjoin { get; set; }

    }
}
