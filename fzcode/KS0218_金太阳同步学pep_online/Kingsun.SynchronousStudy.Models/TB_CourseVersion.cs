using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_CourseVersion")]
    public partial class TB_CourseVersion : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("FirstPageNum", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FirstPageNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("TryUpdate", "((1))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int TryUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Creator", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Creator { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [FieldAttribute("ModuleID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int ModuleID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Description", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UpdateTimes", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int UpdateTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("DownloadTimes", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int DownloadTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateDateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ModifyDateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? ModifyDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Disable", "((0))", EnumFieldUsage.CommonField, DbType.Boolean)]
        public bool Disable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Version", null, EnumFieldUsage.CommonField, DbType.String)]
        public string Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UpdateMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UpdateMD5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UpdateURL", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UpdateURL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CompleteMD5", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CompleteMD5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CompleteURL", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CompleteURL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", "(newid())", EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

    }
}
