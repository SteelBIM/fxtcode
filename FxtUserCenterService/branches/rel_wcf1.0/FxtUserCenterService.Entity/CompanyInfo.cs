using System;
using CAS.Entity.BaseDAModels;

namespace FxtUserCenterService.Entity
{
    [Serializable]
    [TableAttribute("dbo.CompanyInfo")]
    public class CompanyInfo : BaseTO
    {
        private int _companyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        [SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _companyname;
        /// <summary>
        /// 机构名称
        /// </summary>
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        private string _companycode;
        /// <summary>
        /// 机构代码（用户名后缀）
        /// </summary>
        public string companycode
        {
            get { return _companycode; }
            set { _companycode = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _valid = 1;
        /// <summary>
        /// 有效
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private string _businessdb;
        /// <summary>
        /// 业务数据库连接
        /// </summary>
        public string businessdb
        {
            get { return _businessdb; }
            set { _businessdb = value; }
        }
        private string _smsloginname;
        /// <summary>
        /// 短信接口登录名称
        /// </summary>
        public string smsloginname
        {
            get { return _smsloginname; }
            set { _smsloginname = value; }
        }
        private string _smsloginpassword;
        /// <summary>
        /// 短信接口密码
        /// </summary>
        public string smsloginpassword
        {
            get { return _smsloginpassword; }
            set { _smsloginpassword = value; }
        }
        private string _smssendname;
        /// <summary>
        /// 短信发送者
        /// </summary>
        public string smssendname
        {
            get { return _smssendname; }
            set { _smssendname = value; }
        }

        private string _wxid;
        /// <summary>
        /// 微信Id
        /// </summary>
        public string wxid
        {
            get { return _wxid; }
            set { _wxid = value; }
        }

        private string _wxname;
        /// <summary>
        /// 微信名字
        /// </summary>
        public string wxname
        {
            get { return _wxname; }
            set { _wxname = value; }
        }
        private string _signname;
        /// <summary>
        /// 公司标识 caoq 2014-03-10
        /// </summary>
        public string signname
        {
            get { return _signname; }
            set { _signname = value; }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        [SQLReadOnly]
        public DateTime OverDate { get; set; }
    }
}