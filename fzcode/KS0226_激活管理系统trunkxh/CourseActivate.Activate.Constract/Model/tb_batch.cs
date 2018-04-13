using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    /// <summary>
    /// 批次表
    /// </summary>
    public class tb_batch
    {
        /// <summary>
        /// 激活码批次ID
        /// </summary>
        public int batchid { get; set; }
        /// <summary>
        /// 批次码
        /// </summary>
        public string batchcode { get; set; }
        /// <summary>
        /// 批次起始日期
        /// </summary>
        public DateTime? startdate { get; set; }
        /// <summary>
        /// 批次结束日期
        /// </summary>
        public DateTime? enddate { get; set; }     
        public int? indate { get; set; }
        public int? purpose { get; set; }
        public int? activatenum { get; set; }
        public int? activatetypeid { get; set; }
        //0-未启用，1-启用，2-禁用
        public int? status { get; set; }
        public int? masterid { get; set; }
        public string mastername { get; set; }
        /// <summary>
        /// 创建类型
        /// </summary>
        public string createtype { get; set; }
        public string remark { get; set; }
        public DateTime? createtime { get; set; }
    }
}
