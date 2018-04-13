using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_UserFeedback")]
    public partial class TB_UserFeedback : Kingsun.DB.Action
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [FieldAttribute("UserName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserName { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        [FieldAttribute("ProblemDescription", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ProblemDescription { get; set; }

        /// <summary>
        /// 反馈等级
        /// </summary>
        [FieldAttribute("FeedbackLevel", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? FeedbackLevel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldAttribute("CreateDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime? CreateDate { get; set; }

    }
}
