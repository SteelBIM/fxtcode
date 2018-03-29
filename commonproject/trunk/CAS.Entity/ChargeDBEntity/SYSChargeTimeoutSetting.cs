using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 收费超时规则设置表
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_ChargeTimeoutSetting")]
    public class SYSChargeTimeoutSetting : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _timeoutday;
        /// <summary>
        /// 报告完成后超时天数
        /// </summary>
        public int timeoutday
        {
            get { return _timeoutday; }
            set { _timeoutday = value; }
        }
        private int _tstimeoutday;
        /// <summary>
        /// 报告投送后超时天数
        /// </summary>
        public int tstimeoutday
        {
            get { return _tstimeoutday; }
            set { _tstimeoutday = value; }
        }
        private int _kptimeoutday;
        /// <summary>
        /// 报告开票后超时天数
        /// </summary>
        public int kptimeoutday
        {
            get { return _kptimeoutday; }
            set { _kptimeoutday = value; }
        }
        private string _entrusttypecodes;
        /// <summary>
        /// 适用报告类型
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
        private string _departments;
        /// <summary>
        /// 适用机构（分公司）
        /// </summary>
        public string departments
        {
            get { return _departments; }
            set { _departments = value; }
        }
        private string _customers;
        /// <summary>
        /// 适用客户单位
        /// </summary>
        public string customers
        {
            get { return _customers; }
            set { _customers = value; }
        }
        private string _customersfh;
        /// <summary>
        /// 适用客户单位-分行
        /// </summary>
        public string customersfh
        {
            get { return _customersfh; }
            set { _customersfh = value; }
        }
        private string _customerszh;
        /// <summary>
        /// 适用客户单位-支行
        /// </summary>
        public string customerszh
        {
            get { return _customerszh; }
            set { _customerszh = value; }
        }
        private int _createrid;
        /// <summary>
        /// 录入人
        /// </summary>
        public int createrid
        {
            get { return _createrid; }
            set { _createrid = value; }
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
