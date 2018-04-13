using CourseActivate.Account.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    public class vw_employeeention:com_master
    {
        private decimal? _o_bonus = 0;
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal? o_bonus
        {
            get { return _o_bonus; }
            set { _o_bonus = value; }
        }
        public int o_number { get; set; }

        private decimal? _o_payamount = 0;
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? o_payamount
        {
            get { return _o_payamount; }
            set { _o_payamount = value; }
        }
        public string deptname { get; set; }
        /// <summary>
        /// 负责区域
        /// </summary>
        public string areas { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime time { get; set; }
        public int endtime { get; set; }
    }
}
