using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Resource.Constract.Model
{
    public class tb_res_modular
    {
        /// <summary>
        /// 状态,0未启用,1启用,2禁用
        /// </summary>

        public bool? Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 模块级别
        /// </summary>

        public int? ModularLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>

        public int? ModularID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>

        public string ModularName { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>

        public int? ParentID { get; set; }
    }
}
