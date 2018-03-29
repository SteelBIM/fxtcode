using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_EntrustRefund")]
    public class DatEntrustRefund : BaseTO
    {
        private int _entrustrefundid;
        [SQLField("entrustrefundid", EnumDBFieldUsage.PrimaryKey, true)]
        public int entrustrefundid
        {
            get { return _entrustrefundid; }
            set { _entrustrefundid = value; }
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
        private decimal _refundamount;
        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal refundamount
        {
            get { return _refundamount; }
            set { _refundamount = value; }
        }
        private int _refundtype;
        /// <summary>
        /// 现金/支票/转账/其他
        /// </summary>
        public int refundtype
        {
            get { return _refundtype; }
            set { _refundtype = value; }
        }
        private int _recordpersonid;
        /// <summary>
        /// 登记人
        /// </summary>
        public int recordpersonid
        {
            get { return _recordpersonid; }
            set { _recordpersonid = value; }
        }
        private DateTime _refundtime;
        /// <summary>
        /// 退费时间
        /// </summary>
        public DateTime refundtime
        {
            get { return _refundtime; }
            set { _refundtime = value; }
        }
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _handpersonid;
        /// <summary>
        /// 经手人
        /// </summary>
        public int handpersonid
        {
            get { return _handpersonid; }
            set { _handpersonid = value; }
        }
        private int _valid = 1;
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
        private int? _chargemonthlyid;
        /// <summary>
        /// 状态
        /// </summary>
        public int? ChargeMonthlyId
        {
            get { return _chargemonthlyid; }
            set { _chargemonthlyid = value; }
        }
        private int? _chargeentrustid;
        /// <summary>
        /// 收费ID，对应表Dat_ChargeEntrust
        /// </summary>
        public int? chargeentrustid
        {
            get { return _chargeentrustid; }
            set { _chargeentrustid = value; }
        }
    }
}