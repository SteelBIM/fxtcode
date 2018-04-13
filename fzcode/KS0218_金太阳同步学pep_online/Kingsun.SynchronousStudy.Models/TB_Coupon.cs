using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("TB_Coupon")]
    public partial class TB_Coupon : Kingsun.DB.Action
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("ID", null, EnumFieldUsage.PrimaryKey, DbType.Guid)]
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("UserID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("Status", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Status { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CourseID", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CourseID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldAttribute("CouponImage", null, EnumFieldUsage.CommonField, DbType.String)]
        public string CouponImage { get; set; }

    }
}
