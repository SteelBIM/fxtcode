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
        private string _userid;
        /// <summary>
        /// 用户账号
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
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
    }
}