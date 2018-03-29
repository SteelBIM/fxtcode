using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_FpRemindSetting")]
    public class SYSFpRemindSetting : BaseTO
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
        /// 运营方ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _branchid;
        /// <summary>
        /// 分支机构ID
        /// </summary>
        public string branchid
        {
            get { return _branchid; }
            set { _branchid = value; }
        }
        private string _branchname;
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string branchname
        {
            get { return _branchname; }
            set { _branchname = value; }
        }
        private string _biztype;
        /// <summary>
        /// 委托类型ID
        /// </summary>
        public string biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private string _biztypename;
        /// <summary>
        /// 委托类型名称
        /// </summary>
        public string biztypename
        {
            get { return _biztypename; }
            set { _biztypename = value; }
        }
        private string _isreporttypecode;
        /// <summary>
        /// 是否全选了该报告类型
        /// </summary>
        public string isreporttypecode
        {
            get { return _isreporttypecode; }
            set { _isreporttypecode = value; }
        }
        private string _reporttypecode;
        /// <summary>
        /// 报告类型CODE
        /// </summary>
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttypename;
        /// <summary>
        /// 报告类型名称
        /// </summary>
        public string reporttypename
        {
            get { return _reporttypename; }
            set { _reporttypename = value; }
        }
        private string _reportsubtypecodeid;
        /// <summary>
        /// 报告类型CODE明细
        /// </summary>
        public string reportsubtypecodeid
        {
            get { return _reportsubtypecodeid; }
            set { _reportsubtypecodeid = value; }
        }
        private string _reportsubtypename;
        /// <summary>
        /// 报告类型名称明细
        /// </summary>
        public string reportsubtypename
        {
            get { return _reportsubtypename; }
            set { _reportsubtypename = value; }
        }
        private int? _remindday;
        /// <summary>
        /// 提醒天数
        /// </summary>
        public int? remindday
        {
            get { return _remindday; }
            set { _remindday = value; }
        }
        private int? _notremindday;
        /// <summary>
        /// 不提醒天数
        /// </summary>
        public int? notremindday
        {
            get { return _notremindday; }
            set { _notremindday = value; }
        }
        private int? _createuserid;
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime? _createdate;
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }

}
