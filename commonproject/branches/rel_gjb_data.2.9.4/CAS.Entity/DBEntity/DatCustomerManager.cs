using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_CustomerManager")]
    public class DatCustomerManager : BaseTO
    {
        private int _customerid;
        [SQLField("customerid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }
        private string _customername;
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private string _telphone;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string telphone
        {
            get { return _telphone; }
            set { _telphone = value; }
        }
        private string _wxopenid;
        /// <summary>
        /// 微信openid
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }
        private bool? _valid;
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _branchcompanyid;
        /// <summary>
        /// 分支机构
        /// </summary>
        public int? branchcompanyid
        {
            get { return _branchcompanyid; }
            set { _branchcompanyid = value; }
        }
        private int? _businessuserid;
        /// <summary>
        /// 业务员id
        /// </summary>
        public int? businessuserid
        {
            get { return _businessuserid; }
            set { _businessuserid = value; }
        }
        private int _departmentid;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private string _username;
        /// <summary>
        /// 用户账号
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _userpwd;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string userpwd
        {
            get { return _userpwd; }
            set { _userpwd = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        private bool _online;
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool online
        {
            get { return _online; }
            set { _online = value; }
        }

        private string _officephone;
        /// <summary>
        /// 办公室电话
        /// </summary>
        public string officephone
        {
            get { return _officephone; }
            set { _officephone = value; }
        }
        private string _permissionSee;
        /// <summary>
        /// 数据查看权限
        /// </summary>
        public string permissionSee
        {
            get { return _permissionSee; }
            set { _permissionSee = value; }
        }
        private bool _surveylogin=false;
        /// <summary>
        /// 云查勘权限
        /// </summary>
        public bool surveylogin
        {
            get { return _surveylogin; }
            set { _surveylogin = value; }
        }
        private string _rights;
        /// <summary>
        /// 客户权限
        /// </summary>
        public string rights
        {
            get { return _rights; }
            set { _rights = value; }
        }
        private DateTime? _activetime;
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime? activetime
        {
            get { return _activetime; }
            set { _activetime = value; }
        }
        private int? _subcompanyid;
        /// <summary>
        /// 分支机构ID(对应SYS_Bumen表)
        /// </summary>
        public int? subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
    }
}