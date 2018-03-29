using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.FxtUserCenter
{
    [Serializable]
    [TableAttribute("FxtUserCenter.dbo.CompanyInfo")]
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
        public string smsloginname
        {
            get { return _smsloginname; }
            set { _smsloginname = value; }
        }
        private string _smsloginpassword;
        public string smsloginpassword
        {
            get { return _smsloginpassword; }
            set { _smsloginpassword = value; }
        }
        private string _smssendname;
        /// <summary>
        /// 消息短信署名（公司简称）
        /// </summary>
        public string smssendname
        {
            get { return _smssendname; }
            set { _smssendname = value; }
        }
        private string _wxid;
        public string wxid
        {
            get { return _wxid; }
            set { _wxid = value; }
        }
        private string _wxname;
        public string wxname
        {
            get { return _wxname; }
            set { _wxname = value; }
        }
        private string _signname;
        public string signname
        {
            get { return _signname; }
            set { _signname = value; }
        }
        private int? _cityid;
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
        private string _telephone;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        private string _weburl;
        /// <summary>
        /// 公司网址
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
        }
        private string _address;
        /// <summary>
        /// 公司地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _linkman;
        /// <summary>
        /// 联系人
        /// </summary>
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private int? _companytypecode;
        public int? companytypecode
        {
            get { return _companytypecode; }
            set { _companytypecode = value; }
        }
        private string _englishname;
        /// <summary>
        /// 英文名ID
        /// </summary>
        public string englishname
        {
            get { return _englishname; }
            set { _englishname = value; }
        }
        private string _othername;
        /// <summary>
        /// 别名
        /// </summary>
        public string othername
        {
            get { return _othername; }
            set { _othername = value; }
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
        private string _legalman;
        /// <summary>
        /// 法人代表
        /// </summary>
        public string legalman
        {
            get { return _legalman; }
            set { _legalman = value; }
        }
        private int? _fk_usertypecode;
        /// <summary>
        /// 用户类型：5006001公用用户，5006002各城市用户，5006003房讯通用户类型：5006001公用用户，5006002各城市用户，5006003房讯通用户
        /// </summary>
        public int? fk_usertypecode
        {
            get { return _fk_usertypecode; }
            set { _fk_usertypecode = value; }
        }
        private int? _houseaptitudecode;
        /// <summary>
        /// 评估机构房地产评估资质
        /// </summary>
        public int? houseaptitudecode
        {
            get { return _houseaptitudecode; }
            set { _houseaptitudecode = value; }
        }
        private string _houseaptitudefile;
        /// <summary>
        /// 评估机构房地产评估资质附件
        /// </summary>
        public string houseaptitudefile
        {
            get { return _houseaptitudefile; }
            set { _houseaptitudefile = value; }
        }
        private string _landaptitudecode;
        /// <summary>
        /// 评估机构土地评估资质
        /// </summary>
        public string landaptitudecode
        {
            get { return _landaptitudecode; }
            set { _landaptitudecode = value; }
        }
        private string _landaptitudefile;
        /// <summary>
        /// 评估机构土地评估资质附件
        /// </summary>
        public string landaptitudefile
        {
            get { return _landaptitudefile; }
            set { _landaptitudefile = value; }
        }
        private int? _assetaptitudecode;
        /// <summary>
        /// 评估机构资产评估资质
        /// </summary>
        public int? assetaptitudecode
        {
            get { return _assetaptitudecode; }
            set { _assetaptitudecode = value; }
        }
        private int? _assetaptitudefile;
        /// <summary>
        /// 评估机构资产评估资质附件
        /// </summary>
        public int? assetaptitudefile
        {
            get { return _assetaptitudefile; }
            set { _assetaptitudefile = value; }
        }
        private string _organizationcode;
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        public string organizationcode
        {
            get { return _organizationcode; }
            set { _organizationcode = value; }
        }
        private string _logo;
        /// <summary>
        /// logo地址
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
        private int? _suspended;
        /// <summary>
        /// 暂停
        /// </summary>
        public int? suspended
        {
            get { return _suspended; }
            set { _suspended = value; }
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
        /// 营业执照附件
        /// </summary>
        public string businessfile
        {
            get { return _businessfile; }
            set { _businessfile = value; }
        }
        private bool _issigned = false;
        /// <summary>
        /// 是否签约
        /// </summary>
        public bool issigned
        {
            get { return _issigned; }
            set { _issigned = value; }
        }
        private DateTime? _joindate;
        /// <summary>
        /// 加入云估价成员时间
        /// </summary>
        public DateTime? joindate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
    }


}
