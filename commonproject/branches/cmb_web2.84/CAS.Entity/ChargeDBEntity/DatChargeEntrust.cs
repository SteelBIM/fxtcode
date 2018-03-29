using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ChargeEntrust")]
    public class DatChargeEntrust : BaseTO
    {
        private int _chargeentrustid;
        [SQLField("chargeentrustid", EnumDBFieldUsage.PrimaryKey, true)]
        public int chargeentrustid
        {
            get { return _chargeentrustid; }
            set { _chargeentrustid = value; }
        }
        private long _entrustid;
        /// <summary>
        /// 业务Id
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private long? _reportid;
        /// <summary>
        /// 报告id
        /// </summary>
        public long? reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        private bool _ischargemonthly;
        /// <summary>
        /// 是否月结
        /// </summary>
        public bool ischargemonthly
        {
            get { return _ischargemonthly; }
            set { _ischargemonthly = value; }
        }
        private int _chargenameid;
        /// <summary>
        /// 收费标准名称
        /// </summary>
        public int chargenameid
        {
            get { return _chargenameid; }
            set { _chargenameid = value; }
        }
        private decimal _totalamount;
        /// <summary>
        /// 标准收费总额
        /// </summary>
        public decimal totalamount
        {
            get { return _totalamount; }
            set { _totalamount = value; }
        }
        private int? _privilegetype;
        /// <summary>
        /// 优惠类型
        /// </summary>
        public int? privilegetype
        {
            get { return _privilegetype; }
            set { _privilegetype = value; }
        }
        private decimal? _mincharge;
        /// <summary>
        /// 最低收费金额
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
        private int? _approvalid;
        /// <summary>
        /// 审批id
        /// </summary>
        public int? approvalid
        {
            get { return _approvalid; }
            set { _approvalid = value; }
        }
        private decimal _receivable;
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal receivable
        {
            get { return _receivable; }
            set { _receivable = value; }
        }
        private int _valid = 10016001;
        /// <summary>
        /// 状态
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        /// 记录时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _chargetype;
        /// <summary>
        ///收费标准类型,1国家标准,2自定义标准
        /// </summary>
        public int chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }

        private int _worktodoid;
        /// <summary>
        ///审批流程id
        /// </summary>
        public int worktodoid
        {
            get { return _worktodoid; }
            set { _worktodoid = value; }
        }

        private int _chargemonthlyid;
        /// <summary>
        ///月结收费id
        /// </summary>
        public int chargemonthlyid
        {
            get { return _chargemonthlyid; }
            set { _chargemonthlyid = value; }
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
        private int _isremind;
        /// <summary>
        /// 超期提醒状态
        /// </summary>
        public int isremind
        {
            get { return _isremind; }
            set { _isremind = value; }
        }
        private long? _ypid;
        /// <summary>
        /// 预评ID（与ReportId只其中一个有值）
        /// </summary>
        public long? ypid
        {
            get { return _ypid; }
            set { _ypid = value; }
        }
        
        /// <summary>
        /// 收款负责人
        /// </summary>
        [SQLReadOnly]
        public string chargeusername { get; set; }
    }
}
