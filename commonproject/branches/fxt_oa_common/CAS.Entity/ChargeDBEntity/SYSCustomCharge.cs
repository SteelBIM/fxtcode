using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_CustomCharge")]
    public class SYSCustomCharge : BaseTO
    {
        private int _customchargeid;
        [SQLField("customchargeid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customchargeid
        {
            get { return _customchargeid; }
            set { _customchargeid = value; }
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
        private string _name;
        /// <summary>
        /// 收费标准名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
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
    }
}