using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class TB_Order
    {
        public Guid ID { get; set; }
        public string OrderID { get; set; }
        public decimal TotalMoney { get; set; }
        public DateTime CreateDate { get; set; }
        public string State { get; set; }
        public string UserID { get; set; }
        public string PayWay { get; set; }
        public Guid FeeComboID { get; set; }
        public string CourseID { get; set; }
        public int IsDiscount { get; set; }
    }
}
