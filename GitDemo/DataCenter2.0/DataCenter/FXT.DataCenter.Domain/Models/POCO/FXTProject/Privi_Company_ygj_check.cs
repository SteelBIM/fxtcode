using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_ygj_check
    {
        private int _id;
        //[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 公司Id
        /// </summary>
        public int? companyid
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
        /// 英文名
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
        private int? _fk_companytypecode;
        /// <summary>
        /// 用户类型
        /// </summary>
        public int? fk_companytypecode
        {
            get { return _fk_companytypecode; }
            set { _fk_companytypecode = value; }
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
        private string _legalman;
        /// <summary>
        /// 法人代表
        /// </summary>
        public string legalman
        {
            get { return _legalman; }
            set { _legalman = value; }
        }
        private int _houseaptitudecode = 1013001;
        /// <summary>
        /// 评估机构房地产评估资质
        /// </summary>
        public int houseaptitudecode
        {
            get { return _houseaptitudecode; }
            set { _houseaptitudecode = value; }
        }
        private int _landaptitudecode = 1014001;
        /// <summary>
        /// 评估机构土地评估资质
        /// </summary>
        public int landaptitudecode
        {
            get { return _landaptitudecode; }
            set { _landaptitudecode = value; }
        }
        private int _assetsaptitudecode = 1021001;
        /// <summary>
        /// 评估机构资产评估资质
        /// </summary>
        public int assetsaptitudecode
        {
            get { return _assetsaptitudecode; }
            set { _assetsaptitudecode = value; }
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
        /// 工商登记号
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
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _suspended = 0;
        /// <summary>
        /// 暂停
        /// </summary>
        public int suspended
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
        private int _checktype = 9011001;
        /// <summary>
        /// 审核结果类型.
        /// </summary>
        public int checktype
        {
            get { return _checktype; }
            set { _checktype = value; }
        }
        private DateTime? _checkdate;
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? checkdate
        {
            get { return _checkdate; }
            set { _checkdate = value; }
        }
        private string _checkremark;
        /// <summary>
        /// 审核内容
        /// </summary>
        public string checkremark
        {
            get { return _checkremark; }
            set { _checkremark = value; }
        }
        private string _checkuserid;
        /// <summary>
        /// 审核人
        /// </summary>
        public string checkuserid
        {
            get { return _checkuserid; }
            set { _checkuserid = value; }
        }
        private string _houseaptitudeno;
        /// <summary>
        /// 房地产资质证编号
        /// </summary>
        public string houseaptitudeno
        {
            get { return _houseaptitudeno; }
            set { _houseaptitudeno = value; }
        }
        private string _landaptitudeno;
        /// <summary>
        /// 土地资质编号
        /// </summary>
        public string landaptitudeno
        {
            get { return _landaptitudeno; }
            set { _landaptitudeno = value; }
        }
        private string _assetsaptitudeno;
        /// <summary>
        /// 资产评估证书编号
        /// </summary>
        public string assetsaptitudeno
        {
            get { return _assetsaptitudeno; }
            set { _assetsaptitudeno = value; }
        }
        private int? _applycityid;
        /// <summary>
        /// 申请城市
        /// </summary>
        public int? applycityid
        {
            get { return _applycityid; }
            set { _applycityid = value; }
        }
        private int? _houseassociationid;
        /// <summary>
        /// 房地产所属协会
        /// </summary>
        public int? houseassociationid
        {
            get { return _houseassociationid; }
            set { _houseassociationid = value; }
        }
        private int? _landassociationid;
        /// <summary>
        /// 土地资质所属协会
        /// </summary>
        public int? landassociationid
        {
            get { return _landassociationid; }
            set { _landassociationid = value; }
        }
        private int? _assetsassociationid;
        /// <summary>
        /// 资产评估所属协会
        /// </summary>
        public int? assetsassociationid
        {
            get { return _assetsassociationid; }
            set { _assetsassociationid = value; }
        }
        private string _companydesc;
        /// <summary>
        /// 公司简介
        /// </summary>
        public string companydesc
        {
            get { return _companydesc; }
            set { _companydesc = value; }
        }
        private string _fileno;
        /// <summary>
        /// 报告编号前缀
        /// </summary>
        public string fileno
        {
            get { return _fileno; }
            set { _fileno = value; }
        }
        private string _linkmanid;
        /// <summary>
        /// 联系人身份证号
        /// </summary>
        public string linkmanid
        {
            get { return _linkmanid; }
            set { _linkmanid = value; }
        }

    }
}
