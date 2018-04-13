using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Activate.Constract.Model
{
    /// <summary>
    /// 出版社
    /// </summary>
    public class tb_publish
    {
        /// <summary>
        /// 出版社ID
        /// </summary>
        public int publishid { get; set; }
        /// <summary>
        /// 出版社名称
        /// </summary>
        public string publishname { get; set; }
       /// <summary>
       /// 出版社状态（0:未启用，1:启用，2:禁用）
       /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? createTime { get; set; }
    }
}
