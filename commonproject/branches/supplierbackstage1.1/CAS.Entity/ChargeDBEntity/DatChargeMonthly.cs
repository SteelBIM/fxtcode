using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ChargeMonthly")]
    public class DatChargeMonthly : BaseTO
    {
        private int _chargemonthlyid;
        [SQLField("chargemonthlyid", EnumDBFieldUsage.PrimaryKey, true)]
        public int chargemonthlyid
        {
            get { return _chargemonthlyid; }
            set { _chargemonthlyid = value; }
        }
        private string _entrustids;
        /// <summary>
        /// 委托ids
        /// </summary>
        public string entrustids
        {
            get { return _entrustids; }
            set { _entrustids = value; }
        }

        private string _bankname;
        /// <summary>
        /// 委托银行名称
        /// </summary>
        public string bankname
        {
            get { return _bankname; }
            set { _bankname = value; }
        }
        private string _month;
        /// <summary>
        /// 月份
        /// </summary>
        public string month
        {
            get { return _month; }
            set { _month = value; }
        }
        private int _businessuserid;
        /// <summary>
        /// 业务员id
        /// </summary>
        public int businessuserid
        {
            get { return _businessuserid; }
            set { _businessuserid = value; }
        }

        private int _chargetype;
        /// <summary>
        /// 收费标准类型id
        /// </summary>
        public int chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }
        private int _chargenameid;
        /// <summary>
        /// 收费标准id
        /// </summary>
        public int chargenameid
        {
            get { return _chargenameid; }
            set { _chargenameid = value; }
        }
        private decimal? _standardcharge;
        /// <summary>
        /// 标准收费
        /// </summary>
        public decimal? standardcharge
        {
            get { return _standardcharge; }
            set { _standardcharge = value; }
        }
        private int _privilegetype;
        /// <summary>
        /// 优惠类型
        /// </summary>
        public int privilegetype
        {
            get { return _privilegetype; }
            set { _privilegetype = value; }
        }
        private decimal? _mincharge;
        /// <summary>
        /// 最小收费
        /// </summary>
        public decimal? mincharge
        {
            get { return _mincharge; }
            set { _mincharge = value; }
        }
        private decimal? _privilegediscount;
        /// <summary>
        /// 优惠折扣
        /// </summary>
        public decimal? privilegediscount
        {
            get { return _privilegediscount; }
            set { _privilegediscount = value; }
        }
        private decimal? _privilegemoney;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? privilegemoney
        {
            get { return _privilegemoney; }
            set { _privilegemoney = value; }
        }
        private int _approvalid;
        /// <summary>
        /// 审批id
        /// </summary>
        public int approvalid
        {
            get { return _approvalid; }
            set { _approvalid = value; }
        }
        /// <summary>
        ///工作流id
        /// </summary>
        private int _worktodoid;
        /// <summary>
        /// 工作流id
        /// </summary>
        public int worktodoid
        {
            get { return _worktodoid; }
            set { _worktodoid = value; }
        }
        private decimal? _receivable;
        /// <summary>
        /// 应收
        /// </summary>
        public decimal? receivable
        {
            get { return _receivable; }
            set { _receivable = value; }
        }
        private int _statecode = 10016003;
        /// <summary>
        /// 状态码
        /// </summary>
        public int statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _chargeuserid;
        /// <summary>
        ///收款负责人
        /// </summary>
        public int chargeuserid
        {
            get { return _chargeuserid; }
            set { _chargeuserid = value; }
        }
    }
}