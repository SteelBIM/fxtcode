using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 收费收支银行账号信息表
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.SYS_ChargeBankAccount")]
    public class SYSChargeBankAccount : BaseTO
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
        /// 机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _bank;
        /// <summary>
        /// 开户银行：银行+分行+支行
        /// </summary>
        public string bank
        {
            get { return _bank; }
            set { _bank = value; }
        }
        private string _accountname;
        /// <summary>
        /// 开户账号
        /// </summary>
        public string accountname
        {
            get { return _accountname; }
            set { _accountname = value; }
        }
        private string _accounttruename;
        /// <summary>
        /// 开户人姓名
        /// </summary>
        public string accounttruename
        {
            get { return _accounttruename; }
            set { _accounttruename = value; }
        }
    }
}
