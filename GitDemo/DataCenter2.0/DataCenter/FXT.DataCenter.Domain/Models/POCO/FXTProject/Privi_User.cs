using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_User
    {
        private string _userid;
        /// <summary>
        /// ID
        /// </summary>
        //[SQLField("userid", EnumDBFieldUsage.PrimaryKey)]
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _username;
        /// <summary>
        /// 姓名
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private int _fk_companyid;
        /// <summary>
        /// 公司
        /// </summary>
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private int _fk_departmentid;
        /// <summary>
        /// 部门
        /// </summary>
        public int fk_departmentid
        {
            get { return _fk_departmentid; }
            set { _fk_departmentid = value; }
        }
        private int _fk_cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int fk_cityid
        {
            get { return _fk_cityid; }
            set { _fk_cityid = value; }
        }
        private string _password;
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _workcode;
        public string workcode
        {
            get { return _workcode; }
            set { _workcode = value; }
        }
        private string _mobilephone;
        /// <summary>
        /// 手机
        /// </summary>
        public string mobilephone
        {
            get { return _mobilephone; }
            set { _mobilephone = value; }
        }
        private string _officephone;
        /// <summary>
        /// 办公电话
        /// </summary>
        public string officephone
        {
            get { return _officephone; }
            set { _officephone = value; }
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
        private string _email;
        /// <summary>
        /// Email
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
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
        private string _linkman;
        public string linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        private string _createuser;
        public string createuser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int? _online;
        public int? online
        {
            get { return _online; }
            set { _online = value; }
        }
        private DateTime _overdate = DateTime.Now;
        public DateTime overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private DateTime? _lastonlinedate;
        public DateTime? lastonlinedate
        {
            get { return _lastonlinedate; }
            set { _lastonlinedate = value; }
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
        private int? _fk_systypecode;
        public int? fk_systypecode
        {
            get { return _fk_systypecode; }
            set { _fk_systypecode = value; }
        }
        private int _fk_fxt_companyid;
        public int fk_fxt_companyid
        {
            get { return _fk_fxt_companyid; }
            set { _fk_fxt_companyid = value; }
        }
        private int _licence = -1;
        public int licence
        {
            get { return _licence; }
            set { _licence = value; }
        }
        private string _usertoken = "newid";
        /// <summary>
        /// 保存登录令牌
        /// </summary>
        public string usertoken
        {
            get { return _usertoken; }
            set { _usertoken = value; }
        }
        private int? _postid;
        /// <summary>
        /// 职务ID
        /// </summary>
        public int? postid
        {
            get { return _postid; }
            set { _postid = value; }
        }
        private string _regnumber;
        /// <summary>
        /// 估价师注册号
        /// </summary>
        public string regnumber
        {
            get { return _regnumber; }
            set { _regnumber = value; }
        }
        private int _usertypecode = 1018002;
        /// <summary>
        /// 用户类型
        /// </summary>
        public int usertypecode
        {
            get { return _usertypecode; }
            set { _usertypecode = value; }
        }
        private int? _sextype;
        /// <summary>
        /// 性别：1男，0女
        /// </summary>
        public int? sextype
        {
            get { return _sextype; }
            set { _sextype = value; }
        }
        private string _idnumber;
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string idnumber
        {
            get { return _idnumber; }
            set { _idnumber = value; }
        }
        private int _userstatuscode = 9005001;
        /// <summary>
        /// 员工状态
        /// </summary>
        public int userstatuscode
        {
            get { return _userstatuscode; }
            set { _userstatuscode = value; }
        }
        private int _suspended = 0;
        /// <summary>
        /// 账号暂停
        /// </summary>
        public int suspended
        {
            get { return _suspended; }
            set { _suspended = value; }
        }
        private string _userimage;
        /// <summary>
        /// 头像
        /// </summary>
        public string userimage
        {
            get { return _userimage; }
            set { _userimage = value; }
        }
        private int _mastcompanyid = 0;
        /// <summary>
        /// 运营方ID
        /// </summary>
        public int mastcompanyid
        {
            get { return _mastcompanyid; }
            set { _mastcompanyid = value; }
        }
        private int? _suspendedtype;
        /// <summary>
        /// 暂停人类型
        /// </summary>
        public int? suspendedtype
        {
            get { return _suspendedtype; }
            set { _suspendedtype = value; }
        }

    }
}
