using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_ChargeCustom")]
    public class SYSChargeCustom : BaseTO
    {
        private int _customid;
        [SQLField("customid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customid
        {
            get { return _customid; }
            set { _customid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _customname;
        /// <summary>
        /// 收费标准名称
        /// </summary>
        public string customname
        {
            get { return _customname; }
            set { _customname = value; }
        }
        private string _entrusttypecodes;
        /// <summary>
        /// 委托类型编码集(报告类型)
        /// </summary>
        public string entrusttypecodes
        {
            get { return _entrusttypecodes; }
            set { _entrusttypecodes = value; }
        }
        private string _businesstypes;
        /// <summary>
        /// 适用业务类型
        /// </summary>
        public string businesstypes
        {
            get { return _businesstypes; }
            set { _businesstypes = value; }
        }
        private int _chargetype;
        /// <summary>
        /// 收费形式
        /// </summary>
        public int chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }
        private string _departments;
        /// <summary>
        /// 适用机构
        /// </summary>
        public string departments
        {
            get { return _departments; }
            set { _departments = value; }
        }
        private bool _isdefault;
        /// <summary>
        /// 是否设为默认收费标准
        /// </summary>
        public bool isdefault
        {
            get { return _isdefault; }
            set { _isdefault = value; }
        }
        private string _businessstagetypes;
        /// <summary>
        /// 适用业务阶段(预评:2018005,报告:2018006)
        /// </summary>
        public string businessstagetypes
        {
            get { return _businessstagetypes; }
            set { _businessstagetypes = value; }
        }
        private string _customers;
        /// <summary>
        /// 适用客户单位(总行)
        /// </summary>
        public string customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
        private string _customersfh;
        /// <summary>
        /// 适用客户单位（分行）
        /// </summary>
        public string customersfh
        {
            get { return _customersfh; }
            set { _customersfh = value; }
        }
        private string _customerszh;
        /// <summary>
        /// 适用客户单位(支行)
        /// </summary>
        public string customerszh
        {
            get { return _customerszh; }
            set { _customerszh = value; }
        }
        private int _chargestandardtype=(int)ChargeStandard.自定义标准;
        /// <summary>
        /// 收费标准类型(1:国家标准，2:自定义标准)
        /// </summary>
        public int chargestandardtype
        {
            get { return _chargestandardtype; }
            set { _chargestandardtype = value; }
        }
        private string _reportsubtype;
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string reportsubtype
        {
            get { return _reportsubtype; }
            set { _reportsubtype = value; }
        }
    }
}
