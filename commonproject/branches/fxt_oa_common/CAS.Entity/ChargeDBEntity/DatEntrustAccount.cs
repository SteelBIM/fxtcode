using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_EntrustAccount")]
    public class DatEntrustAccount : BaseTO
    {
        private int _entrustaccountid;
        [SQLField("entrustaccountid", EnumDBFieldUsage.PrimaryKey, true)]
        public int entrustaccountid
        {
            get { return _entrustaccountid; }
            set { _entrustaccountid = value; }
        }

        private long _entrustid;
        /// <summary>
        /// 委托id
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }

        private decimal? _receipts;
        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal? receipts
        {
            get { return _receipts; }
            set { _receipts = value; }
        }
        private int? _chargetype;
        /// <summary>
        /// 收费类型
        /// </summary>
        public int? chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }
        private int? _billtype;
        /// <summary>
        /// 账单类型
        /// </summary>
        public int? billtype
        {
            get { return _billtype; }
            set { _billtype = value; }
        }
        private int? _receiptsperson;
        /// <summary>
        /// 收费人
        /// </summary>
        public int? receiptsperson
        {
            get { return _receiptsperson; }
            set { _receiptsperson = value; }
        }
        private DateTime? _closeaccounttime;
        /// <summary>
        /// 结单时间
        /// </summary>
        public DateTime? closeaccounttime
        {
            get { return _closeaccounttime; }
            set { _closeaccounttime = value; }
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
    }
}