using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_YPRequestReportRemindSet")]
    public class SYSYPRequestReportRemindSet : BaseTO
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
        private string _branchid;
        /// <summary>
        /// 分支机构id
        /// </summary>
        public string branchid
        {
            get { return _branchid; }
            set { _branchid = value; }
        }
        private string _branchname;
        /// <summary>
        /// 分支机构
        /// </summary>
        public string branchname
        {
            get { return _branchname; }
            set { _branchname = value; }
        }
        private string _custcompanyid;
        /// <summary>
        /// 银行id
        /// </summary>
        public string custcompanyid
        {
            get { return _custcompanyid; }
            set { _custcompanyid = value; }
        }
        private string _custbranchid;
        /// <summary>
        /// 分行
        /// </summary>
        public string custbranchid
        {
            get { return _custbranchid; }
            set { _custbranchid = value; }
        }
        private string _custsubbranchid;
        /// <summary>
        /// 支行
        /// </summary>
        public string custsubbranchid
        {
            get { return _custsubbranchid; }
            set { _custsubbranchid = value; }
        }
        private string _customercompanyfullname;
        /// <summary>
        /// 银行名称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }
        private string _biztype;
        /// <summary>
        /// 委托类型
        /// </summary>
        public string biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private string _biztypename;
        /// <summary>
        /// 委托类型
        /// </summary>
        public string biztypename
        {
            get { return _biztypename; }
            set { _biztypename = value; }
        }
        private string _reporttypecode;
        /// <summary>
        /// 报告类型code
        /// </summary>
        public string reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reporttypename;
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttypename
        {
            get { return _reporttypename; }
            set { _reporttypename = value; }
        }
        private string _reportsubtypecodeid;
        /// <summary>
        /// 报告子类型codeid
        /// </summary>
        public string reportsubtypecodeid
        {
            get { return _reportsubtypecodeid; }
            set { _reportsubtypecodeid = value; }
        }
        private string _reportsubtypename;
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string reportsubtypename
        {
            get { return _reportsubtypename; }
            set { _reportsubtypename = value; }
        }
        private int _remindday;
        /// <summary>
        /// 提醒时间（单位：天）
        /// </summary>
        public int remindday
        {
            get { return _remindday; }
            set { _remindday = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _customertype;
        /// <summary>
        /// 客户类型
        /// </summary>
        public string customertype
        {
            get { return _customertype; }
            set { _customertype = value; }
        }
    }
}
