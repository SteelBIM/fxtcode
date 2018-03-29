using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    public class Dat_MonthlyCharge:BaseTO
    {
        public int businessuserid
        {
            get;
            set;
        }  

        public string bankname
        {
            get;
            set;
        }
        public int reportcount
        {
            get;
            set;
        }
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
        public decimal? standardcharge
        {
            get;
            set;
        }
        /// <summary>
        /// 应收总值
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
        /// 完成时间时间
        /// </summary>
        public string completedate
        {
            get;
            set;
        }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? closeaccounttime
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
        public int? statusvalue
        {
            get;
            set;
        }
        /// <summary>
        /// 收费状态名
        /// </summary>
        public string statusName
        {
            get;
            set;
        }

        /// <summary>
        /// 优惠类型
        /// </summary>
        public int? privilegetype { get; set; }
        /// <summary>
        /// 最低收费
        /// </summary>
        public decimal? mincharge { get; set; }

        /// <summary>
        /// 审批类型id
        /// </summary>
        public int? approvalid { get; set; }
        /// <summary>
        /// 收费标准
        /// </summary>
        public int? chargenameid { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno { get; set; }
        /// <summary>
        /// 审批流程
        /// </summary>
        public int? worktodoid { get; set; }
        /// <summary>
        ///行号
        /// </summary>
        public long rowno { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal billtotalamount { get; set; }
    }
}
