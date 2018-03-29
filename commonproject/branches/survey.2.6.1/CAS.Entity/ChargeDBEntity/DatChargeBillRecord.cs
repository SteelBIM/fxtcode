using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ChargeBillRecord")]
    public class DatChargeBillRecord : BaseTO
    {
        private int _billrecordid;
        [SQLField("billrecordid", EnumDBFieldUsage.PrimaryKey, true)]
        public int billrecordid
        {
            get { return _billrecordid; }
            set { _billrecordid = value; }
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
        private int _billtype;
        /// <summary>
        /// 票据类型
        /// </summary>
        public int billtype
        {
            get { return _billtype; }
            set { _billtype = value; }
        }
        private string _billcode;
        /// <summary>
        /// 票据号
        /// </summary>
        public string billcode
        {
            get { return _billcode; }
            set { _billcode = value; }
        }
        private string _customername;
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private decimal _billamount;
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal billamount
        {
            get { return _billamount; }
            set { _billamount = value; }
        }
        private int _billtitle;
        /// <summary>
        /// 票据抬头
        /// </summary>
        public int billtitle
        {
            get { return _billtitle; }
            set { _billtitle = value; }
        }
        private int _drawerid;
        /// <summary>
        /// 开票人
        /// </summary>
        public int drawerid
        {
            get { return _drawerid; }
            set { _drawerid = value; }
        }
        private DateTime? _billtime;
        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime? billtime
        {
            get { return _billtime; }
            set { _billtime = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 状态,0:作废,1:未作废,2:红冲
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _abolishreason;
        /// <summary>
        /// 作废原因
        /// </summary>
        public string abolishreason
        {
            get { return _abolishreason; }
            set { _abolishreason = value; }
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