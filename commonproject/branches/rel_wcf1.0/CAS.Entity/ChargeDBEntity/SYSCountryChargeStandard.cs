using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_CountryChargeStandard")]
    public class SYSCountryChargeStandard : BaseTO
    {
        private int _countrystandardid;
        [SQLField("countrystandardid", EnumDBFieldUsage.PrimaryKey, true)]
        public int countrystandardid
        {
            get { return _countrystandardid; }
            set { _countrystandardid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _chargetypecode;
        /// <summary>
        /// 评估收费类型编码
        /// </summary>
        public int chargetypecode
        {
            get { return _chargetypecode; }
            set { _chargetypecode = value; }
        }
        private int _assesstotalmax;
        /// <summary>
        /// 评估总值(万元)最小值
        /// </summary>
        public int assesstotalmax
        {
            get { return _assesstotalmax; }
            set { _assesstotalmax = value; }
        }
        private int _assesstotalmin;
        /// <summary>
        /// 评估总值(万元)最大值
        /// </summary>
        public int assesstotalmin
        {
            get { return _assesstotalmin; }
            set { _assesstotalmin = value; }
        }
        private decimal _countryratemax;
        /// <summary>
        /// 国家最高费率
        /// </summary>
        public decimal countryratemax
        {
            get { return _countryratemax; }
            set { _countryratemax = value; }
        }
        private decimal _countryratemin;
        /// <summary>
        /// 国家最低费率
        /// </summary>
        public decimal countryratemin
        {
            get { return _countryratemin; }
            set { _countryratemin = value; }
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
        private int _sort;
        public int sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
    }
}