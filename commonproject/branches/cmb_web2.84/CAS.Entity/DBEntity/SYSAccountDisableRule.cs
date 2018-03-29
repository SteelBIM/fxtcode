using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AccountDisableRule")]
    public class SYSAccountDisableRule : BaseTO
    {
        private int _ruleid;
        [SQLField("ruleid", EnumDBFieldUsage.PrimaryKey, true)]
        public int ruleid
        {
            get { return _ruleid; }
            set { _ruleid = value; }
        }
        private int _reporttype;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int reporttype
        {
            get { return _reporttype; }
            set { _reporttype = value; }
        }
        private int _ruletype;
        /// <summary>
        /// 1.收费，2.归档
        /// </summary>
        public int ruletype
        {
            get { return _ruletype; }
            set { _ruletype = value; }
        }
        private decimal? _chargemaxprice;
        /// <summary>
        /// 收费限制金额
        /// </summary>
        public decimal? chargemaxprice
        {
            get { return _chargemaxprice; }
            set { _chargemaxprice = value; }
        }
        private bool _isuse;
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool isuse
        {
            get { return _isuse; }
            set { _isuse = value; }
        }
        private DateTime? _updatedate;
        public DateTime? updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private int? _updateuserid;
        public int? updateuserid
        {
            get { return _updateuserid; }
            set { _updateuserid = value; }
        }
        private bool _valid;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

    public class SYS_AccountDisableRule : SYSAccountDisableRule
    {
        [SQLReadOnly]
        public string reporttypename { get; set; }
    }

    public class AccountDisableResult:BaseTO
    {
        /// <summary>
        /// 归档违规数量
        /// </summary>
        public int backupcount { get; set; }
        /// <summary>
        /// 收费超期数量
        /// </summary>
        public int chargecount { get; set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        public int accountstatus{ get; set; }
    }
}