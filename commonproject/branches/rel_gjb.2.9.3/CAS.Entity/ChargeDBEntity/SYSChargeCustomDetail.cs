using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_ChargeCustomDetail")]
    public class SYSChargeCustomDetail : BaseTO
    {
        private int _customchargedetailid;
        [SQLField("customchargedetailid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customchargedetailid
        {
            get { return _customchargedetailid; }
            set { _customchargedetailid = value; }
        }
        private int _customid;
        /// <summary>
        /// 收费标准类型Id
        /// </summary>
        public int customid
        {
            get { return _customid; }
            set { _customid = value; }
        }
        private int _assesstotalmin;
        /// <summary>
        /// 评估总值(万元)最小值
        /// </summary>
        public int assesstotalmin
        {
            get { return _assesstotalmin; }
            set { _assesstotalmin = value; }
        }
        private int _assesstotalmax;
        /// <summary>
        /// 评估总值(万元)最大值
        /// </summary>
        public int assesstotalmax
        {
            get { return _assesstotalmax; }
            set { _assesstotalmax = value; }
        }
        private int _companystandardchargetype;
        /// <summary>
        /// 公司标准收费类型
        /// </summary>
        public int companystandardchargetype
        {
            get { return _companystandardchargetype; }
            set { _companystandardchargetype = value; }
        }
        private decimal _companystandardcharge;
        /// <summary>
        /// 公司标准收费
        /// </summary>
        public decimal companystandardcharge
        {
            get { return _companystandardcharge; }
            set { _companystandardcharge = value; }
        }
        private int _companyminchargetype;
        /// <summary>
        /// 公司最低收费类型
        /// </summary>
        public int companyminchargetype
        {
            get { return _companyminchargetype; }
            set { _companyminchargetype = value; }
        }
        private decimal _companymincharge;
        /// <summary>
        /// 公司最低收费
        /// </summary>
        public decimal companymincharge
        {
            get { return _companymincharge; }
            set { _companymincharge = value; }
        }
        private int _sort = 1;
        public int sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
    }
}