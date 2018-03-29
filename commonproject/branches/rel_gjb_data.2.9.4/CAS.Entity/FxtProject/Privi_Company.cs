using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.FxtProject
{
    [Serializable]
    [TableAttribute("fxtproject.dbo.Privi_Company")]
    public class Privi_Company : BaseTO
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
        private string _companyname;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        private string _englishname;
        /// <summary>
        /// 英文名称
        /// </summary>
        public string englishname
        {
            get { return _englishname; }
            set { _englishname = value; }
        }
        private int? _fk_companytypecode;
        /// <summary>
        /// 公司类型
        /// </summary>
        public int? fk_companytypecode
        {
            get { return _fk_companytypecode; }
            set { _fk_companytypecode = value; }
        }
        private int? _fk_cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int? fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
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
        private string _telephone;
        /// <summary>
        /// 电话
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
        private string _weburl;
        /// <summary>
        /// 公司网站
        /// </summary>
        public string weburl
        {
            get { return _weburl; }
            set { _weburl = value; }
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
        private string _linkman;
        /// <summary>
        /// 联系人
        /// </summary>
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
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
        private int? _fk_usertypecode = 5006002;
        /// <summary>
        /// 用户类型：5006001公用用户，5006002各城市用户，5006003房讯通用户
        /// </summary>
        public int? fk_usertypecode
        {
            get { return _fk_usertypecode; }
            set { _fk_usertypecode = value; }
        }
        private int? _ownerid = 0;
        /// <summary>
        /// 创建者
        /// </summary>
        public int? ownerid
        {
            get { return _ownerid; }
            set { _ownerid = value; }
        }
        private DateTime? _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int? _cvalid = 1;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? cvalid
        {
            get { return _cvalid; }
            set { _cvalid = value; }
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
        private string _pinyin;
        /// <summary>
        /// 拼音
        /// </summary>
        public string pinyin
        {
            get { return _pinyin; }
            set { _pinyin = value; }
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
        private int? _landaptitudecode;
        /// <summary>
        /// 评估机构土地评估资质
        /// </summary>
        public int? landaptitudecode
        {
            get { return _landaptitudecode; }
            set { _landaptitudecode = value; }
        }
        private int? _parentid = 0;
        /// <summary>
        /// 上级公司/机构
        /// </summary>
        public int? parentid
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        private string _companycode;
        /// <summary>
        /// 组织机构代码证
        /// </summary>
        public string companycode
        {
            get { return _companycode; }
            set { _companycode = value; }
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
        private string _legalcode;
        /// <summary>
        /// 法人代码
        /// </summary>
        public string legalcode
        {
            get { return _legalcode; }
            set { _legalcode = value; }
        }
        private int? _suspended = 0;
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
    }

}
