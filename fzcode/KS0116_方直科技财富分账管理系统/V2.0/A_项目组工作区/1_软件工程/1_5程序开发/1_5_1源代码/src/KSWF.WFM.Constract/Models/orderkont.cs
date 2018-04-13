using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 订单结算呈现
    /// </summary>
    public class orderkont
    {
        public string productname { get; set; }
        public int channel { get; set; }
        public string p_category { get; set; }
        public string p_version { get; set; }
        public decimal? divided { get; set; }
        public decimal? class_divided { get; set; }
        public int ordernumber { get; set; }
        /// <summary>
        /// 销售总金额
        /// </summary>
        public decimal? o_payamount { get; set; }
        /// <summary>
        /// 总销售毛利
        /// </summary>
        public decimal? o_actamount { get; set; }
        /// <summary>
        /// 用户总提成金额
        /// </summary>
        public decimal? totalo_bonus { get; set; }
        
        public int classnumber { get; set; }
        /// <summary>
        /// 绑定的班级销售额
        /// </summary>
        public decimal? classpayamount { get; set; }
        /// <summary>
        /// 绑定的班级销售毛利
        /// </summary>
        public decimal? classactamount { get; set; }
        /// <summary>
        /// 基础提成金额
        /// </summary>
        public decimal? basis_bonus { get; set; }
        /// <summary>
        /// 班级奖励
        /// </summary>
        public decimal? p_class_bonus { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        public decimal? total { get; set; }
    }
}
