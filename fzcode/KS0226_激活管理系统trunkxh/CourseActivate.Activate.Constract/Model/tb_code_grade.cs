using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;


namespace CourseActivate.Activate.Constract.Model
{
    public partial class tb_code_grade 
    {
        /// <summary>
        /// 
        /// </summary>
       
        public int? Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
       
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public int? GradeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public string GradeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
       
        public string Remark { get; set; }

    }
}
