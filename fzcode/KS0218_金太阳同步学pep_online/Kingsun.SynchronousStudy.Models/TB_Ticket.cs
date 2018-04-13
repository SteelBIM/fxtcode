using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_Ticket")]
    public partial class TB_Ticket : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ModularID", "((14))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? ModularID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [FieldAttribute("StartDate", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 使用券类型0表示0元购，1表示抵用券
        /// </summary>
        [FieldAttribute("Type", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Type { get; set; }

        /// <summary>
        /// 0正式使用，1已过期
        /// </summary>
        [FieldAttribute("Status", "((0))", EnumFieldUsage.CommonField, DbType.Int32)]
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CreateTime", "(getdate())", EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        [FieldAttribute("Price", null, EnumFieldUsage.CommonField, DbType.Decimal)]
        public decimal? Price { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [FieldAttribute("EndDate", null, EnumFieldUsage.CommonField, DbType.DateTime)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        [FieldAttribute("ImgUrl", null, EnumFieldUsage.CommonField, DbType.String)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey | EnumFieldUsage.SeedField, DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 使用券名称
        /// </summary>
        [FieldAttribute("TicketName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TicketName { get; set; }

        /// <summary>
        /// 课程ID，如1,2,3
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int CourseID { get; set; }

    }
}
