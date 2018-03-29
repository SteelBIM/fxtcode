using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_EntrustOver")]
    public class DatEntrustOver : BaseTO
    {
        private int _entrustoverid;
        [SQLField("entrustoverid", EnumDBFieldUsage.PrimaryKey, true)]
        public int entrustoverid
        {
            get { return _entrustoverid; }
            set { _entrustoverid = value; }
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
        private decimal _brokeragepercentage;
        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal brokeragepercentage
        {
            get { return _brokeragepercentage; }
            set { _brokeragepercentage = value; }
        }
        private decimal _brokerage;
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal brokerage
        {
            get { return _brokerage; }
            set { _brokerage = value; }
        }
        private int _salesmanid;
        /// <summary>
        /// 业务员Id
        /// </summary>
        public int salesmanid
        {
            get { return _salesmanid; }
            set { _salesmanid = value; }
        }
        private DateTime _brokerageclosetime;
        /// <summary>
        /// 佣金结算时间
        /// </summary>
        public DateTime brokerageclosetime
        {
            get { return _brokerageclosetime; }
            set { _brokerageclosetime = value; }
        }
        private decimal? _otherout;
        /// <summary>
        /// 其他支出
        /// </summary>
        public decimal? otherout
        {
            get { return _otherout; }
            set { _otherout = value; }
        }
        private decimal? _grossprofit;
        /// <summary>
        /// 毛利润
        /// </summary>
        public decimal? grossprofit
        {
            get { return _grossprofit; }
            set { _grossprofit = value; }
        }
        private DateTime _overtime;
        /// <summary>
        /// 结单时间
        /// </summary>
        public DateTime overtime
        {
            get { return _overtime; }
            set { _overtime = value; }
        }
        private int _createrid;
        /// <summary>
        /// 记录创建人
        /// </summary>
        public int createrid
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
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
        /// 月结id
        /// </summary>
        public int? chargemonthlyid
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