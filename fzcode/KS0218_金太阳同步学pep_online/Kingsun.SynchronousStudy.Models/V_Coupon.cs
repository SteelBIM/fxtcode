using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;


namespace Kingsun.SynchronousStudy.Models
{
    [TableAttribute("V_Coupon")]
    public partial class V_Coupon : Kingsun.DB.Action
    {
        [FieldAttribute("ID", null, EnumFieldUsage.CommonField, DbType.Guid)]
        public Guid? ID { get; set; }

        [FieldAttribute("Status", null, EnumFieldUsage.CommonField, DbType.Int32)]
        public int? Status { get; set; }

        [FieldAttribute("TelePhone", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TelePhone { get; set; }

        [FieldAttribute("TeachingNaterialName", null, EnumFieldUsage.CommonField, DbType.String)]
        public string TeachingNaterialName { get; set; }

    }
    public class CouponListModel
    {
        public int EditionID { get; set; }
        public string TextbookVersion { get; set; }
        public string TicketName { get; set; }

        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImgUrl { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }
}
