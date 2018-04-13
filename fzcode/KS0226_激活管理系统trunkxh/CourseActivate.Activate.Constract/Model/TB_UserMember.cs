using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    public class TB_UserMember
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Months { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CourseID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? TbOrderID { get; set; }
    }
}
