using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;


namespace CourseActivate.Activate.Constract.Model
{
    public partial class tb_code_period 
    {
        /// <summary>
        /// 
        /// </summary>
        
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public int? Sort { get; set; }

        /// <summary>
        /// 学段ID
        /// </summary>
        
        public int? PeriodID { get; set; }

        /// <summary>
        /// 学段名称
        /// </summary>
        
        public string PeriodName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public string Remark { get; set; }

    }
}
