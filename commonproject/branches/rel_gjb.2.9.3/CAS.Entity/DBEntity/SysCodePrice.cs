using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.sys_CodePrice")]
    public class SysCodePrice : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _code;
        /// <summary>
        /// CODE
        /// </summary>
        public int? code
        {
            get { return _code; }
            set { _code = value; }
        }
        private int? _subcode;
        /// <summary>
        /// 当需要两个code值确定系数时使用
        /// </summary>
        public int? subcode
        {
            get { return _subcode; }
            set { _subcode = value; }
        }
        private string _codename;
        /// <summary>
        /// 系数说明
        /// </summary>
        public string codename
        {
            get { return _codename; }
            set { _codename = value; }
        }
        private decimal _price;
        /// <summary>
        /// 影响价格的百分比
        /// </summary>
        public decimal price
        {
            get { return _price; }
            set { _price = value; }
        }
        private int? _purposecode;
        /// <summary>
        /// 用途Code（1002）
        /// </summary>
        public int? purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }        
        private int? _typecode;
        /// <summary>
        /// 修正系数类型（10001）
        /// </summary>
        public int? typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
    }
}
