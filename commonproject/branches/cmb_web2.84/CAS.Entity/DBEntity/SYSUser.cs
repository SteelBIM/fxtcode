using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_User")]
    public class SYSUser : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _username;
        /// <summary>
        /// 用户名
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
        private string _truename;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string truename
        {
            get { return _truename; }
            set { _truename = value; }
        }
        private string _serils;
        /// <summary>
        /// 用户编号
        /// </summary>
        public string serils
        {
            get { return _serils; }
            set { _serils = value; }
        }
        private int? _danweiid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? danweiid
        {
            get { return _danweiid; }
            set { _danweiid = value; }
        }
        private int _subcompanyid;
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _department;
        /// <summary>
        /// 部门
        /// </summary>
        public string department
        {
            get { return _department; }
            set { _department = value; }
        }
        private int? _departmentid;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? departmentid
        {
            get { return _departmentid; }
            set { _departmentid = value; }
        }
        private string _jiaose;
        /// <summary>
        /// 角色
        /// </summary>
        public string jiaose
        {
            get { return _jiaose; }
            set { _jiaose = value; }
        }
        private string _jiaoseids;
        /// <summary>
        /// 角色Ids
        /// </summary>
        public string jiaoseids
        {
            get { return _jiaoseids; }
            set { _jiaoseids = value; }
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
        private string _zhiwei;
        /// <summary>
        /// 职位
        /// </summary>
        public string zhiwei
        {
            get { return _zhiwei; }
            set { _zhiwei = value; }
        }
        private string _zaigang;
        /// <summary>
        /// 在岗
        /// </summary>
        public string zaigang
        {
            get { return _zaigang; }
            set { _zaigang = value; }
        }
        private string _emailstr;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string emailstr
        {
            get { return _emailstr; }
            set { _emailstr = value; }
        }
        private string _mobile;
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }
        private string _iflogin;
        /// <summary>
        /// 是否允许登录
        /// </summary>
        public string iflogin
        {
            get { return _iflogin; }
            set { _iflogin = value; }
        }
        private string _sex;
        /// <summary>
        /// 性别
        /// </summary>
        public string sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        private string _backinfo;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string backinfo
        {
            get { return _backinfo; }
            set { _backinfo = value; }
        }
        private string _birthday;
        /// <summary>
        /// 生日
        /// </summary>
        public string birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }
        private string _mingzu;
        /// <summary>
        /// 民族
        /// </summary>
        public string mingzu
        {
            get { return _mingzu; }
            set { _mingzu = value; }
        }
        private string _sfzserils;
        /// <summary>
        /// 身份证号
        /// </summary>
        public string sfzserils
        {
            get { return _sfzserils; }
            set { _sfzserils = value; }
        }
        private string _hunying;
        /// <summary>
        /// 婚姻
        /// </summary>
        public string hunying
        {
            get { return _hunying; }
            set { _hunying = value; }
        }
        private string _zhengzhimianmao;
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string zhengzhimianmao
        {
            get { return _zhengzhimianmao; }
            set { _zhengzhimianmao = value; }
        }
        private string _jiguan;
        /// <summary>
        /// 籍贯
        /// </summary>
        public string jiguan
        {
            get { return _jiguan; }
            set { _jiguan = value; }
        }
        private string _hukou;
        /// <summary>
        /// 户口
        /// </summary>
        public string hukou
        {
            get { return _hukou; }
            set { _hukou = value; }
        }
        private string _xueli;
        /// <summary>
        /// 学历
        /// </summary>
        public string xueli
        {
            get { return _xueli; }
            set { _xueli = value; }
        }
        private string _zhicheng;
        /// <summary>
        /// 职称
        /// </summary>
        public string zhicheng
        {
            get { return _zhicheng; }
            set { _zhicheng = value; }
        }
        private string _biyeyuanxiao;
        /// <summary>
        /// 毕业院校
        /// </summary>
        public string biyeyuanxiao
        {
            get { return _biyeyuanxiao; }
            set { _biyeyuanxiao = value; }
        }
        private string _zhuanye;
        /// <summary>
        /// 专业
        /// </summary>
        public string zhuanye
        {
            get { return _zhuanye; }
            set { _zhuanye = value; }
        }
        private string _canjiagongzuotime;
        /// <summary>
        /// 参加工作时间
        /// </summary>
        public string canjiagongzuotime
        {
            get { return _canjiagongzuotime; }
            set { _canjiagongzuotime = value; }
        }
        private string _jiarubendanweitime;
        /// <summary>
        /// 加入本单位时间
        /// </summary>
        public string jiarubendanweitime
        {
            get { return _jiarubendanweitime; }
            set { _jiarubendanweitime = value; }
        }
        private string _jiatingdianhua;
        /// <summary>
        /// 家庭电话
        /// </summary>
        public string jiatingdianhua
        {
            get { return _jiatingdianhua; }
            set { _jiatingdianhua = value; }
        }
        private string _jiatingaddress;
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string jiatingaddress
        {
            get { return _jiatingaddress; }
            set { _jiatingaddress = value; }
        }
        private string _gangweibiandong;
        /// <summary>
        /// 岗位变动
        /// </summary>
        public string gangweibiandong
        {
            get { return _gangweibiandong; }
            set { _gangweibiandong = value; }
        }
        private string _jiaoyuebeijing;
        /// <summary>
        /// 教育背景
        /// </summary>
        public string jiaoyuebeijing
        {
            get { return _jiaoyuebeijing; }
            set { _jiaoyuebeijing = value; }
        }
        private string _gongzuojianli;
        /// <summary>
        /// 工作简历
        /// </summary>
        public string gongzuojianli
        {
            get { return _gongzuojianli; }
            set { _gongzuojianli = value; }
        }
        private string _shehuiguanxi;
        /// <summary>
        /// 社会关系
        /// </summary>
        public string shehuiguanxi
        {
            get { return _shehuiguanxi; }
            set { _shehuiguanxi = value; }
        }
        private string _jiangchengjilu;
        /// <summary>
        /// 奖惩记录
        /// </summary>
        public string jiangchengjilu
        {
            get { return _jiangchengjilu; }
            set { _jiangchengjilu = value; }
        }
        private string _zhiwuqingkuang;
        /// <summary>
        /// 职务情况
        /// </summary>
        public string zhiwuqingkuang
        {
            get { return _zhiwuqingkuang; }
            set { _zhiwuqingkuang = value; }
        }
        private string _peixunjilu;
        /// <summary>
        /// 培训记录
        /// </summary>
        public string peixunjilu
        {
            get { return _peixunjilu; }
            set { _peixunjilu = value; }
        }
        private string _danbaojilu;
        /// <summary>
        /// 担保记录
        /// </summary>
        public string danbaojilu
        {
            get { return _danbaojilu; }
            set { _danbaojilu = value; }
        }
        private string _naodonghetong;
        /// <summary>
        /// 劳动合同
        /// </summary>
        public string naodonghetong
        {
            get { return _naodonghetong; }
            set { _naodonghetong = value; }
        }
        private string _shebaojiaona;
        /// <summary>
        /// 社保缴纳
        /// </summary>
        public string shebaojiaona
        {
            get { return _shebaojiaona; }
            set { _shebaojiaona = value; }
        }
        private string _tijianjilu;
        /// <summary>
        /// 体检记录
        /// </summary>
        public string tijianjilu
        {
            get { return _tijianjilu; }
            set { _tijianjilu = value; }
        }
        private string _beizhustr;
        /// <summary>
        /// 备注记录
        /// </summary>
        public string beizhustr
        {
            get { return _beizhustr; }
            set { _beizhustr = value; }
        }
        private string _fujian;
        /// <summary>
        /// 附件文件
        /// </summary>
        public string fujian
        {
            get { return _fujian; }
            set { _fujian = value; }
        }
        private string _pop3username;
        /// <summary>
        /// Pop3用户名
        /// </summary>
        public string pop3username
        {
            get { return _pop3username; }
            set { _pop3username = value; }
        }
        private string _pop3userpwd;
        /// <summary>
        /// Pop3密码
        /// </summary>
        public string pop3userpwd
        {
            get { return _pop3userpwd; }
            set { _pop3userpwd = value; }
        }
        private string _pop3server;
        /// <summary>
        /// Pop3地址
        /// </summary>
        public string pop3server
        {
            get { return _pop3server; }
            set { _pop3server = value; }
        }
        private string _pop3port;
        /// <summary>
        /// Pop3端口
        /// </summary>
        public string pop3port
        {
            get { return _pop3port; }
            set { _pop3port = value; }
        }
        private string _smtpusername;
        /// <summary>
        /// SMTP用户名
        /// </summary>
        public string smtpusername
        {
            get { return _smtpusername; }
            set { _smtpusername = value; }
        }
        private string _smtpuserpwd;
        /// <summary>
        /// SMTP密码
        /// </summary>
        public string smtpuserpwd
        {
            get { return _smtpuserpwd; }
            set { _smtpuserpwd = value; }
        }
        private string _smtpserver;
        /// <summary>
        /// SMTP服务器
        /// </summary>
        public string smtpserver
        {
            get { return _smtpserver; }
            set { _smtpserver = value; }
        }
        private string _smtpfromemail;
        /// <summary>
        /// SMTP邮件地址
        /// </summary>
        public string smtpfromemail
        {
            get { return _smtpfromemail; }
            set { _smtpfromemail = value; }
        }
        private string _tixingtime = "600";
        /// <summary>
        /// 提醒间隔
        /// </summary>
        public string tixingtime
        {
            get { return _tixingtime; }
            set { _tixingtime = value; }
        }
        private string _iftixing = "是";
        /// <summary>
        /// 是否提醒
        /// </summary>
        public string iftixing
        {
            get { return _iftixing; }
            set { _iftixing = value; }
        }
        private string _daohanglist;
        /// <summary>
        /// 系统导航条
        /// </summary>
        public string daohanglist
        {
            get { return _daohanglist; }
            set { _daohanglist = value; }
        }
        private string _xkeguandep;
        /// <summary>
        /// 可管部门
        /// </summary>
        public string xkeguandep
        {
            get { return _xkeguandep; }
            set { _xkeguandep = value; }
        }
        private string _xiashuuser;
        /// <summary>
        /// 下属员工
        /// </summary>
        public string xiashuuser
        {
            get { return _xiashuuser; }
            set { _xiashuuser = value; }
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
        private int _userstatus = 1;
        /// <summary>
        /// 用户状态 1在职 2离职
        /// </summary>
        public int userstatus
        {
            get { return _userstatus; }
            set { _userstatus = value; }
        }
        private int? _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _loginip;
        /// <summary>
        /// 登录IP
        /// </summary>
        public string loginip
        {
            get { return _loginip; }
            set { _loginip = value; }
        }
        private int _tixingweizhi = 0;
        public int tixingweizhi
        {
            get { return _tixingweizhi; }
            set { _tixingweizhi = value; }
        }
        private bool _issale = false;
        /// <summary>
        /// 是否是专做业务
        /// </summary>
        public bool issale
        {
            get { return _issale; }
            set { _issale = value; }
        }
        private bool _isappraisers = false;
        /// <summary>
        /// 是否是专做报告
        /// </summary>
        public bool isappraisers
        {
            get { return _isappraisers; }
            set { _isappraisers = value; }
        }

        private string _wxopenid ;
        /// <summary>
        /// 新增微信OPenid
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }

        private string _headphoto;
        /// <summary>
        /// 头像
        /// </summary>
        public string headphoto
        {
            get { return _headphoto; }
            set { _headphoto = value; }
        }

        private int _accountstatus;
        /// <summary>
        /// 账号状态  
        /// 1、正常 2、锁定 3、禁止登陆
        /// </summary>
        public int accountstatus
        {
            get { return _accountstatus; }
            set { _accountstatus = value; }
        }

        private DateTime? _disabledate;
        /// <summary>
        /// 账号禁用时间
        /// </summary>
        public DateTime? disabledate
        {
            get { return _disabledate; }
            set { _disabledate = value; }
        }
        private DateTime? _hetongendtime;
        /// <summary>
        /// 合同截止日期
        /// </summary>
        public DateTime? hetongendtime
        {
            get { return _hetongendtime; }
            set { _hetongendtime = value; }
        }
    }
}