using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 此表已作废
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_Charge")]
    public class DatCharge : BaseTO
    {
        private long _chargeid;
        /// <summary>
        /// ID
        /// </summary>
        [SQLField("chargeid", EnumDBFieldUsage.PrimaryKey, true)]
        public long chargeid
        {
            get { return _chargeid; }
            set { _chargeid = value; }
        }
        private int _businesstype;
        /// <summary>
        /// 业务类型2018005/2018006
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
        private long _objectid;
        /// <summary>
        /// 业务对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private bool _ischarge;
        /// <summary>
        /// 是否已收费
        /// </summary>
        public bool ischarge
        {
            get { return _ischarge; }
            set { _ischarge = value; }
        }
        private bool _isinvoice = false;
        /// <summary>
        /// 是否开发票
        /// </summary>
        public bool isinvoice
        {
            get { return _isinvoice; }
            set { _isinvoice = value; }
        }

        private double? _chargevalue;
        /// <summary>
        /// 实收费用
        /// </summary>
        public double? chargevalue
        {
            get { return _chargevalue; }
            set { _chargevalue = value; }
        }
        private int? _chargeuserid;
        /// <summary>
        /// 收费人
        /// </summary>
        public int? chargeuserid
        {
            get { return _chargeuserid; }
            set { _chargeuserid = value; }
        }
        private DateTime? _chargedate;
        /// <summary>
        /// 收费日期
        /// </summary>
        public DateTime? chargedate
        {
            get { return _chargedate; }
            set { _chargedate = value; }
        }
        private DateTime _createdate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
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
        private double? _brokerage;
        /// <summary>
        /// 佣金
        /// </summary>
        public double? brokerage
        {
            get { return _brokerage; }
            set { _brokerage = value; }
        }
        private double? _otherexpenses;
        /// <summary>
        /// 其它支出
        /// </summary>
        public double? otherexpenses
        {
            get { return _otherexpenses; }
            set { _otherexpenses = value; }
        }
        private double? _actualrevenue;
        /// <summary>
        /// 实际收入
        /// </summary>
        public double? actualrevenue
        {
            get { return _actualrevenue; }
            set { _actualrevenue = value; }
        }
        private double? _rebate;
        /// <summary>
        /// 折扣
        /// </summary>
        public double? rebate
        {
            get { return _rebate; }
            set { _rebate = value; }
        }

        private int? _brokeragerate;
        /// <summary>
        /// 佣金比率
        /// </summary>
        public int? brokeragerate
        {
            get { return _brokeragerate; }
            set { _brokeragerate = value; }
        }
        private DateTime? _bokerjiesuandate;
        /// <summary>
        /// 佣金结算日期
        /// </summary>
        public DateTime? bokerjiesuandate
        {
            get { return _bokerjiesuandate; }
            set { _bokerjiesuandate = value; }
        }
        private int _invoicetypecode=0;
        /// <summary>
        /// 开票方式
        /// </summary>
        public int invoicetypecode
        {
            get { return _invoicetypecode; }
            set { _invoicetypecode = value; }
        }
        private int _chargetypecode=0;
        /// <summary>
        /// 收款方式
        /// </summary>
        public int chargetypecode
        {
            get { return _chargetypecode; }
            set { _chargetypecode = value; }
        }

        private int _paymentuserid=0;
        /// <summary>
        /// 缴款人
        /// </summary>
        public int paymentuserid
        {
            get { return _paymentuserid; }
            set { _paymentuserid = value; }
        }

        private double _returncost;
        /// <summary>
        /// 退报告费
        /// </summary>
        public double returncost
        {
            get { return _returncost; }
            set { _returncost = value; }
        }

        //默认待收费
        private int _chargestatecode = 10016001;
        /// <summary>
        /// 收费状态
        /// </summary>
        public int chargestatecode
        {
            get { return _chargestatecode; }
            set { _chargestatecode = value; }
        }
    }
}
