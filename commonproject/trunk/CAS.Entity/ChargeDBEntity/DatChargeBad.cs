using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 坏账业务表
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_ChargeBad")]
    public class DatChargeBad : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _chargeentrustid;
        /// <summary>
        /// 收费表ID
        /// </summary>
        public int chargeentrustid
        {
            get { return _chargeentrustid; }
            set { _chargeentrustid = value; }
        }
        private decimal _badmoney;
        /// <summary>
        /// 坏账金额
        /// </summary>
        public decimal badmoney
        {
            get { return _badmoney; }
            set { _badmoney = value; }
        }
        private int _loguserid;
        /// <summary>
        /// 登记人
        /// </summary>
        public int loguserid
        {
            get { return _loguserid; }
            set { _loguserid = value; }
        }
        private string _remark;
        /// <summary>
        /// 说明
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
