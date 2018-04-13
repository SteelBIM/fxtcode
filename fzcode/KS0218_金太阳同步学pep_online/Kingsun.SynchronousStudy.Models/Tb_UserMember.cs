using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserMember")]
    public partial class TB_UserMember : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Status", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("StartDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("EndDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Months", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Months { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TbOrderID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? TbOrderID { get; set; }


        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ModuleID { get; set; }

    }
}
