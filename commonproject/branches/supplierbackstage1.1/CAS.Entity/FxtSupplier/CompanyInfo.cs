using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    /// <summary>
    /// 基础信息
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.CompanyInfo")]
    public class CompanyInfo : BaseTO
    {
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        [SQLField("companyid", EnumDBFieldUsage.PrimaryKey, true)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int? _fxtcompanyid;
        /// <summary>
        /// 房讯通客户ID
        /// </summary>
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _supplierno;
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string supplierno
        {
            get { return _supplierno; }
            set { _supplierno = value; }
        }
        private string _companyname;
        /// <summary>
        /// 公司名称
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
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool? _valid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int? _provinceid;
        /// <summary>
        /// 省份ID
        /// </summary>
        public int? provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _shortname;
        /// <summary>
        /// 公司简称
        /// </summary>
        public string shortname
        {
            get { return _shortname; }
            set { _shortname = value; }
        }
        private string _weburl;
        /// <summary>
        /// 公司网站
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _legalman;
        /// <summary>
        /// 法人代表
        /// </summary>
        public string legalman
        {
            get { return _legalman; }
            set { _legalman = value; }
        }
        private string _organizationcode;
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string organizationcode
        {
            get { return _organizationcode; }
            set { _organizationcode = value; }
        }
        private string _organizationfile;
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        public string organizationfile
        {
            get { return _organizationfile; }
            set { _organizationfile = value; }
        }
        private string _logo;
        /// <summary>
        /// Logo
        /// </summary>
        public string logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private string _regnumber;
        /// <summary>
        /// 工商注册号
        /// </summary>
        public string regnumber
        {
            get { return _regnumber; }
            set { _regnumber = value; }
        }
        private DateTime? _regbegindate;
        /// <summary>
        /// 工商登记日期
        /// </summary>
        public DateTime? regbegindate
        {
            get { return _regbegindate; }
            set { _regbegindate = value; }
        }
        private DateTime? _regenddate;
        /// <summary>
        /// 工商到期日期
        /// </summary>
        public DateTime? regenddate
        {
            get { return _regenddate; }
            set { _regenddate = value; }
        }
        private string _businessnumber;
        /// <summary>
        /// 营业执照号码
        /// </summary>
        public string businessnumber
        {
            get { return _businessnumber; }
            set { _businessnumber = value; }
        }
        private string _businessfile;
        /// <summary>
        /// 营业执照附件 businessscope
        /// </summary>
        public string businessfile
        {
            get { return _businessfile; }
            set { _businessfile = value; }
        }
        private string _businessscope;
        /// <summary>
        /// 经营范围
        /// </summary>
        public string businessscope
        {
            get { return _businessscope; }
            set { _businessscope = value; }
        }
        private string _projectexperience;
        /// <summary>
        /// 项目经验
        /// </summary>
        public string projectexperience
        {
            get { return _projectexperience; }
            set { _projectexperience = value; }
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
        private string _telephone;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private string _fax;
        /// <summary>
        /// 传真
        /// </summary>
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private string _introduction;
        /// <summary>
        /// 公司介绍
        /// </summary>
        public string introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        private string _applycertificationfile;
        /// <summary>
        /// 申请认证公函
        /// </summary>
        public string applycertificationfile
        {
            get { return _applycertificationfile; }
            set { _applycertificationfile = value; }
        }
    }

}
