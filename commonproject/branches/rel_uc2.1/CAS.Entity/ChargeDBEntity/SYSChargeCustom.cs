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
        /// 委托类型编码集
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
    }
}
