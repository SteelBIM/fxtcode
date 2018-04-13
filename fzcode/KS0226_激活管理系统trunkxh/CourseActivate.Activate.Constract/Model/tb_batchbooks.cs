using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    /// <summary>
    /// 批次使用书本ID范围
    /// </summary>
    public class tb_batchbooks
    {
        /// <summary>
        /// 书本ID,0表示所有书
        /// </summary>
        public int batchbookid { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public int? batchid { get; set; }

        /// <summary>
        /// 书本ID
        /// </summary>
        public int? bookid { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createTime { get; set; }
    }
}
