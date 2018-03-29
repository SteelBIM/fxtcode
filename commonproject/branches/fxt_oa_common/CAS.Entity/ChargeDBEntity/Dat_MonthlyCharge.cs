using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_MonthlyCharge : DatEntrust
    {

        /// <summary>
        /// 评估总值
        /// </summary>
        public decimal? querytotalprice
        {
            get;
            set;
        }
        /// <summary>
        /// 标准收费总额
        /// </summary>
        public decimal? standardtotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 实收总值
        /// </summary>
        public decimal? receivable
        {
            get;
            set;
        }
        /// <summary>
        ///优惠折扣
        /// </summary>
        public decimal? privilegediscount
        {
            get;
            set;
        }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? privilegemoney
        {
            get;
            set;
        }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal? money
        {
            get;
            set;
        }
        /// <summary>
        /// 退费总额
        /// </summary>
        public decimal? refundamount
        {
            get;
            set;
        }
        /// <summary>
        /// 真名
        /// </summary>
        public string truename
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime
        {
            get;
            set;
        }
        /// <summary>
        /// 结单时间
        /// </summary>
        public DateTime? overtime
        {
            get;
            set;
        }
        /// <summary>
        /// 收费状态值
        /// </summary>
        public int? chargestate
        {
            get;
            set;
        }
        /// <summary>
        /// 收费状态值
        /// </summary>
        public string chargestatename
        {
            get;
            set;
        }

    }
}
