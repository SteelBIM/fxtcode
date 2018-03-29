using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Dat_AmountDetails")]
    public class DatAmountDetails : BaseTO
    {
        private int _amountdetailsid;
        /// <summary>
        /// 明细ID
        /// </summary>
        [SQLField("amountdetailsid", EnumDBFieldUsage.PrimaryKey, true)]
        public int amountdetailsid
        {
            get { return _amountdetailsid; }
            set { _amountdetailsid = value; }
        }
        private int _acceptbusinessid;
        /// <summary>
        /// 业务受理ID
        /// </summary>
        public int acceptbusinessid
        {
            get { return _acceptbusinessid; }
            set { _acceptbusinessid = value; }
        }
        private decimal _money;
        /// <summary>
        /// 金额
        /// </summary>
        public decimal money
        {
            get { return _money; }
            set { _money = value; }
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
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _settlementdate;
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? settlementdate
        {
            get { return _settlementdate; }
            set { _settlementdate = value; }
        }
    }

}
