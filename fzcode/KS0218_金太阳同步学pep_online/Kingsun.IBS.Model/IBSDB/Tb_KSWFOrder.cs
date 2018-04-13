using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using Kingsun.DB;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 财富分账订单
    /// </summary>
    public partial class Tb_KSWFOrder : Kingsun.DB.Action
    {
        public string OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? AreaID { get; set; }
        public string AreaPath { get; set; }
        public int? SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int? GradeID { get; set; }
        public string GradeName { get; set; }
        public Guid? ClassID { get; set; }
        public string ClassName { get; set; }
        public string ProductNO { get; set; }
        public string ProductName { get; set; }
        public int? TeacherUserID { get; set; }
        public string TeacherUserName { get; set; }
        public string BuyUserID { get; set; }
        public string BuyUserPhone { get; set; }
        public decimal PayAmount { get; set; }
        public int? PayType { get; set; }
        public int? Channel { get; set; }
        public string UserClientIP { get; set; }
        public int paynumber { get; set; }


    }
}
