using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;


namespace CourseActivate.Activate.Constract.Model
{
    public partial class tb_bookkey 
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? createtime { get; set; }

        /// <summary>
        /// 书本id
        /// </summary>
        public int? bookid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string bookname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? masterid { get; set; }

    }
}
