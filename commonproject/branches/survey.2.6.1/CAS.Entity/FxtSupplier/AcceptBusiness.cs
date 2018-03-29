using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.AcceptBusiness")]
    public class AcceptBusiness : BaseTO
    {
        private int _id;
        /// <summary>
        /// id
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 供应商id
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int? _businessid;
        /// <summary>
        /// 业务Id
        /// </summary>
        public int? businessid
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
        private int? _state;
        /// <summary>
        /// 业务受理状态:0-拒绝；1-完成；2-未受理;3-任务处理中
        /// </summary>
        public int? state
        {
            get { return _state; }
            set { _state = value; }
        }
        private DateTime? _businessdate;
        /// <summary>
        /// 业务处理时间
        /// </summary>
        public DateTime? businessdate
        {
            get { return _businessdate; }
            set { _businessdate = value; }
        }
        private string _remark;
        /// <summary>
        /// 业务描述
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 业务所属城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private DateTime? _submitdate;
        /// <summary>
        /// 交付时间
        /// </summary>
        public DateTime? submitdate
        {
            get { return _submitdate; }
            set { _submitdate = value; }
        }
        private string _ordernumber;
        /// <summary>
        /// 业务编号
        /// </summary>
        public string ordernumber
        {
            get { return _ordernumber; }
            set { _ordernumber = value; }
        }
        private DateTime? _acceptlimitdate;
        /// <summary>
        /// 业务受理时限
        /// </summary>
        public DateTime? acceptlimitdate
        {
            get { return _acceptlimitdate; }
            set { _acceptlimitdate = value; }
        }
        private DateTime? _limittime;
        /// <summary>
        /// 业务完成时限
        /// </summary>
        public DateTime? limittime
        {
            get { return _limittime; }
            set { _limittime = value; }
        }
        private decimal? _price;
        /// <summary>
        /// 费用：业务完成后实际支付。
        /// </summary>
        public decimal? price
        {
            get { return _price; }
            set { _price = value; }
        }
        private decimal? _quote;
        /// <summary>
        /// 报价：指业务最初的报价
        /// </summary>
        public decimal? quote
        {
            get { return _quote; }
            set { _quote = value; }
        }
        private int? _supplieruserid;
        /// <summary>
        /// 供应商联系人的id
        /// </summary>
        public int? supplieruserid
        {
            get { return _supplieruserid; }
            set { _supplieruserid = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _repeallimitedate;
        /// <summary>
        /// 撤销/退单时间
        /// </summary>
        public DateTime? repeallimitedate
        {
            get { return _repeallimitedate; }
            set { _repeallimitedate = value; }
        }
        private int? _beforestate;
        /// <summary>
        /// 记录之前的状态，当退单/撤销业务驳回，回到推单/撤销业务前的状态
        /// </summary>
        public int? beforestate
        {
            get { return _beforestate; }
            set { _beforestate = value; }
        }
        private int? _prevacceptbusinessid;
        /// <summary>
        /// 记录重新分配之前的业务ID
        /// </summary>
        public int? prevacceptbusinessid
        {
            get { return _prevacceptbusinessid; }
            set { _prevacceptbusinessid = value; }
        }
        private bool? _isredistribution;
        /// <summary>
        /// 是否已重新分配，这笔业务
        /// </summary>
        public bool? isredistribution
        {
            get { return _isredistribution; }
            set { _isredistribution = value; }
        }
        private int? _bill;
        /// <summary>
        /// 所属账单
        /// </summary>
        public int? bill
        {
            get { return _bill; }
            set { _bill = value; }
        }
        private DateTime? _suredate;
        /// <summary>
        /// 确认业务完成的时间
        /// </summary>
        public DateTime? suredate
        {
            get { return _suredate; }
            set { _suredate = value; }
        }
        private DateTime? _settledate;
        /// <summary>
        /// 结算日期
        /// </summary>
        public DateTime? settledate
        {
            get { return _settledate; }
            set { _settledate = value; }
        }
        private DateTime? _remindersdate;
        /// <summary>
        /// 业务催办时间
        /// </summary>
        public DateTime? remindersdate
        {
            get { return _remindersdate; }
            set { _remindersdate = value; }
        }
        private string _stateremark;
        /// <summary>
        /// 状态说明，考虑到从跟进标准查出当前状态的备注的效率会有些慢，做了个冗余字段
        /// </summary>
        public string stateremark
        {
            get { return _stateremark; }
            set { _stateremark = value; }
        }
    }
}
