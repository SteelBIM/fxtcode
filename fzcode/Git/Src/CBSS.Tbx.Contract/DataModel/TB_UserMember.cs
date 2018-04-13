using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TB_UserMember
    {
        public Guid? ID { get; set; }
        
        public DateTime? CreateTime { get; set; }
        
        public int? Status { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public int? Months { get; set; }
        
        public string CourseID { get; set; }
        
        public string UserID { get; set; }
        
        public Guid? TbOrderID { get; set; }
    }
}
