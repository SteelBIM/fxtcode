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
    }

}
